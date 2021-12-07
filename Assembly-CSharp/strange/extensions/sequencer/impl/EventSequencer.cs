using System;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.sequencer.api;

namespace strange.extensions.sequencer.impl
{
	// Token: 0x02000278 RID: 632
	public class EventSequencer : Sequencer
	{
		// Token: 0x06000CDD RID: 3293 RVA: 0x00056688 File Offset: 0x00054A88
		protected override ISequenceCommand createCommand(object cmd, object data)
		{
			base.injectionBinder.Bind<ISequenceCommand>().To(cmd);
			if (data is IEvent)
			{
				base.injectionBinder.Bind<IEvent>().ToValue(data).ToInject(false);
			}
			ISequenceCommand instance = base.injectionBinder.GetInstance<ISequenceCommand>();
			instance.data = data;
			if (data is IEvent)
			{
				base.injectionBinder.Unbind<IEvent>();
			}
			base.injectionBinder.Unbind<ISequenceCommand>();
			return instance;
		}
	}
}
