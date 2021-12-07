using System;
using strange.framework.api;

namespace strange.extensions.command.api
{
	// Token: 0x02000214 RID: 532
	public interface ICommandBinder : IBinder
	{
		// Token: 0x06000A16 RID: 2582
		void ReactTo(object trigger);

		// Token: 0x06000A17 RID: 2583
		void ReactTo(object trigger, object data);

		// Token: 0x06000A18 RID: 2584
		void ReleaseCommand(ICommand command);

		// Token: 0x06000A19 RID: 2585
		void Stop(object key);

		// Token: 0x06000A1A RID: 2586
		ICommandBinding Bind<T>();

		// Token: 0x06000A1B RID: 2587
		ICommandBinding Bind(object value);

		// Token: 0x06000A1C RID: 2588
		ICommandBinding GetBinding<T>();
	}
}
