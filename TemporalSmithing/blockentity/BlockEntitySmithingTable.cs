using System;
using System.Drawing;
using System.IO;
using System.Text;
using temporalsmithing.content.modifier;
using temporalsmithing.@enum;
using temporalsmithing.gui;
using temporalsmithing.util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace temporalsmithing.blockentity;

public class BlockEntitySmithingTable : BlockEntityContainer {

	private readonly InventorySmithingTable inv;
	private readonly string whiteText = ColorToHex.Transform(Color.White);
	internal string CurrentModifierKey;
	private long lastHitSoundMs;
	internal int PerformedHits;
	internal int RequiredHits;

	public BlockEntitySmithingTable() {
		inv = new InventorySmithingTable(this, Api);
		Inventory.SlotModified += Inventory_SlotModified;
	}

	internal GuiDialogSmithingTable InvDialog { get; set; }

	public sealed override InventoryBase Inventory => inv;
	public override string InventoryClassName => "smithingtable";

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving) {
		PerformedHits = tree.GetInt("PerformedHits");
		base.FromTreeAttributes(tree, worldForResolving);
	}

	public override void ToTreeAttributes(ITreeAttribute tree) {
		tree.SetInt("PerformedHits", PerformedHits);
		base.ToTreeAttributes(tree);
	}

	public override void Initialize(ICoreAPI api) {
		base.Initialize(api);

		var modStack = inv.GetModifierSlot().Itemstack;
		var mod = RunePowers.Instance.GetModifier(modStack);
		if (mod is RunePowerUnknown) return;
		RequiredHits = mod.GetRequiredHitsToApply();
		CurrentModifierKey = RunePowers.Instance.GetModifierKey(modStack);
	}

	public void OnPlayerRightClick(IPlayer byPlayer) {
		if (Api.Side == EnumAppSide.Client) TryOpenInvDialog(byPlayer);
	}

	public bool OnPlayerLeftClick(IPlayer player, ItemSlot slot) {
		var world = player.Entity.World;
		var stack = slot?.Itemstack;
		if (stack?.Item is ItemHammer) {
			if (GetDelta(player, lastHitSoundMs) <= 770) return true;
			world.PlaySoundAt(new AssetLocation("game:sounds/effect/anvilhit"), player, player);
			lastHitSoundMs = world.ElapsedMilliseconds;

			NotifyItemDamage(EnumSmithingMode.AddModification);

			return true;
		}

		var offHand = player.Entity.LeftHandItemSlot;
		if (stack?.Item is ItemChisel && offHand.Itemstack?.Item is ItemHammer) {
			if (GetDelta(player, lastHitSoundMs) > 610) {
				world.PlaySoundAt(new AssetLocation("game:sounds/effect/anvilhit"), player, player);
				lastHitSoundMs = world.ElapsedMilliseconds;

				NotifyItemDamage(EnumSmithingMode.RemoveModification);
			}

			return true;
		}

		return false;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc) {
		if (inv.IsItemInInputSlot()) {
			var stack = inv.GetInputSlot()?.Itemstack;
			if (stack == null) return;

			dsc.AppendLine().AppendLine("Item: " + stack.GetName());
			dsc.AppendLine("Slots: " + inv.GetCachedModifiers().Count + " / " + inv.GetUnlockedSlots());
			if (!inv.IsItemModifiable()) {
				var fontColor = ColorToHex.Transform(Color.Orange);
				var errorMessage = Lang.Get("temporalsmithing:gui-smithing-table.input-slot-invalid");
				dsc.AppendLine($"<font color=\"#{fontColor}\"><icon name=warning></font> {errorMessage}");
				return;
			}

			if (inv.IsValidModifier()) {
				dsc.AppendLine();
				dsc.AppendLine(Lang.Get("temporalsmithing:smithing-table.action"));
				var mod = RunePowers.Instance.GetModifier(CurrentModifierKey);
				var iconColor = "#" + ColorToHex.Transform(mod.GetIconColor());
				var iconKey = mod.GetIconKey();
				var modName = Lang.Get(mod.GetFullLangKey());
				dsc.AppendLine($"<font color=\"{iconColor}\"><icon name={iconKey}/></font>" +
							   $"<font color=\"{whiteText}\"> {modName}</font>");

				var percentage = (float)PerformedHits / RequiredHits * 100;
				dsc.AppendLine(Lang.Get("temporalsmithing:smithing-table.progress",
					percentage, PerformedHits, RequiredHits));

				return;
			}

			var mods = inv.GetCachedModifiers();
			if (mods.Count == 0) {
				var infoMessage = Lang.Get("temporalsmithing:gui-smithing-table.no-modifiers-applied");
				dsc.AppendLine(infoMessage);
				return;
			}

			dsc.AppendLine(Lang.Get("temporalsmithing:applied-modifiers"));
			for (var i = 0; i < Math.Min(mods.Count, 4); i++) {
				var entry = mods[i];
				var iconColor = "#" + ColorToHex.Transform(entry.RunePower.GetIconColor());
				var iconKey = entry.RunePower.GetIconKey();
				var modName = Lang.Get(entry.RunePower.GetFullLangKey());
				dsc.AppendLine($"<font color=\"{iconColor}\"><icon name={iconKey}/></font>" +
							   $"<font color=\"{whiteText}\"> {modName}</font>");
			}

			if (mods.Count >= 5) dsc.AppendLine(Lang.Get("temporalsmithing:applied-modifiers.and-more", mods.Count - 4));
		}
	}

	#region Networking

	public void ResetPerformedHits() {
		if (Api is not ICoreClientAPI capi) return;
		WriteData(out var data, x => x.Write(PerformedHits = 0));
		capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, 5002);
	}

	public void UpdateRequiredHits(int newRequiredHits) {
		if (Api is not ICoreClientAPI capi) return;
		WriteData(out var data, x => x.Write(RequiredHits = newRequiredHits));
		capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, 5010, data);
	}

	public void SetCurrentModifierKey(string modKey) {
		if (Api is not ICoreClientAPI capi) return;
		WriteData(out var data, x => x.Write(CurrentModifierKey = modKey));
		capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, 5011, data);
	}

	public void ForceCloseGuiForAll() {
		if (Api is not ICoreClientAPI capi) return;
		capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, 5020);
		InvDialog?.TryClose();
	}

	public override void OnReceivedClientPacket(IPlayer player, int packetid, byte[] data) {
		base.OnReceivedClientPacket(player, packetid, data);
		if (Api is not ICoreServerAPI sapi) return;

		if (packetid < 1000) {
			Inventory.InvNetworkUtil.HandleClientPacket(player, packetid, data);
			Api.World.BlockAccessor.GetChunkAtBlockPos(Pos.X, Pos.Y, Pos.Z).MarkModified();
		}
		else {
			var playerEntity = player.Entity;

			var activeSlot = player.InventoryManager.ActiveHotbarSlot;
			var item = activeSlot?.Itemstack?.Item;
			var offhandSlot = playerEntity.LeftHandItemSlot;
			var offhandItem = offhandSlot?.Itemstack?.Item;
			var array = Array.Empty<byte>();
			var modifierSlot = inv.GetModifierSlot();
			switch (packetid) {
				case 5000: // Performed Add Modification Action
					if (item is ItemHammer) {
						item.DamageItem(playerEntity.World, playerEntity, activeSlot);
						activeSlot.MarkDirty();

						PerformedHits++;
						if (PerformedHits >= RequiredHits) {
							var mod = RunePowers.Instance.GetModifier(CurrentModifierKey);
							if (mod is not RunePowerUnknown) {
								var inputSlot = inv.GetInputSlot();
								mod.OnModificationFinish?.Invoke(Api, inputSlot, modifierSlot);
								inputSlot.MarkDirty();
							}

							modifierSlot.Itemstack = null;
							modifierSlot.MarkDirty();
						}
						else {
							WriteData(out array, x => x.Write(PerformedHits));
							sapi.Network.SendBlockEntityPacket(player as IServerPlayer, Pos, 6000, array);
						}
					}

					break;
				case 5001: // Performed Remove Modification Action
					if (item is ItemChisel && offhandItem is ItemHammer) {
						item.DamageItem(playerEntity.World, playerEntity, activeSlot);
						activeSlot.MarkDirty();
						offhandItem.DamageItem(playerEntity.World, playerEntity, offhandSlot);
						offhandSlot.MarkDirty();

						PerformedHits++;
						if (PerformedHits >= RequiredHits) {
							var selectedSlot = inv.GetSelectedModifierSlot();
							if (selectedSlot is ItemSlotModifier { HeldRunePower: { } } ism) {
								var mod = ism.HeldRunePower.RunePower;
								var retrievalChance = mod.GetItemRetrievalChanceOnRemoval();
								if (new Random().NextDouble() <= retrievalChance) {
									var sourceItemStack = ism.HeldRunePower.SourceItem;
									modifierSlot.Itemstack = sourceItemStack;
									modifierSlot.MarkDirty();
								}

								if (mod is not RunePowerUnknown) {
									var inputSlot = inv.GetInputSlot();
									mod.OnModifierRemoved?.Invoke(Api, inputSlot, ism.HeldRunePower);
									inputSlot.MarkDirty();
								}

								ism.Itemstack = null;
								ism.HeldRunePower = null;
								ism.MarkDirty();
								sapi.Network.SendBlockEntityPacket(player as IServerPlayer, Pos, 6020, array);
							}
						}
						else {
							WriteData(out array, x => x.Write(PerformedHits));
							sapi.Network.SendBlockEntityPacket(player as IServerPlayer, Pos, 6000, array);
						}
					}

					break;
				case 5002: // Reset PerformedHits
					WriteData(out array, x => x.Write(PerformedHits = 0));
					sapi.Network.SendBlockEntityPacket(player as IServerPlayer, Pos, 6000, array);
					break;
				case 5010: // Set RequiredHits
				{
					using var output = new MemoryStream(data);
					var reader = new BinaryReader(output);
					var newRequiredHits = GameMath.Max(0, reader.ReadInt32());

					WriteData(out array, x => x.Write(RequiredHits = newRequiredHits));
					sapi.Network.SendBlockEntityPacket(player as IServerPlayer, Pos, 6010, array);
					break;
				}
				case 5011: // Set CurrentModifierKey
				{
					using var output = new MemoryStream(data);
					var reader = new BinaryReader(output);
					var currentModifierKey = reader.ReadString();

					WriteData(out array, x => x.Write(CurrentModifierKey = currentModifierKey));
					sapi.Network.SendBlockEntityPacket(player as IServerPlayer, Pos, 6011, array);
					break;
				}
				case 5020: // Force Close InvDialog
					sapi.Network.SendBlockEntityPacket(player as IServerPlayer, Pos, 1001, array);
					break;
				case 1001:
					player.InventoryManager?.CloseInventory(Inventory);
					break;
				case 1000:
					player.InventoryManager?.OpenInventory(Inventory);
					break;
			}
		}
	}

	public override void OnReceivedServerPacket(int packetid, byte[] data) {
		switch (packetid) {
			case 6000: {
				using var output = new MemoryStream(data);
				var reader = new BinaryReader(output);
				reader.ReadInt16(); // Skip two bytes - idk why they are there?
				var newPerformedHits = reader.ReadInt16();

				PerformedHits = newPerformedHits;
				InvDialog?.MarkDirty();
				break;
			}
			case 6010: {
				using var output = new MemoryStream(data);
				var reader = new BinaryReader(output);
				reader.ReadInt16(); // Skip two bytes - idk why they are there?
				var newRequiredHits = reader.ReadInt16();

				RequiredHits = newRequiredHits;
				InvDialog.MarkDirty();
				break;
			}
			case 6011: {
				using var output = new MemoryStream(data);
				var reader = new BinaryReader(output);
				reader.ReadInt16(); // Skip two bytes - idk why they are there?
				var newRequiredHits = reader.ReadString();

				CurrentModifierKey = newRequiredHits;
				break;
			}
			case 6020: {
				var selectedSlot = inv.GetSelectedModifierSlot();
				if (selectedSlot is ItemSlotModifier { HeldRunePower: { } } ism) ism.HeldRunePower = null;
				inv.BlockEntity?.InvDialog?.MarkDirty();
				break;
			}
			case 1001:
				(Api.World as IClientWorldAccessor)?.Player.InventoryManager.CloseInventory(Inventory);
				InvDialog?.TryClose();
				InvDialog?.Dispose();
				InvDialog = null;
				break;
		}
	}

	private void WriteData(out byte[] array, Action<BinaryWriter> action) {
		using var output = new MemoryStream();
		var binaryWriter = new BinaryWriter(output);
		action.Invoke(binaryWriter);
		array = output.ToArray();
	}

	#endregion

	#region Helper Methods

	private long GetDelta(IPlayer player, long ms) {
		return player.Entity.World.ElapsedMilliseconds - ms;
	}

	private void Inventory_SlotModified(int slotId) {
		MarkDirty();
	}

	private void NotifyItemDamage(EnumSmithingMode smithingMode) {
		if (Api is not ICoreClientAPI capi) return;

		switch (smithingMode) {
			case EnumSmithingMode.AddModification:
				if (!inv.IsValidModifier()) return;
				break;
			case EnumSmithingMode.RemoveModification:
				if (inv.GetSelectedModifierSlot() is null) return;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(smithingMode), smithingMode, null);
		}

		var packetId = smithingMode == EnumSmithingMode.AddModification ? 5000 : 5001;
		capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, packetId);
	}

	private void TryOpenInvDialog(IPlayer byPlayer) {
		if (InvDialog == null && Api is ICoreClientAPI capi) {
			InvDialog = new GuiDialogSmithingTable(inv, Pos, Api as ICoreClientAPI, this);
			InvDialog.OnClosed += () => {
				InvDialog = null;
				capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, 1001);
				capi.Network.SendPacketClient(Inventory.Close(byPlayer));
			};
			InvDialog.OpenSound = AssetLocation.Create("sounds/block/barrelopen", Block.Code.Domain);
			InvDialog.CloseSound = AssetLocation.Create("sounds/block/barrelclose", Block.Code.Domain);
			InvDialog.TryOpen();
			capi.Network.SendPacketClient(Inventory.Open(byPlayer));
			capi.Network.SendBlockEntityPacket(Pos.X, Pos.Y, Pos.Z, 1000);
		}
	}

	#endregion

}