using System;
using UnityEngine;

namespace OrbCreationExtensions
{
	// Token: 0x02000013 RID: 19
	public static class ColorExtensions
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00007A19 File Offset: 0x00005E19
		public static string MakeString(this Color aColor)
		{
			return aColor.MakeString(false);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007A22 File Offset: 0x00005E22
		public static string MakeString(this Color aColor, bool includeAlpha)
		{
			return aColor.MakeString(includeAlpha);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00007A30 File Offset: 0x00005E30
		public static string MakeString(this Color32 aColor, bool includeAlpha)
		{
			string text = Convert.ToString(aColor.r, 16).ToUpper();
			string text2 = Convert.ToString(aColor.g, 16).ToUpper();
			string text3 = Convert.ToString(aColor.b, 16).ToUpper();
			string text4 = Convert.ToString(aColor.a, 16).ToUpper();
			while (text.Length < 2)
			{
				text = "0" + text;
			}
			while (text2.Length < 2)
			{
				text2 = "0" + text2;
			}
			while (text3.Length < 2)
			{
				text3 = "0" + text3;
			}
			while (text4.Length < 2)
			{
				text4 = "0" + text4;
			}
			if (includeAlpha)
			{
				return string.Concat(new string[]
				{
					"#",
					text,
					text2,
					text3,
					text4
				});
			}
			return "#" + text + text2 + text3;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00007B38 File Offset: 0x00005F38
		public static Color MakeColor(this Color aValue)
		{
			return aValue;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00007B3C File Offset: 0x00005F3C
		public static Vector3 MakeHSB(this Color c)
		{
			float num = Mathf.Min(c.r, Mathf.Min(c.g, c.b));
			float num2 = Mathf.Max(c.r, Mathf.Max(c.g, c.b));
			float num3 = num2 - num;
			float num4 = 0f;
			float z = num2;
			if (num2 == c.r)
			{
				if (c.g >= c.b)
				{
					if (num3 == 0f)
					{
						num4 = 0f;
					}
					else
					{
						num4 = 60f * (c.g - c.b) / num3;
					}
				}
				else if (c.g < c.b)
				{
					num4 = 60f * (c.g - c.b) / num3 + 360f;
				}
			}
			else if (num2 == c.g)
			{
				num4 = 60f * (c.b - c.r) / num3 + 120f;
			}
			else if (num2 == c.b)
			{
				num4 = 60f * (c.r - c.g) / num3 + 240f;
			}
			float y;
			if (num2 == 0f)
			{
				y = 0f;
			}
			else
			{
				y = 1f - num / num2;
			}
			return new Vector3(num4 / 360f, y, z);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00007CB4 File Offset: 0x000060B4
		public static float Hue(this Color c)
		{
			return c.MakeHSB().x;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00007CD0 File Offset: 0x000060D0
		public static float Saturation(this Color c)
		{
			return c.MakeHSB().y;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00007CEC File Offset: 0x000060EC
		public static float Brightness(this Color c)
		{
			return c.MakeHSB().z;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00007D08 File Offset: 0x00006108
		public static float GetColorDistance(this Color color1, Color color2)
		{
			Vector3 a = new Vector3(color1.r, color1.g, color1.b);
			Vector3 b = new Vector3(color2.r, color2.g, color2.b);
			return Vector3.Distance(a, b);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00007D54 File Offset: 0x00006154
		public static float GrayScale(this Color color)
		{
			return (color.r + color.g + color.b) / 3f;
		}
	}
}
