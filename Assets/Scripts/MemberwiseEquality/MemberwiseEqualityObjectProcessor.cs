// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MemberwiseEquality
{
	public static class MemberwiseEqualityObjectProcessor
	{
		public static void ProcessType(Type type, ProcessContext context)
		{
			try
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
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
				UnityEngine.Debug.LogError("Unhandled processor field: " + arg);
			}
		}

		public static bool IgnoreEqualityComparison(FieldInfo field)
		{
			return field.GetCustomAttributes(typeof(IgnoreRollbackAttribute), true).Length > 0;
		}

		public static bool IsUnhandledDictionary(FieldInfo field)
		{
			Type[] genericArguments = field.FieldType.GetGenericArguments();
			return MemberwiseEqualityObjectProcessor.isDictionary(field) && ((!genericArguments[0].IsValueType && !(genericArguments[0] == typeof(string))) || (!genericArguments[1].IsValueType && !(genericArguments[1] == typeof(string))));
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

		private static bool isFixedCapacityList(FieldInfo field)
		{
			if (field.FieldType == typeof(string) || !field.FieldType.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = field.FieldType.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(FixedCapacityList<>);
		}

		private static bool isMemberwiseEqualityObject(FieldInfo field)
		{
			return typeof(MemberwiseEqualityObject).IsAssignableFrom(field.FieldType);
		}
	}
}
