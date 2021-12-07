// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using System;

namespace strange.extensions.context.impl
{
	public class ContextException : Exception
	{
		public ContextExceptionType type
		{
			get;
			set;
		}

		public ContextException()
		{
		}

		public ContextException(string message, ContextExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
