using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.harmony;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment
internal class HarmonyCollectibleObjectOnAttackWith {

	public static bool Patch(IWorldAccessor world, Entity byEntity, Entity attackedEntity, ItemSlot itemslot) {
		var slots = Modifiers.GetModifiableSlots(byEntity);
		var continueCode = true;

		bool OnAttackingWithInternal(ModifierEntry x, bool val) =>
			x.Modifier.OnAttackingWith(val, world, byEntity, attackedEntity, itemslot, x);

		continueCode = Modifiers.Instance.PerformOnSlots(slots, true, OnAttackingWithInternal);
		return continueCode;
	}

}