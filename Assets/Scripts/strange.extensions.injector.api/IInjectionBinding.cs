// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.injector.api
{
	public interface IInjectionBinding : IBinding
	{
		bool isCrossContext
		{
			get;
		}

		bool toInject
		{
			get;
		}

		InjectionBindingType type
		{
			get;
			set;
		}

		object key
		{
			get;
		}

		object name
		{
			get;
		}

		object value
		{
			get;
		}

		Enum keyConstraint
		{
			get;
			set;
		}

		Enum valueConstraint
		{
			get;
			set;
		}

		IInjectionBinding ToSingleton();

		IInjectionBinding ToValue(object o);

		IInjectionBinding SetValue(object o);

		IInjectionBinding CrossContext();

		IInjectionBinding ToInject(bool value);

		IInjectionBinding Bind<T>();

		IInjectionBinding Bind(object key);

		IInjectionBinding To<T>();

		IInjectionBinding To(object o);

		IInjectionBinding ToName<T>();

		IInjectionBinding ToName(object o);

		IInjectionBinding Named<T>();

		IInjectionBinding Named(object o);
	}
}
