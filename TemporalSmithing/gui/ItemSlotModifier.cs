using System.Text;
using temporalsmithing.content.modifier;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace temporalsmithing.gui;

public class ItemSlotModifier : ItemSlot {

	private readonly InventorySmithingTable inv;
	private AppliedRune heldAppliedRunePower;
	private bool selected;

	public ItemSlotModifier(InventorySmithingTable inventory, AppliedRune heldAppliedRunePower) : base(inventory) {
		inv = inventory;
		HeldAppliedRunePower = heldAppliedRunePower;
	}

	public AppliedRune HeldAppliedRunePower {
		get => heldAppliedRunePower;
		internal set {
			var init = heldAppliedRunePower is null;
			heldAppliedRunePower = value;
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
		return HeldAppliedRunePower is not null;
	}

	public override string GetStackName() {
		if (itemstack is null || !HoldsModifier()) return "";

		return itemstack.GetName();
	}

	public override string GetStackDescription(IClientWorldAccessor world, bool extendedDebugInfo) {
		var builder = new StringBuilder();

		HeldAppliedRunePower.Rune.WriteDescription(HeldAppliedRunePower.SourceItem, builder);

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
		if (heldAppliedRunePower is not null)
			blockEntity.UpdateRequiredHits(state
				? heldAppliedRunePower.Rune.GetRequiredHitsToRemove()
				: heldAppliedRunePower.Rune.GetRequiredHitsToApply());
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
