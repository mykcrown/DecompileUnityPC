// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPerformanceTracker : ITickable
{
	bool HasStarted
	{
		get;
	}

	void Start();

	PerformanceTracker.PerformanceReport End();

	void RecordFPS(float displayFPS, float gameFPS);

	void RecordPing(float ping, float serverPing);

	void RecordRollbackDuration(double durationMs);

	void RecordRollbackFrames(int frames);

	void RecordWaiting(double waitingMs);

	void RecordSkippedFrames(int skippedFrames);
}
