// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.mediation.api;
using System;

namespace strange.extensions.mediation.impl
{
	public class MediationException : Exception
	{
		public MediationExceptionType type
		{
			get;
			set;
		}

		public MediationException()
		{
		}

		public MediationException(string message, MediationExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
