// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using strange.extensions.reflector.impl;
using strange.framework.api;
using strange.framework.impl;
using System;
using System.Collections.Generic;

namespace strange.extensions.injector.impl
{
	public class InjectionBinder : Binder, IInjectionBinder, IInstanceProvider
	{
		private IInjector _injector;

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

		public InjectionBinder()
		{
			this.injector = new Injector();
			this.injector.binder = this;
			this.injector.reflector = new ReflectionBinder();
		}

		public object GetInstance(Type key)
		{
			return this.GetInstance(key, null);
		}

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

		protected virtual IInjector GetInjectorForBinding(IInjectionBinding binding)
		{
			return this.injector;
		}

		public T GetInstance<T>()
		{
			object instance = this.GetInstance(typeof(T));
			return (T)((object)instance);
		}

		public T GetInstance<T>(object name)
		{
			object instance = this.GetInstance(typeof(T), name);
			return (T)((object)instance);
		}

		public override IBinding GetRawBinding()
		{
			return new InjectionBinding(new Binder.BindingResolver(this.resolver));
		}

		public new IInjectionBinding Bind<T>()
		{
			return base.Bind<T>() as IInjectionBinding;
		}

		public IInjectionBinding Bind(Type key)
		{
			return base.Bind(key) as IInjectionBinding;
		}

		public new virtual IInjectionBinding GetBinding<T>()
		{
			return base.GetBinding<T>() as IInjectionBinding;
		}

		public new virtual IInjectionBinding GetBinding<T>(object name)
		{
			return base.GetBinding<T>(name) as IInjectionBinding;
		}

		public new virtual IInjectionBinding GetBinding(object key)
		{
			return base.GetBinding(key) as IInjectionBinding;
		}

		public new virtual IInjectionBinding GetBinding(object key, object name)
		{
			return base.GetBinding(key, name) as IInjectionBinding;
		}

		public int ReflectAll()
		{
			List<Type> list = new List<Type>();
			foreach (KeyValuePair<object, Dictionary<object, IBinding>> current in this.bindings)
			{
				Dictionary<object, IBinding> value = current.Value;
				foreach (KeyValuePair<object, IBinding> current2 in value)
				{
					IBinding value2 = current2.Value;
					Type item = (!(value2.value is Type)) ? value2.value.GetType() : ((Type)value2.value);
					if (list.IndexOf(item) == -1)
					{
						list.Add(item);
					}
				}
			}
			return this.Reflect(list);
		}

		public int Reflect(List<Type> list)
		{
			int num = 0;
			foreach (Type current in list)
			{
				if (!current.IsPrimitive && !(current == typeof(decimal)) && !(current == typeof(string)))
				{
					num++;
					this.injector.reflector.Get(current);
				}
			}
			return num;
		}
	}
}
