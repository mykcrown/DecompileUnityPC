// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.injector.api
{
	public interface ICrossContextInjectionBinder : IInjectionBinder, IInstanceProvider
	{
		IInjectionBinder CrossContextBinder
		{
			get;
			set;
		}
	}
}
