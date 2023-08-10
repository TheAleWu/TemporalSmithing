using System;

namespace temporalsmithing.timer;

public class DefaultTimer : Timer {

	private readonly TimeSpan duration;
	private readonly Guid timerId;
	private long endMs;
	private bool alive = true;
	private int extensions = 0;
	public object[] Data { get; }
	public Action<object[]> OnFinish { get; set; } = _ => { };
	public Predicate<DefaultTimer> LifeCondition = x => true;

	internal DefaultTimer(TimeSpan duration, object[] data) {
		timerId = Guid.NewGuid();
		this.duration = duration;
		Data = data;
	}

	internal override void Start() {
		endMs = (long)(DateTimeOffset.Now.ToUnixTimeMilliseconds() + duration.TotalMilliseconds);
	}

	internal override void Test() {
		if (endMs > DateTimeOffset.Now.ToUnixTimeMilliseconds() && LifeCondition.Invoke(this)) {
			return;
		}

		Stop();
	}

	internal override void Stop() {
		OnFinish.Invoke(Data);
		TimerRegistry.RemoveTimer(this);
		alive = false;
	}

	internal override void Extend(TimeSpan span) {
		if (!alive) return;
		endMs += (long)span.TotalMilliseconds;
		extensions++;
	}

	internal override int GetPerformedExtensions() {
		return extensions;
	}

	internal override Guid GetTimerId() {
		return timerId;
	}

	internal override bool IsAlive() {
		return alive;
	}

}