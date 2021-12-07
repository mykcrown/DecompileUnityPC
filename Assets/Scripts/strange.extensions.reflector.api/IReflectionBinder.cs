// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.extensions.reflector.api
{
	public interface IReflectionBinder
	{
		IReflectedClass Get(Type type);

		IReflectedClass Get<T>();
	}
}
