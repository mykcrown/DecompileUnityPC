// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.api;
using System;

namespace strange.extensions.dispatcher.impl
{
	public class DispatcherException : Exception
	{
		public DispatcherExceptionType type
		{
			get;
			set;
		}

		public DispatcherException()
		{
		}

		public DispatcherException(string message, DispatcherExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
