using temporalsmithing.content.modifier;
using temporalsmithing.content.modifier.events;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.harmony;

internal class HarmonyCollectibleObjectOnAttackWith {

	public static bool Patch(IWorldAccessor world, Entity byEntity, Entity attackedEntity, ItemSlot itemslot) {
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(itemslot);
		var cancelled = false;
		foreach (var entry in runesGrouped) {
			var @event = new PlayerAttackingEntityEvent(entry.Value, world, byEntity,
				attackedEntity, itemslot);
			entry.Key.OnPlayerAttackingEntity(@event);
			cancelled |= @event.Cancelled;
		}

		return !cancelled;
	}

}
