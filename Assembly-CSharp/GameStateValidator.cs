using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

// Token: 0x0200086D RID: 2157
public static class GameStateValidator
{
	// Token: 0x060035D4 RID: 13780 RVA: 0x000FE6EC File Offset: 0x000FCAEC
	static GameStateValidator()
	{
		foreach (Type type in GameState.GetAllGameStateTypes())
		{
			GameStateValidator.Validate(type);
		}
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x000FE750 File Offset: 0x000FCB50
	public static void Validate(Type type)
	{
		if (!GameStateValidator.validatedTypes.Contains(type))
		{
			GameStateValidator.validatedTypes.Add(type);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Rollback State Class is invalid:\n");
			if (!GameStateValidator.validateType(type, stringBuilder))
			{
				Debug.LogError(stringBuilder.ToString());
			}
		}
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x000FE7A4 File Offset: 0x000FCBA4
	private static bool validateType(Type type, StringBuilder errorBuilder)
	{
		errorBuilder.AppendFormat("{0}:\n\t", type);
		bool flag = false;
		if (!type.IsSerializable)
		{
			flag = true;
			errorBuilder.Append("Not serializable\n");
		}
		if (!type.IsAssignableFrom(typeof(GameState)))
		{
			flag = true;
			errorBuilder.Append("Is not a GameState\n");
		}
		foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
		{
			flag |= !GameStateValidator.validateField(field, errorBuilder);
		}
		return !flag;
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x000FE830 File Offset: 0x000FCC30
	private static bool validateField(FieldInfo field, StringBuilder errorBuilder)
	{
		if (field.GetCustomAttributes(typeof(IgnoreRollbackAttribute), true).Length > 0)
		{
			return true;
		}
		if (field.FieldType.IsPrimitive || field.FieldType.IsValueType || field.FieldType.IsEnum || field.FieldType == typeof(string))
		{
			return true;
		}
		if (!field.FieldType.IsSerializable)
		{
			errorBuilder.AppendFormat("{0}- not serializable,\n\t", field.Name);
			return false;
		}
		if (field.FieldType.IsArray)
		{
			bool flag = true;
			if (field.GetCustomAttributes(typeof(IsClonedManually), true).Length == 0)
			{
				flag = false;
				errorBuilder.AppendFormat("{0} ({1}) not supported; must clone manually and tag with [IsClonedManually],\n\t", field.Name, field.FieldType);
			}
			Type elementType = field.FieldType.GetElementType();
			if (!elementType.IsSerializable)
			{
				errorBuilder.AppendFormat("{0}- not serializable,\n\t", field.Name);
				flag = false;
			}
			return flag && GameStateValidator.validateType(elementType, errorBuilder);
		}
		if (typeof(IEnumerable).IsAssignableFrom(field.FieldType))
		{
			bool flag2 = true;
			if (field.GetCustomAttributes(typeof(IsClonedManually), true).Length == 0)
			{
				flag2 = false;
				errorBuilder.AppendFormat("{0}- not properly cloned,\n\t", field.Name);
			}
			Type[] genericArguments = field.FieldType.GetGenericArguments();
			if (genericArguments.Length > 0)
			{
				Type type = field.FieldType.GetGenericArguments()[0];
				if (!type.IsSerializable)
				{
					flag2 = false;
					errorBuilder.AppendFormat("{0} ({1}) not supported; must clone manually and tag with [IsClonedManually],\n\t", field.Name, field.FieldType);
				}
				return flag2 && GameStateValidator.validateType(type, errorBuilder);
			}
			errorBuilder.AppendFormat("Unrecognized IEnumerable {0},\n\t", field.FieldType);
			return false;
		}
		else
		{
			if (field.GetCustomAttributes(typeof(IsClonedManually), true).Length > 0)
			{
				return true;
			}
			errorBuilder.AppendFormat("{0} ({1}) not supported; must clone manually and tag with [IsClonedManually],\n\t", field.Name, field.FieldType);
			return false;
		}
	}

	// Token: 0x040024DB RID: 9435
	private static HashSet<Type> validatedTypes = new HashSet<Type>();
}
