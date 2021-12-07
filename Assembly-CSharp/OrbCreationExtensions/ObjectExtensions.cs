using System;
using UnityEngine;

namespace OrbCreationExtensions
{
	// Token: 0x02000017 RID: 23
	public static class ObjectExtensions
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00008180 File Offset: 0x00006580
		public static string MakeString(this object anObject)
		{
			if (anObject == null)
			{
				return string.Empty;
			}
			if (anObject.GetType() == typeof(string))
			{
				return (string)anObject;
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeString();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeString();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeString();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeString();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeString();
			}
			return string.Empty + anObject;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00008284 File Offset: 0x00006684
		public static int MakeInt(this object anObject)
		{
			if (anObject == null)
			{
				return 0;
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeInt();
			}
			return 0;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00008380 File Offset: 0x00006780
		public static bool MakeBool(this object anObject)
		{
			if (anObject == null)
			{
				return false;
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeBool();
			}
			return !(anObject.GetType() == typeof(string)) || ((string)anObject).MakeBool();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000847C File Offset: 0x0000687C
		public static float MakeFloat(this object anObject)
		{
			if (anObject == null)
			{
				return 0f;
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeFloat();
			}
			return 0f;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00008580 File Offset: 0x00006980
		public static double MakeDouble(this object anObject)
		{
			if (anObject == null)
			{
				return 0.0;
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeDouble();
			}
			return 0.0;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000868C File Offset: 0x00006A8C
		public static Color MakeColor(this object anObject)
		{
			if (anObject == null)
			{
				new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				if (anObject.GetType() == typeof(Color))
				{
					return ((Color)anObject).MakeColor();
				}
				if (anObject.GetType() == typeof(string))
				{
					return ((string)anObject).MakeColor();
				}
				if (anObject.GetType() == typeof(Vector3))
				{
					return ((Vector3)anObject).MakeColor();
				}
				if (anObject.GetType() == typeof(Vector4))
				{
					return ((Vector4)anObject).MakeColor();
				}
			}
			return new Color(0f, 0f, 0f, 0f);
		}
	}
}
