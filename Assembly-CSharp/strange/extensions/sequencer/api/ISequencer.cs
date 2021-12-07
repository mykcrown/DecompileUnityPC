using System;
using strange.extensions.command.api;
using strange.framework.api;

namespace strange.extensions.sequencer.api
{
	// Token: 0x02000275 RID: 629
	public interface ISequencer : ICommandBinder, IBinder
	{
		// Token: 0x06000CD4 RID: 3284
		void ReleaseCommand(ISequenceCommand command);

		// Token: 0x06000CD5 RID: 3285
		ISequenceBinding Bind<T>();

		// Token: 0x06000CD6 RID: 3286
		ISequenceBinding Bind(object value);
	}
}
