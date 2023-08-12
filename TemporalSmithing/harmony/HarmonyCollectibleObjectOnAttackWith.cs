using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.harmony;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment
internal class HarmonyCollectibleObjectOnAttackWith {

	public static bool Patch(IWorldAccessor world, Entity byEntity, Entity attackedEntity, ItemSlot itemslot) {
		return RunePowers.Instance.PerformOnSlot(itemslot, true, DamageItemInternal);

		bool DamageItemInternal(RunePowerEntry x, bool val) =>
			x.RunePower.OnAttackingWith(val, world, byEntity, attackedEntity, itemslot, x);
	}

}
