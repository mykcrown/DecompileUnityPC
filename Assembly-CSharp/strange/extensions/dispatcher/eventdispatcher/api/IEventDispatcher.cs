using System;
using strange.extensions.dispatcher.api;

namespace strange.extensions.dispatcher.eventdispatcher.api
{
	// Token: 0x02000235 RID: 565
	public interface IEventDispatcher : IDispatcher
	{
		// Token: 0x06000B0A RID: 2826
		IEventBinding Bind(object key);

		// Token: 0x06000B0B RID: 2827
		void AddListener(object evt, EventCallback callback);

		// Token: 0x06000B0C RID: 2828
		void AddListener(object evt, EmptyCallback callback);

		// Token: 0x06000B0D RID: 2829
		void RemoveListener(object evt, EventCallback callback);

		// Token: 0x06000B0E RID: 2830
		void RemoveListener(object evt, EmptyCallback callback);

		// Token: 0x06000B0F RID: 2831
		bool HasListener(object evt, EventCallback callback);

		// Token: 0x06000B10 RID: 2832
		bool HasListener(object evt, EmptyCallback callback);

		// Token: 0x06000B11 RID: 2833
		void UpdateListener(bool toAdd, object evt, EventCallback callback);

		// Token: 0x06000B12 RID: 2834
		void UpdateListener(bool toAdd, object evt, EmptyCallback callback);

		// Token: 0x06000B13 RID: 2835
		void ReleaseEvent(IEvent evt);
	}
}
