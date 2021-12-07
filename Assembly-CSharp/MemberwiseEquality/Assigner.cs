using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Beebyte.Obfuscator;

namespace MemberwiseEquality
{
	// Token: 0x02000B27 RID: 2855
	public static class Assigner
	{
		// Token: 0x060052E5 RID: 21221 RVA: 0x001AC414 File Offset: 0x001AA814
		public static void StartMakeAssignFunction(out DynamicMethod m, out ILGenerator cg, Type assignType)
		{
			m = new DynamicMethod("assign", typeof(void), new Type[]
			{
				typeof(object),
				typeof(object)
			}, typeof(Assigner), true);
			cg = m.GetILGenerator();
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x001AC46C File Offset: 0x001AA86C
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

		// Token: 0x060052E7 RID: 21223 RVA: 0x001AC5D2 File Offset: 0x001AA9D2
		public static void CloneEnumerable(FieldInfo field, ILGenerator cg, Type assignType)
		{
			Assigner.AssignField(field, cg, assignType, Assigner.AssignMode.CloneEnumerable);
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x001AC5DD File Offset: 0x001AA9DD
		public static void CloneDictionary(FieldInfo field, ILGenerator cg, Type assignType)
		{
			Assigner.AssignField(field, cg, assignType, Assigner.AssignMode.CloneDictionary);
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x001AC5E8 File Offset: 0x001AA9E8
		public static void CloneCloneableObject(FieldInfo field, ILGenerator cg, Type assignType)
		{
			Assigner.AssignField(field, cg, assignType, Assigner.AssignMode.CloneCloneableObject);
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x001AC5F4 File Offset: 0x001AA9F4
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
				foreach (T t in source)
				{
					array[num] = (t.Clone() as T);
					num++;
				}
				return array;
			}
			if (source is List<T>)
			{
				List<T> list = new List<T>(source.Count<T>());
				foreach (T t2 in source)
				{
					list.Add(t2.Clone() as T);
				}
				return list;
			}
			throw new Exception("Don't know how to clone IEnumerable of type " + source.GetType());
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x001AC718 File Offset: 0x001AAB18
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

		// Token: 0x060052EC RID: 21228 RVA: 0x001AC782 File Offset: 0x001AAB82
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

		// Token: 0x060052ED RID: 21229 RVA: 0x001AC7AE File Offset: 0x001AABAE
		[SkipRename]
		private static T cloneCloneableObject<T>(T source) where T : CloneableObject
		{
			return source.Clone() as T;
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x001AC7C8 File Offset: 0x001AABC8
		public static Action<object, object> EndMakeAssignFunction(Type assignType, ILGenerator cg, DynamicMethod m)
		{
			cg.Emit(OpCodes.Ret);
			Type typeFromHandle = typeof(Action<object, object>);
			Delegate @delegate = m.CreateDelegate(typeFromHandle);
			return (Action<object, object>)@delegate;
		}

		// Token: 0x02000B28 RID: 2856
		public enum AssignMode
		{
			// Token: 0x040034AE RID: 13486
			Assign,
			// Token: 0x040034AF RID: 13487
			CloneDictionary,
			// Token: 0x040034B0 RID: 13488
			CloneEnumerable,
			// Token: 0x040034B1 RID: 13489
			CloneCloneableObject
		}
	}
}
