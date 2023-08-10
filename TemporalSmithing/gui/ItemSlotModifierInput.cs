using Vintagestory.API.Common;

namespace temporalsmithing.gui;

public class ItemSlotModifierInput : ItemSlot {

	private readonly InventorySmithingTable inv;

	public ItemSlotModifierInput(InventorySmithingTable inventory) : base(inventory) {
		inv = inventory;
	}

	public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = EnumMergePriority.AutoMerge) {
		return CanHold(sourceSlot);
	}

	public override bool CanHold(ItemSlot sourceSlot) {
		return inv.GetSelectedModifierSlot() is null;
	}

}