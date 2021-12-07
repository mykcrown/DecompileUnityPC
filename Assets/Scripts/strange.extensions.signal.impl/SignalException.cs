// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.signal.api;
using System;

namespace strange.extensions.signal.impl
{
	public class SignalException : Exception
	{
		public SignalExceptionType type
		{
			get;
			set;
		}

		public SignalException()
		{
		}

		public SignalException(string message, SignalExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
