// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.reflector.api;
using System;

namespace strange.extensions.injector.api
{
	public interface IInjector
	{
		IInjectorFactory factory
		{
			get;
			set;
		}

		IInjectionBinder binder
		{
			get;
			set;
		}

		IReflectionBinder reflector
		{
			get;
			set;
		}

		object Instantiate(IInjectionBinding binding);

		object Inject(object target);

		object Inject(object target, bool attemptConstructorInjection);

		void Uninject(object target);
	}
}
