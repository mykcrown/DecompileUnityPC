// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using MemberwiseEquality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MemberwiseEquality
{
	public static class Assigner
	{
		public enum AssignMode
		{
			Assign,
			CloneDictionary,
			CloneEnumerable,
			CloneCloneableObject
		}

		public static void StartMakeAssignFunction(out DynamicMethod m, out ILGenerator cg, Type assignType)
		{
			m = new DynamicMethod("assign", typeof(void), new Type[]
			{
				typeof(object),
				typeof(object)
			}, typeof(Assigner), true);
			cg = m.GetILGenerator();
		}

		public static void AssignField(FieldInfo field, ILGenerator cg, Type assignType, Assigner.AssignMode mode = Assigner.AssignMode.Assign)
		{
			cg.Emit(OpCodes.Ldarg_0);
			cg.Emit(OpCodes.Ldarg_1);
			cg.Emit(OpCodes.Ldfld, field);
			if (mode != Assigner.AssignMode.CloneDictionary)
			{
				if (mode != Assigner.AssignMode.CloneEnumerable)
				{
					if (mode == Assigner.AssignMode.CloneCloneableObject)
					{
						MethodInfo methodInfo = typeof(Assigner).GetMethod("cloneCloneableObject", BindingFlags.Static | BindingFlags.NonPublic);
						methodInfo = methodInfo.MakeGenericMethod(new Type[]
						{
							field.FieldType
						});
						cg.Emit(OpCodes.Call, methodInfo);
					}
				}
				else
				{
					Type type = (!field.FieldType.IsArray) ? field.FieldType.GetGenericArguments()[0] : field.FieldType.GetElementType();
					MethodInfo methodInfo2 = typeof(Assigner).GetMethod("cloneEnumerable", BindingFlags.Static | BindingFlags.NonPublic);
					if (typeof(CloneableObject).IsAssignableFrom(type))
					{
						methodInfo2 = typeof(Assigner).GetMethod("deepCloneEnumerable", BindingFlags.Static | BindingFlags.NonPublic);
					}
					methodInfo2 = methodInfo2.MakeGenericMethod(new Type[]
					{
						type
					});
					cg.Emit(OpCodes.Call, methodInfo2);
				}
			}
			else
			{
				Type[] genericArguments = field.FieldType.GetGenericArguments();
				MethodInfo methodInfo3 = typeof(Assigner).GetMethod("cloneDictionary", BindingFlags.Static | BindingFlags.NonPublic);
				methodInfo3 = methodInfo3.MakeGenericMethod(genericArguments);
				cg.Emit(OpCodes.Call, methodInfo3);
			}
			cg.Emit(OpCodes.Stfld, field);
		}

		public static void CloneEnumerable(FieldInfo field, ILGenerator cg, Type assignType)
		{
			Assigner.AssignField(field, cg, assignType, Assigner.AssignMode.CloneEnumerable);
		}

		public static void CloneDictionary(FieldInfo field, ILGenerator cg, Type assignType)
		{
			Assigner.AssignField(field, cg, assignType, Assigner.AssignMode.CloneDictionary);
		}

		public static void CloneCloneableObject(FieldInfo field, ILGenerator cg, Type assignType)
		{
			Assigner.AssignField(field, cg, assignType, Assigner.AssignMode.CloneCloneableObject);
		}

		[SkipRename]
		private static IEnumerable<T> deepCloneEnumerable<T>(IEnumerable<T> source) where T : CloneableObject
		{
			if (source == null)
			{
				return null;
			}
			if (source is Array)
			{
				T[] array = new T[source.Count<T>()];
				int num = 0;
				foreach (T current in source)
				{
					array[num] = (current.Clone() as T);
					num++;
				}
				return array;
			}
			if (source is List<T>)
			{
				List<T> list = new List<T>(source.Count<T>());
				foreach (T current2 in source)
				{
					list.Add(current2.Clone() as T);
				}
				return list;
			}
			throw new Exception("Don't know how to clone IEnumerable of type " + source.GetType());
		}

		[SkipRename]
		private static IEnumerable<T> cloneEnumerable<T>(IEnumerable<T> source)
		{
			if (source == null)
			{
				return null;
			}
			if (source is Array)
			{
				return source.ToArray<T>();
			}
			if (source is CloneList<T>)
			{
				return ((CloneList<T>)source).ShallowClone() as CloneList<T>;
			}
			if (source is List<T>)
			{
				return new List<T>(source);
			}
			throw new Exception("Don't know how to clone IEnumerable of type " + source.GetType());
		}

		[SkipRename]
		private static Dictionary<T, U> cloneDictionary<T, U>(Dictionary<T, U> source)
		{
			if (source == null)
			{
				return null;
			}
			if (source is SerializableDictionary<T, U>)
			{
				return (source as SerializableDictionary<T, U>).ShallowClone() as Dictionary<T, U>;
			}
			return new Dictionary<T, U>(source);
		}

		[SkipRename]
		private static T cloneCloneableObject<T>(T source) where T : CloneableObject
		{
			return source.Clone() as T;
		}

		public static Action<object, object> EndMakeAssignFunction(Type assignType, ILGenerator cg, DynamicMethod m)
		{
			cg.Emit(OpCodes.Ret);
			Type typeFromHandle = typeof(Action<object, object>);
			Delegate @delegate = m.CreateDelegate(typeFromHandle);
			return (Action<object, object>)@delegate;
		}
	}
}
