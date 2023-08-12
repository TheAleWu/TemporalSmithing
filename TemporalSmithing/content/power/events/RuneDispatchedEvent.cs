using System.Collections.Generic;

namespace temporalsmithing.content.modifier.events;

public abstract class RuneDispatchedEvent {

	public readonly List<AppliedRune> entries;

	protected RuneDispatchedEvent(List<AppliedRune> entries) {
		this.entries = entries;
	}

}
