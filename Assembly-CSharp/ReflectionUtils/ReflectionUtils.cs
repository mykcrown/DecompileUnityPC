using System;
using System.Reflection;

namespace ReflectionUtils
{
	// Token: 0x02000B59 RID: 2905
	public static class ReflectionUtils
	{
		// Token: 0x06005424 RID: 21540 RVA: 0x001B1060 File Offset: 0x001AF460
		public static T GetPrivatePropertyValue<T>(this object obj, string propName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			PropertyInfo property = obj.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
			}
			return (T)((object)property.GetValue(obj, null));
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x001B10C8 File Offset: 0x001AF4C8
		public static T GetPrivateFieldValue<T>(this object obj, string propName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Type type = obj.GetType();
			FieldInfo fieldInfo = null;
			while (fieldInfo == null && type != null)
			{
				fieldInfo = type.GetField(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				type = type.BaseType;
			}
			if (fieldInfo == null)
			{
				throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
			}
			return (T)((object)fieldInfo.GetValue(obj));
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x001B1158 File Offset: 0x001AF558
		public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
		{
			Type type = obj.GetType();
			if (type.GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) == null)
			{
				throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
			}
			type.InvokeMember(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty, null, obj, new object[]
			{
				val
			});
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x001B11C0 File Offset: 0x001AF5C0
		public static void SetPrivateFieldValue<T>(this object obj, string propName, T val)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Type type = obj.GetType();
			FieldInfo fieldInfo = null;
			while (fieldInfo == null && type != null)
			{
				fieldInfo = type.GetField(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				type = type.BaseType;
			}
			if (fieldInfo == null)
			{
				throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
			}
			fieldInfo.SetValue(obj, val);
		}
	}
}
