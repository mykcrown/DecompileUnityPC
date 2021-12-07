// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public abstract class RollbackState : CloneableObject
{
	private static readonly bool validateDeterminism = true;

	private static HashSet<Type> validatedTypes = new HashSet<Type>();

	public RollbackState()
	{
		if (RollbackState.validateDeterminism && !RollbackState.validatedTypes.Contains(base.GetType()))
		{
			RollbackState.validatedTypes.Add(base.GetType());
			string message = "Rollback State Class is invalid:\n";
			if (!this.validateType(base.GetType(), ref message))
			{
				UnityEngine.Debug.LogError(message);
			}
		}
	}

	public virtual void Clear()
	{
	}

	protected void copyDictionary<TKey, TValue>(Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> target)
	{
		target.Clear();
		foreach (KeyValuePair<TKey, TValue> current in source)
		{
			target[current.Key] = source[current.Key];
		}
	}

	protected void copyList<T>(List<T> source, List<T> target)
	{
		if (target.Capacity < source.Capacity)
		{
			throw new UnityException(string.Concat(new object[]
			{
				"Insufficient capacity in list ",
				source,
				" ",
				source.Capacity,
				" => ",
				target.Capacity
			}));
		}
		target.Clear();
		int count = source.Count;
		for (int i = 0; i < count; i++)
		{
			target.Add(source[i]);
		}
	}

	private bool validateType(Type type, ref string errorMessage)
	{
		errorMessage = errorMessage + type + ":\n\t";
		bool flag = false;
		if (!type.IsSerializable)
		{
			flag = true;
			errorMessage += "Not serializable\n";
		}
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo field = fields[i];
			flag |= !this.validateField(field, ref errorMessage);
		}
		return !flag;
	}

	private bool validateField(FieldInfo field, ref string errorMessage)
	{
		if (field.GetCustomAttributes(typeof(IgnoreRollbackAttribute), true).Length > 0)
		{
			return true;
		}
		if (field.FieldType == typeof(float) && field.GetCustomAttributes(typeof(IgnoreFloatValidation), false).Length == 0)
		{
			errorMessage = errorMessage + field.Name + "- is a float, used Fixed instead!  If you need the field to be a float (visual data or something) use the IgnoreFloatValidation attribute.\n\t";
			return false;
		}
		if (field.FieldType.IsPrimitive || field.FieldType.IsValueType || field.FieldType.IsEnum || field.FieldType == typeof(string))
		{
			return true;
		}
		if (!field.FieldType.IsSerializable)
		{
			errorMessage = errorMessage + field.Name + "- not serializable,\n\t";
			return false;
		}
		if (field.FieldType.IsArray)
		{
			bool flag = true;
			if (field.GetCustomAttributes(typeof(IsClonedManually), true).Length == 0)
			{
				flag = false;
				errorMessage = errorMessage + field.Name + "- not properly cloned,\n\t";
			}
			Type elementType = field.FieldType.GetElementType();
			if (!elementType.IsSerializable)
			{
				errorMessage = errorMessage + field.Name + "- not serializable,\n\t";
				flag = false;
			}
			return flag && this.validateType(elementType, ref errorMessage);
		}
		if (typeof(IEnumerable).IsAssignableFrom(field.FieldType))
		{
			bool flag2 = true;
			if (field.GetCustomAttributes(typeof(IsClonedManually), true).Length == 0)
			{
				flag2 = false;
				errorMessage = errorMessage + field.Name + "- not properly cloned,\n\t";
			}
			Type[] genericArguments = field.FieldType.GetGenericArguments();
			if (genericArguments.Length > 0)
			{
				Type type = field.FieldType.GetGenericArguments()[0];
				if (!type.IsSerializable)
				{
					errorMessage = errorMessage + field.Name + "- not serializable,\n\t";
					flag2 = false;
				}
				return flag2 && this.validateType(type, ref errorMessage);
			}
			string text = errorMessage;
			errorMessage = string.Concat(new object[]
			{
				text,
				"Unrecognized IEnumerable ",
				field.FieldType,
				",\n\t"
			});
			return false;
		}
		else
		{
			if (field.GetCustomAttributes(typeof(IsClonedManually), true).Length > 0)
			{
				return true;
			}
			string text = errorMessage;
			errorMessage = string.Concat(new object[]
			{
				text,
				field.Name,
				" (",
				field.FieldType,
				") not supported; must clone manually and tag with [IsClonedManually],\n\t"
			});
			return false;
		}
	}
}
