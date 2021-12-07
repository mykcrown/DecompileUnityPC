using System;

// Token: 0x020006D3 RID: 1747
public interface IMatchDeepLogging
{
	// Token: 0x06002BC7 RID: 11207
	void BeginMatch(string matchId, string opponentName);

	// Token: 0x06002BC8 RID: 11208
	void EndMatch();

	// Token: 0x06002BC9 RID: 11209
	void UpdateLoopBegin();

	// Token: 0x06002BCA RID: 11210
	void UpdateLoopEnd();

	// Token: 0x06002BCB RID: 11211
	void RecordTickStart(int frameDelta, int currentFrame);

	// Token: 0x06002BCC RID: 11212
	void RecordTickEnd();

	// Token: 0x06002BCD RID: 11213
	void RecordProcessingEnd();

	// Token: 0x06002BCE RID: 11214
	void RecordRenderFrame();

	// Token: 0x06002BCF RID: 11215
	void RecordRenderWaitError();

	// Token: 0x06002BD0 RID: 11216
	void RecordFrameWaitError();

	// Token: 0x06002BD1 RID: 11217
	void RecordRenderDelay();

	// Token: 0x06002BD2 RID: 11218
	void RecordSyncWait();

	// Token: 0x06002BD3 RID: 11219
	void RecordPreRender();

	// Token: 0x06002BD4 RID: 11220
	void RecordPostRender();

	// Token: 0x06002BD5 RID: 11221
	void RecordRollbackStatus();

	// Token: 0x06002BD6 RID: 11222
	void RecordRemoteInput(int playerID, int startFrame, int numInputs);

	// Token: 0x06002BD7 RID: 11223
	void RecordRollback(int framesTicked, double rollbackDurationMs);

	// Token: 0x06002BD8 RID: 11224
	void RecordBroadcastLocalInputs(int localPlayerID, int playerID, int beginFrame);
}
