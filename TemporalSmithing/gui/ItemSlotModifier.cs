using System.Text;
using temporalsmithing.content.modifier;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace temporalsmithing.gui;

public class ItemSlotModifier : ItemSlot {

	private readonly InventorySmithingTable inv;
	private ModifierEntry heldModifier;
	private bool selected;

	public ItemSlotModifier(InventorySmithingTable inventory, ModifierEntry heldModifier) : base(inventory) {
		inv = inventory;
		HeldModifier = heldModifier;
	}

	public ModifierEntry HeldModifier {
		get => heldModifier;
		internal set {
			var init = heldModifier is null;
			heldModifier = value;
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
		return HeldModifier is not null;
	}

	public override string GetStackName() {
		if (itemstack is null || !HoldsModifier()) return "";

		return itemstack.GetName();
	}

	public override string GetStackDescription(IClientWorldAccessor world, bool extendedDebugInfo) {
		var builder = new StringBuilder();

		HeldModifier.Modifier.WriteDescription(HeldModifier.SourceItem, builder);

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
		if (heldModifier is not null) blockEntity.UpdateRequiredHits(heldModifier.Modifier.GetRequiredHitsToRemove());
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