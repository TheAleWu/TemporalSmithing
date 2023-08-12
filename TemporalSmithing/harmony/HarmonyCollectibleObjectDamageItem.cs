﻿using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.harmony;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment
internal class HarmonyCollectibleObjectDamageItem {

	public static bool Patch(IWorldAccessor world, Entity byEntity, ItemSlot itemslot, int amount = 1) {
		return RunePowers.Instance.PerformOnSlot(itemslot, true, DamageItemInternal);

		bool DamageItemInternal(RunePowerEntry x, bool val) =>
			x.RunePower.OnDamageItem(val, world, byEntity, itemslot, amount);
	}

}
