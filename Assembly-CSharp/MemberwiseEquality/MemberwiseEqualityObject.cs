using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Beebyte.Obfuscator;
using UnityEngine;

namespace MemberwiseEquality
{
	// Token: 0x02000B2C RID: 2860
	[Serializable]
	public class MemberwiseEqualityObject
	{
		// Token: 0x060052FE RID: 21246 RVA: 0x0000A9C4 File Offset: 0x00008DC4
		public MemberwiseEqualityObject()
		{
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return;
			}
			MemberwiseEqualityObject.registerMemberwiseType(base.GetType());
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x0000A9E4 File Offset: 0x00008DE4
		public static void RecursivelyRegisterMemberwiseTypes(Type type)
		{
			if (typeof(MemberwiseEqualityObject).IsAssignableFrom(type))
			{
				MemberwiseEqualityObject.registerMemberwiseType(type);
				foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					MemberwiseEqualityObject.RecursivelyRegisterMemberwiseTypes(fieldInfo.FieldType);
				}
			}
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x0000AA38 File Offset: 0x00008E38
		private static void registerMemberwiseType(Type type)
		{
			if (!MemberwiseEqualityObject._functions.ContainsKey(type))
			{
				MemberwiseEqualityObject.MemberwiseFunctions memberwiseFunctions = new MemberwiseEqualityObject.MemberwiseFunctions();
				memberwiseFunctions.EqualsFunc = MakeEqualsProcessor.MakeEqualsMethod(type);
				memberwiseFunctions.GetHashCodeFunc = MakeGetHashCodeProcessor.MakeGetHashCodeMethod(type);
				memberwiseFunctions.AssignAction = AssignObjectProcessor.MakeAssignMethod(type, AssignObjectProcessor.AssignMethod.DeepAssign);
				memberwiseFunctions.ShallowAssignAction = AssignObjectProcessor.MakeAssignMethod(type, AssignObjectProcessor.AssignMethod.ShallowAssign);
				MemberwiseEqualityObject._functions[type] = memberwiseFunctions;
			}
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x0000AA99 File Offset: 0x00008E99
		private static bool ignoreEqualityComparison(FieldInfo field)
		{
			return field.GetCustomAttributes(typeof(IgnoreRollbackAttribute), true).Length > 0;
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x0000AAB4 File Offset: 0x00008EB4
		private static bool isUnhandledDictionary(FieldInfo field)
		{
			Type[] genericArguments = field.FieldType.GetGenericArguments();
			return MemberwiseEqualityObject.isDictionary(field) && ((!genericArguments[0].IsValueType && !(genericArguments[0] == typeof(string))) || (!genericArguments[1].IsValueType && !(genericArguments[1] == typeof(string))));
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x0000AB28 File Offset: 0x00008F28
		private static bool isDictionary(FieldInfo field)
		{
			if (field.FieldType == typeof(string) || !field.FieldType.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = field.FieldType.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(Dictionary<, >) || genericTypeDefinition == typeof(SerializableDictionary<, >);
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x0000AB95 File Offset: 0x00008F95
		private static bool isEnumerable(FieldInfo field)
		{
			return field.FieldType != typeof(string) && field.FieldType.GetInterface(typeof(IEnumerable).FullName) != null;
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x0000ABD4 File Offset: 0x00008FD4
		private static bool isMemberwiseEqualityObject(FieldInfo field)
		{
			return typeof(MemberwiseEqualityObject).IsAssignableFrom(field.FieldType);
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x0000ABEC File Offset: 0x00008FEC
		public string GetMemberStringByIndex(int count)
		{
			Type type = base.GetType();
			FieldInfo[] array = (from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			orderby f.Name
			select f).ToArray<FieldInfo>();
			int num = 0;
			foreach (FieldInfo fieldInfo in array)
			{
				if (!MemberwiseEqualityObject.ignoreEqualityComparison(fieldInfo))
				{
					if (MemberwiseEqualityObject.isUnhandledDictionary(fieldInfo))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Unhandled dictionary ",
							fieldInfo.Name,
							" in ",
							type,
							"; tag with IgnoreRollbackValidation or remove"
						}));
					}
					else
					{
						if (count == num)
						{
							return fieldInfo.Name + ": " + fieldInfo.GetValue(this);
						}
						num++;
					}
				}
			}
			return "(null)";
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x0000ACD0 File Offset: 0x000090D0
		public object GetMemberByIndex(int count)
		{
			Type type = base.GetType();
			FieldInfo[] array = (from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			orderby f.Name
			select f).ToArray<FieldInfo>();
			int num = 0;
			foreach (FieldInfo fieldInfo in array)
			{
				if (!MemberwiseEqualityObject.ignoreEqualityComparison(fieldInfo))
				{
					if (MemberwiseEqualityObject.isUnhandledDictionary(fieldInfo))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Unhandled dictionary ",
							fieldInfo.Name,
							" in ",
							type,
							"; tag with IgnoreRollbackValidation or remove"
						}));
					}
					else
					{
						if (count == num)
						{
							return fieldInfo.GetValue(this);
						}
						num++;
					}
				}
			}
			return null;
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x0000AD9C File Offset: 0x0000919C
		public int GetSubHash(int count)
		{
			Type type = base.GetType();
			int num = 0;
			FieldInfo[] array = (from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			orderby f.Name
			select f).ToArray<FieldInfo>();
			int num2 = 0;
			foreach (FieldInfo fieldInfo in array)
			{
				if (!MemberwiseEqualityObject.ignoreEqualityComparison(fieldInfo))
				{
					if (MemberwiseEqualityObject.isUnhandledDictionary(fieldInfo))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Unhandled dictionary ",
							fieldInfo.Name,
							" in ",
							type,
							"; tag with IgnoreRollbackValidation or remove"
						}));
					}
					else
					{
						object value = fieldInfo.GetValue(this);
						if (value is MemberwiseEqualityObject)
						{
							num ^= ((MemberwiseEqualityObject)value).GetMemberwiseHashCode();
						}
						else
						{
							num ^= fieldInfo.GetValue(this).GetHashCode();
						}
						num2++;
						if (num2 >= count)
						{
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x0000AEA8 File Offset: 0x000092A8
		[SkipRename]
		public int GetMemberwiseHashCode()
		{
			if (this is ISituationalValidation && !((ISituationalValidation)this).ShouldValidate)
			{
				return 0;
			}
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return base.GetHashCode();
			}
			int result = 0;
			try
			{
				result = MemberwiseEqualityObject._functions[base.GetType()].GetHashCodeFunc(this);
			}
			catch (Exception ex)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Error getting hash code for type ",
					base.GetType(),
					": ",
					ex
				}));
			}
			return result;
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x0000AF4C File Offset: 0x0000934C
		public void Assign(object source)
		{
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return;
			}
			MemberwiseEqualityObject._functions[base.GetType()].AssignAction(this, source);
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x0000AF75 File Offset: 0x00009375
		public void ShallowAssign(object source)
		{
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return;
			}
			MemberwiseEqualityObject._functions[base.GetType()].ShallowAssignAction(this, source);
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x0000AF9E File Offset: 0x0000939E
		[SkipRename]
		public virtual bool MemberwiseEquals(object obj)
		{
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return base.Equals(obj);
			}
			return MemberwiseEqualityObject._functions[base.GetType()].EqualsFunc(this, obj);
		}

		// Token: 0x040034B8 RID: 13496
		public static bool MemberwiseEqualityEnabled = true;

		// Token: 0x040034B9 RID: 13497
		private static Dictionary<Type, MemberwiseEqualityObject.MemberwiseFunctions> _functions = new Dictionary<Type, MemberwiseEqualityObject.MemberwiseFunctions>();

		// Token: 0x02000B2D RID: 2861
		private class MemberwiseFunctions
		{
			// Token: 0x040034BD RID: 13501
			public Func<object, object, bool> EqualsFunc;

			// Token: 0x040034BE RID: 13502
			public Func<object, int> GetHashCodeFunc;

			// Token: 0x040034BF RID: 13503
			public Action<object, object> AssignAction;

			// Token: 0x040034C0 RID: 13504
			public Action<object, object> ShallowAssignAction;
		}
	}
}
