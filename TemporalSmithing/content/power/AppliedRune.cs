using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.content.modifier;

public class AppliedRune {

	public AppliedRune(string entryId, Rune rune, IItemStack sourceItem, ITreeAttribute attributes) {
		EntryId = entryId;
		Attributes = attributes;
		SourceItem = sourceItem.Clone();
		Rune = rune;
	}

	public string EntryId { get; }
	public Rune Rune { get; }
	public ItemStack SourceItem { get; }
	public ITreeAttribute Attributes { get; }

}
