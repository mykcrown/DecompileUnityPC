// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.extensions.mediation.api
{
	public interface IView
	{
		bool requiresContext
		{
			get;
			set;
		}

		bool registeredWithContext
		{
			get;
			set;
		}

		bool autoRegisterWithContext
		{
			get;
		}
	}
}
