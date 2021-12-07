// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ITimeStatTrackerManager : ITickable
{
	bool DebugTrackersEnabled
	{
		get;
	}

	void InitDebug();

	void Register(TimeStatTracker tracker);

	void Unregister(TimeStatTracker tracker);
}
