using System;

namespace strange.extensions.dispatcher.api
{
	// Token: 0x0200022D RID: 557
	public interface ITriggerable
	{
		// Token: 0x06000AF2 RID: 2802
		bool Trigger<T>(object data);

		// Token: 0x06000AF3 RID: 2803
		bool Trigger(object key, object data);
	}
}
