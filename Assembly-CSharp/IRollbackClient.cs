using System;
using System.Collections.Generic;

// Token: 0x0200086E RID: 2158
public interface IRollbackClient : IRollbackStateOwner
{
	// Token: 0x17000D13 RID: 3347
	// (get) Token: 0x060035D8 RID: 13784
	bool IsNetworkGame { get; }

	// Token: 0x17000D14 RID: 3348
	// (get) Token: 0x060035D9 RID: 13785
	int Frame { get; }

	// Token: 0x17000D15 RID: 3349
	// (get) Token: 0x060035DA RID: 13786
	int GameStartInputFrame { get; }

	// Token: 0x060035DB RID: 13787
	void ReportWaiting(double waitDuration);

	// Token: 0x060035DC RID: 13788
	void ReportHealth(NetworkHealthReport health);

	// Token: 0x060035DD RID: 13789
	void ReportErrors(List<string> errors);

	// Token: 0x060035DE RID: 13790
	void Halt();

	// Token: 0x060035DF RID: 13791
	void TickInput(int frame, bool isSkippedFrame);

	// Token: 0x060035E0 RID: 13792
	void TickFrame();

	// Token: 0x060035E1 RID: 13793
	void TickUpdate();

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x060035E2 RID: 13794
	bool IsGameComplete { get; }

	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x060035E3 RID: 13795
	// (set) Token: 0x060035E4 RID: 13796
	bool IsRollingBack { get; set; }
}
