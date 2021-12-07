// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using System;

namespace strange.extensions.injector.impl
{
	public class InjectionException : Exception
	{
		public InjectionExceptionType type
		{
			get;
			set;
		}

		public InjectionException()
		{
		}

		public InjectionException(string message, InjectionExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
