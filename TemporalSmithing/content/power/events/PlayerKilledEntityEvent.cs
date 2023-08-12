using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.content.modifier.events;

public class PlayerKilledEntityEvent : RuneDispatchedEvent {

	public readonly Entity entity;
	public readonly DamageSource damageSource;

	public PlayerKilledEntityEvent(List<AppliedRune> entries, Entity entity, DamageSource damageSource) :
		base(entries) {
		this.entity = entity;
		this.damageSource = damageSource;
	}

}
