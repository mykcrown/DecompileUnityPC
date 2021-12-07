using System;
using System.Collections.Generic;
using strange.extensions.injector.api;
using strange.extensions.reflector.impl;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.injector.impl
{
	// Token: 0x0200024C RID: 588
	public class InjectionBinder : Binder, IInjectionBinder, IInstanceProvider
	{
		// Token: 0x06000BB1 RID: 2993 RVA: 0x000542F4 File Offset: 0x000526F4
		public InjectionBinder()
		{
			this.injector = new Injector();
			this.injector.binder = this;
			this.injector.reflector = new ReflectionBinder();
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00054323 File Offset: 0x00052723
		public object GetInstance(Type key)
		{
			return this.GetInstance(key, null);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00054330 File Offset: 0x00052730
		public virtual object GetInstance(Type key, object name)
		{
			IInjectionBinding binding = this.GetBinding(key, name);
			if (binding == null)
			{
				throw new InjectionException(string.Concat(new object[]
				{
					"InjectionBinder has no binding for:\n\tkey: ",
					key,
					"\nname: ",
					name
				}), InjectionExceptionType.NULL_BINDING);
			}
			return this.GetInjectorForBinding(binding).Instantiate(binding);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00054385 File Offset: 0x00052785
		protected virtual IInjector GetInjectorForBinding(IInjectionBinding binding)
		{
			return this.injector;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00054390 File Offset: 0x00052790
		public T GetInstance<T>()
		{
			object instance = this.GetInstance(typeof(T));
			return (T)((object)instance);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x000543B8 File Offset: 0x000527B8
		public T GetInstance<T>(object name)
		{
			object instance = this.GetInstance(typeof(T), name);
			return (T)((object)instance);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000543DF File Offset: 0x000527DF
		public override IBinding GetRawBinding()
		{
			return new InjectionBinding(new Binder.BindingResolver(this.resolver));
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x000543F3 File Offset: 0x000527F3
		// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x000543FB File Offset: 0x000527FB
		public IInjector injector
		{
			get
			{
				return this._injector;
			}
			set
			{
				if (this._injector != null)
				{
					this._injector.binder = null;
				}
				this._injector = value;
				this._injector.binder = this;
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00054427 File Offset: 0x00052827
		public new IInjectionBinding Bind<T>()
		{
			return base.Bind<T>() as IInjectionBinding;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00054434 File Offset: 0x00052834
		public IInjectionBinding Bind(Type key)
		{
			return base.Bind(key) as IInjectionBinding;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00054442 File Offset: 0x00052842
		public new virtual IInjectionBinding GetBinding<T>()
		{
			return base.GetBinding<T>() as IInjectionBinding;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0005444F File Offset: 0x0005284F
		public new virtual IInjectionBinding GetBinding<T>(object name)
		{
			return base.GetBinding<T>(name) as IInjectionBinding;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0005445D File Offset: 0x0005285D
		public new virtual IInjectionBinding GetBinding(object key)
		{
			return base.GetBinding(key) as IInjectionBinding;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0005446B File Offset: 0x0005286B
		public new virtual IInjectionBinding GetBinding(object key, object name)
		{
			return base.GetBinding(key, name) as IInjectionBinding;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0005447C File Offset: 0x0005287C
		public int ReflectAll()
		{
			List<Type> list = new List<Type>();
			foreach (KeyValuePair<object, Dictionary<object, IBinding>> keyValuePair in this.bindings)
			{
				Dictionary<object, IBinding> value = keyValuePair.Value;
				foreach (KeyValuePair<object, IBinding> keyValuePair2 in value)
				{
					IBinding value2 = keyValuePair2.Value;
					Type item = (!(value2.value is Type)) ? value2.value.GetType() : ((Type)value2.value);
					if (list.IndexOf(item) == -1)
					{
						list.Add(item);
					}
				}
			}
			return this.Reflect(list);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00054578 File Offset: 0x00052978
		public int Reflect(List<Type> list)
		{
			int num = 0;
			foreach (Type type in list)
			{
				if (!type.IsPrimitive && !(type == typeof(decimal)) && !(type == typeof(string)))
				{
					num++;
					this.injector.reflector.Get(type);
				}
			}
			return num;
		}

		// Token: 0x04000771 RID: 1905
		private IInjector _injector;
	}
}
