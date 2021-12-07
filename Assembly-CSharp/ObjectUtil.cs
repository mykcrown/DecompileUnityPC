using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

// Token: 0x02000B34 RID: 2868
public class ObjectUtil
{
	// Token: 0x06005336 RID: 21302 RVA: 0x001AE70C File Offset: 0x001ACB0C
	public static T Convert<T>(object value)
	{
		Type typeFromHandle = typeof(T);
		if (typeFromHandle == ObjectUtil.stringType)
		{
			return (T)((object)value);
		}
		if (typeFromHandle == ObjectUtil.intType)
		{
			int num;
			int.TryParse((string)value, out num);
			return (T)((object)num);
		}
		if (typeFromHandle == ObjectUtil.floatType)
		{
			float num2;
			float.TryParse((string)value, out num2);
			return (T)((object)num2);
		}
		if (typeFromHandle == ObjectUtil.boolType)
		{
			bool flag;
			bool.TryParse((string)value, out flag);
			return (T)((object)flag);
		}
		throw new UnityException("Unhandled value type");
	}

	// Token: 0x06005337 RID: 21303 RVA: 0x001AE7C4 File Offset: 0x001ACBC4
	public static void FillFromHashtable(Hashtable data, object target)
	{
		Type type = target.GetType();
		IEnumerator enumerator = data.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string text = (string)obj;
				FieldInfo field = type.GetField(text);
				if (field != null)
				{
					if (field.FieldType == typeof(bool) && data[text].GetType() == typeof(string))
					{
						field.SetValue(target, ObjectUtil.Convert<bool>(data[text]));
					}
					else if (field.FieldType == typeof(int) && data[text].GetType() == typeof(string))
					{
						field.SetValue(target, ObjectUtil.Convert<int>(data[text]));
					}
					else if (field.FieldType == typeof(float) && data[text].GetType() == typeof(string))
					{
						field.SetValue(target, ObjectUtil.Convert<float>(data[text]));
					}
					else
					{
						field.SetValue(target, data[text]);
					}
				}
				else
				{
					PropertyInfo property = type.GetProperty(text);
					if (property != null)
					{
						if (property.PropertyType == typeof(bool) && data[text].GetType() == typeof(string))
						{
							property.SetValue(target, ObjectUtil.Convert<bool>(data[text]), null);
						}
						else if (property.PropertyType == typeof(int) && data[text].GetType() == typeof(string))
						{
							property.SetValue(target, ObjectUtil.Convert<int>(data[text]), null);
						}
						else if (property.PropertyType == typeof(float) && data[text].GetType() == typeof(string))
						{
							property.SetValue(target, ObjectUtil.Convert<float>(data[text]), null);
						}
						else
						{
							property.SetValue(target, data[text], null);
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x040034DA RID: 13530
	private static Type stringType = typeof(string);

	// Token: 0x040034DB RID: 13531
	private static Type intType = typeof(int);

	// Token: 0x040034DC RID: 13532
	private static Type floatType = typeof(float);

	// Token: 0x040034DD RID: 13533
	private static Type boolType = typeof(bool);
}
