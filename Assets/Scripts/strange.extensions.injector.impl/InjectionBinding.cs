// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using strange.framework.api;
using strange.framework.impl;
using System;

namespace strange.extensions.injector.impl
{
	public class InjectionBinding : Binding, IInjectionBinding, IBinding
	{
		private InjectionBindingType _type;

		private bool _toInject = true;

		private bool _isCrossContext;

		public InjectionBindingType type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		public bool toInject
		{
			get
			{
				return this._toInject;
			}
		}

		public bool isCrossContext
		{
			get
			{
				return this._isCrossContext;
			}
		}

		public InjectionBinding(Binder.BindingResolver resolver)
		{
			this.resolver = resolver;
			base.keyConstraint = BindingConstraintType.MANY;
			base.valueConstraint = BindingConstraintType.ONE;
		}

		public IInjectionBinding ToInject(bool value)
		{
			this._toInject = value;
			return this;
		}

		public IInjectionBinding ToSingleton()
		{
			if (this.type == InjectionBindingType.VALUE)
			{
				return this;
			}
			this.type = InjectionBindingType.SINGLETON;
			if (this.resolver != null)
			{
				this.resolver(this);
			}
			return this;
		}

		public IInjectionBinding ToValue(object o)
		{
			this.type = InjectionBindingType.VALUE;
			this.SetValue(o);
			return this;
		}

		public IInjectionBinding SetValue(object o)
		{
			Type type = o.GetType();
			object[] array = base.key as object[];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				object obj = array[i];
				Type type2 = (!(obj is Type)) ? obj.GetType() : (obj as Type);
				if (!type2.IsAssignableFrom(type) && !this.HasGenericAssignableFrom(type2, type))
				{
					throw new InjectionException(string.Concat(new object[]
					{
						"Injection cannot bind a value that does not extend or implement the binding type. ",
						type2,
						" ",
						type
					}), InjectionExceptionType.ILLEGAL_BINDING_VALUE);
				}
			}
			this.To(o);
			return this;
		}

		protected bool HasGenericAssignableFrom(Type keyType, Type objType)
		{
			return keyType.IsGenericType;
		}

		protected bool IsGenericTypeAssignable(Type givenType, Type genericType)
		{
			Type[] interfaces = givenType.GetInterfaces();
			Type[] array = interfaces;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i];
				if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
			}
			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}
			Type baseType = givenType.BaseType;
			return !(baseType == null) && this.IsGenericTypeAssignable(baseType, genericType);
		}

		public IInjectionBinding CrossContext()
		{
			this._isCrossContext = true;
			if (this.resolver != null)
			{
				this.resolver(this);
			}
			return this;
		}

		public new IInjectionBinding Bind<T>()
		{
			return base.Bind<T>() as IInjectionBinding;
		}

		public new IInjectionBinding Bind(object key)
		{
			return base.Bind(key) as IInjectionBinding;
		}

		public new IInjectionBinding To<T>()
		{
			return base.To<T>() as IInjectionBinding;
		}

		public new IInjectionBinding To(object o)
		{
			return base.To(o) as IInjectionBinding;
		}

		public new IInjectionBinding ToName<T>()
		{
			return base.ToName<T>() as IInjectionBinding;
		}

		public new IInjectionBinding ToName(object o)
		{
			return base.ToName(o) as IInjectionBinding;
		}

		public new IInjectionBinding Named<T>()
		{
			return base.Named<T>() as IInjectionBinding;
		}

		public new IInjectionBinding Named(object o)
		{
			return base.Named(o) as IInjectionBinding;
		}
	}
}
