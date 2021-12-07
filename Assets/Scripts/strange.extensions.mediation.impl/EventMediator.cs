// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using System;

namespace strange.extensions.mediation.impl
{
	public class EventMediator : Mediator
	{
		[Inject(ContextKeys.CONTEXT_DISPATCHER)]
		public IEventDispatcher dispatcher
		{
			get;
			set;
		}
	}
}
