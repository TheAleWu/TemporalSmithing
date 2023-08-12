using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.behavior.collectible;

public class RuneBehavior : CollectibleBehavior {

	public RuneBehavior(CollectibleObject collObj) : base(collObj) { }

	public string ModifierKey { get; private set; }
	public string[] AdditionalData { get; private set; }

	public override void Initialize(JsonObject properties) {
		ModifierKey = properties["key"].AsString();
		AdditionalData = properties["data"].AsArray(Array.Empty<string>());
	}

}
