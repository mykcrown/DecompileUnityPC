// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace FixedPoint
{
	public class FixedMath
	{
		public static readonly Fixed PI = (Fixed)3.1415926535897931;

		public static readonly Fixed HalfPI = FixedMath.PI / 2;

		public static readonly Fixed TwoThirdsPI = FixedMath.PI * (Fixed)0.66666666666666663;

		public static readonly Fixed TwoPI = FixedMath.PI * 2;

		public static readonly Fixed PIOver180F = FixedMath.PI / 180;

		private static readonly int sqrtShift = 24;

		private static readonly int sqrtResultShift = 16 - (FixedMath.sqrtShift >> 1);

		public static readonly Fixed SqrtTwo = FixedMath.Sqrt(2);

		public static readonly Fixed InverseSqrtTwo = 1 / FixedMath.Sqrt(2);

		private static readonly int degreesPrecision = 10;

		private static readonly Fixed scaledDegrees = FixedMath.degreesPrecision * 90 / FixedMath.HalfPI;

		public static Fixed Deg2Rad = (Fixed)0.01745329238474369;

		public static Fixed Rad2Deg = (Fixed)57.295780181884766;

		public static Fixed Sqrt(Fixed f)
		{
			long num = (long)Math.Sqrt((double)(f.RawValue << 8));
			Fixed result;
			result.RawValue = num << FixedMath.sqrtResultShift;
			return result;
		}

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
						(int)((Fixed)((double)Mathf.Sin(((float)i + (float)j / (float)FixedMath.degreesPrecision) * 0.0174532924f))).RawValue,
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

		private static Fixed mul(Fixed F1, Fixed F2)
		{
			return F1 * F2;
		}

		public static Fixed Cos(Fixed i)
		{
			return FixedMath.Sin(i + FixedMath.HalfPI);
		}

		public static Fixed Tan(Fixed i)
		{
			return FixedMath.Sin(i) / FixedMath.Cos(i);
		}

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

		public static Fixed Acos(Fixed F)
		{
			return FixedMath.HalfPI - FixedMath.Asin(F);
		}

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

		public static Fixed Abs(Fixed F)
		{
			if (F < 0)
			{
				return F.Inverse;
			}
			return F;
		}

		public static bool rectContainsPoint(FixedRect rect, Vector2F point, bool centeredAtTopLeft = true)
		{
			return !((point.x < rect.Left || point.x > rect.Left + rect.Width || ((!centeredAtTopLeft) ? (point.y < rect.Top) : (point.y > rect.Top))) ? true : ((!centeredAtTopLeft) ? (point.y > rect.Top + rect.Height) : (point.y < rect.Top - rect.Height)));
		}

		public static Fixed Clamp01(Fixed T)
		{
			return FixedMath.Clamp(T, 0, 1);
		}

		public static Fixed Max(Fixed a, Fixed b)
		{
			return (!(a > b)) ? b : a;
		}

		public static Fixed Min(Fixed a, Fixed b)
		{
			return (!(a < b)) ? b : a;
		}

		public static Fixed Clamp(Fixed N, Fixed min, Fixed max)
		{
			return FixedMath.Max(FixedMath.Min(N, max), min);
		}

		public static Fixed Modulo(Fixed a, Fixed b)
		{
			Fixed @fixed = a % b;
			return (!(@fixed < 0)) ? @fixed : (@fixed + b);
		}

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

		public static Fixed Wrap01(Fixed n)
		{
			return FixedMath.Wrap(n, 0, 1);
		}

		public static Fixed WrapDegrees(Fixed n)
		{
			return FixedMath.Wrap(n, 0, 360);
		}

		public static Fixed WrapRadians(Fixed n)
		{
			return FixedMath.Wrap(n, 0, FixedMath.TwoPI);
		}

		public static void Swap(ref Fixed a, ref Fixed b)
		{
			Fixed @fixed = a;
			a = b;
			b = @fixed;
		}

		public static Fixed Pow(Fixed a, int b)
		{
			Fixed @fixed = 1;
			while (true)
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

		public static int Floor(Fixed val)
		{
			double num = (double)val.RawValue;
			return (int)(num / 65536.0);
		}

		public static bool ApproximatelyEqual(Fixed a, Fixed b)
		{
			return FixedMath.ApproximatelyEqual(a, b, Fixed.Epsilon);
		}

		public static bool ApproximatelyEqual(Fixed a, Fixed b, Fixed epsilon)
		{
			return FixedMath.Abs(a - b) <= epsilon;
		}

		public static bool ApproximatelyOrGreater(Fixed a, Fixed b)
		{
			return FixedMath.ApproximatelyOrGreater(a, b, Fixed.Epsilon);
		}

		public static bool ApproximatelyOrGreater(Fixed a, Fixed b, Fixed epsilon)
		{
			return a >= b - epsilon;
		}

		public static bool ApproximatelyOrLess(Fixed a, Fixed b)
		{
			return FixedMath.ApproximatelyOrLess(a, b, Fixed.Epsilon);
		}

		public static bool ApproximatelyOrLess(Fixed a, Fixed b, Fixed epsilon)
		{
			return a <= b + epsilon;
		}

		public static Fixed Lerp(Fixed a, Fixed b, Fixed t)
		{
			return (b - a) * t + a;
		}

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

		public static Fixed Sign(Fixed f)
		{
			return (!(f < 0)) ? 1 : (-1);
		}

		public static bool InsideSmallAngle(Vector3F testVector, Vector3F min, Vector3F max)
		{
			return Vector3F.Dot(Vector3F.Cross(min, max), Vector3F.Cross(min, testVector)) >= 0 && Vector3F.Dot(Vector3F.Cross(max, testVector), Vector3F.Cross(max, min)) >= 0;
		}

		public static Vector3F Reflect(Vector3F targetVector, Vector3F normal)
		{
			return targetVector - 2 * Vector3F.Dot(targetVector, normal) * normal;
		}

		public static bool InsideSmallAngle(Fixed angle, Fixed min, Fixed max)
		{
			Vector3F min2 = MathUtil.AngleToVector(min);
			Vector3F max2 = MathUtil.AngleToVector(max);
			Vector3F testVector = MathUtil.AngleToVector(angle);
			return FixedMath.InsideSmallAngle(testVector, min2, max2);
		}

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
	}
}
