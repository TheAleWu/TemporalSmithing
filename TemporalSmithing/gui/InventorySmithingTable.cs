using System;
using System.Collections.Generic;
using System.Linq;
using temporalsmithing.behavior.collectible;
using temporalsmithing.blockentity;
using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace temporalsmithing.gui;

public class InventorySmithingTable : InventoryDisplayed {

	internal const int MinSlots = 2;
	internal readonly BlockEntitySmithingTable BlockEntity;
	private readonly List<ModifierEntry> modifiers = new();
	internal readonly Errors Validation;

	public InventorySmithingTable(BlockEntitySmithingTable be, ICoreAPI api) : base(be, MinSlots + 9 * 6,
		"smithingtable-0", api) {
		BlockEntity = be;
		PerishableFactorByFoodCategory = null;
		Validation = new Errors(this);

		GetInputSlot().BackgroundIcon = "hammer";
	}

	public ItemSlot GetInputSlot() {
		return this[0];
	}

	public ItemSlot GetModifierSlot() {
		return this[1];
	}

	public bool IsItemInInputSlot() {
		return !GetInputSlot().Empty;
	}

	public bool IsValidModifier() {
		return IsItemInModifierSlot() && Modifiers.Instance.IsValidModifier(GetModifierSlot().Itemstack);
	}

	public ItemSlot GetSelectedModifierSlot() {
		return slots.FirstOrDefault(x => x is ItemSlotModifier ism && ism.IsSelected());
	}

	public bool IsItemModifiable() {
		if (!IsItemInInputSlot()) return false;

		return GetInputSlot().Itemstack.Collectible.CollectibleBehaviors
							 .Any(x => x is ModifiableBehavior);
	}

	public override void OnItemSlotModified(ItemSlot slot) {
		base.OnItemSlotModified(slot);
		if (slot == GetInputSlot()) {
			RefreshModifiers();
			BlockEntity.ResetPerformedHits();
		}

		if (slot != GetModifierSlot()) return;
		var mod = Modifiers.Instance.GetModifier(slot.Itemstack);
		BlockEntity.UpdateRequiredHits(slot.Empty ? 0 : mod.GetRequiredHitsToApply());
	}

	public List<ModifierEntry> GetCachedModifiers() {
		return modifiers;
	}

	public int GetUnlockedSlots() {
		return IsItemInInputSlot()
			? GetInputSlot().Itemstack.Attributes?.GetInt(UnlockingModifier.UnlockedSlotsKey) ?? 0
			: 0;
	}

	private void RefreshModifiers() {
		modifiers.Clear();
		var stack = GetInputSlot().Itemstack;
		if (IsItemModifiable() && stack != null)
			modifiers.AddRange(Modifiers.Instance.ReadAppliedModifiers(stack));

		DistributeModifiersOnSlots();
		UpdateModifierSlots();
	}

	private void DistributeModifiersOnSlots() {
		var cachedModifiers = GetCachedModifiers();
		for (var i = 0; i < GetUnlockedSlots(); i++) {
			var modSlot = slots[MinSlots + i] as ItemSlotModifier;
			if (modSlot is null) continue;

			modSlot.HeldModifier = i < cachedModifiers.Count
				? cachedModifiers[i]
				: null;
		}
	}

	private void UpdateModifierSlots() {
		for (var i = MinSlots; i < slots.Length; i++) {
			if (slots[i] is not ItemSlotModifier slot) continue;
			if (!slot.HoldsModifier()) {
				slots[i].Itemstack = null;
				continue;
			}

			if (slot.HeldModifier?.SourceItem is null) return;

			var sourceItem = new ItemStack(slot.HeldModifier?.SourceItem?.Item);
			slot.Itemstack = sourceItem;
		}
	}

	protected override ItemSlot NewSlot(int slotId) {
		switch (slotId) {
			case 1:
				return new ItemSlotModifierInput(this);
			case < MinSlots:
				return base.NewSlot(slotId);
		}

		var modIndex = slotId - MinSlots;
		var mod = modIndex < modifiers.Count ? modifiers[modIndex] : null;
		var slot = new ItemSlotModifier(this, mod);
		return slot;
	}

	public override void LateInitialize(string inventoryId, ICoreAPI api) {
		base.LateInitialize(inventoryId, api);

		RefreshModifiers();
		UpdateModifierSlots();
	}

	public override object ActivateSlot(int slotId, ItemSlot sourceSlot, ref ItemStackMoveOperation op) {
		if (slotId < MinSlots || this[slotId] is not ItemSlotModifier slot)
			return base.ActivateSlot(slotId, sourceSlot, ref op);

		if (slot.HoldsModifier() && GetModifierSlot().Itemstack is null) slot.ToggleSelected();

		return base.ActivateSlot(slotId, sourceSlot, ref op);
	}

	internal class Errors {

		private readonly InventorySmithingTable inv;

		public Errors(InventorySmithingTable inv) {
			this.inv = inv;
		}

		public bool IsInputSlotInvalid() {
			return inv.IsItemInInputSlot() && !inv.IsItemModifiable();
		}

		public bool IsModifierSlotInvalid() {
			return inv.IsItemInModifierSlot() && inv.IsItemModifiable() && !inv.IsValidModifier() &&
				   !inv.CanUnlockModificationSlot() && !inv.CanItemBeModifiedByModifier();
		}

		public bool AreAllSlotsOccupied() {
			return inv.IsItemInModifierSlot() && inv.IsItemModifiable() && inv.IsValidModifier() &&
				   !inv.HasFreeModificationSlots();
		}

	}

	#region Modifier Slot Condition Methods

	private bool IsItemInModifierSlot() {
		return !GetModifierSlot().Empty;
	}

	private bool HasFreeModificationSlots() {
		return (IsValidModifier() &&
				GetCachedModifiers().Count < GetUnlockedSlots()) ||
			   CanUnlockModificationSlot();
	}

	private bool CanUnlockModificationSlot() {
		return Modifiers.Instance.GetModifier(GetModifierSlot().Itemstack) is UnlockingModifier;
	}

	private bool CanItemBeModifiedByModifier() {
		var stack = GetInputSlot().Itemstack?.Item;
		if (stack?.CollectibleBehaviors is null) return false;
		foreach (var behavior in stack.CollectibleBehaviors)
			if (behavior is ModifiableBehavior mb && mb.PossibleModifiers.Contains(BlockEntity.CurrentModifierKey))
				return true;

		return false;
	}

	#endregion

	#region Prevent modifier slot drops when broken

	public override void DropSlots(Vec3d pos, params int[] slotsIds) {
		foreach (var slotsId in slotsIds) {
			if (slotsId >= MinSlots) continue;
			var itemSlot = slotsId >= 0 ? this[slotsId] : throw new Exception("Negative slot-id?!");
			if (itemSlot.Itemstack == null) continue;
			Api.World.SpawnItemEntity(itemSlot.Itemstack, pos);
			itemSlot.Itemstack = null;
			itemSlot.MarkDirty();
		}
	}

	public override void DropAll(Vec3d pos, int maxStackSize = 0) {
		for (var i = 0; i < MinSlots; i++) {
			var itemSlot = this[i];
			if (itemSlot.Itemstack == null) continue;
			if (maxStackSize > 0)
				while (itemSlot.StackSize > 0) {
					var quantity = GameMath.Clamp(itemSlot.StackSize, 1, maxStackSize);
					Api.World.SpawnItemEntity(itemSlot.TakeOut(quantity), pos);
				}
			else
				Api.World.SpawnItemEntity(itemSlot.Itemstack, pos);

			itemSlot.Itemstack = null;
			itemSlot.MarkDirty();
		}
	}

	#endregion

}