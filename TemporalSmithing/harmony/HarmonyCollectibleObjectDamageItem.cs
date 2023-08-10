using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.harmony;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment
internal class HarmonyCollectibleObjectDamageItem {

	public static bool Patch(IWorldAccessor world, Entity byEntity, ItemSlot itemslot, int amount = 1) {
		var slots = Modifiers.GetModifiableSlots(byEntity);
		var continueCode = true;

		bool DamageItemInternal(ModifierEntry x, bool val) =>
			x.Modifier.OnDamageItem(val, world, byEntity, itemslot, amount);

		continueCode = Modifiers.Instance.PerformOnSlots(slots, true, DamageItemInternal);
		return continueCode;
	}

}