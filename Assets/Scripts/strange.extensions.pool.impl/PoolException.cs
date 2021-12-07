// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.pool.api;
using System;

namespace strange.extensions.pool.impl
{
	public class PoolException : Exception
	{
		public PoolExceptionType type
		{
			get;
			set;
		}

		public PoolException()
		{
		}

		public PoolException(string message, PoolExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
