using System;

namespace strange.extensions.dispatcher.api
{
	// Token: 0x0200022E RID: 558
	public interface ITriggerProvider
	{
		// Token: 0x06000AF4 RID: 2804
		void AddTriggerable(ITriggerable target);

		// Token: 0x06000AF5 RID: 2805
		void RemoveTriggerable(ITriggerable target);

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000AF6 RID: 2806
		int Triggerables { get; }
	}
}
