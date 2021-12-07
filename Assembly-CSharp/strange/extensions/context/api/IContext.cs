using System;
using strange.framework.api;

namespace strange.extensions.context.api
{
	// Token: 0x02000222 RID: 546
	public interface IContext : IBinder
	{
		// Token: 0x06000A8C RID: 2700
		IContext Start();

		// Token: 0x06000A8D RID: 2701
		void Launch();

		// Token: 0x06000A8E RID: 2702
		IContext AddContext(IContext context);

		// Token: 0x06000A8F RID: 2703
		IContext RemoveContext(IContext context);

		// Token: 0x06000A90 RID: 2704
		void AddView(object view);

		// Token: 0x06000A91 RID: 2705
		void RemoveView(object view);

		// Token: 0x06000A92 RID: 2706
		object GetContextView();
	}
}
