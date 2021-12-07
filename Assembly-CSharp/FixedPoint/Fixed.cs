using System;

namespace FixedPoint
{
	// Token: 0x02000B1C RID: 2844
	[Serializable]
	public struct Fixed
	{
		// Token: 0x06005189 RID: 20873 RVA: 0x00152838 File Offset: 0x00150C38
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

		// Token: 0x0600518A RID: 20874 RVA: 0x00152868 File Offset: 0x00150C68
		public static Fixed Create(double DoubleValue)
		{
			DoubleValue *= 65536.0;
			Fixed result;
			result.RawValue = (long)Math.Round(DoubleValue);
			return result;
		}

		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x0600518B RID: 20875 RVA: 0x00152891 File Offset: 0x00150C91
		public int IntegerPart
		{
			get
			{
				return (int)(this.RawValue >> 16);
			}
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x0600518C RID: 20876 RVA: 0x0015289D File Offset: 0x00150C9D
		public Fixed FractionalPart
		{
			get
			{
				return this - FixedMath.Floor(this);
			}
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x001528B5 File Offset: 0x00150CB5
		public int ToInt()
		{
			return (int)(this.RawValue >> 16);
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x001528C1 File Offset: 0x00150CC1
		public double ToDouble()
		{
			return (double)this.RawValue / 65536.0;
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x001528D4 File Offset: 0x00150CD4
		public float ToFloat()
		{
			return (float)this.RawValue / 65536f;
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06005190 RID: 20880 RVA: 0x001528E3 File Offset: 0x00150CE3
		public Fixed Inverse
		{
			get
			{
				return Fixed.Create(-this.RawValue, false);
			}
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x001528F4 File Offset: 0x00150CF4
		public static Fixed FromParts(int PreDecimal, int PostDecimal)
		{
			Fixed result = Fixed.Create((long)PreDecimal, true);
			if (PostDecimal != 0)
			{
				result.RawValue += (Fixed.Create((double)PostDecimal) / 1000).RawValue;
			}
			return result;
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x00152938 File Offset: 0x00150D38
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

		// Token: 0x06005193 RID: 20883 RVA: 0x001529A3 File Offset: 0x00150DA3
		public static Fixed operator *(Fixed one, int multi)
		{
			return one * multi;
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x001529B1 File Offset: 0x00150DB1
		public static Fixed operator *(int multi, Fixed one)
		{
			return one * multi;
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x001529BF File Offset: 0x00150DBF
		public static Fixed operator *(Fixed one, float multi)
		{
			return one * (Fixed)((double)multi);
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x001529CE File Offset: 0x00150DCE
		public static Fixed operator *(float multi, Fixed one)
		{
			return one * (Fixed)((double)multi);
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x001529E0 File Offset: 0x00150DE0
		public static Fixed operator /(Fixed one, Fixed other)
		{
			Fixed result;
			result.RawValue = (one.RawValue << 16) / other.RawValue;
			return result;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x00152A07 File Offset: 0x00150E07
		public static Fixed operator /(Fixed one, int divisor)
		{
			return one / divisor;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x00152A15 File Offset: 0x00150E15
		public static Fixed operator /(int divisor, Fixed one)
		{
			return divisor / one;
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x00152A24 File Offset: 0x00150E24
		public static Fixed operator %(Fixed one, Fixed other)
		{
			Fixed result;
			result.RawValue = one.RawValue % other.RawValue;
			return result;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x00152A48 File Offset: 0x00150E48
		public static Fixed operator %(Fixed one, int divisor)
		{
			return one % divisor;
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x00152A56 File Offset: 0x00150E56
		public static Fixed operator %(int divisor, Fixed one)
		{
			return divisor % one;
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x00152A64 File Offset: 0x00150E64
		public static Fixed operator +(Fixed one, Fixed other)
		{
			Fixed result;
			result.RawValue = one.RawValue + other.RawValue;
			return result;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x00152A88 File Offset: 0x00150E88
		public static Fixed operator +(Fixed one, int other)
		{
			return one + other;
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x00152A96 File Offset: 0x00150E96
		public static Fixed operator +(int other, Fixed one)
		{
			return one + other;
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x00152AA4 File Offset: 0x00150EA4
		public static Fixed operator -(Fixed one, Fixed other)
		{
			Fixed result;
			result.RawValue = one.RawValue - other.RawValue;
			return result;
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x00152AC8 File Offset: 0x00150EC8
		public static Fixed operator -(Fixed one, int other)
		{
			return one - other;
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x00152AD6 File Offset: 0x00150ED6
		public static Fixed operator -(int other, Fixed one)
		{
			return other - one;
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x00152AE4 File Offset: 0x00150EE4
		public static bool operator ==(Fixed one, Fixed other)
		{
			return one.RawValue == other.RawValue;
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x00152AF6 File Offset: 0x00150EF6
		public static bool operator ==(Fixed one, int other)
		{
			return one == other;
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x00152B04 File Offset: 0x00150F04
		public static bool operator ==(int other, Fixed one)
		{
			return other == one;
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x00152B12 File Offset: 0x00150F12
		public static bool operator !=(Fixed one, Fixed other)
		{
			return one.RawValue != other.RawValue;
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x00152B27 File Offset: 0x00150F27
		public static bool operator !=(Fixed one, int other)
		{
			return one != other;
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x00152B35 File Offset: 0x00150F35
		public static bool operator !=(int other, Fixed one)
		{
			return other != one;
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x00152B43 File Offset: 0x00150F43
		public static bool operator >=(Fixed one, Fixed other)
		{
			return one.RawValue >= other.RawValue;
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x00152B58 File Offset: 0x00150F58
		public static bool operator >=(Fixed one, int other)
		{
			return one >= other;
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x00152B66 File Offset: 0x00150F66
		public static bool operator >=(int other, Fixed one)
		{
			return other >= one;
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x00152B74 File Offset: 0x00150F74
		public static bool operator <=(Fixed one, Fixed other)
		{
			return one.RawValue <= other.RawValue;
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x00152B89 File Offset: 0x00150F89
		public static bool operator <=(Fixed one, int other)
		{
			return one <= other;
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x00152B97 File Offset: 0x00150F97
		public static bool operator <=(int other, Fixed one)
		{
			return other <= one;
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x00152BA5 File Offset: 0x00150FA5
		public static bool operator >(Fixed one, Fixed other)
		{
			return one.RawValue > other.RawValue;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x00152BB7 File Offset: 0x00150FB7
		public static bool operator >(Fixed one, int other)
		{
			return one > other;
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x00152BC5 File Offset: 0x00150FC5
		public static bool operator >(int other, Fixed one)
		{
			return other > one;
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x00152BD3 File Offset: 0x00150FD3
		public static bool operator <(Fixed one, Fixed other)
		{
			return one.RawValue < other.RawValue;
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x00152BE5 File Offset: 0x00150FE5
		public static bool operator <(Fixed one, int other)
		{
			return one < other;
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x00152BF3 File Offset: 0x00150FF3
		public static bool operator <(int other, Fixed one)
		{
			return other < one;
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x00152C01 File Offset: 0x00151001
		public static explicit operator int(Fixed src)
		{
			return (int)(src.RawValue >> 16);
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x00152C0E File Offset: 0x0015100E
		public static explicit operator double(Fixed src)
		{
			return src.ToDouble();
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x00152C17 File Offset: 0x00151017
		public static explicit operator float(Fixed src)
		{
			return (float)src.ToDouble();
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x00152C21 File Offset: 0x00151021
		public static implicit operator Fixed(int src)
		{
			return Fixed.Create((long)src, true);
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00152C2B File Offset: 0x0015102B
		public static implicit operator Fixed(uint src)
		{
			return Fixed.Create((long)((ulong)src), true);
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x00152C35 File Offset: 0x00151035
		public static implicit operator Fixed(long src)
		{
			return Fixed.Create(src, true);
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x00152C3E File Offset: 0x0015103E
		public static implicit operator Fixed(ulong src)
		{
			return Fixed.Create((long)src, true);
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x00152C47 File Offset: 0x00151047
		public static explicit operator Fixed(double src)
		{
			return Fixed.Create(src);
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x00152C4F File Offset: 0x0015104F
		public static Fixed operator -(Fixed src)
		{
			return Fixed.Create(-src.RawValue, false);
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x00152C5F File Offset: 0x0015105F
		public static Fixed operator <<(Fixed one, int Amount)
		{
			return Fixed.Create(one.RawValue << Amount, false);
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x00152C73 File Offset: 0x00151073
		public static Fixed operator >>(Fixed one, int Amount)
		{
			return Fixed.Create(one.RawValue >> Amount, false);
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x00152C88 File Offset: 0x00151088
		public override bool Equals(object obj)
		{
			return obj is Fixed && ((Fixed)obj).RawValue == this.RawValue;
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x00152CB8 File Offset: 0x001510B8
		public override int GetHashCode()
		{
			return this.RawValue.GetHashCode();
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x00152CCC File Offset: 0x001510CC
		public override string ToString()
		{
			return this.ToDouble().ToString();
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x00152CF0 File Offset: 0x001510F0
		public string ToString(string format)
		{
			return this.ToDouble().ToString(format);
		}

		// Token: 0x04003478 RID: 13432
		public long RawValue;

		// Token: 0x04003479 RID: 13433
		public const int SHIFT_AMOUNT = 16;

		// Token: 0x0400347A RID: 13434
		public const int HALF_SHIFT_AMOUNT = 8;

		// Token: 0x0400347B RID: 13435
		public const int FRACTIONAL_BITS = 16;

		// Token: 0x0400347C RID: 13436
		public const int INTEGER_BITS = 48;

		// Token: 0x0400347D RID: 13437
		public const long OneD = 65536L;

		// Token: 0x0400347E RID: 13438
		public const int OneI = 65536;

		// Token: 0x0400347F RID: 13439
		public static Fixed One = Fixed.Create(1L, true);

		// Token: 0x04003480 RID: 13440
		public static Fixed MinusOne = Fixed.Create(-1L, true);

		// Token: 0x04003481 RID: 13441
		public static Fixed Zero = Fixed.Create(0L, true);

		// Token: 0x04003482 RID: 13442
		public static Fixed NaN = Fixed.Create(double.NaN);

		// Token: 0x04003483 RID: 13443
		public static Fixed Infinity = Fixed.Create(double.PositiveInfinity);

		// Token: 0x04003484 RID: 13444
		public static Fixed Epsilon = (Fixed)0.001;

		// Token: 0x04003485 RID: 13445
		public static Fixed MaxValue = Fixed.Create(70368744177664L, true);

		// Token: 0x04003486 RID: 13446
		public static Fixed MinValue = Fixed.Create(1L, false);

		// Token: 0x04003487 RID: 13447
		public const long FractionMask = 65535L;
	}
}
