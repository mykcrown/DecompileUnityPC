// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.extensions.injector.api
{
	public interface IInjectorFactory
	{
		object Get(IInjectionBinding binding);

		object Get(IInjectionBinding binding, object[] args);
	}
}
