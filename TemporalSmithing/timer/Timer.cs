using System;

namespace temporalsmithing.timer;

public abstract class Timer {

	internal abstract void Start();

	internal abstract void Test();

	internal abstract void Stop();

	internal abstract void Extend(TimeSpan span);

	internal abstract int GetPerformedExtensions();

	internal abstract Guid GetTimerId();

	internal abstract bool IsAlive();

}