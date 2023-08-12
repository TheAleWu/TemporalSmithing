using System.Collections.Generic;
using Vintagestory.API.Common;

namespace temporalsmithing.content.modifier.events;

public class InteractionWithStartEvent : RuneDispatchedEvent {

	public readonly ItemSlot slot;
	public readonly EntityAgent byEntity;
	public readonly BlockSelection blockSel;
	public readonly EntitySelection entitySel;
	public readonly bool firstEvent;
	public EnumHandHandling HandHandling;
	public EnumHandling Handling;

	public InteractionWithStartEvent(List<AppliedRune> entries, ItemSlot slot, EntityAgent byEntity,
									 BlockSelection blockSel, EntitySelection entitySel, bool firstEvent,
									 EnumHandHandling handHandling, EnumHandling handling) : base(entries) {
		this.slot = slot;
		this.byEntity = byEntity;
		this.blockSel = blockSel;
		this.entitySel = entitySel;
		this.firstEvent = firstEvent;
		HandHandling = handHandling;
		Handling = handling;
	}

}
