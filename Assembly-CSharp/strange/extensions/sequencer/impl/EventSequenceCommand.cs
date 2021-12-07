using System;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace strange.extensions.sequencer.impl
{
	// Token: 0x02000277 RID: 631
	public class EventSequenceCommand : SequenceCommand
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00056488 File Offset: 0x00054888
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x00056490 File Offset: 0x00054890
		[Inject(ContextKeys.CONTEXT_DISPATCHER)]
		public IEventDispatcher dispatcher { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00056499 File Offset: 0x00054899
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x000564A1 File Offset: 0x000548A1
		[Inject]
		public IEvent evt { get; set; }
	}
}
