using System;

// Token: 0x0200088F RID: 2191
public interface IRollbackStatus
{
	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x060036FF RID: 14079
	bool IsRollingBack { get; }

	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x06003700 RID: 14080
	int FullyConfirmedFrame { get; }

	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x06003701 RID: 14081
	bool RollbackEnabled { get; }

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x06003702 RID: 14082
	int InputDelayFrames { get; }

	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x06003703 RID: 14083
	int InputDelayPing { get; }

	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x06003704 RID: 14084
	int InitialTimestepDelta { get; }

	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x06003705 RID: 14085
	int CalculatedLatencyMs { get; }

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x06003706 RID: 14086
	long MsSinceStart { get; }

	// Token: 0x17000D65 RID: 3429
	// (get) Token: 0x06003707 RID: 14087
	int LowestInputAckFrame { get; }
}
