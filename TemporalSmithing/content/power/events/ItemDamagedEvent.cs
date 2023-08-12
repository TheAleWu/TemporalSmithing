using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.content.modifier.events;

public class ItemDamagedEvent : CancelableRuneDispatchedEvent {

	public readonly IWorldAccessor world;
	public readonly Entity byEntity;
	public readonly ItemSlot itemSlot;
	public readonly int amount;

	public ItemDamagedEvent(List<AppliedRune> entries, IWorldAccessor world, Entity byEntity, ItemSlot itemSlot,
							int amount = 1) : base(entries) {
		this.world = world;
		this.byEntity = byEntity;
		this.itemSlot = itemSlot;
		this.amount = amount;
	}

}
