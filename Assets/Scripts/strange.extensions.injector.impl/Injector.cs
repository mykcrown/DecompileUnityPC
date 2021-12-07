// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using strange.extensions.reflector.api;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace strange.extensions.injector.impl
{
	public class Injector : IInjector
	{
		private Dictionary<IInjectionBinding, int> infinityLock;

		private const int INFINITY_LIMIT = 10;

		public IInjectorFactory factory
		{
			get;
			set;
		}

		public IInjectionBinder binder
		{
			get;
			set;
		}

		public IReflectionBinder reflector
		{
			get;
			set;
		}

		public Injector()
		{
			this.factory = new InjectorFactory();
		}

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

		public object Inject(object target)
		{
			return this.Inject(target, true);
		}

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
			Type[] array2 = constructorParameters;
			for (int i = 0; i < array2.Length; i++)
			{
				Type t = array2[i];
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

		private void injectValueIntoPoint(object value, object target, PropertyInfo point)
		{
			this.failIf(target == null, "Attempt to inject into a null target", InjectionExceptionType.NULL_TARGET);
			this.failIf(point == null, "Attempt to inject into a null point", InjectionExceptionType.NULL_INJECTION_POINT);
			this.failIf(value == null, "Attempt to inject null into a target object", InjectionExceptionType.NULL_VALUE_INJECTION);
			point.SetValue(target, value, null);
		}

		private void postInject(object target, IReflectedClass reflection)
		{
			this.failIf(target == null, "Attempt to PostConstruct a null target", InjectionExceptionType.NULL_TARGET);
			this.failIf(reflection == null, "Attempt to PostConstruct without a reflection", InjectionExceptionType.NULL_REFLECTION);
			MethodInfo[] postConstructors = reflection.postConstructors;
			if (postConstructors != null)
			{
				MethodInfo[] array = postConstructors;
				for (int i = 0; i < array.Length; i++)
				{
					MethodInfo methodInfo = array[i];
					methodInfo.Invoke(target, null);
				}
			}
		}

		private void performUninjection(object target, IReflectedClass reflection)
		{
			int num = reflection.setters.Length;
			for (int i = 0; i < num; i++)
			{
				KeyValuePair<Type, PropertyInfo> keyValuePair = reflection.setters[i];
				keyValuePair.Value.SetValue(target, null, null);
			}
		}

		private void failIf(bool condition, string message, InjectionExceptionType type)
		{
			this.failIf(condition, message, type, null, null, null);
		}

		private void failIf(bool condition, string message, InjectionExceptionType type, Type t, object name)
		{
			this.failIf(condition, message, type, t, name, null);
		}

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
	}
}
