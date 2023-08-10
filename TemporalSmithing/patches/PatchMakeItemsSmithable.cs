using temporalsmithing.util;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace temporalsmithing.patches;

public class PatchMakeSwordsSmithable : BulkPatch {

	public PatchMakeSwordsSmithable() : base(new[] {
		SwordsPatch()
	}) { }

	private static JsonPatch SwordsPatch() {
		var obj = EmptyJsonObject()
		   .Append("name", "modifiable")
		   .Append("properties", EmptyJsonObject()
			   .Append("possibleModifiers", new object[] {
					"rune-of-ripping", "rune-of-yield", "rune-of-hardening"
				})
			);
		return new JsonPatch {
			File = new AssetLocation("game:itemtypes/tool/blade"),
			Op = EnumJsonPatchOp.Add,
			Path = "/behaviors/-",
			Value = obj
		};
	}

}