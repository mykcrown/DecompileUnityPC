using System;
using strange.framework.api;

namespace strange.extensions.command.api
{
	// Token: 0x02000215 RID: 533
	public interface ICommandBinding : IBinding
	{
		// Token: 0x06000A1D RID: 2589
		ICommandBinding Once();

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000A1E RID: 2590
		// (set) Token: 0x06000A1F RID: 2591
		bool isOneOff { get; set; }

		// Token: 0x06000A20 RID: 2592
		ICommandBinding InParallel();

		// Token: 0x06000A21 RID: 2593
		ICommandBinding InSequence();

		// Token: 0x06000A22 RID: 2594
		ICommandBinding Pooled();

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000A23 RID: 2595
		// (set) Token: 0x06000A24 RID: 2596
		bool isSequence { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000A25 RID: 2597
		// (set) Token: 0x06000A26 RID: 2598
		bool isPooled { get; set; }

		// Token: 0x06000A27 RID: 2599
		ICommandBinding Bind<T>();

		// Token: 0x06000A28 RID: 2600
		ICommandBinding Bind(object key);

		// Token: 0x06000A29 RID: 2601
		ICommandBinding To<T>();

		// Token: 0x06000A2A RID: 2602
		ICommandBinding To(object o);

		// Token: 0x06000A2B RID: 2603
		ICommandBinding ToName<T>();

		// Token: 0x06000A2C RID: 2604
		ICommandBinding ToName(object o);

		// Token: 0x06000A2D RID: 2605
		ICommandBinding Named<T>();

		// Token: 0x06000A2E RID: 2606
		ICommandBinding Named(object o);
	}
}
