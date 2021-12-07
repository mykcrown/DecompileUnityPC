using System;

// Token: 0x02000520 RID: 1312
public interface IMoveDelegate
{
	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x06001C66 RID: 7270
	MoveData Data { get; }

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x06001C67 RID: 7271
	MoveModel Model { get; }

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x06001C68 RID: 7272
	int TotalFrames { get; }
}
