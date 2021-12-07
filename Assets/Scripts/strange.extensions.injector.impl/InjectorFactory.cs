// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using System;

namespace strange.extensions.injector.impl
{
	public class InjectorFactory : IInjectorFactory
	{
		public object Get(IInjectionBinding binding, object[] args)
		{
			if (binding == null)
			{
				throw new InjectionException("InjectorFactory cannot act on null binding", InjectionExceptionType.NULL_BINDING);
			}
			InjectionBindingType type = binding.type;
			if (type == InjectionBindingType.SINGLETON)
			{
				return this.singletonOf(binding, args);
			}
			if (type != InjectionBindingType.VALUE)
			{
				return this.instanceOf(binding, args);
			}
			return this.valueOf(binding);
		}

		public object Get(IInjectionBinding binding)
		{
			return this.Get(binding, null);
		}

		protected object singletonOf(IInjectionBinding binding, object[] args)
		{
			if (binding.value != null)
			{
				if (binding.value.GetType().IsInstanceOfType(typeof(Type)))
				{
					object obj = this.createFromValue(binding.value, args);
					if (obj == null)
					{
						return null;
					}
					binding.SetValue(obj);
				}
			}
			else
			{
				binding.SetValue(this.generateImplicit((binding.key as object[])[0], args));
			}
			return binding.value;
		}

		protected object generateImplicit(object key, object[] args)
		{
			Type type = key as Type;
			if (!type.IsInterface && !type.IsAbstract)
			{
				return this.createFromValue(key, args);
			}
			throw new InjectionException("InjectorFactory can't instantiate an Interface or Abstract Class. Class: " + key.ToString(), InjectionExceptionType.NOT_INSTANTIABLE);
		}

		protected object valueOf(IInjectionBinding binding)
		{
			return binding.value;
		}

		protected object instanceOf(IInjectionBinding binding, object[] args)
		{
			if (binding.value != null)
			{
				return this.createFromValue(binding.value, args);
			}
			object o = this.generateImplicit((binding.key as object[])[0], args);
			return this.createFromValue(o, args);
		}

		protected object createFromValue(object o, object[] args)
		{
			Type type = (!(o is Type)) ? o.GetType() : (o as Type);
			object result = null;
			try
			{
				if (args == null || args.Length == 0)
				{
					result = Activator.CreateInstance(type);
				}
				else
				{
					result = Activator.CreateInstance(type, args);
				}
			}
			catch
			{
			}
			return result;
		}
	}
}
