// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.reflector.api;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace strange.extensions.reflector.impl
{
	public class ReflectedClass : IReflectedClass
	{
		public ConstructorInfo Constructor
		{
			get;
			set;
		}

		public Type[] ConstructorParameters
		{
			get;
			set;
		}

		public object[] ConstructorParameterNames
		{
			get;
			set;
		}

		public MethodInfo[] PostConstructors
		{
			get;
			set;
		}

		public KeyValuePair<Type, PropertyInfo>[] Setters
		{
			get;
			set;
		}

		public object[] SetterNames
		{
			get;
			set;
		}

		public bool PreGenerated
		{
			get;
			set;
		}

		public ConstructorInfo constructor
		{
			get
			{
				return this.Constructor;
			}
			set
			{
				this.Constructor = value;
			}
		}

		public Type[] constructorParameters
		{
			get
			{
				return this.ConstructorParameters;
			}
			set
			{
				this.ConstructorParameters = value;
			}
		}

		public MethodInfo[] postConstructors
		{
			get
			{
				return this.PostConstructors;
			}
			set
			{
				this.PostConstructors = value;
			}
		}

		public KeyValuePair<Type, PropertyInfo>[] setters
		{
			get
			{
				return this.Setters;
			}
			set
			{
				this.Setters = value;
			}
		}

		public object[] setterNames
		{
			get
			{
				return this.SetterNames;
			}
			set
			{
				this.SetterNames = value;
			}
		}

		public bool preGenerated
		{
			get
			{
				return this.PreGenerated;
			}
			set
			{
				this.PreGenerated = value;
			}
		}
	}
}
