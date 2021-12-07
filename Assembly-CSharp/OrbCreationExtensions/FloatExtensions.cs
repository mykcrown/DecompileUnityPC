using System;
using UnityEngine;

namespace OrbCreationExtensions
{
	// Token: 0x02000014 RID: 20
	public static class FloatExtensions
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00007D73 File Offset: 0x00006173
		public static string MakeString(this float aFloat)
		{
			return string.Empty + aFloat;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00007D88 File Offset: 0x00006188
		public static string MakeString(this float aFloat, int decimals)
		{
			if (decimals <= 0)
			{
				return string.Empty + Mathf.RoundToInt(aFloat);
			}
			string format = "{0:F" + decimals + "}";
			return string.Format(format, aFloat);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00007DD4 File Offset: 0x000061D4
		public static int MakeInt(this float aFloat)
		{
			return Mathf.FloorToInt(aFloat);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00007DDD File Offset: 0x000061DD
		public static bool MakeBool(this float aFloat)
		{
			return aFloat > 0f;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00007DE7 File Offset: 0x000061E7
		public static float MakeFloat(this float aFloat)
		{
			return aFloat;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00007DEA File Offset: 0x000061EA
		public static double MakeDouble(this float aFloat)
		{
			return (double)aFloat;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00007DEE File Offset: 0x000061EE
		public static string MakeString(this double aDouble)
		{
			return string.Empty + aDouble;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007E00 File Offset: 0x00006200
		public static string MakeString(this double aDouble, int decimals)
		{
			if (decimals <= 0)
			{
				int num = (int)aDouble;
				if (num >= 0 && aDouble - (double)num >= 0.5)
				{
					num++;
				}
				if (num < 0 && aDouble - (double)num <= -0.5)
				{
					num--;
				}
				return string.Empty + num;
			}
			string format = "{0:F" + decimals + "}";
			return string.Format(format, aDouble);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00007E84 File Offset: 0x00006284
		public static int MakeInt(this double aDouble)
		{
			return (int)aDouble;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007E88 File Offset: 0x00006288
		public static bool MakeBool(this double aDouble)
		{
			return aDouble > 0.0;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007E96 File Offset: 0x00006296
		public static float MakeFloat(this double aDouble)
		{
			return (float)aDouble;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007E9A File Offset: 0x0000629A
		public static double MakeDouble(this double aDouble)
		{
			return aDouble;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007E9D File Offset: 0x0000629D
		public static float To180Angle(this float f)
		{
			while (f <= -180f)
			{
				f += 360f;
			}
			while (f > 180f)
			{
				f -= 360f;
			}
			return f;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007ED2 File Offset: 0x000062D2
		public static float To360Angle(this float f)
		{
			while (f < 0f)
			{
				f += 360f;
			}
			while (f >= 360f)
			{
				f -= 360f;
			}
			return f;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007F07 File Offset: 0x00006307
		public static float RadToCompassAngle(this float rad)
		{
			return (rad * 57.29578f).DegreesToCompassAngle();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007F15 File Offset: 0x00006315
		public static float DegreesToCompassAngle(this float angle)
		{
			angle = 90f - angle;
			return angle.To360Angle();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00007F28 File Offset: 0x00006328
		public static float CompassAngleLerp(this float from, float to, float portion)
		{
			float num = (to - from).To180Angle();
			num *= Mathf.Clamp01(portion);
			return (from + num).To360Angle();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007F50 File Offset: 0x00006350
		public static float RelativePositionBetweenAngles(this float angle, float from, float to)
		{
			from = from.To360Angle();
			to = to.To360Angle();
			if (from - to > 180f)
			{
				from -= 360f;
			}
			if (to - from > 180f)
			{
				to -= 360f;
			}
			angle = angle.To360Angle();
			if (from < to)
			{
				if (angle >= from && angle < to)
				{
					return (angle - from) / (to - from);
				}
				if (angle - 360f >= from && angle - 360f < to)
				{
					return (angle - 360f - from) / (to - from);
				}
			}
			if (from > to)
			{
				if (angle < from && angle >= to)
				{
					return (angle - to) / (from - to);
				}
				if (angle - 360f < from && angle - 360f >= to)
				{
					return (angle - 360f - to) / (from - to);
				}
			}
			return -1f;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00008030 File Offset: 0x00006430
		public static float Distance(this float f1, float f2)
		{
			return Mathf.Abs(f1 - f2);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000803C File Offset: 0x0000643C
		public static float Round(this float f, int decimals)
		{
			float num = Mathf.Pow(10f, (float)decimals);
			f = Mathf.Round(f * num);
			return f / num;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00008063 File Offset: 0x00006463
		public static float Cube(this float f)
		{
			return f * f;
		}
	}
}
