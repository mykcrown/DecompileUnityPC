// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace strange.extensions.reflector.api
{
	public interface IReflectedClass
	{
		ConstructorInfo Constructor
		{
			get;
			set;
		}

		Type[] ConstructorParameters
		{
			get;
			set;
		}

		object[] ConstructorParameterNames
		{
			get;
			set;
		}

		MethodInfo[] PostConstructors
		{
			get;
			set;
		}

		KeyValuePair<Type, PropertyInfo>[] Setters
		{
			get;
			set;
		}

		object[] SetterNames
		{
			get;
			set;
		}

		bool PreGenerated
		{
			get;
			set;
		}

		ConstructorInfo constructor
		{
			get;
			set;
		}

		Type[] constructorParameters
		{
			get;
			set;
		}

		MethodInfo[] postConstructors
		{
			get;
			set;
		}

		KeyValuePair<Type, PropertyInfo>[] setters
		{
			get;
			set;
		}

		object[] setterNames
		{
			get;
			set;
		}

		bool preGenerated
		{
			get;
			set;
		}
	}
}
