using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.content.modifier.events;

public class PlayerAttackingEntityEvent : CancelableRuneDispatchedEvent {

	public readonly IWorldAccessor world;
	public readonly Entity byEntity;
	public readonly Entity attackedEntity;
	public readonly ItemSlot itemSlot;

	public PlayerAttackingEntityEvent(List<AppliedRune> entries, IWorldAccessor world, Entity byEntity,
									  Entity attackedEntity,
									  ItemSlot itemSlot)
		: base(entries) {
		this.world = world;
		this.byEntity = byEntity;
		this.attackedEntity = attackedEntity;
		this.itemSlot = itemSlot;
	}

}
