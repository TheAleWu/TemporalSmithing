using System;
using System.Collections.Generic;

namespace temporalsmithing.timer;

public class TimerRegistry {

	private static readonly object[] NoData = Array.Empty<object>();
	private static readonly List<Timer> registry = new();

	private TimerRegistry() { }

	public static DefaultTimer StartDefaultTimer(TimeSpan duration, object[] data = null) {
		var timer = new DefaultTimer(duration, data);
		registry.Add(timer);
		timer.Start();
		return timer;
	}

	public static ProccingTimer StartProccingTimer(TimeSpan duration, TimeSpan interval, object[] data = null) {
		var timer = new ProccingTimer(duration, interval, data);
		registry.Add(timer);
		timer.Start();
		return timer;
	}

	protected internal static void RemoveTimer(Timer timer) {
		registry.RemoveAll(x => x.GetTimerId() == timer.GetTimerId());
	}

	public static void DoTick(float elapsed) {
		foreach (var timer in new List<Timer>(registry)) timer.Test();
	}

}