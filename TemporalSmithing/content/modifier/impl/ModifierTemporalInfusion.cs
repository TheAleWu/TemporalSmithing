using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace temporalsmithing.content.modifier.impl;

public class ModifierTemporalInfusion : UnlockingModifier {

	public override int GetRequiredHitsToApply() {
		return 10;
	}

}
