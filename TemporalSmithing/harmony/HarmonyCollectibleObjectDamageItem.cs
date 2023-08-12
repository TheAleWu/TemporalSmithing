using temporalsmithing.content.modifier;
using temporalsmithing.content.modifier.events;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.harmony;

internal class HarmonyCollectibleObjectDamageItem {

	public static bool Patch(IWorldAccessor world, Entity byEntity, ItemSlot itemslot, int amount = 1) {
		var runesGrouped = RuneService.Instance.ReadRunesGrouped(itemslot);
		var cancelled = false;
		foreach (var entry in runesGrouped) {
			var @event = new ItemDamagedEvent(entry.Value, world, byEntity, itemslot, amount);
			entry.Key.OnItemDamaged(@event);
			cancelled |= @event.Cancelled;
		}

		return !cancelled;
	}

}
