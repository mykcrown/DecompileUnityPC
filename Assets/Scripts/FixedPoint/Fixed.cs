// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace FixedPoint
{
	[Serializable]
	public struct Fixed
	{
		public long RawValue;

		public const int SHIFT_AMOUNT = 16;

		public const int HALF_SHIFT_AMOUNT = 8;

		public const int FRACTIONAL_BITS = 16;

		public const int INTEGER_BITS = 48;

		public const long OneD = 65536L;

		public const int OneI = 65536;

		public static Fixed One = Fixed.Create(1L, true);

		public static Fixed MinusOne = Fixed.Create(-1L, true);

		public static Fixed Zero = Fixed.Create(0L, true);

		public static Fixed NaN = Fixed.Create(double.NaN);

		public static Fixed Infinity = Fixed.Create(double.PositiveInfinity);

		public static Fixed Epsilon = (Fixed)0.001;

		public static Fixed MaxValue = Fixed.Create(70368744177664L, true);

		public static Fixed MinValue = Fixed.Create(1L, false);

		public const long FractionMask = 65535L;

		public int IntegerPart
		{
			get
			{
				return (int)(this.RawValue >> 16);
			}
		}

		public Fixed FractionalPart
		{
			get
			{
				return this - FixedMath.Floor(this);
			}
		}

		public Fixed Inverse
		{
			get
			{
				return Fixed.Create(-this.RawValue, false);
			}
		}

		public static Fixed Create(long StartingRawValue, bool UseMultiple)
		{
			Fixed result;
			result.RawValue = StartingRawValue;
			if (UseMultiple)
			{
				result.RawValue <<= 16;
			}
			return result;
		}

		public static Fixed Create(double DoubleValue)
		{
			DoubleValue *= 65536.0;
			Fixed result;
			result.RawValue = (long)Math.Round(DoubleValue);
			return result;
		}

		public int ToInt()
		{
			return (int)(this.RawValue >> 16);
		}

		public double ToDouble()
		{
			return (double)this.RawValue / 65536.0;
		}

		public float ToFloat()
		{
			return (float)this.RawValue / 65536f;
		}

		public static Fixed FromParts(int PreDecimal, int PostDecimal)
		{
			Fixed result = Fixed.Create((long)PreDecimal, true);
			if (PostDecimal != 0)
			{
				result.RawValue += (Fixed.Create((double)PostDecimal) / 1000).RawValue;
			}
			return result;
		}

		public static Fixed operator *(Fixed one, Fixed other)
		{
			if (one == Fixed.NaN || other == Fixed.NaN)
			{
				return Fixed.NaN;
			}
			long num = (one.RawValue >> 16) * other.RawValue;
			long num2 = (one.RawValue & 65535L) * other.RawValue >> 16;
			Fixed result;
			result.RawValue = num + num2;
			return result;
		}

		public static Fixed operator *(Fixed one, int multi)
		{
			return one * multi;
		}

		public static Fixed operator *(int multi, Fixed one)
		{
			return one * multi;
		}

		public static Fixed operator *(Fixed one, float multi)
		{
			return one * (Fixed)((double)multi);
		}

		public static Fixed operator *(float multi, Fixed one)
		{
			return one * (Fixed)((double)multi);
		}

		public static Fixed operator /(Fixed one, Fixed other)
		{
			Fixed result;
			result.RawValue = (one.RawValue << 16) / other.RawValue;
			return result;
		}

		public static Fixed operator /(Fixed one, int divisor)
		{
			return one / divisor;
		}

		public static Fixed operator /(int divisor, Fixed one)
		{
			return divisor / one;
		}

		public static Fixed operator %(Fixed one, Fixed other)
		{
			Fixed result;
			result.RawValue = one.RawValue % other.RawValue;
			return result;
		}

		public static Fixed operator %(Fixed one, int divisor)
		{
			return one % divisor;
		}

		public static Fixed operator %(int divisor, Fixed one)
		{
			return divisor % one;
		}

		public static Fixed operator +(Fixed one, Fixed other)
		{
			Fixed result;
			result.RawValue = one.RawValue + other.RawValue;
			return result;
		}

		public static Fixed operator +(Fixed one, int other)
		{
			return one + other;
		}

		public static Fixed operator +(int other, Fixed one)
		{
			return one + other;
		}

		public static Fixed operator -(Fixed one, Fixed other)
		{
			Fixed result;
			result.RawValue = one.RawValue - other.RawValue;
			return result;
		}

		public static Fixed operator -(Fixed one, int other)
		{
			return one - other;
		}

		public static Fixed operator -(int other, Fixed one)
		{
			return other - one;
		}

		public static bool operator ==(Fixed one, Fixed other)
		{
			return one.RawValue == other.RawValue;
		}

		public static bool operator ==(Fixed one, int other)
		{
			return one == other;
		}

		public static bool operator ==(int other, Fixed one)
		{
			return other == one;
		}

		public static bool operator !=(Fixed one, Fixed other)
		{
			return one.RawValue != other.RawValue;
		}

		public static bool operator !=(Fixed one, int other)
		{
			return one != other;
		}

		public static bool operator !=(int other, Fixed one)
		{
			return other != one;
		}

		public static bool operator >=(Fixed one, Fixed other)
		{
			return one.RawValue >= other.RawValue;
		}

		public static bool operator >=(Fixed one, int other)
		{
			return one >= other;
		}

		public static bool operator >=(int other, Fixed one)
		{
			return other >= one;
		}

		public static bool operator <=(Fixed one, Fixed other)
		{
			return one.RawValue <= other.RawValue;
		}

		public static bool operator <=(Fixed one, int other)
		{
			return one <= other;
		}

		public static bool operator <=(int other, Fixed one)
		{
			return other <= one;
		}

		public static bool operator >(Fixed one, Fixed other)
		{
			return one.RawValue > other.RawValue;
		}

		public static bool operator >(Fixed one, int other)
		{
			return one > other;
		}

		public static bool operator >(int other, Fixed one)
		{
			return other > one;
		}

		public static bool operator <(Fixed one, Fixed other)
		{
			return one.RawValue < other.RawValue;
		}

		public static bool operator <(Fixed one, int other)
		{
			return one < other;
		}

		public static bool operator <(int other, Fixed one)
		{
			return other < one;
		}

		public static explicit operator int(Fixed src)
		{
			return (int)(src.RawValue >> 16);
		}

		public static explicit operator double(Fixed src)
		{
			return src.ToDouble();
		}

		public static explicit operator float(Fixed src)
		{
			return (float)src.ToDouble();
		}

		public static implicit operator Fixed(int src)
		{
			return Fixed.Create((long)src, true);
		}

		public static implicit operator Fixed(uint src)
		{
			return Fixed.Create((long)((ulong)src), true);
		}

		public static implicit operator Fixed(long src)
		{
			return Fixed.Create(src, true);
		}

		public static implicit operator Fixed(ulong src)
		{
			return Fixed.Create((long)src, true);
		}

		public static explicit operator Fixed(double src)
		{
			return Fixed.Create(src);
		}

		public static Fixed operator -(Fixed src)
		{
			return Fixed.Create(-src.RawValue, false);
		}

		public static Fixed operator <<(Fixed one, int Amount)
		{
			return Fixed.Create(one.RawValue << Amount, false);
		}

		public static Fixed operator >>(Fixed one, int Amount)
		{
			return Fixed.Create(one.RawValue >> Amount, false);
		}

		public override bool Equals(object obj)
		{
			return obj is Fixed && ((Fixed)obj).RawValue == this.RawValue;
		}

		public override int GetHashCode()
		{
			return this.RawValue.GetHashCode();
		}

		public override string ToString()
		{
			return this.ToDouble().ToString();
		}

		public string ToString(string format)
		{
			return this.ToDouble().ToString(format);
		}
	}
}
