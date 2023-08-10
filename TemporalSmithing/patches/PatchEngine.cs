using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace temporalsmithing.patches;

public class PatchEngine {

	private readonly ICoreAPI api;
	private readonly ModJsonPatchLoader patchLoader;
	private int applied;
	private int notFound;
	private int error;

	public int Applied => applied;
	public int NotFound => notFound;
	public int Error => error;

	public PatchEngine(ICoreAPI api) {
		this.api = api;
		patchLoader = api.ModLoader.GetModSystem<ModJsonPatchLoader>();

		var patchLoaderType = typeof(ModJsonPatchLoader);
		var apiField = patchLoaderType.GetField("api",
			BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
		apiField?.SetValue(patchLoader, api);
	}

	public void ApplyPatch(CodePatch patch) {
		api.Logger.Audit($"Trying to apply patch {patch}");
		patch.Apply(patchLoader, ref applied, ref notFound, ref error);
	}

}