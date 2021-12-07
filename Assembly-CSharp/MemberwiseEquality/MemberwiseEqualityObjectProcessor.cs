using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MemberwiseEquality
{
	// Token: 0x02000B2F RID: 2863
	public static class MemberwiseEqualityObjectProcessor
	{
		// Token: 0x06005313 RID: 21267 RVA: 0x001AD510 File Offset: 0x001AB910
		public static void ProcessType(Type type, ProcessContext context)
		{
			try
			{
				foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					context.OnBeginProcessField(fieldInfo, type);
					if (MemberwiseEqualityObjectProcessor.IgnoreEqualityComparison(fieldInfo))
					{
						context.IsIgnoreEqualityComparison(fieldInfo, type);
					}
					else if (MemberwiseEqualityObjectProcessor.IsUnhandledDictionary(fieldInfo))
					{
						context.IsUnhandledDictionary(fieldInfo, type);
					}
					else
					{
						if (fieldInfo.FieldType.IsEnum)
						{
							context.IsEnum(fieldInfo, type);
						}
						else if (fieldInfo.FieldType.IsValueType)
						{
							context.IsValueType(fieldInfo, type);
						}
						else
						{
							if (fieldInfo.FieldType.IsInterface)
							{
								context.IsInterface(fieldInfo, type);
							}
							else if (MemberwiseEqualityObjectProcessor.isDictionary(fieldInfo))
							{
								context.IsDictionary(fieldInfo, type);
							}
							else if (MemberwiseEqualityObjectProcessor.isFixedCapacityList(fieldInfo))
							{
								context.IsFixedCapacityList(fieldInfo, type);
							}
							else if (MemberwiseEqualityObjectProcessor.isEnumerable(fieldInfo))
							{
								context.IsEnumerable(fieldInfo, type);
							}
							else if (MemberwiseEqualityObjectProcessor.isMemberwiseEqualityObject(fieldInfo))
							{
								context.IsMemberwiseEqualityObject(fieldInfo, type);
							}
							else
							{
								context.IsGenericObject(fieldInfo, type);
							}
							context.OnProcessedReferenceField(fieldInfo, type);
						}
						context.OnProcessedField(fieldInfo, type);
					}
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Unhandled processor field: " + arg);
			}
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x001AD6C0 File Offset: 0x001ABAC0
		public static bool IgnoreEqualityComparison(FieldInfo field)
		{
			return field.GetCustomAttributes(typeof(IgnoreRollbackAttribute), true).Length > 0;
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x001AD6D8 File Offset: 0x001ABAD8
		public static bool IsUnhandledDictionary(FieldInfo field)
		{
			Type[] genericArguments = field.FieldType.GetGenericArguments();
			return MemberwiseEqualityObjectProcessor.isDictionary(field) && ((!genericArguments[0].IsValueType && !(genericArguments[0] == typeof(string))) || (!genericArguments[1].IsValueType && !(genericArguments[1] == typeof(string))));
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x001AD74C File Offset: 0x001ABB4C
		private static bool isDictionary(FieldInfo field)
		{
			if (field.FieldType == typeof(string) || !field.FieldType.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = field.FieldType.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(Dictionary<, >) || genericTypeDefinition == typeof(SerializableDictionary<, >);
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x001AD7B9 File Offset: 0x001ABBB9
		private static bool isEnumerable(FieldInfo field)
		{
			return field.FieldType != typeof(string) && field.FieldType.GetInterface(typeof(IEnumerable).FullName) != null;
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x001AD7F8 File Offset: 0x001ABBF8
		private static bool isFixedCapacityList(FieldInfo field)
		{
			if (field.FieldType == typeof(string) || !field.FieldType.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = field.FieldType.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(FixedCapacityList<>);
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x001AD84D File Offset: 0x001ABC4D
		private static bool isMemberwiseEqualityObject(FieldInfo field)
		{
			return typeof(MemberwiseEqualityObject).IsAssignableFrom(field.FieldType);
		}
	}
}
