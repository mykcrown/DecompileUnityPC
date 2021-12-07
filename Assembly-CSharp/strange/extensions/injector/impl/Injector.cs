using System;
using System.Collections.Generic;
using System.Reflection;
using strange.extensions.injector.api;
using strange.extensions.reflector.api;

namespace strange.extensions.injector.impl
{
	// Token: 0x0200024F RID: 591
	public class Injector : IInjector
	{
		// Token: 0x06000BDA RID: 3034 RVA: 0x000549CC File Offset: 0x00052DCC
		public Injector()
		{
			this.factory = new InjectorFactory();
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x000549DF File Offset: 0x00052DDF
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x000549E7 File Offset: 0x00052DE7
		public IInjectorFactory factory { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x000549F0 File Offset: 0x00052DF0
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x000549F8 File Offset: 0x00052DF8
		public IInjectionBinder binder { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x00054A01 File Offset: 0x00052E01
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x00054A09 File Offset: 0x00052E09
		public IReflectionBinder reflector { get; set; }

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00054A14 File Offset: 0x00052E14
		public object Instantiate(IInjectionBinding binding)
		{
			this.failIf(this.binder == null, "Attempt to instantiate from Injector without a Binder", InjectionExceptionType.NO_BINDER);
			this.failIf(this.factory == null, "Attempt to inject into Injector without a Factory", InjectionExceptionType.NO_FACTORY);
			this.armorAgainstInfiniteLoops(binding);
			object obj = null;
			Type type = null;
			if (binding.value is Type)
			{
				type = (binding.value as Type);
			}
			else if (binding.value == null)
			{
				object[] array = binding.key as object[];
				type = (array[0] as Type);
				if (type.IsPrimitive || type == typeof(decimal) || type == typeof(string))
				{
					obj = binding.value;
				}
			}
			else
			{
				obj = binding.value;
			}
			if (obj == null)
			{
				IReflectedClass reflectedClass = this.reflector.Get(type);
				Type[] constructorParameters = reflectedClass.constructorParameters;
				object[] constructorParameterNames = reflectedClass.ConstructorParameterNames;
				int num = constructorParameters.Length;
				object[] array2 = new object[num];
				for (int i = 0; i < num; i++)
				{
					array2[i] = this.getValueInjection(constructorParameters[i], constructorParameterNames[i], null);
				}
				obj = this.factory.Get(binding, array2);
				if (obj != null)
				{
					if (binding.toInject)
					{
						obj = this.Inject(obj, false);
					}
					if (binding.type == InjectionBindingType.SINGLETON || binding.type == InjectionBindingType.VALUE)
					{
						binding.ToInject(false);
					}
				}
			}
			this.infinityLock = null;
			return obj;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00054B8F File Offset: 0x00052F8F
		public object Inject(object target)
		{
			return this.Inject(target, true);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00054B9C File Offset: 0x00052F9C
		public object Inject(object target, bool attemptConstructorInjection)
		{
			this.failIf(this.binder == null, "Attempt to inject into Injector without a Binder", InjectionExceptionType.NO_BINDER);
			this.failIf(this.reflector == null, "Attempt to inject without a reflector", InjectionExceptionType.NO_REFLECTOR);
			this.failIf(target == null, "Attempt to inject into null instance", InjectionExceptionType.NULL_TARGET);
			Type type = target.GetType();
			if (type.IsPrimitive || type == typeof(decimal) || type == typeof(string))
			{
				return target;
			}
			IReflectedClass reflection = this.reflector.Get(type);
			if (attemptConstructorInjection)
			{
				target = this.performConstructorInjection(target, reflection);
			}
			this.performSetterInjection(target, reflection);
			this.postInject(target, reflection);
			return target;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00054C50 File Offset: 0x00053050
		public void Uninject(object target)
		{
			this.failIf(this.binder == null, "Attempt to inject into Injector without a Binder", InjectionExceptionType.NO_BINDER);
			this.failIf(this.reflector == null, "Attempt to inject without a reflector", InjectionExceptionType.NO_REFLECTOR);
			this.failIf(target == null, "Attempt to inject into null instance", InjectionExceptionType.NULL_TARGET);
			Type type = target.GetType();
			if (type.IsPrimitive || type == typeof(decimal) || type == typeof(string))
			{
				return;
			}
			IReflectedClass reflection = this.reflector.Get(type);
			this.performUninjection(target, reflection);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00054CEC File Offset: 0x000530EC
		private object performConstructorInjection(object target, IReflectedClass reflection)
		{
			this.failIf(target == null, "Attempt to perform constructor injection into a null object", InjectionExceptionType.NULL_TARGET);
			this.failIf(reflection == null, "Attempt to perform constructor injection without a reflection", InjectionExceptionType.NULL_REFLECTION);
			ConstructorInfo constructor = reflection.constructor;
			this.failIf(constructor == null, "Attempt to construction inject a null constructor", InjectionExceptionType.NULL_CONSTRUCTOR);
			Type[] constructorParameters = reflection.constructorParameters;
			object[] constructorParameterNames = reflection.ConstructorParameterNames;
			object[] array = new object[constructorParameters.Length];
			int num = 0;
			foreach (Type t in constructorParameters)
			{
				array[num] = this.getValueInjection(t, constructorParameterNames[num], target);
				num++;
			}
			if (array.Length == 0)
			{
				return target;
			}
			object obj = constructor.Invoke(array);
			return (obj != null) ? obj : target;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00054DAC File Offset: 0x000531AC
		private void performSetterInjection(object target, IReflectedClass reflection)
		{
			this.failIf(target == null, "Attempt to inject into a null object", InjectionExceptionType.NULL_TARGET);
			this.failIf(reflection == null, "Attempt to inject without a reflection", InjectionExceptionType.NULL_REFLECTION);
			this.failIf(reflection.setters.Length != reflection.setterNames.Length, "Attempt to perform setter injection with mismatched names.\nThere must be exactly as many names as setters.", InjectionExceptionType.SETTER_NAME_MISMATCH);
			int num = reflection.setters.Length;
			for (int i = 0; i < num; i++)
			{
				KeyValuePair<Type, PropertyInfo> keyValuePair = reflection.setters[i];
				object valueInjection = this.getValueInjection(keyValuePair.Key, reflection.setterNames[i], target);
				this.injectValueIntoPoint(valueInjection, target, keyValuePair.Value);
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00054E50 File Offset: 0x00053250
		private object getValueInjection(Type t, object name, object target)
		{
			IInjectionBinding binding = this.binder.GetBinding(t, name);
			this.failIf(binding == null, "Attempt to Instantiate a null binding.", InjectionExceptionType.NULL_BINDING, t, name, target);
			if (binding.type == InjectionBindingType.VALUE)
			{
				if (!binding.toInject)
				{
					return binding.value;
				}
				object result = this.Inject(binding.value, false);
				binding.ToInject(false);
				return result;
			}
			else
			{
				if (binding.type == InjectionBindingType.SINGLETON)
				{
					if (binding.value is Type || binding.value == null)
					{
						this.Instantiate(binding);
					}
					return binding.value;
				}
				return this.Instantiate(binding);
			}
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00054EF1 File Offset: 0x000532F1
		private void injectValueIntoPoint(object value, object target, PropertyInfo point)
		{
			this.failIf(target == null, "Attempt to inject into a null target", InjectionExceptionType.NULL_TARGET);
			this.failIf(point == null, "Attempt to inject into a null point", InjectionExceptionType.NULL_INJECTION_POINT);
			this.failIf(value == null, "Attempt to inject null into a target object", InjectionExceptionType.NULL_VALUE_INJECTION);
			point.SetValue(target, value, null);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00054F34 File Offset: 0x00053334
		private void postInject(object target, IReflectedClass reflection)
		{
			this.failIf(target == null, "Attempt to PostConstruct a null target", InjectionExceptionType.NULL_TARGET);
			this.failIf(reflection == null, "Attempt to PostConstruct without a reflection", InjectionExceptionType.NULL_REFLECTION);
			MethodInfo[] postConstructors = reflection.postConstructors;
			if (postConstructors != null)
			{
				foreach (MethodInfo methodInfo in postConstructors)
				{
					methodInfo.Invoke(target, null);
				}
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00054F94 File Offset: 0x00053394
		private void performUninjection(object target, IReflectedClass reflection)
		{
			int num = reflection.setters.Length;
			for (int i = 0; i < num; i++)
			{
				KeyValuePair<Type, PropertyInfo> keyValuePair = reflection.setters[i];
				keyValuePair.Value.SetValue(target, null, null);
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00054FDD File Offset: 0x000533DD
		private void failIf(bool condition, string message, InjectionExceptionType type)
		{
			this.failIf(condition, message, type, null, null, null);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00054FEB File Offset: 0x000533EB
		private void failIf(bool condition, string message, InjectionExceptionType type, Type t, object name)
		{
			this.failIf(condition, message, type, t, name, null);
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00054FFB File Offset: 0x000533FB
		private void failIf(bool condition, string message, InjectionExceptionType type, Type t, object name, object target)
		{
			if (condition)
			{
				message = message + "\n\t\ttarget: " + target;
				message = message + "\n\t\ttype: " + t;
				message = message + "\n\t\tname: " + name;
				throw new InjectionException(message, type);
			}
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00055038 File Offset: 0x00053438
		private void armorAgainstInfiniteLoops(IInjectionBinding binding)
		{
			if (binding == null)
			{
				return;
			}
			if (this.infinityLock == null)
			{
				this.infinityLock = new Dictionary<IInjectionBinding, int>();
			}
			if (!this.infinityLock.ContainsKey(binding))
			{
				this.infinityLock.Add(binding, 0);
			}
			this.infinityLock[binding] = this.infinityLock[binding] + 1;
			if (this.infinityLock[binding] > 10)
			{
				throw new InjectionException("There appears to be a circular dependency. Terminating loop.", InjectionExceptionType.CIRCULAR_DEPENDENCY);
			}
		}

		// Token: 0x04000776 RID: 1910
		private Dictionary<IInjectionBinding, int> infinityLock;

		// Token: 0x04000777 RID: 1911
		private const int INFINITY_LIMIT = 10;
	}
}
