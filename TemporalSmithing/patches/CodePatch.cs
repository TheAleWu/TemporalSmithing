using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;
using Vintagestory.ServerMods.NoObf;

namespace temporalsmithing.patches;

/*
 * Code patches are used for big bulk patches to avoid a huge amount of patch files
 */
public abstract class CodePatch {

	public abstract void Apply(ModJsonPatchLoader patchLoader, ref int applied, ref int notFound, ref int error);

	protected static JsonObject EmptyJsonObject() => new(new JObject());

}