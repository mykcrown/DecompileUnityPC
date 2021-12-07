// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using MemberwiseEquality;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MemberwiseEquality
{
	[Serializable]
	public class MemberwiseEqualityObject
	{
		private class MemberwiseFunctions
		{
			public Func<object, object, bool> EqualsFunc;

			public Func<object, int> GetHashCodeFunc;

			public Action<object, object> AssignAction;

			public Action<object, object> ShallowAssignAction;
		}

		public static bool MemberwiseEqualityEnabled;

		private static Dictionary<Type, MemberwiseEqualityObject.MemberwiseFunctions> _functions;

		private static Func<FieldInfo, string> __f__am_cache0;

		private static Func<FieldInfo, string> __f__am_cache1;

		private static Func<FieldInfo, string> __f__am_cache2;

		static MemberwiseEqualityObject()
		{
			MemberwiseEqualityObject.MemberwiseEqualityEnabled = true;
			MemberwiseEqualityObject._functions = new Dictionary<Type, MemberwiseEqualityObject.MemberwiseFunctions>();
		}

		public MemberwiseEqualityObject()
		{
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return;
			}
			MemberwiseEqualityObject.registerMemberwiseType(base.GetType());
		}

		public static void RecursivelyRegisterMemberwiseTypes(Type type)
		{
			if (typeof(MemberwiseEqualityObject).IsAssignableFrom(type))
			{
				MemberwiseEqualityObject.registerMemberwiseType(type);
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
					MemberwiseEqualityObject.RecursivelyRegisterMemberwiseTypes(fieldInfo.FieldType);
				}
			}
		}

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

		private static bool ignoreEqualityComparison(FieldInfo field)
		{
			return field.GetCustomAttributes(typeof(IgnoreRollbackAttribute), true).Length > 0;
		}

		private static bool isUnhandledDictionary(FieldInfo field)
		{
			Type[] genericArguments = field.FieldType.GetGenericArguments();
			return MemberwiseEqualityObject.isDictionary(field) && ((!genericArguments[0].IsValueType && !(genericArguments[0] == typeof(string))) || (!genericArguments[1].IsValueType && !(genericArguments[1] == typeof(string))));
		}

		private static bool isDictionary(FieldInfo field)
		{
			if (field.FieldType == typeof(string) || !field.FieldType.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = field.FieldType.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(Dictionary<, >) || genericTypeDefinition == typeof(SerializableDictionary<, >);
		}

		private static bool isEnumerable(FieldInfo field)
		{
			return field.FieldType != typeof(string) && field.FieldType.GetInterface(typeof(IEnumerable).FullName) != null;
		}

		private static bool isMemberwiseEqualityObject(FieldInfo field)
		{
			return typeof(MemberwiseEqualityObject).IsAssignableFrom(field.FieldType);
		}

		public string GetMemberStringByIndex(int count)
		{
			Type type = base.GetType();
			IEnumerable<FieldInfo> arg_2C_0 = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (MemberwiseEqualityObject.__f__am_cache0 == null)
			{
				MemberwiseEqualityObject.__f__am_cache0 = new Func<FieldInfo, string>(MemberwiseEqualityObject._GetMemberStringByIndex_m__0);
			}
			FieldInfo[] array = arg_2C_0.OrderBy(MemberwiseEqualityObject.__f__am_cache0).ToArray<FieldInfo>();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				if (!MemberwiseEqualityObject.ignoreEqualityComparison(fieldInfo))
				{
					if (MemberwiseEqualityObject.isUnhandledDictionary(fieldInfo))
					{
						UnityEngine.Debug.LogError(string.Concat(new object[]
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

		public object GetMemberByIndex(int count)
		{
			Type type = base.GetType();
			IEnumerable<FieldInfo> arg_2C_0 = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (MemberwiseEqualityObject.__f__am_cache1 == null)
			{
				MemberwiseEqualityObject.__f__am_cache1 = new Func<FieldInfo, string>(MemberwiseEqualityObject._GetMemberByIndex_m__1);
			}
			FieldInfo[] array = arg_2C_0.OrderBy(MemberwiseEqualityObject.__f__am_cache1).ToArray<FieldInfo>();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				if (!MemberwiseEqualityObject.ignoreEqualityComparison(fieldInfo))
				{
					if (MemberwiseEqualityObject.isUnhandledDictionary(fieldInfo))
					{
						UnityEngine.Debug.LogError(string.Concat(new object[]
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

		public int GetSubHash(int count)
		{
			Type type = base.GetType();
			int num = 0;
			IEnumerable<FieldInfo> arg_2E_0 = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (MemberwiseEqualityObject.__f__am_cache2 == null)
			{
				MemberwiseEqualityObject.__f__am_cache2 = new Func<FieldInfo, string>(MemberwiseEqualityObject._GetSubHash_m__2);
			}
			FieldInfo[] array = arg_2E_0.OrderBy(MemberwiseEqualityObject.__f__am_cache2).ToArray<FieldInfo>();
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				if (!MemberwiseEqualityObject.ignoreEqualityComparison(fieldInfo))
				{
					if (MemberwiseEqualityObject.isUnhandledDictionary(fieldInfo))
					{
						UnityEngine.Debug.LogError(string.Concat(new object[]
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
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Error getting hash code for type ",
					base.GetType(),
					": ",
					ex
				}));
			}
			return result;
		}

		public void Assign(object source)
		{
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return;
			}
			MemberwiseEqualityObject._functions[base.GetType()].AssignAction(this, source);
		}

		public void ShallowAssign(object source)
		{
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return;
			}
			MemberwiseEqualityObject._functions[base.GetType()].ShallowAssignAction(this, source);
		}

		[SkipRename]
		public virtual bool MemberwiseEquals(object obj)
		{
			if (!MemberwiseEqualityObject.MemberwiseEqualityEnabled)
			{
				return base.Equals(obj);
			}
			return MemberwiseEqualityObject._functions[base.GetType()].EqualsFunc(this, obj);
		}

		private static string _GetMemberStringByIndex_m__0(FieldInfo f)
		{
			return f.Name;
		}

		private static string _GetMemberByIndex_m__1(FieldInfo f)
		{
			return f.Name;
		}

		private static string _GetSubHash_m__2(FieldInfo f)
		{
			return f.Name;
		}
	}
}
