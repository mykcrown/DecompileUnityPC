// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace OrbCreationExtensions
{
	public static class ColorExtensions
	{
		public static string MakeString(this Color aColor)
		{
			return aColor.MakeString(false);
		}

		public static string MakeString(this Color aColor, bool includeAlpha)
		{
			return aColor.MakeString(includeAlpha);
		}

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

		public static Color MakeColor(this Color aValue)
		{
			return aValue;
		}

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

		public static float Hue(this Color c)
		{
			return c.MakeHSB().x;
		}

		public static float Saturation(this Color c)
		{
			return c.MakeHSB().y;
		}

		public static float Brightness(this Color c)
		{
			return c.MakeHSB().z;
		}

		public static float GetColorDistance(this Color color1, Color color2)
		{
			Vector3 a = new Vector3(color1.r, color1.g, color1.b);
			Vector3 b = new Vector3(color2.r, color2.g, color2.b);
			return Vector3.Distance(a, b);
		}

		public static float GrayScale(this Color color)
		{
			return (color.r + color.g + color.b) / 3f;
		}
	}
}
