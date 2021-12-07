// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Reflection;

public class ProcessContext
{
	public Action<FieldInfo, Type> IsIgnoreEqualityComparison;

	public Action<FieldInfo, Type> IsUnhandledDictionary;

	public Action<FieldInfo, Type> OnBeginProcessField;

	public Action<FieldInfo, Type> IsEnum;

	public Action<FieldInfo, Type> IsValueType;

	public Action<FieldInfo, Type> IsInterface;

	public Action<FieldInfo, Type> IsDictionary;

	public Action<FieldInfo, Type> IsEnumerable;

	public Action<FieldInfo, Type> IsFixedCapacityList;

	public Action<FieldInfo, Type> IsMemberwiseEqualityObject;

	public Action<FieldInfo, Type> IsGenericObject;

	public Action<FieldInfo, Type> OnProcessedReferenceField;

	public Action<FieldInfo, Type> OnProcessedField;

	public static void NoOp(FieldInfo field, Type type)
	{
	}
}
