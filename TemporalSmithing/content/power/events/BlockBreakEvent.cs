using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace temporalsmithing.content.modifier.events;

public class BlockBreakEvent : RuneDispatchedEvent {

	public readonly IServerPlayer byPlayer;
	public readonly BlockSelection blockSel;
	public float DropQuantityMultiplier;
	public EnumHandling Handling;

	public BlockBreakEvent(List<AppliedRune> entries, IServerPlayer byPlayer, BlockSelection blockSel,
						   float dropQuantityMultiplier, EnumHandling handling) : base(entries) {
		this.byPlayer = byPlayer;
		this.blockSel = blockSel;
		DropQuantityMultiplier = dropQuantityMultiplier;
		Handling = handling;
	}

}
