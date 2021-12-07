// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Cloning
{
	public static class CloneHelper<T> where T : class
	{
		private static Dictionary<Type, Delegate> _cachedILShallow = new Dictionary<Type, Delegate>();

		private static Dictionary<Type, Delegate> _cachedILDeep = new Dictionary<Type, Delegate>();

		private static CloneType? _globalCloneType = new CloneType?(CloneType.ShallowCloning);

		public static T Clone(T obj)
		{
			CloneHelper<T>._globalCloneType = null;
			return CloneHelper<T>.CloneObjectWithILDeep(obj);
		}

		public static T Clone(T obj, CloneType cloneType)
		{
			CloneType? globalCloneType = CloneHelper<T>._globalCloneType;
			if (globalCloneType.HasValue)
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
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				iLGenerator.Emit(OpCodes.Newobj, constructor);
				iLGenerator.Emit(OpCodes.Stloc_0);
				FieldInfo[] fields = myObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo field = fields[i];
					iLGenerator.Emit(OpCodes.Ldloc_0);
					iLGenerator.Emit(OpCodes.Ldarg_0);
					iLGenerator.Emit(OpCodes.Ldfld, field);
					iLGenerator.Emit(OpCodes.Stfld, field);
				}
				iLGenerator.Emit(OpCodes.Ldloc_0);
				iLGenerator.Emit(OpCodes.Ret);
				@delegate = dynamicMethod.CreateDelegate(typeof(Func<T, T>));
				CloneHelper<T>._cachedILShallow.Add(typeof(T), @delegate);
			}
			return ((Func<T, T>)@delegate)(myObject);
		}

		private static T CloneObjectWithILDeep(T myObject)
		{
			Delegate @delegate = null;
			if (!CloneHelper<T>._cachedILDeep.TryGetValue(typeof(T), out @delegate))
			{
				DynamicMethod dynamicMethod = new DynamicMethod("DoDeepClone", typeof(T), new Type[]
				{
					typeof(T)
				}, Assembly.GetExecutingAssembly().ManifestModule, true);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				LocalBuilder localBuilder = iLGenerator.DeclareLocal(myObject.GetType());
				ConstructorInfo constructor = myObject.GetType().GetConstructor(Type.EmptyTypes);
				iLGenerator.Emit(OpCodes.Newobj, constructor);
				iLGenerator.Emit(OpCodes.Stloc, localBuilder);
				FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
					if (CloneHelper<T>._globalCloneType == CloneType.DeepCloning)
					{
						if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string))
						{
							iLGenerator.Emit(OpCodes.Ldloc, localBuilder);
							iLGenerator.Emit(OpCodes.Ldarg_0);
							iLGenerator.Emit(OpCodes.Ldfld, fieldInfo);
							iLGenerator.Emit(OpCodes.Stfld, fieldInfo);
						}
						else if (fieldInfo.FieldType.IsClass)
						{
							CloneHelper<T>.CopyReferenceType(iLGenerator, localBuilder, fieldInfo);
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
								iLGenerator.Emit(OpCodes.Ldloc, localBuilder);
								iLGenerator.Emit(OpCodes.Ldarg_0);
								iLGenerator.Emit(OpCodes.Ldfld, fieldInfo);
								iLGenerator.Emit(OpCodes.Stfld, fieldInfo);
							}
							else if (fieldInfo.FieldType.IsClass)
							{
								CloneHelper<T>.CopyReferenceType(iLGenerator, localBuilder, fieldInfo);
							}
						}
						else
						{
							iLGenerator.Emit(OpCodes.Ldloc, localBuilder);
							iLGenerator.Emit(OpCodes.Ldarg_0);
							iLGenerator.Emit(OpCodes.Ldfld, fieldInfo);
							iLGenerator.Emit(OpCodes.Stfld, fieldInfo);
						}
					}
				}
				iLGenerator.Emit(OpCodes.Ldloc_0);
				iLGenerator.Emit(OpCodes.Ret);
				@delegate = dynamicMethod.CreateDelegate(typeof(Func<T, T>));
				CloneHelper<T>._cachedILDeep.Add(typeof(T), @delegate);
			}
			return ((Func<T, T>)@delegate)(myObject);
		}

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
				FieldInfo[] fields = field.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
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

		private static CloneType GetCloneTypeForField(FieldInfo field)
		{
			object[] customAttributes = field.GetCustomAttributes(typeof(CloneAttribute), true);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				return (customAttributes[0] as CloneAttribute).CloneType;
			}
			if (!CloneHelper<T>._globalCloneType.HasValue)
			{
				return CloneType.ShallowCloning;
			}
			return CloneHelper<T>._globalCloneType.Value;
		}
	}
}
