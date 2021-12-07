// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;
using System.Collections.Generic;

namespace strange.extensions.injector.api
{
	public interface IInjectionBinder : IInstanceProvider
	{
		IInjector injector
		{
			get;
			set;
		}

		object GetInstance(Type key, object name);

		T GetInstance<T>(object name);

		int Reflect(List<Type> list);

		int ReflectAll();

		void ResolveBinding(IBinding binding, object key);

		IInjectionBinding Bind<T>();

		IInjectionBinding Bind(Type key);

		IBinding Bind(object key);

		IInjectionBinding GetBinding<T>();

		IInjectionBinding GetBinding<T>(object name);

		IInjectionBinding GetBinding(object key);

		IInjectionBinding GetBinding(object key, object name);

		void Unbind<T>();

		void Unbind<T>(object name);

		void Unbind(object key);

		void Unbind(object key, object name);

		void Unbind(IBinding binding);
	}
}
