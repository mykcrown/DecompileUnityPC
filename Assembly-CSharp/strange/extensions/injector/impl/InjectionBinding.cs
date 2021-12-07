using System;
using strange.extensions.injector.api;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.injector.impl
{
	// Token: 0x0200024D RID: 589
	public class InjectionBinding : Binding, IInjectionBinding, IBinding
	{
		// Token: 0x06000BC2 RID: 3010 RVA: 0x00054733 File Offset: 0x00052B33
		public InjectionBinding(Binder.BindingResolver resolver)
		{
			this.resolver = resolver;
			base.keyConstraint = BindingConstraintType.MANY;
			base.valueConstraint = BindingConstraintType.ONE;
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x00054761 File Offset: 0x00052B61
		// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x00054769 File Offset: 0x00052B69
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

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x00054772 File Offset: 0x00052B72
		public bool toInject
		{
			get
			{
				return this._toInject;
			}
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0005477A File Offset: 0x00052B7A
		public IInjectionBinding ToInject(bool value)
		{
			this._toInject = value;
			return this;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x00054784 File Offset: 0x00052B84
		public bool isCrossContext
		{
			get
			{
				return this._isCrossContext;
			}
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0005478C File Offset: 0x00052B8C
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

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000547BB File Offset: 0x00052BBB
		public IInjectionBinding ToValue(object o)
		{
			this.type = InjectionBindingType.VALUE;
			this.SetValue(o);
			return this;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000547D0 File Offset: 0x00052BD0
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

		// Token: 0x06000BCB RID: 3019 RVA: 0x0005487B File Offset: 0x00052C7B
		protected bool HasGenericAssignableFrom(Type keyType, Type objType)
		{
			return keyType.IsGenericType;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0005488C File Offset: 0x00052C8C
		protected bool IsGenericTypeAssignable(Type givenType, Type genericType)
		{
			Type[] interfaces = givenType.GetInterfaces();
			foreach (Type type in interfaces)
			{
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

		// Token: 0x06000BCD RID: 3021 RVA: 0x00054916 File Offset: 0x00052D16
		public IInjectionBinding CrossContext()
		{
			this._isCrossContext = true;
			if (this.resolver != null)
			{
				this.resolver(this);
			}
			return this;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00054937 File Offset: 0x00052D37
		public new IInjectionBinding Bind<T>()
		{
			return base.Bind<T>() as IInjectionBinding;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00054944 File Offset: 0x00052D44
		public new IInjectionBinding Bind(object key)
		{
			return base.Bind(key) as IInjectionBinding;
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00054952 File Offset: 0x00052D52
		public new IInjectionBinding To<T>()
		{
			return base.To<T>() as IInjectionBinding;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0005495F File Offset: 0x00052D5F
		public new IInjectionBinding To(object o)
		{
			return base.To(o) as IInjectionBinding;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0005496D File Offset: 0x00052D6D
		public new IInjectionBinding ToName<T>()
		{
			return base.ToName<T>() as IInjectionBinding;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0005497A File Offset: 0x00052D7A
		public new IInjectionBinding ToName(object o)
		{
			return base.ToName(o) as IInjectionBinding;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00054988 File Offset: 0x00052D88
		public new IInjectionBinding Named<T>()
		{
			return base.Named<T>() as IInjectionBinding;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00054995 File Offset: 0x00052D95
		public new IInjectionBinding Named(object o)
		{
			return base.Named(o) as IInjectionBinding;
		}

		// Token: 0x04000772 RID: 1906
		private InjectionBindingType _type;

		// Token: 0x04000773 RID: 1907
		private bool _toInject = true;

		// Token: 0x04000774 RID: 1908
		private bool _isCrossContext;
	}
}
