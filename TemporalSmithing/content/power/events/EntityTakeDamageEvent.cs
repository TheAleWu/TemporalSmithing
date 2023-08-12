using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.content.modifier.events;

public class EntityTakeDamageEvent : RuneDispatchedEvent {

	public readonly Entity entity;
	public readonly DamageSource damageSource;
	public float Damage;

	public EntityTakeDamageEvent(List<AppliedRune> entries, Entity entity, DamageSource damageSource, float damage) : base(entries) {
		this.entity = entity;
		this.damageSource = damageSource;
		Damage = damage;
	}

}
