using System;
using System.Text;

// Token: 0x02000AA0 RID: 2720
public class BitField
{
	// Token: 0x06004FE2 RID: 20450 RVA: 0x0014DB2C File Offset: 0x0014BF2C
	public static int CountBitFlags(ulong bitField)
	{
		int num = 0;
		while (bitField > 0UL)
		{
			bitField &= bitField - 1UL;
			num++;
		}
		return num;
	}

	// Token: 0x06004FE3 RID: 20451 RVA: 0x0014DB57 File Offset: 0x0014BF57
	public static int CountBitFlags(uint bitField)
	{
		return BitField.CountBitFlags((ulong)bitField);
	}

	// Token: 0x06004FE4 RID: 20452 RVA: 0x0014DB60 File Offset: 0x0014BF60
	public static int CountBitFlags(ushort bitField)
	{
		return BitField.CountBitFlags((ulong)bitField);
	}

	// Token: 0x06004FE5 RID: 20453 RVA: 0x0014DB69 File Offset: 0x0014BF69
	public static int CountBitFlags(byte bitField)
	{
		return BitField.CountBitFlags((ulong)bitField);
	}

	// Token: 0x06004FE6 RID: 20454 RVA: 0x0014DB72 File Offset: 0x0014BF72
	public static ulong AddBitFlag(ulong bitField, int flag)
	{
		return bitField |= 1UL << flag;
	}

	// Token: 0x06004FE7 RID: 20455 RVA: 0x0014DB80 File Offset: 0x0014BF80
	public static uint AddBitFlag(uint bitField, int flag)
	{
		return bitField |= 1U << flag;
	}

	// Token: 0x06004FE8 RID: 20456 RVA: 0x0014DB8D File Offset: 0x0014BF8D
	public static ushort AddBitFlag(ushort bitField, int flag)
	{
		return bitField |= (ushort)(1 << flag);
	}

	// Token: 0x06004FE9 RID: 20457 RVA: 0x0014DB9C File Offset: 0x0014BF9C
	public static byte AddBitFlag(byte bitField, int flag)
	{
		return bitField |= (byte)(1 << flag);
	}

	// Token: 0x06004FEA RID: 20458 RVA: 0x0014DBAB File Offset: 0x0014BFAB
	public static ulong RemoveBitFlag(ulong bitField, int flag)
	{
		return bitField &= ~(1UL << flag);
	}

	// Token: 0x06004FEB RID: 20459 RVA: 0x0014DBBA File Offset: 0x0014BFBA
	public static uint RemoveBitFlag(uint bitField, int flag)
	{
		return bitField &= ~(1U << flag);
	}

	// Token: 0x06004FEC RID: 20460 RVA: 0x0014DBC8 File Offset: 0x0014BFC8
	public static ushort RemoveBitFlag(ushort bitField, int flag)
	{
		return bitField &= ~(ushort)(1 << flag);
	}

	// Token: 0x06004FED RID: 20461 RVA: 0x0014DBD9 File Offset: 0x0014BFD9
	public static byte RemoveBitFlag(byte bitField, int flag)
	{
		return bitField &= ~(byte)(1 << flag);
	}

	// Token: 0x06004FEE RID: 20462 RVA: 0x0014DBEA File Offset: 0x0014BFEA
	public static bool HasBitFlag(ulong bitField, int flag)
	{
		return (bitField & 1UL << flag) > 0UL;
	}

	// Token: 0x06004FEF RID: 20463 RVA: 0x0014DBF9 File Offset: 0x0014BFF9
	public static bool HasBitFlag(uint bitField, int flag)
	{
		return (bitField & 1U << flag) > 0U;
	}

	// Token: 0x06004FF0 RID: 20464 RVA: 0x0014DC06 File Offset: 0x0014C006
	public static bool HasBitFlag(ushort bitField, int flag)
	{
		return (bitField & (ushort)(1 << flag)) > 0;
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x0014DC14 File Offset: 0x0014C014
	public static bool HasBitFlag(byte bitField, int flag)
	{
		return (bitField & (byte)(1 << flag)) > 0;
	}

	// Token: 0x06004FF2 RID: 20466 RVA: 0x0014DC24 File Offset: 0x0014C024
	public static ulong WriteByte(ulong bitField, int index, byte pattern, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > 64)
		{
			throw new ArgumentOutOfRangeException();
		}
		for (int i = 0; i < leastSigBits; i++)
		{
			if (BitField.HasBitFlag(pattern, i))
			{
				bitField = BitField.AddBitFlag(bitField, index + i);
			}
			else
			{
				bitField = BitField.RemoveBitFlag(bitField, index + i);
			}
		}
		return bitField;
	}

	// Token: 0x06004FF3 RID: 20467 RVA: 0x0014DC84 File Offset: 0x0014C084
	public static uint WriteByte(uint bitField, int index, byte pattern, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > 32)
		{
			throw new ArgumentOutOfRangeException();
		}
		for (int i = 0; i < leastSigBits; i++)
		{
			if (BitField.HasBitFlag(pattern, i))
			{
				bitField = BitField.AddBitFlag(bitField, index + i);
			}
			else
			{
				bitField = BitField.RemoveBitFlag(bitField, index + i);
			}
		}
		return bitField;
	}

	// Token: 0x06004FF4 RID: 20468 RVA: 0x0014DCE4 File Offset: 0x0014C0E4
	public static ushort WriteByte(ushort bitField, int index, byte pattern, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > 16)
		{
			throw new ArgumentOutOfRangeException();
		}
		for (int i = 0; i < leastSigBits; i++)
		{
			if (BitField.HasBitFlag(pattern, i))
			{
				bitField = BitField.AddBitFlag(bitField, index + i);
			}
			else
			{
				bitField = BitField.RemoveBitFlag(bitField, index + i);
			}
		}
		return bitField;
	}

	// Token: 0x06004FF5 RID: 20469 RVA: 0x0014DD44 File Offset: 0x0014C144
	public static byte WriteByte(byte bitField, int index, byte pattern, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > 8)
		{
			throw new ArgumentOutOfRangeException();
		}
		for (int i = 0; i < leastSigBits; i++)
		{
			if (BitField.HasBitFlag(pattern, i))
			{
				bitField = BitField.AddBitFlag(bitField, index + i);
			}
			else
			{
				bitField = BitField.RemoveBitFlag(bitField, index + i);
			}
		}
		return bitField;
	}

	// Token: 0x06004FF6 RID: 20470 RVA: 0x0014DDA4 File Offset: 0x0014C1A4
	public static byte ReadByte(ulong bitField, int index, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > 64)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (leastSigBits > 8 || leastSigBits <= 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		byte b = 0;
		for (int i = 0; i < leastSigBits; i++)
		{
			if (BitField.HasBitFlag(bitField, index + i))
			{
				b = BitField.AddBitFlag(b, i);
			}
		}
		return b;
	}

	// Token: 0x06004FF7 RID: 20471 RVA: 0x0014DE08 File Offset: 0x0014C208
	public static byte ReadByte(uint bitField, int index, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > 32)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (leastSigBits > 8 || leastSigBits <= 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		byte b = 0;
		for (int i = 0; i < leastSigBits; i++)
		{
			if (BitField.HasBitFlag(bitField, index + i))
			{
				b = BitField.AddBitFlag(b, i);
			}
		}
		return b;
	}

	// Token: 0x06004FF8 RID: 20472 RVA: 0x0014DE6C File Offset: 0x0014C26C
	public static byte ReadByte(ushort bitField, int index, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > 16)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (leastSigBits > 8 || leastSigBits <= 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		byte b = 0;
		for (int i = 0; i < leastSigBits; i++)
		{
			if (BitField.HasBitFlag(bitField, index + i))
			{
				b = BitField.AddBitFlag(b, i);
			}
		}
		return b;
	}

	// Token: 0x06004FF9 RID: 20473 RVA: 0x0014DED0 File Offset: 0x0014C2D0
	public static byte ReadByte(byte bitField, int index, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > 8)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (leastSigBits > 8 || leastSigBits <= 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		byte b = 0;
		for (int i = 0; i < leastSigBits; i++)
		{
			if (BitField.HasBitFlag(bitField, index + i))
			{
				b = BitField.AddBitFlag(b, i);
			}
		}
		return b;
	}

	// Token: 0x06004FFA RID: 20474 RVA: 0x0014DF34 File Offset: 0x0014C334
	private static string toStringWithLength(ulong bitField, int length)
	{
		StringBuilder stringBuilder = new StringBuilder(length);
		for (int i = length - 1; i >= 0; i--)
		{
			stringBuilder.Append((!BitField.HasBitFlag(bitField, i)) ? "0" : "1");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06004FFB RID: 20475 RVA: 0x0014DF84 File Offset: 0x0014C384
	public static string ToString(ulong bitField)
	{
		return BitField.toStringWithLength(bitField, 64);
	}

	// Token: 0x06004FFC RID: 20476 RVA: 0x0014DF8E File Offset: 0x0014C38E
	public static string ToString(uint bitField)
	{
		return BitField.toStringWithLength((ulong)bitField, 32);
	}

	// Token: 0x06004FFD RID: 20477 RVA: 0x0014DF99 File Offset: 0x0014C399
	public static string ToString(ushort bitField)
	{
		return BitField.toStringWithLength((ulong)bitField, 16);
	}

	// Token: 0x06004FFE RID: 20478 RVA: 0x0014DFA4 File Offset: 0x0014C3A4
	public static string ToString(byte bitField)
	{
		return BitField.toStringWithLength((ulong)bitField, 8);
	}
}
