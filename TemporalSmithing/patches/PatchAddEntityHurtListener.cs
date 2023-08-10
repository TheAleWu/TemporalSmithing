using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.ServerMods.NoObf;

namespace temporalsmithing.patches;

public class PatchAddEntityHurtListener : BulkPatch {

	public PatchAddEntityHurtListener(ICoreAPI api) : base(FileLocations(api), TemplatePatch()) { }

	private static IEnumerable<AssetLocation> FileLocations(ICoreAPI api) {
		var dict = api.Assets.GetMany<JObject>(api.Logger, "entities/");
		return dict.Select(pair => pair.Key).ToArray();
	}

	private static JsonPatch TemplatePatch() {
		return new JsonPatch {
			Side = EnumAppSide.Server,
			Op = EnumJsonPatchOp.Add,
			Path = "/server/behaviors/-",
			Value = JsonObject.FromJson("{\"code\": \"entityhurtlistener\"}")
		};
	}

}