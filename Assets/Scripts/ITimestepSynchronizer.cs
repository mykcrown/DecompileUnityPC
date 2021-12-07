// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ITimestepSynchronizer
{
	int CompensationOffsetMs
	{
		get;
	}

	bool SyncronizationComplete
	{
		get;
	}

	bool AllRemoteClientsSyncronized
	{
		get;
	}

	bool ShouldSend
	{
		get;
	}

	void Reset();

	void RecordTimestepSyncEvent(int clientID, long remoteInputMs, int remoteLatencyMs, bool syncronizationComplete);

	int MessageLatencyMsTo(int clientId);

	void OnSend();
}
