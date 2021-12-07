// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.framework.impl
{
	public class BinderException : Exception
	{
		public BinderExceptionType type
		{
			get;
			set;
		}

		public BinderException()
		{
		}

		public BinderException(string message, BinderExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
