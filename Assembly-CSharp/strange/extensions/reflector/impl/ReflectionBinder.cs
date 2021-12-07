using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using strange.extensions.reflector.api;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.reflector.impl
{
	// Token: 0x02000270 RID: 624
	public class ReflectionBinder : strange.framework.impl.Binder, IReflectionBinder
	{
		// Token: 0x06000CB9 RID: 3257 RVA: 0x00055F40 File Offset: 0x00054340
		public IReflectedClass Get<T>()
		{
			return this.Get(typeof(T));
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00055F54 File Offset: 0x00054354
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

		// Token: 0x06000CBB RID: 3259 RVA: 0x00055FD4 File Offset: 0x000543D4
		public override IBinding GetRawBinding()
		{
			IBinding rawBinding = base.GetRawBinding();
			rawBinding.valueConstraint = BindingConstraintType.ONE;
			return rawBinding;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00055FF8 File Offset: 0x000543F8
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
			foreach (ParameterInfo parameterInfo in parameters)
			{
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

		// Token: 0x06000CBD RID: 3261 RVA: 0x000560CC File Offset: 0x000544CC
		private ConstructorInfo findPreferredConstructor(Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
			if (constructors.Length == 1)
			{
				return constructors[0];
			}
			int num = int.MaxValue;
			ConstructorInfo result = null;
			foreach (ConstructorInfo constructorInfo in constructors)
			{
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

		// Token: 0x06000CBE RID: 3262 RVA: 0x00056158 File Offset: 0x00054558
		private void mapPostConstructors(IReflectedClass reflected, IBinding binding, Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
			ArrayList arrayList = new ArrayList();
			foreach (MethodInfo methodInfo in methods)
			{
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

		// Token: 0x06000CBF RID: 3263 RVA: 0x000561E8 File Offset: 0x000545E8
		private void mapSetters(IReflectedClass reflected, IBinding binding, Type type)
		{
			KeyValuePair<Type, PropertyInfo>[] array = new KeyValuePair<Type, PropertyInfo>[0];
			object[] array2 = new object[0];
			MemberInfo[] array3 = type.FindMembers(MemberTypes.Property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty, null, null);
			foreach (MemberInfo memberInfo in array3)
			{
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
			foreach (MemberInfo memberInfo2 in array5)
			{
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

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00056334 File Offset: 0x00054734
		private object[] Add(object value, object[] list)
		{
			object[] array = list;
			int num = array.Length;
			list = new object[num + 1];
			array.CopyTo(list, 0);
			list[num] = value;
			return list;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00056360 File Offset: 0x00054760
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
