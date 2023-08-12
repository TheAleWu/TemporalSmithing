using System.Drawing;
using System.Linq;
using System.Reflection;
using temporalsmithing.content.modifier.events;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace temporalsmithing.content.modifier.impl;

public class RuneYield : Rune {

	public RuneYield() {
		OnModificationFinish += (api, modified, modifier) => ApplyOnItem(api, modified, modifier);
		OnModifierRemoved += RemoveFromItem;
	}

	public override string GetKey() {
		return "rune-of-yield";
	}

	public override string GetIconKey() {
		return "clover";
	}

	public override Color GetIconColor() {
		return Color.FromArgb(74, 170, 0);
	}

	public override void OnKillEntityWith(PlayerKilledEntityEvent @event) {
		var modifierToAdd = 0.2f;
		var totalAvgAdd = 1.0f;
		for (var i = 0; i < @event.entries.Count; i++) {
			totalAvgAdd += modifierToAdd;
			modifierToAdd /= 2;
		}

		var behavior = @event.entity.SidedProperties.Behaviors.FirstOrDefault(x => x is EntityBehaviorHarvestable)
			as EntityBehaviorHarvestable;
		var field = typeof(EntityBehaviorHarvestable).GetField("jsonDrops",
			BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
		if (field?.GetValue(behavior) is not BlockDropItemStack[] array) return;

		foreach (var entry in array) {
			var quantity = entry.Quantity;
			quantity.avg *= totalAvgAdd;
			quantity.var *= totalAvgAdd;
		}
	}

}
