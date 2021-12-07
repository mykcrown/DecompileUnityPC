using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000881 RID: 2177
[Serializable]
public abstract class RollbackState : CloneableObject
{
	// Token: 0x060036A9 RID: 13993 RVA: 0x0000B024 File Offset: 0x00009424
	public RollbackState()
	{
		if (RollbackState.validateDeterminism && !RollbackState.validatedTypes.Contains(base.GetType()))
		{
			RollbackState.validatedTypes.Add(base.GetType());
			string message = "Rollback State Class is invalid:\n";
			if (!this.validateType(base.GetType(), ref message))
			{
				Debug.LogError(message);
			}
		}
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x0000B086 File Offset: 0x00009486
	public virtual void Clear()
	{
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x0000B088 File Offset: 0x00009488
	protected void copyDictionary<TKey, TValue>(Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> target)
	{
		target.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
		{
			target[keyValuePair.Key] = source[keyValuePair.Key];
		}
	}

	// Token: 0x060036AC RID: 13996 RVA: 0x0000B0F8 File Offset: 0x000094F8
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

	// Token: 0x060036AD RID: 13997 RVA: 0x0000B18C File Offset: 0x0000958C
	private bool validateType(Type type, ref string errorMessage)
	{
		errorMessage = errorMessage + type + ":\n\t";
		bool flag = false;
		if (!type.IsSerializable)
		{
			flag = true;
			errorMessage += "Not serializable\n";
		}
		foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
		{
			flag |= !this.validateField(field, ref errorMessage);
		}
		return !flag;
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x0000B1F8 File Offset: 0x000095F8
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

	// Token: 0x04002536 RID: 9526
	private static readonly bool validateDeterminism = true;

	// Token: 0x04002537 RID: 9527
	private static HashSet<Type> validatedTypes = new HashSet<Type>();
}
