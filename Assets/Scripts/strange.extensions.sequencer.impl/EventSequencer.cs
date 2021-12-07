// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.sequencer.api;
using System;

namespace strange.extensions.sequencer.impl
{
	public class EventSequencer : Sequencer
	{
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
