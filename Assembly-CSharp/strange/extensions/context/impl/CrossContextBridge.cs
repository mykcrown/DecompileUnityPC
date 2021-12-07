using System;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.context.impl
{
	// Token: 0x02000229 RID: 553
	public class CrossContextBridge : Binder, ITriggerable
	{
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x00052F14 File Offset: 0x00051314
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x00052F1C File Offset: 0x0005131C
		[Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
		public IEventDispatcher crossContextDispatcher { get; set; }

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00052F28 File Offset: 0x00051328
		public override IBinding Bind(object key)
		{
			IBinding rawBinding = this.GetRawBinding();
			rawBinding.Bind(key);
			this.resolver(rawBinding);
			return rawBinding;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00052F4C File Offset: 0x0005134C
		public bool Trigger<T>(object data)
		{
			return this.Trigger(typeof(T), data);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00052F60 File Offset: 0x00051360
		public bool Trigger(object key, object data)
		{
			IBinding binding = this.GetBinding(key, null);
			if (binding != null && !this.eventsInProgress.Contains(key))
			{
				this.eventsInProgress.Add(key);
				this.crossContextDispatcher.Dispatch(key, data);
				this.eventsInProgress.Remove(key);
			}
			return true;
		}

		// Token: 0x0400072C RID: 1836
		protected HashSet<object> eventsInProgress = new HashSet<object>();
	}
}
