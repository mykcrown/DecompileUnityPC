// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.reflector.api;
using strange.framework.api;
using strange.framework.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace strange.extensions.reflector.impl
{
	public class ReflectionBinder : strange.framework.impl.Binder, IReflectionBinder
	{
		public IReflectedClass Get<T>()
		{
			return this.Get(typeof(T));
		}

		public IReflectedClass Get(Type type)
		{
			IBinding binding = this.GetBinding(type);
			IReflectedClass reflectedClass2;
			if (binding == null)
			{
				binding = this.GetRawBinding();
				IReflectedClass reflectedClass = new ReflectedClass();
				this.mapPreferredConstructor(reflectedClass, binding, type);
				this.mapPostConstructors(reflectedClass, binding, type);
				this.mapSetters(reflectedClass, binding, type);
				binding.Bind(type).To(reflectedClass);
				reflectedClass2 = (binding.value as IReflectedClass);
				reflectedClass2.PreGenerated = false;
			}
			else
			{
				reflectedClass2 = (binding.value as IReflectedClass);
				reflectedClass2.PreGenerated = true;
			}
			return reflectedClass2;
		}

		public override IBinding GetRawBinding()
		{
			IBinding rawBinding = base.GetRawBinding();
			rawBinding.valueConstraint = BindingConstraintType.ONE;
			return rawBinding;
		}

		private void mapPreferredConstructor(IReflectedClass reflected, IBinding binding, Type type)
		{
			ConstructorInfo constructorInfo = this.findPreferredConstructor(type);
			if (constructorInfo == null)
			{
				throw new ReflectionException("The reflector requires concrete classes.\nType " + type + " has no constructor. Is it an interface?", ReflectionExceptionType.CANNOT_REFLECT_INTERFACE);
			}
			ParameterInfo[] parameters = constructorInfo.GetParameters();
			Type[] array = new Type[parameters.Length];
			object[] array2 = new object[parameters.Length];
			int num = 0;
			ParameterInfo[] array3 = parameters;
			for (int i = 0; i < array3.Length; i++)
			{
				ParameterInfo parameterInfo = array3[i];
				Type parameterType = parameterInfo.ParameterType;
				array[num] = parameterType;
				object[] customAttributes = parameterInfo.GetCustomAttributes(typeof(Name), false);
				if (customAttributes.Length > 0)
				{
					array2[num] = ((Name)customAttributes[0]).name;
				}
				num++;
			}
			reflected.Constructor = constructorInfo;
			reflected.ConstructorParameters = array;
			reflected.ConstructorParameterNames = array2;
		}

		private ConstructorInfo findPreferredConstructor(Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
			if (constructors.Length == 1)
			{
				return constructors[0];
			}
			int num = 2147483647;
			ConstructorInfo result = null;
			ConstructorInfo[] array = constructors;
			for (int i = 0; i < array.Length; i++)
			{
				ConstructorInfo constructorInfo = array[i];
				object[] customAttributes = constructorInfo.GetCustomAttributes(typeof(Construct), true);
				if (customAttributes.Length > 0)
				{
					return constructorInfo;
				}
				int num2 = constructorInfo.GetParameters().Length;
				if (num2 < num)
				{
					num = num2;
					result = constructorInfo;
				}
			}
			return result;
		}

		private void mapPostConstructors(IReflectedClass reflected, IBinding binding, Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
			ArrayList arrayList = new ArrayList();
			MethodInfo[] array = methods;
			for (int i = 0; i < array.Length; i++)
			{
				MethodInfo methodInfo = array[i];
				object[] customAttributes = methodInfo.GetCustomAttributes(typeof(PostConstruct), true);
				if (customAttributes.Length > 0)
				{
					arrayList.Add(methodInfo);
				}
			}
			arrayList.Sort(new PriorityComparer());
			MethodInfo[] postConstructors = (MethodInfo[])arrayList.ToArray(typeof(MethodInfo));
			reflected.postConstructors = postConstructors;
		}

		private void mapSetters(IReflectedClass reflected, IBinding binding, Type type)
		{
			KeyValuePair<Type, PropertyInfo>[] array = new KeyValuePair<Type, PropertyInfo>[0];
			object[] array2 = new object[0];
			MemberInfo[] array3 = type.FindMembers(MemberTypes.Property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty, null, null);
			MemberInfo[] array4 = array3;
			for (int i = 0; i < array4.Length; i++)
			{
				MemberInfo memberInfo = array4[i];
				object[] customAttributes = memberInfo.GetCustomAttributes(typeof(Inject), true);
				if (customAttributes.Length > 0)
				{
					throw new ReflectionException(string.Concat(new string[]
					{
						"The class ",
						type.Name,
						" has a non-public Injection setter ",
						memberInfo.Name,
						". Make the setter public to allow injection."
					}), ReflectionExceptionType.CANNOT_INJECT_INTO_NONPUBLIC_SETTER);
				}
			}
			MemberInfo[] array5 = type.FindMembers(MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty, null, null);
			MemberInfo[] array6 = array5;
			for (int j = 0; j < array6.Length; j++)
			{
				MemberInfo memberInfo2 = array6[j];
				object[] customAttributes2 = memberInfo2.GetCustomAttributes(typeof(Inject), true);
				if (customAttributes2.Length > 0)
				{
					Inject inject = customAttributes2[0] as Inject;
					PropertyInfo propertyInfo = memberInfo2 as PropertyInfo;
					Type propertyType = propertyInfo.PropertyType;
					KeyValuePair<Type, PropertyInfo> value = new KeyValuePair<Type, PropertyInfo>(propertyType, propertyInfo);
					array = this.AddKV(value, array);
					object name = inject.name;
					array2 = this.Add(name, array2);
				}
			}
			reflected.Setters = array;
			reflected.SetterNames = array2;
		}

		private object[] Add(object value, object[] list)
		{
			object[] array = list;
			int num = array.Length;
			list = new object[num + 1];
			array.CopyTo(list, 0);
			list[num] = value;
			return list;
		}

		private KeyValuePair<Type, PropertyInfo>[] AddKV(KeyValuePair<Type, PropertyInfo> value, KeyValuePair<Type, PropertyInfo>[] list)
		{
			KeyValuePair<Type, PropertyInfo>[] array = list;
			int num = array.Length;
			list = new KeyValuePair<Type, PropertyInfo>[num + 1];
			array.CopyTo(list, 0);
			list[num] = value;
			return list;
		}
	}
}
