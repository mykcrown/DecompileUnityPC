using System;

// Token: 0x0200083E RID: 2110
public interface IDebugReplaySystem
{
	// Token: 0x17000CD6 RID: 3286
	// (get) Token: 0x060034ED RID: 13549
	bool RecordStates { get; }

	// Token: 0x17000CD7 RID: 3287
	// (get) Token: 0x060034EE RID: 13550
	bool RecordHashes { get; }

	// Token: 0x060034EF RID: 13551
	RollbackStateContainer GetStateAtFrameEnd(int frame);

	// Token: 0x060034F0 RID: 13552
	short GetHashAtFrameEnd(int frame);
}
