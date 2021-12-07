using System;

// Token: 0x02000B6D RID: 2925
public interface ITimeStatTrackerManager : ITickable
{
	// Token: 0x060054A0 RID: 21664
	void InitDebug();

	// Token: 0x17001385 RID: 4997
	// (get) Token: 0x060054A1 RID: 21665
	bool DebugTrackersEnabled { get; }

	// Token: 0x060054A2 RID: 21666
	void Register(TimeStatTracker tracker);

	// Token: 0x060054A3 RID: 21667
	void Unregister(TimeStatTracker tracker);
}
