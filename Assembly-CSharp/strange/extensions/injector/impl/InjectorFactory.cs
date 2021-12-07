using System;
using strange.extensions.injector.api;

namespace strange.extensions.injector.impl
{
	// Token: 0x02000250 RID: 592
	public class InjectorFactory : IInjectorFactory
	{
		// Token: 0x06000BF0 RID: 3056 RVA: 0x000550C4 File Offset: 0x000534C4
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

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0005511B File Offset: 0x0005351B
		public object Get(IInjectionBinding binding)
		{
			return this.Get(binding, null);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00055128 File Offset: 0x00053528
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

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000551A8 File Offset: 0x000535A8
		protected object generateImplicit(object key, object[] args)
		{
			Type type = key as Type;
			if (!type.IsInterface && !type.IsAbstract)
			{
				return this.createFromValue(key, args);
			}
			throw new InjectionException("InjectorFactory can't instantiate an Interface or Abstract Class. Class: " + key.ToString(), InjectionExceptionType.NOT_INSTANTIABLE);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000551F1 File Offset: 0x000535F1
		protected object valueOf(IInjectionBinding binding)
		{
			return binding.value;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000551FC File Offset: 0x000535FC
		protected object instanceOf(IInjectionBinding binding, object[] args)
		{
			if (binding.value != null)
			{
				return this.createFromValue(binding.value, args);
			}
			object o = this.generateImplicit((binding.key as object[])[0], args);
			return this.createFromValue(o, args);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00055240 File Offset: 0x00053640
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
