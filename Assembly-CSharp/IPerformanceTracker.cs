using System;

// Token: 0x02000B3E RID: 2878
public interface IPerformanceTracker : ITickable
{
	// Token: 0x0600539D RID: 21405
	void Start();

	// Token: 0x0600539E RID: 21406
	PerformanceTracker.PerformanceReport End();

	// Token: 0x17001366 RID: 4966
	// (get) Token: 0x0600539F RID: 21407
	bool HasStarted { get; }

	// Token: 0x060053A0 RID: 21408
	void RecordFPS(float displayFPS, float gameFPS);

	// Token: 0x060053A1 RID: 21409
	void RecordPing(float ping, float serverPing);

	// Token: 0x060053A2 RID: 21410
	void RecordRollbackDuration(double durationMs);

	// Token: 0x060053A3 RID: 21411
	void RecordRollbackFrames(int frames);

	// Token: 0x060053A4 RID: 21412
	void RecordWaiting(double waitingMs);

	// Token: 0x060053A5 RID: 21413
	void RecordSkippedFrames(int skippedFrames);
}
