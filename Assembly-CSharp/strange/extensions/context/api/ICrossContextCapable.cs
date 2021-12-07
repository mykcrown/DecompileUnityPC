using System;
using strange.extensions.dispatcher.api;
using strange.extensions.injector.api;

namespace strange.extensions.context.api
{
	// Token: 0x02000224 RID: 548
	public interface ICrossContextCapable
	{
		// Token: 0x06000A95 RID: 2709
		void AssignCrossContext(ICrossContextCapable childContext);

		// Token: 0x06000A96 RID: 2710
		void RemoveCrossContext(ICrossContextCapable childContext);

		// Token: 0x06000A97 RID: 2711
		object GetComponent<T>();

		// Token: 0x06000A98 RID: 2712
		object GetComponent<T>(object name);

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000A99 RID: 2713
		// (set) Token: 0x06000A9A RID: 2714
		ICrossContextInjectionBinder injectionBinder { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000A9B RID: 2715
		// (set) Token: 0x06000A9C RID: 2716
		IDispatcher crossContextDispatcher { get; set; }
	}
}
