using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace temporalsmithing.patches;

public class BulkPatch : CodePatch {

	private readonly JsonPatch[] patch;

	public BulkPatch(JsonPatch[] patch) {
		this.patch = patch;
	}

	public BulkPatch(IEnumerable<AssetLocation> locations, JsonPatch templatePatch) {
		var patches = locations.Select(assetLocation => new JsonPatch {
			Condition = templatePatch.Condition,
			Op = templatePatch.Op,
			File = assetLocation.Clone(),
			Path = templatePatch.Path,
			Side = templatePatch.Side,
			Value = templatePatch.Value,
			DependsOn = templatePatch.DependsOn,
			FromPath = templatePatch.FromPath
		}).ToArray();

		patch = patches;
	}

	public override void Apply(ModJsonPatchLoader patchLoader, ref int applied, ref int notFound, ref int error) {
		var loc = new AssetLocation(GetType().Name);
		foreach (var jsonPatch in patch) {
			if (jsonPatch is null) continue;
			patchLoader.ApplyPatch(0, loc, jsonPatch, ref applied, ref notFound, ref error);
		}
	}

}