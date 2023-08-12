using System;
using System.Drawing;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.content.modifier.impl; 

public class RunePowerHardening : RunePower {

	public RunePowerHardening() {
		OnModificationFinish += (api, modified, modifier) => ApplyOnItem(api, modified, modifier);
		OnModifierRemoved += RemoveFromItem;
	}

	public override string GetKey() {
		return "rune-of-hardening";
	}

	public override string GetIconKey() {
		return "goldbar";
	}

	public override Color GetIconColor() {
		return Color.FromArgb(150, 150, 80);
	}

	public override int GetMaxOfModifier() {
		return 5;
	}

	public override bool OnDamageItem(bool continueCode, IWorldAccessor world, Entity byEntity, ItemSlot itemslot, int amount = 1) {
		if (!continueCode) return false;
		var apply = new Random().NextDouble() <= 0.1;
		apply = !apply;
		return apply;
	}

}
