// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.framework.api
{
	public interface IBinding
	{
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

		bool isWeak
		{
			get;
		}

		IBinding Bind<T>();

		IBinding Bind(object key);

		IBinding To<T>();

		IBinding To(object o);

		IBinding ToName<T>();

		IBinding ToName(object o);

		IBinding Named<T>();

		IBinding Named(object o);

		void RemoveKey(object o);

		void RemoveValue(object o);

		void RemoveName(object o);

		IBinding Weak();
	}
}
