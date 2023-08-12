using System.Collections.Generic;
using Vintagestory.API.Common;

namespace temporalsmithing.content.modifier.events;

public class InteractionWithStepEvent : CancelableRuneDispatchedEvent {

	public readonly ItemSlot slot;
	public readonly EntityAgent byEntity;
	public readonly BlockSelection blockSel;
	public readonly EntitySelection entitySel;
	public EnumHandling Handling;

	public InteractionWithStepEvent(List<AppliedRune> entries, ItemSlot slot, EntityAgent byEntity,
									BlockSelection blockSel, EntitySelection entitySel,
									EnumHandling handling) : base(entries) {
		this.slot = slot;
		this.byEntity = byEntity;
		this.blockSel = blockSel;
		this.entitySel = entitySel;
		Handling = handling;
	}

}
