using System;
using System.Collections.Generic;

// Token: 0x02000877 RID: 2167
public interface IRollbackInputStatus
{
	// Token: 0x17000D27 RID: 3367
	// (get) Token: 0x06003623 RID: 13859
	Dictionary<int, int> PlayerInputAckStatus { get; }

	// Token: 0x17000D28 RID: 3368
	// (get) Token: 0x06003624 RID: 13860
	Dictionary<int, int> PlayerFrameReceived { get; }

	// Token: 0x17000D29 RID: 3369
	// (get) Token: 0x06003625 RID: 13861
	int FrameWithAllInputs { get; }

	// Token: 0x17000D2A RID: 3370
	// (get) Token: 0x06003626 RID: 13862
	int LowestInputAckFrame { get; }

	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x06003627 RID: 13863
	int CalculatedLatencyMs { get; }
}
