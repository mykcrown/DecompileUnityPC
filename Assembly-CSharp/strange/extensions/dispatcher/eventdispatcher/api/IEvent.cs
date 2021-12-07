using System;

namespace strange.extensions.dispatcher.eventdispatcher.api
{
	// Token: 0x02000231 RID: 561
	public interface IEvent
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000AF7 RID: 2807
		// (set) Token: 0x06000AF8 RID: 2808
		object type { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000AF9 RID: 2809
		// (set) Token: 0x06000AFA RID: 2810
		IEventDispatcher target { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000AFB RID: 2811
		// (set) Token: 0x06000AFC RID: 2812
		object data { get; set; }
	}
}
