using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace temporalsmithing.patches;

public class SoloPatch : CodePatch {

	private readonly JsonPatch patch;

	public SoloPatch(JsonPatch patch) {
		this.patch = patch;
	}

	public override void Apply(ModJsonPatchLoader patchLoader, ref int applied, ref int notFound, ref int error) {
		var loc = new AssetLocation(GetType().Name);
		patchLoader.ApplyPatch(0, loc, patch, ref applied, ref notFound, ref error);
	}

}