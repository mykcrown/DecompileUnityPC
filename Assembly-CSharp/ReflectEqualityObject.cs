using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x02000B30 RID: 2864
public class ReflectEqualityObject
{
	// Token: 0x0600531B RID: 21275 RVA: 0x001AD870 File Offset: 0x001ABC70
	public ReflectEqualityObject()
	{
		Type type = base.GetType();
		if (!ReflectEqualityObject._properties.ContainsKey(type))
		{
			ReflectEqualityObject._properties[type] = type.GetProperties();
		}
	}

	// Token: 0x0600531C RID: 21276 RVA: 0x001AD8AC File Offset: 0x001ABCAC
	public override bool Equals(object obj)
	{
		Type type = base.GetType();
		if (type.IsAssignableFrom(obj.GetType()))
		{
			foreach (PropertyInfo propertyInfo in ReflectEqualityObject._properties[type])
			{
				if (!propertyInfo.GetValue(this, null).Equals(propertyInfo.GetValue(obj, null)))
				{
					return false;
				}
			}
			return true;
		}
		return base.Equals(obj);
	}

	// Token: 0x0600531D RID: 21277 RVA: 0x001AD91C File Offset: 0x001ABD1C
	public override int GetHashCode()
	{
		int num = 0;
		foreach (PropertyInfo propertyInfo in ReflectEqualityObject._properties[base.GetType()])
		{
			num ^= propertyInfo.GetValue(this, null).GetHashCode();
		}
		return num;
	}

	// Token: 0x040034CE RID: 13518
	private static Dictionary<Type, PropertyInfo[]> _properties = new Dictionary<Type, PropertyInfo[]>();
}
