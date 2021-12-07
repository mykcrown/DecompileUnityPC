using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Cloning
{
	// Token: 0x02000AB0 RID: 2736
	public static class CloneHelper<T> where T : class
	{
		// Token: 0x06005051 RID: 20561 RVA: 0x0014E8F4 File Offset: 0x0014CCF4
		public static T Clone(T obj)
		{
			CloneHelper<T>._globalCloneType = null;
			return CloneHelper<T>.CloneObjectWithILDeep(obj);
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x0014E918 File Offset: 0x0014CD18
		public static T Clone(T obj, CloneType cloneType)
		{
			CloneType? globalCloneType = CloneHelper<T>._globalCloneType;
			if (globalCloneType != null)
			{
				CloneHelper<T>._globalCloneType = new CloneType?(cloneType);
			}
			switch (cloneType)
			{
			case CloneType.None:
				throw new InvalidOperationException("No need to call this method?");
			case CloneType.ShallowCloning:
				return CloneHelper<T>.CloneObjectWithILShallow(obj);
			case CloneType.DeepCloning:
				return CloneHelper<T>.CloneObjectWithILDeep(obj);
			default:
				return (T)((object)null);
			}
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x0014E980 File Offset: 0x0014CD80
		private static T CloneObjectWithILShallow(T myObject)
		{
			Delegate @delegate = null;
			if (!CloneHelper<T>._cachedILShallow.TryGetValue(typeof(T), out @delegate))
			{
				DynamicMethod dynamicMethod = new DynamicMethod("DoShallowClone", typeof(T), new Type[]
				{
					typeof(T)
				}, Assembly.GetExecutingAssembly().ManifestModule, true);
				ConstructorInfo constructor = myObject.GetType().GetConstructor(new Type[0]);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Newobj, constructor);
				ilgenerator.Emit(OpCodes.Stloc_0);
				foreach (FieldInfo field in myObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					ilgenerator.Emit(OpCodes.Ldloc_0);
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Ldfld, field);
					ilgenerator.Emit(OpCodes.Stfld, field);
				}
				ilgenerator.Emit(OpCodes.Ldloc_0);
				ilgenerator.Emit(OpCodes.Ret);
				@delegate = dynamicMethod.CreateDelegate(typeof(Func<T, T>));
				CloneHelper<T>._cachedILShallow.Add(typeof(T), @delegate);
			}
			return ((Func<T, T>)@delegate)(myObject);
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x0014EAC4 File Offset: 0x0014CEC4
		private static T CloneObjectWithILDeep(T myObject)
		{
			Delegate @delegate = null;
			if (!CloneHelper<T>._cachedILDeep.TryGetValue(typeof(T), out @delegate))
			{
				DynamicMethod dynamicMethod = new DynamicMethod("DoDeepClone", typeof(T), new Type[]
				{
					typeof(T)
				}, Assembly.GetExecutingAssembly().ManifestModule, true);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(myObject.GetType());
				ConstructorInfo constructor = myObject.GetType().GetConstructor(Type.EmptyTypes);
				ilgenerator.Emit(OpCodes.Newobj, constructor);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (CloneHelper<T>._globalCloneType == CloneType.DeepCloning)
					{
						if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string))
						{
							ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
							ilgenerator.Emit(OpCodes.Ldarg_0);
							ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
							ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
						}
						else if (fieldInfo.FieldType.IsClass)
						{
							CloneHelper<T>.CopyReferenceType(ilgenerator, localBuilder, fieldInfo);
						}
					}
					else
					{
						CloneType cloneTypeForField = CloneHelper<T>.GetCloneTypeForField(fieldInfo);
						if (cloneTypeForField != CloneType.ShallowCloning)
						{
							if (cloneTypeForField != CloneType.DeepCloning)
							{
								if (cloneTypeForField != CloneType.None)
								{
								}
							}
							else if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string))
							{
								ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
								ilgenerator.Emit(OpCodes.Ldarg_0);
								ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
								ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
							}
							else if (fieldInfo.FieldType.IsClass)
							{
								CloneHelper<T>.CopyReferenceType(ilgenerator, localBuilder, fieldInfo);
							}
						}
						else
						{
							ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
							ilgenerator.Emit(OpCodes.Ldarg_0);
							ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
							ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
						}
					}
				}
				ilgenerator.Emit(OpCodes.Ldloc_0);
				ilgenerator.Emit(OpCodes.Ret);
				@delegate = dynamicMethod.CreateDelegate(typeof(Func<T, T>));
				CloneHelper<T>._cachedILDeep.Add(typeof(T), @delegate);
			}
			return ((Func<T, T>)@delegate)(myObject);
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x0014ED6C File Offset: 0x0014D16C
		private static void CopyReferenceType(ILGenerator generator, LocalBuilder cloneVar, FieldInfo field)
		{
			if (field.FieldType.IsSubclassOf(typeof(Delegate)))
			{
				return;
			}
			LocalBuilder localBuilder = generator.DeclareLocal(field.FieldType);
			if (field.FieldType.GetInterface("IEnumerable") != null && field.FieldType.GetInterface("IList") != null)
			{
				if (field.FieldType.IsGenericType)
				{
					Type type = field.FieldType.GetGenericArguments()[0];
					Type type2 = Type.GetType("System.Collections.Generic.IEnumerable`1[" + type.FullName + "]");
					ConstructorInfo constructor = field.FieldType.GetConstructor(new Type[]
					{
						type2
					});
					if (constructor != null && CloneHelper<T>.GetCloneTypeForField(field) == CloneType.ShallowCloning)
					{
						generator.Emit(OpCodes.Ldarg_0);
						generator.Emit(OpCodes.Ldfld, field);
						generator.Emit(OpCodes.Newobj, constructor);
						generator.Emit(OpCodes.Stloc, localBuilder);
						generator.Emit(OpCodes.Ldloc, cloneVar);
						generator.Emit(OpCodes.Ldloc, localBuilder);
						generator.Emit(OpCodes.Stfld, field);
					}
					else
					{
						constructor = field.FieldType.GetConstructor(Type.EmptyTypes);
						if (constructor != null)
						{
							generator.Emit(OpCodes.Newobj, constructor);
							generator.Emit(OpCodes.Stloc, localBuilder);
							generator.Emit(OpCodes.Ldloc, cloneVar);
							generator.Emit(OpCodes.Ldloc, localBuilder);
							generator.Emit(OpCodes.Stfld, field);
							CloneHelper<T>.CloneList(generator, field, type, localBuilder);
						}
					}
				}
			}
			else
			{
				ConstructorInfo constructor2 = field.FieldType.GetConstructor(new Type[0]);
				generator.Emit(OpCodes.Newobj, constructor2);
				generator.Emit(OpCodes.Stloc, localBuilder);
				generator.Emit(OpCodes.Ldloc, cloneVar);
				generator.Emit(OpCodes.Ldloc, localBuilder);
				generator.Emit(OpCodes.Stfld, field);
				foreach (FieldInfo fieldInfo in field.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string))
					{
						generator.Emit(OpCodes.Ldloc_1);
						generator.Emit(OpCodes.Ldarg_0);
						generator.Emit(OpCodes.Ldfld, field);
						generator.Emit(OpCodes.Ldfld, fieldInfo);
						generator.Emit(OpCodes.Stfld, fieldInfo);
					}
				}
			}
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x0014EFE4 File Offset: 0x0014D3E4
		private static void CloneList(ILGenerator generator, FieldInfo listField, Type typeToClone, LocalBuilder cloneVar)
		{
			Type type = Type.GetType("System.Collections.Generic.IEnumerator`1[" + typeToClone.FullName + "]");
			Type type2 = Type.GetType(string.Concat(new string[]
			{
				listField.FieldType.Namespace,
				".",
				listField.FieldType.Name,
				"+Enumerator[[",
				typeToClone.FullName,
				"]]"
			}));
			LocalBuilder local = generator.DeclareLocal(type);
			LocalBuilder local2 = generator.DeclareLocal(typeof(bool));
			Label label = generator.DefineLabel();
			Label label2 = generator.DefineLabel();
			MethodInfo method = listField.FieldType.GetMethod("GetEnumerator");
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldfld, listField);
			generator.Emit(OpCodes.Callvirt, method);
			if (type2 != null)
			{
				generator.Emit(OpCodes.Box, type2);
			}
			generator.Emit(OpCodes.Stloc, local);
			generator.Emit(OpCodes.Br_S, label);
			generator.MarkLabel(label2);
			generator.Emit(OpCodes.Nop);
			generator.Emit(OpCodes.Ldloc, cloneVar);
			generator.Emit(OpCodes.Ldloc, local);
			MethodInfo getMethod = type.GetProperty("Current").GetGetMethod();
			generator.Emit(OpCodes.Callvirt, getMethod);
			Type type3 = Type.GetType(string.Concat(new string[]
			{
				typeof(CloneHelper<T>).Namespace,
				".",
				typeof(CloneHelper<T>).Name,
				"[",
				getMethod.ReturnType.FullName,
				"]"
			}));
			MethodInfo method2 = type3.GetMethod("CloneObjectWithILDeep", BindingFlags.Static | BindingFlags.NonPublic);
			generator.Emit(OpCodes.Call, method2);
			MethodInfo method3 = listField.FieldType.GetMethod("Add");
			generator.Emit(OpCodes.Callvirt, method3);
			generator.Emit(OpCodes.Nop);
			generator.MarkLabel(label);
			generator.Emit(OpCodes.Nop);
			generator.Emit(OpCodes.Ldloc, local);
			MethodInfo method4 = typeof(IEnumerator).GetMethod("MoveNext");
			generator.Emit(OpCodes.Callvirt, method4);
			generator.Emit(OpCodes.Stloc, local2);
			generator.Emit(OpCodes.Ldloc, local2);
			generator.Emit(OpCodes.Brtrue_S, label2);
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x0014F240 File Offset: 0x0014D640
		private static CloneType GetCloneTypeForField(FieldInfo field)
		{
			object[] customAttributes = field.GetCustomAttributes(typeof(CloneAttribute), true);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				return (customAttributes[0] as CloneAttribute).CloneType;
			}
			if (CloneHelper<T>._globalCloneType == null)
			{
				return CloneType.ShallowCloning;
			}
			return CloneHelper<T>._globalCloneType.Value;
		}

		// Token: 0x040033B8 RID: 13240
		private static Dictionary<Type, Delegate> _cachedILShallow = new Dictionary<Type, Delegate>();

		// Token: 0x040033B9 RID: 13241
		private static Dictionary<Type, Delegate> _cachedILDeep = new Dictionary<Type, Delegate>();

		// Token: 0x040033BA RID: 13242
		private static CloneType? _globalCloneType = new CloneType?(CloneType.ShallowCloning);
	}
}
