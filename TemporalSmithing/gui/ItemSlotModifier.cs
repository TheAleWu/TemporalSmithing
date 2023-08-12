using System.Text;
using temporalsmithing.content.modifier;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace temporalsmithing.gui;

public class ItemSlotModifier : ItemSlot {

	private readonly InventorySmithingTable inv;
	private RunePowerEntry heldRunePower;
	private bool selected;

	public ItemSlotModifier(InventorySmithingTable inventory, RunePowerEntry heldRunePower) : base(inventory) {
		inv = inventory;
		HeldRunePower = heldRunePower;
	}

	public RunePowerEntry HeldRunePower {
		get => heldRunePower;
		internal set {
			var init = heldRunePower is null;
			heldRunePower = value;
			if (!init) SetSelected(false);
		}
	}

	public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = EnumMergePriority.AutoMerge) {
		return false;
	}

	public override bool CanTake() {
		return false;
	}

	public override bool CanHold(ItemSlot sourceSlot) {
		return false;
	}

	public bool HoldsModifier() {
		return HeldRunePower is not null;
	}

	public override string GetStackName() {
		if (itemstack is null || !HoldsModifier()) return "";

		return itemstack.GetName();
	}

	public override string GetStackDescription(IClientWorldAccessor world, bool extendedDebugInfo) {
		var builder = new StringBuilder();

		HeldRunePower.RunePower.WriteDescription(HeldRunePower.SourceItem, builder);

		return builder.ToString();
	}

	public bool IsSelected() {
		return selected;
	}

	private void SetSelected(bool state) {
		DrawUnavailable = state;
		selected = state;

		var blockEntity = inv?.BlockEntity;
		if (blockEntity is null) return;

		blockEntity.ResetPerformedHits();
		if (heldRunePower is not null) blockEntity.UpdateRequiredHits(heldRunePower.RunePower.GetRequiredHitsToRemove());
		blockEntity.InvDialog?.MarkDirty();
	}

	public void ToggleSelected() {
		if (selected) {
			SetSelected(false);
			return;
		}

		foreach (var slot in inventory) {
			if (slot is not ItemSlotModifier { selected: true } ism) continue;
			ism.SetSelected(false);
		}

		SetSelected(true);
	}

}