// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.extensions.dispatcher.eventdispatcher.api
{
	public interface IEvent
	{
		object type
		{
			get;
			set;
		}

		IEventDispatcher target
		{
			get;
			set;
		}

		object data
		{
			get;
			set;
		}
	}
}
