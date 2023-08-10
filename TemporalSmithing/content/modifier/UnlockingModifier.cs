using Vintagestory.API.Common;

namespace temporalsmithing.content.modifier;

public class UnlockingModifier : Modifier {

	public const string UnlockedSlotsKey = "unlockedSlots";

	protected UnlockingModifier(int unlockedSlots = 1) {
		OnModificationFinish += (_, modified, _) => AddSlotsToStack(modified, unlockedSlots);
	}

	private static void AddSlotsToStack(ItemSlot slot, int unlockedSlots) {
		if (slot.Empty) return;
		var stack = slot.Itemstack;
		var slots = unlockedSlots;
		if (stack.Attributes.HasAttribute(UnlockedSlotsKey)) slots += stack.Attributes.GetInt(UnlockedSlotsKey);

		stack.Attributes.SetInt(UnlockedSlotsKey, slots);
		slot.MarkDirty();
	}

	public override string GetKey() {
		return "unlock";
	}

	public override string GetIconKey() {
		return "unlock";
	}

}