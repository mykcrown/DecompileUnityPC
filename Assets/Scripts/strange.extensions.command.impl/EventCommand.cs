// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using System;

namespace strange.extensions.command.impl
{
	public class EventCommand : Command
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

		public override void Retain()
		{
			base.Retain();
		}

		public override void Release()
		{
			base.Release();
		}
	}
}
