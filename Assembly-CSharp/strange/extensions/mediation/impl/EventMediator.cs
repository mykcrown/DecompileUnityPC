using System;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace strange.extensions.mediation.impl
{
	// Token: 0x0200025C RID: 604
	public class EventMediator : Mediator
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00055353 File Offset: 0x00053753
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x0005535B File Offset: 0x0005375B
		[Inject(ContextKeys.CONTEXT_DISPATCHER)]
		public IEventDispatcher dispatcher { get; set; }
	}
}
