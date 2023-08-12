using System.Collections.Generic;

namespace temporalsmithing.content.modifier.events;

public abstract class CancelableRuneDispatchedEvent {

	public readonly List<AppliedRune> Entries;
	public bool Cancelled;

	protected CancelableRuneDispatchedEvent(List<AppliedRune> entries) {
		Entries = entries;
		Cancelled = false;
	}

}
