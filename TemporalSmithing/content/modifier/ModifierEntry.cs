using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.content.modifier;

public class ModifierEntry {

	public ModifierEntry(string entryId, Modifier modifier, IItemStack sourceItem, ITreeAttribute attributes) {
		EntryId = entryId;
		Attributes = attributes;
		SourceItem = sourceItem.Clone();
		Modifier = modifier;
	}

	public string EntryId { get; }
	public Modifier Modifier { get; }
	public ItemStack SourceItem { get; }
	public ITreeAttribute Attributes { get; }

}