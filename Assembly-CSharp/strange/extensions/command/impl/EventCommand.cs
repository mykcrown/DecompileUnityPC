using System;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace strange.extensions.command.impl
{
	// Token: 0x0200021B RID: 539
	public class EventCommand : Command
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x000525AD File Offset: 0x000509AD
		// (set) Token: 0x06000A7B RID: 2683 RVA: 0x000525B5 File Offset: 0x000509B5
		[Inject(ContextKeys.CONTEXT_DISPATCHER)]
		public IEventDispatcher dispatcher { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x000525BE File Offset: 0x000509BE
		// (set) Token: 0x06000A7D RID: 2685 RVA: 0x000525C6 File Offset: 0x000509C6
		[Inject]
		public IEvent evt { get; set; }

		// Token: 0x06000A7E RID: 2686 RVA: 0x000525CF File Offset: 0x000509CF
		public override void Retain()
		{
			base.Retain();
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x000525D7 File Offset: 0x000509D7
		public override void Release()
		{
			base.Release();
		}
	}
}
