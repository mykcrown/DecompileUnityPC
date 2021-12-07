using System;

// Token: 0x020007A1 RID: 1953
public interface ITimestepSynchronizer
{
	// Token: 0x0600301E RID: 12318
	void Reset();

	// Token: 0x0600301F RID: 12319
	void RecordTimestepSyncEvent(int clientID, long remoteInputMs, int remoteLatencyMs, bool syncronizationComplete);

	// Token: 0x17000B8C RID: 2956
	// (get) Token: 0x06003020 RID: 12320
	int CompensationOffsetMs { get; }

	// Token: 0x06003021 RID: 12321
	int MessageLatencyMsTo(int clientId);

	// Token: 0x17000B8D RID: 2957
	// (get) Token: 0x06003022 RID: 12322
	bool SyncronizationComplete { get; }

	// Token: 0x17000B8E RID: 2958
	// (get) Token: 0x06003023 RID: 12323
	bool AllRemoteClientsSyncronized { get; }

	// Token: 0x17000B8F RID: 2959
	// (get) Token: 0x06003024 RID: 12324
	bool ShouldSend { get; }

	// Token: 0x06003025 RID: 12325
	void OnSend();
}
