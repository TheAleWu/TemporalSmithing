using System;

namespace temporalsmithing.timer;

public class ProccingTimer : DefaultTimer {

	private readonly TimeSpan interval;
	private long lastProc;
	public Action<object[]> OnInvocation { get; set; } = _ => { };

	protected internal ProccingTimer(TimeSpan duration, TimeSpan interval, object[] data) : base(duration, data) {
		this.interval = interval;
		UpdateLastProc();
	}

	internal override void Test() {
		if (lastProc <= DateTimeOffset.Now.ToUnixTimeMilliseconds()) {
			UpdateLastProc();
			OnInvocation.Invoke(Data);
		}

		base.Test();
	}

	private void UpdateLastProc() {
		lastProc = (long)(DateTimeOffset.Now.ToUnixTimeMilliseconds() + interval.TotalMilliseconds);
	}

}