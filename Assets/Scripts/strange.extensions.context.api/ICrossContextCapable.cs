// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.api;
using strange.extensions.injector.api;
using System;

namespace strange.extensions.context.api
{
	public interface ICrossContextCapable
	{
		ICrossContextInjectionBinder injectionBinder
		{
			get;
			set;
		}

		IDispatcher crossContextDispatcher
		{
			get;
			set;
		}

		void AssignCrossContext(ICrossContextCapable childContext);

		void RemoveCrossContext(ICrossContextCapable childContext);

		object GetComponent<T>();

		object GetComponent<T>(object name);
	}
}
