using System;

namespace strange.extensions.dispatcher.api
{
	// Token: 0x0200022C RID: 556
	public interface IDispatcher
	{
		// Token: 0x06000AF0 RID: 2800
		void Dispatch(object eventType);

		// Token: 0x06000AF1 RID: 2801
		void Dispatch(object eventType, object data);
	}
}
