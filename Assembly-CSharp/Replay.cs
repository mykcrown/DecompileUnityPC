using System;
using System.Collections.Generic;

// Token: 0x02000842 RID: 2114
[Serializable]
public class Replay
{
	// Token: 0x17000CE3 RID: 3299
	// (get) Token: 0x0600350E RID: 13582 RVA: 0x000F946B File Offset: 0x000F786B
	public bool IsValid
	{
		get
		{
			return this.startPayload != null && this.savedStates != null && this.recordedInputs != null;
		}
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x000F9494 File Offset: 0x000F7894
	public void Clear()
	{
		this.savedHashes.Clear();
		this.savedStates.Clear();
		this.recordedInputs.Clear();
		this.forfeitFrame = 0;
		this.startPayload = null;
		this.remoteInputsByFrame.Clear();
		this.localPlayers.Clear();
	}

	// Token: 0x0400247B RID: 9339
	private static int DEFAULT_CAPACITY = 30600;

	// Token: 0x0400247C RID: 9340
	public DateTime timestamp;

	// Token: 0x0400247D RID: 9341
	public ReplayType replayType;

	// Token: 0x0400247E RID: 9342
	public GameLoadPayload startPayload;

	// Token: 0x0400247F RID: 9343
	public int forfeitFrame;

	// Token: 0x04002480 RID: 9344
	public List<RollbackStateContainer> savedStates = new List<RollbackStateContainer>();

	// Token: 0x04002481 RID: 9345
	public List<short> savedHashes = new List<short>(Replay.DEFAULT_CAPACITY);

	// Token: 0x04002482 RID: 9346
	public List<RollbackInput[]> recordedInputs = new List<RollbackInput[]>();

	// Token: 0x04002483 RID: 9347
	public Dictionary<int, List<RollbackInput>> remoteInputsByFrame = new Dictionary<int, List<RollbackInput>>();

	// Token: 0x04002484 RID: 9348
	public List<PlayerNum> localPlayers = new List<PlayerNum>();
}
