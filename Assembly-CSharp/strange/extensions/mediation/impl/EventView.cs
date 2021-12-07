using System;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace strange.extensions.mediation.impl
{
	// Token: 0x0200025D RID: 605
	public class EventView : View
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00055503 File Offset: 0x00053903
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x0005550B File Offset: 0x0005390B
		[Inject]
		public IEventDispatcher dispatcher { get; set; }
	}
}
