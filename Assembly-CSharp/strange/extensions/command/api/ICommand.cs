using System;

namespace strange.extensions.command.api
{
	// Token: 0x02000213 RID: 531
	public interface ICommand
	{
		// Token: 0x06000A08 RID: 2568
		void Execute();

		// Token: 0x06000A09 RID: 2569
		void Retain();

		// Token: 0x06000A0A RID: 2570
		void Release();

		// Token: 0x06000A0B RID: 2571
		void Fail();

		// Token: 0x06000A0C RID: 2572
		void Cancel();

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000A0D RID: 2573
		// (set) Token: 0x06000A0E RID: 2574
		bool IsClean { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000A0F RID: 2575
		bool retain { get; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000A10 RID: 2576
		// (set) Token: 0x06000A11 RID: 2577
		bool cancelled { get; set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000A12 RID: 2578
		// (set) Token: 0x06000A13 RID: 2579
		object data { get; set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000A14 RID: 2580
		// (set) Token: 0x06000A15 RID: 2581
		int sequenceId { get; set; }
	}
}
