using System;
using strange.framework.api;

namespace strange.extensions.dispatcher.eventdispatcher.api
{
	// Token: 0x02000234 RID: 564
	public interface IEventBinding : IBinding
	{
		// Token: 0x06000B05 RID: 2821
		EventCallbackType TypeForCallback(EventCallback callback);

		// Token: 0x06000B06 RID: 2822
		EventCallbackType TypeForCallback(EmptyCallback callback);

		// Token: 0x06000B07 RID: 2823
		IEventBinding Bind(object key);

		// Token: 0x06000B08 RID: 2824
		IEventBinding To(EventCallback callback);

		// Token: 0x06000B09 RID: 2825
		IEventBinding To(EmptyCallback callback);
	}
}
