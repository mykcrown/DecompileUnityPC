// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.eventdispatcher.api;
using System;

namespace strange.extensions.mediation.impl
{
	public class EventView : View
	{
		[Inject]
		public IEventDispatcher dispatcher
		{
			get;
			set;
		}
	}
}
