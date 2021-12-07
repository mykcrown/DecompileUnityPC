// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using System;

namespace strange.extensions.sequencer.impl
{
	public class EventSequenceCommand : SequenceCommand
	{
		[Inject(ContextKeys.CONTEXT_DISPATCHER)]
		public IEventDispatcher dispatcher
		{
			get;
			set;
		}

		[Inject]
		public IEvent evt
		{
			get;
			set;
		}
	}
}
