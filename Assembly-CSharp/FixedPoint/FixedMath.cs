using System;
using UnityEngine;

namespace FixedPoint
{
	// Token: 0x02000B1E RID: 2846
	public class FixedMath
	{
		// Token: 0x060051CA RID: 20938 RVA: 0x00152E84 File Offset: 0x00151284
		public static Fixed Sqrt(Fixed f)
		{
			long num = (long)Math.Sqrt((double)(f.RawValue << 8));
			Fixed result;
			result.RawValue = num << FixedMath.sqrtResultShift;
			return result;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x00152EB4 File Offset: 0x001512B4
		public static Fixed Sin(Fixed i)
		{
			while (i < 0)
			{
				i += FixedMath.TwoPI;
			}
			if (i > FixedMath.TwoPI)
			{
				i %= FixedMath.TwoPI;
			}
			int integerPart = (i * FixedMath.scaledDegrees).IntegerPart;
			return SineTableData.SINE_TABLE[integerPart];
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x00152F20 File Offset: 0x00151320
		public static void GenerateSineTable(int shift)
		{
			string text = string.Empty;
			text += "namespace FixedPoint\n{\n\tpublic class SineTableData\n\t{\n\t\tpublic static Fixed[] SINE_TABLE =\n\t\t{\n";
			for (int i = 0; i < 361; i++)
			{
				for (int j = 0; j < FixedMath.degreesPrecision; j++)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"\t\t\tFixed.Create(",
						(int)((Fixed)((double)Mathf.Sin(((float)i + (float)j / (float)FixedMath.degreesPrecision) * 0.017453292f))).RawValue,
						", false), "
					});
					if (j == FixedMath.degreesPrecision - 1)
					{
						text += "\n";
					}
				}
			}
			text += "\t\t};\n\t}\n}";
			Serialization.WriteString("Assets/Wavedash/Scripts/Util/Math/SineTableData.cs", text);
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x00152FEC File Offset: 0x001513EC
		private static Fixed mul(Fixed F1, Fixed F2)
		{
			return F1 * F2;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x00152FF5 File Offset: 0x001513F5
		public static Fixed Cos(Fixed i)
		{
			return FixedMath.Sin(i + FixedMath.HalfPI);
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x00153007 File Offset: 0x00151407
		public static Fixed Tan(Fixed i)
		{
			return FixedMath.Sin(i) / FixedMath.Cos(i);
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x0015301C File Offset: 0x0015141C
		public static Fixed Asin(Fixed f)
		{
			int num = (SineTableData.ASIN_TABLE.Length - 1) / 2;
			Fixed val = f * num + num;
			int num2 = FixedMath.Floor(val);
			int num3 = FixedMath.Ceil(val);
			Fixed fractionalPart = val.FractionalPart;
			Fixed @fixed = SineTableData.ASIN_TABLE[num2];
			Fixed b = @fixed;
			if (num3 < SineTableData.ASIN_TABLE.Length - 1)
			{
				b = SineTableData.ASIN_TABLE[num3];
			}
			return FixedMath.Lerp(@fixed, b, fractionalPart);
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x0015309C File Offset: 0x0015149C
		public static Fixed Atan(Fixed F)
		{
			if (F > 0)
			{
				if (F.IntegerPart >= 524288)
				{
					return FixedMath.PI / 2;
				}
			}
			else if (F.IntegerPart * -1 >= 524288)
			{
				return -FixedMath.PI / 2;
			}
			Fixed other = F * F;
			Fixed other2 = FixedMath.Sqrt(Fixed.One + other);
			return FixedMath.Asin(F / other2);
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x00153120 File Offset: 0x00151520
		public static Fixed Acos(Fixed F)
		{
			return FixedMath.HalfPI - FixedMath.Asin(F);
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x00153134 File Offset: 0x00151534
		public static Fixed Atan2(Fixed Y, Fixed X)
		{
			if (X.RawValue == 0L && Y.RawValue == 0L)
			{
				return 0;
			}
			Fixed result = 0;
			if (X > 0)
			{
				result = FixedMath.Atan(Y / X);
			}
			else if (X < 0)
			{
				if (Y >= 0)
				{
					result = FixedMath.PI - FixedMath.Atan(FixedMath.Abs(Y / X));
				}
				else
				{
					result = (FixedMath.PI - FixedMath.Atan(FixedMath.Abs(Y / X))).Inverse;
				}
			}
			else
			{
				result = ((!(Y >= 0)) ? FixedMath.PI.Inverse : FixedMath.PI) / Fixed.Create(2L, true);
			}
			return result;
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x0015321D File Offset: 0x0015161D
		public static Fixed Abs(Fixed F)
		{
			if (F < 0)
			{
				return F.Inverse;
			}
			return F;
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x00153234 File Offset: 0x00151634
		public static bool rectContainsPoint(FixedRect rect, Vector2F point, bool centeredAtTopLeft = true)
		{
			return !((point.x < rect.Left || point.x > rect.Left + rect.Width || ((!centeredAtTopLeft) ? (point.y < rect.Top) : (point.y > rect.Top))) ? true : ((!centeredAtTopLeft) ? (point.y > rect.Top + rect.Height) : (point.y < rect.Top - rect.Height)));
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x00153302 File Offset: 0x00151702
		public static Fixed Clamp01(Fixed T)
		{
			return FixedMath.Clamp(T, 0, 1);
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x00153316 File Offset: 0x00151716
		public static Fixed Max(Fixed a, Fixed b)
		{
			return (!(a > b)) ? b : a;
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x0015332B File Offset: 0x0015172B
		public static Fixed Min(Fixed a, Fixed b)
		{
			return (!(a < b)) ? b : a;
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x00153340 File Offset: 0x00151740
		public static Fixed Clamp(Fixed N, Fixed min, Fixed max)
		{
			return FixedMath.Max(FixedMath.Min(N, max), min);
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x00153350 File Offset: 0x00151750
		public static Fixed Modulo(Fixed a, Fixed b)
		{
			Fixed @fixed = a % b;
			return (!(@fixed < 0)) ? @fixed : (@fixed + b);
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x00153380 File Offset: 0x00151780
		public static Fixed Wrap(Fixed n, Fixed min, Fixed max)
		{
			if (max < min)
			{
				FixedMath.Swap(ref min, ref max);
			}
			Fixed a = n - min;
			Fixed b = max - min;
			return FixedMath.Modulo(a, b) + min;
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x001533BF File Offset: 0x001517BF
		public static Fixed Wrap01(Fixed n)
		{
			return FixedMath.Wrap(n, 0, 1);
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x001533D3 File Offset: 0x001517D3
		public static Fixed WrapDegrees(Fixed n)
		{
			return FixedMath.Wrap(n, 0, 360);
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x001533EB File Offset: 0x001517EB
		public static Fixed WrapRadians(Fixed n)
		{
			return FixedMath.Wrap(n, 0, FixedMath.TwoPI);
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x00153400 File Offset: 0x00151800
		public static void Swap(ref Fixed a, ref Fixed b)
		{
			Fixed @fixed = a;
			a = b;
			b = @fixed;
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x00153428 File Offset: 0x00151828
		public static Fixed Pow(Fixed a, int b)
		{
			Fixed @fixed = 1;
			for (;;)
			{
				if ((b & 1) != 0)
				{
					@fixed = a * @fixed;
				}
				b >>= 1;
				if (b == 0)
				{
					break;
				}
				a *= a;
			}
			return @fixed;
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x00153468 File Offset: 0x00151868
		public static int Ceil(Fixed val)
		{
			double num = (double)val.RawValue;
			int num2 = (int)(num / 65536.0);
			int num3 = (int)(num - (double)(num2 * 65536));
			if (num3 > 0)
			{
				num2++;
			}
			return num2;
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x001534A4 File Offset: 0x001518A4
		public static int Floor(Fixed val)
		{
			double num = (double)val.RawValue;
			return (int)(num / 65536.0);
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x001534C6 File Offset: 0x001518C6
		public static bool ApproximatelyEqual(Fixed a, Fixed b)
		{
			return FixedMath.ApproximatelyEqual(a, b, Fixed.Epsilon);
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x001534D4 File Offset: 0x001518D4
		public static bool ApproximatelyEqual(Fixed a, Fixed b, Fixed epsilon)
		{
			return FixedMath.Abs(a - b) <= epsilon;
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x001534E8 File Offset: 0x001518E8
		public static bool ApproximatelyOrGreater(Fixed a, Fixed b)
		{
			return FixedMath.ApproximatelyOrGreater(a, b, Fixed.Epsilon);
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x001534F6 File Offset: 0x001518F6
		public static bool ApproximatelyOrGreater(Fixed a, Fixed b, Fixed epsilon)
		{
			return a >= b - epsilon;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x00153505 File Offset: 0x00151905
		public static bool ApproximatelyOrLess(Fixed a, Fixed b)
		{
			return FixedMath.ApproximatelyOrLess(a, b, Fixed.Epsilon);
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x00153513 File Offset: 0x00151913
		public static bool ApproximatelyOrLess(Fixed a, Fixed b, Fixed epsilon)
		{
			return a <= b + epsilon;
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x00153522 File Offset: 0x00151922
		public static Fixed Lerp(Fixed a, Fixed b, Fixed t)
		{
			return (b - a) * t + a;
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x00153538 File Offset: 0x00151938
		public static Fixed DeltaAngle(Fixed current, Fixed target)
		{
			Fixed @fixed = (target - current) % 360;
			if (@fixed < 0)
			{
				@fixed += 360;
			}
			if (@fixed > 180)
			{
				@fixed -= 360;
			}
			return @fixed;
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x0015358C File Offset: 0x0015198C
		public static Fixed MoveTowardsAngle(Fixed current, Fixed target, Fixed maxDelta)
		{
			Fixed @fixed = FixedMath.DeltaAngle(current, target);
			Fixed result;
			if (-maxDelta < @fixed && @fixed < maxDelta)
			{
				result = target;
			}
			else
			{
				target = current + @fixed;
				result = FixedMath.MoveTowards(current, target, maxDelta);
			}
			return result;
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x001535D8 File Offset: 0x001519D8
		public static Fixed MoveTowards(Fixed current, Fixed target, Fixed maxDelta)
		{
			Fixed result;
			if (FixedMath.Abs(target - current) <= maxDelta)
			{
				result = target;
			}
			else
			{
				result = current + FixedMath.Sign(target - current) * maxDelta;
			}
			return result;
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x00153620 File Offset: 0x00151A20
		public static Vector3F MoveTowards(Vector3F current, Vector3F target, Fixed maxDistanceDelta)
		{
			Vector3F a = target - current;
			Fixed magnitude = a.magnitude;
			if (magnitude <= maxDistanceDelta || magnitude == 0)
			{
				return target;
			}
			return current + a / magnitude * maxDistanceDelta;
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x0015366F File Offset: 0x00151A6F
		public static Fixed Sign(Fixed f)
		{
			return (!(f < 0)) ? 1 : -1;
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x00153689 File Offset: 0x00151A89
		public static bool InsideSmallAngle(Vector3F testVector, Vector3F min, Vector3F max)
		{
			return Vector3F.Dot(Vector3F.Cross(min, max), Vector3F.Cross(min, testVector)) >= 0 && Vector3F.Dot(Vector3F.Cross(max, testVector), Vector3F.Cross(max, min)) >= 0;
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x001536C5 File Offset: 0x00151AC5
		public static Vector3F Reflect(Vector3F targetVector, Vector3F normal)
		{
			return targetVector - 2 * Vector3F.Dot(targetVector, normal) * normal;
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x001536E0 File Offset: 0x00151AE0
		public static bool InsideSmallAngle(Fixed angle, Fixed min, Fixed max)
		{
			Vector3F min2 = MathUtil.AngleToVector(min);
			Vector3F max2 = MathUtil.AngleToVector(max);
			Vector3F testVector = MathUtil.AngleToVector(angle);
			return FixedMath.InsideSmallAngle(testVector, min2, max2);
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x0015370C File Offset: 0x00151B0C
		public static Fixed ClampAngle(Fixed angle, Fixed min, Fixed max)
		{
			Fixed @fixed = min;
			Fixed fixed2 = max;
			min = FixedMath.Wrap(min, 0, 360);
			max = FixedMath.Wrap(max, 0, 360);
			if (FixedMath.ApproximatelyEqual(min, max))
			{
				return min;
			}
			Vector3F min2 = MathUtil.AngleToVector(min);
			Vector3F max2 = MathUtil.AngleToVector(max);
			Vector3F vector3F = MathUtil.AngleToVector(angle);
			Fixed degrees = (max + 360 - min) % 360 / 2 + min;
			Vector3F vector3F2 = MathUtil.AngleToVector(degrees);
			if (FixedMath.InsideSmallAngle(vector3F, min2, vector3F2) || FixedMath.InsideSmallAngle(vector3F, vector3F2, max2))
			{
				return angle;
			}
			return (!(Vector3F.Cross(vector3F2, vector3F).z >= 0)) ? @fixed : fixed2;
		}

		// Token: 0x0400348A RID: 13450
		public static readonly Fixed PI = (Fixed)3.141592653589793;

		// Token: 0x0400348B RID: 13451
		public static readonly Fixed HalfPI = FixedMath.PI / 2;

		// Token: 0x0400348C RID: 13452
		public static readonly Fixed TwoThirdsPI = FixedMath.PI * (Fixed)0.6666666666666666;

		// Token: 0x0400348D RID: 13453
		public static readonly Fixed TwoPI = FixedMath.PI * 2;

		// Token: 0x0400348E RID: 13454
		public static readonly Fixed PIOver180F = FixedMath.PI / 180;

		// Token: 0x0400348F RID: 13455
		private static readonly int sqrtShift = 24;

		// Token: 0x04003490 RID: 13456
		private static readonly int sqrtResultShift = 16 - (FixedMath.sqrtShift >> 1);

		// Token: 0x04003491 RID: 13457
		public static readonly Fixed SqrtTwo = FixedMath.Sqrt(2);

		// Token: 0x04003492 RID: 13458
		public static readonly Fixed InverseSqrtTwo = 1 / FixedMath.Sqrt(2);

		// Token: 0x04003493 RID: 13459
		private static readonly int degreesPrecision = 10;

		// Token: 0x04003494 RID: 13460
		private static readonly Fixed scaledDegrees = FixedMath.degreesPrecision * 90 / FixedMath.HalfPI;

		// Token: 0x04003495 RID: 13461
		public static Fixed Deg2Rad = (Fixed)0.01745329238474369;

		// Token: 0x04003496 RID: 13462
		public static Fixed Rad2Deg = (Fixed)57.295780181884766;
	}
}
