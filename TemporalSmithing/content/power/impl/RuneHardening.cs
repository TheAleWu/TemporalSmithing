using System;
using System.Drawing;
using temporalsmithing.content.modifier.events;

namespace temporalsmithing.content.modifier.impl;

public class RuneHardening : Rune {

	public RuneHardening() {
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

	public override void OnItemDamaged(ItemDamagedEvent @event) {
		if (@event.Cancelled) return;
		var random = new Random().NextDouble();
		var threshold = 0.1 * @event.Entries.Count;
		@event.Cancelled = random <= threshold;
	}

}
