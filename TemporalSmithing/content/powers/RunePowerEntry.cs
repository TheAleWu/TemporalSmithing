using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.content.modifier;

public class RunePowerEntry {

	public RunePowerEntry(string entryId, RunePower runePower, IItemStack sourceItem, ITreeAttribute attributes) {
		EntryId = entryId;
		Attributes = attributes;
		SourceItem = sourceItem.Clone();
		RunePower = runePower;
	}

	public string EntryId { get; }
	public RunePower RunePower { get; }
	public ItemStack SourceItem { get; }
	public ITreeAttribute Attributes { get; }

}
