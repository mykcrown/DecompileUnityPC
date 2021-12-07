using System;

// Token: 0x020004DE RID: 1246
public interface IMoveSkipAheadComponent
{
	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x06001B55 RID: 6997
	bool ShouldSkipToFrame { get; }

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x06001B56 RID: 6998
	int SkipToFrame { get; }

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06001B57 RID: 6999
	bool ShouldSkipToMove { get; }

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06001B58 RID: 7000
	MoveData SkipToMove { get; }
}
