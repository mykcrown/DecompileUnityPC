// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Reflection;

namespace ReflectionUtils
{
	public static class ReflectionUtils
	{
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
