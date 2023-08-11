using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace temporalsmithing.content.modifier.impl;

public class ModifierYield : Modifier {

	public ModifierYield() {
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

	public override float GetItemRetrievalChanceOnRemoval() {
		return 0;
	}

	public override float OnAttackedWith(Entity entity, DamageSource damageSource, float damage, ModifierEntry currentEntry) {
		var player = damageSource.SourceEntity as EntityPlayer;
		if (player is not null) {
			
		}
		return base.OnAttackedWith(entity, damageSource, damage, currentEntry);
	}

	public override void OnKillEntityWith(Entity entity, DamageSource damagesource, ModifierEntry currentEntry) {
		var currentlyApplied = entity.Attributes.GetInt("razorSharpCount");
		var modifierToAdd = 0.2f / (float) Math.Pow(2, currentlyApplied);
		
		var behavior = entity.SidedProperties.Behaviors.FirstOrDefault(x => x is EntityBehaviorHarvestable)
			as EntityBehaviorHarvestable;
		var field = typeof(EntityBehaviorHarvestable).GetField("jsonDrops",
			BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
		if (field?.GetValue(behavior) is not BlockDropItemStack[] array) return;
		
		foreach (var entry in array) {
			var quantity = entry.Quantity;
			quantity.avg += modifierToAdd;
		}
		
		entity.Attributes.SetInt("razorSharpCount", ++currentlyApplied);
	}

}
