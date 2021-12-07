// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

public static class GameStateValidator
{
	private static HashSet<Type> validatedTypes;

	static GameStateValidator()
	{
		GameStateValidator.validatedTypes = new HashSet<Type>();
		foreach (Type current in GameState.GetAllGameStateTypes())
		{
			GameStateValidator.Validate(current);
		}
	}

	public static void Validate(Type type)
	{
		if (!GameStateValidator.validatedTypes.Contains(type))
		{
			GameStateValidator.validatedTypes.Add(type);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Rollback State Class is invalid:\n");
			if (!GameStateValidator.validateType(type, stringBuilder))
			{
				UnityEngine.Debug.LogError(stringBuilder.ToString());
			}
		}
	}

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
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo field = fields[i];
			flag |= !GameStateValidator.validateField(field, errorBuilder);
		}
		return !flag;
	}

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
}
