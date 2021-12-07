using System;
using System.Collections.Generic;

// Token: 0x0200087A RID: 2170
public interface IRollbackLayer
{
	// Token: 0x17000D45 RID: 3397
	// (get) Token: 0x06003680 RID: 13952
	bool HasStarted { get; }

	// Token: 0x17000D46 RID: 3398
	// (get) Token: 0x06003681 RID: 13953
	bool HasDesynced { get; }

	// Token: 0x17000D47 RID: 3399
	// (get) Token: 0x06003682 RID: 13954
	ITimekeeper Timekeeper { get; }

	// Token: 0x06003683 RID: 13955
	void StartSession(int localport, string remoteip, int remoteport);

	// Token: 0x06003684 RID: 13956
	bool Idle(int timeout, out bool clientTicked, int maxFrames);

	// Token: 0x06003685 RID: 13957
	void SaveLocalInputs(int frame, List<RollbackInput> localInputs, bool broadcast);

	// Token: 0x06003686 RID: 13958
	void TickGameQuit();

	// Token: 0x06003687 RID: 13959
	void TickNotFrame();

	// Token: 0x06003688 RID: 13960
	void ValidFrame();

	// Token: 0x06003689 RID: 13961
	void FillSkippedLocalInput(int frame, RollbackInput input);

	// Token: 0x0600368A RID: 13962
	void CheckMissingInputs();

	// Token: 0x0600368B RID: 13963
	void SyncInputForFrame(ref RollbackInput[] frameInputs);

	// Token: 0x0600368C RID: 13964
	bool OnFrameAdvanced();

	// Token: 0x0600368D RID: 13965
	void ResetToFrame(int frame);

	// Token: 0x0600368E RID: 13966
	void ForceRollback(int toFrame);

	// Token: 0x0600368F RID: 13967
	void Destroy();
}
