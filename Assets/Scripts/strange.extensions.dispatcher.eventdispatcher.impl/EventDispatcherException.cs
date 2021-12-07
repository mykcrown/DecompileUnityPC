// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.eventdispatcher.api;
using System;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	public class EventDispatcherException : Exception
	{
		public EventDispatcherExceptionType type
		{
			get;
			set;
		}

		public EventDispatcherException()
		{
		}

		public EventDispatcherException(string message, EventDispatcherExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
