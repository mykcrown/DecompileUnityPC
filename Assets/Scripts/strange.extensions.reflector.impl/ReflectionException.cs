// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.reflector.api;
using System;

namespace strange.extensions.reflector.impl
{
	public class ReflectionException : Exception
	{
		public ReflectionExceptionType type
		{
			get;
			set;
		}

		public ReflectionException()
		{
		}

		public ReflectionException(string message, ReflectionExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
