// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text;

public class BitField
{
	public static int CountBitFlags(ulong bitField)
	{
		int num = 0;
		while (bitField > 0uL)
		{
			bitField &= bitField - 1uL;
			num++;
		}
		return num;
	}

	public static int CountBitFlags(uint bitField)
	{
		return BitField.CountBitFlags((ulong)bitField);
	}

	public static int CountBitFlags(ushort bitField)
	{
		return BitField.CountBitFlags((ulong)bitField);
	}

	public static int CountBitFlags(byte bitField)
	{
		return BitField.CountBitFlags((ulong)bitField);
	}

	public static ulong AddBitFlag(ulong bitField, int flag)
	{
		return bitField |= 1uL << flag;
	}

	public static uint AddBitFlag(uint bitField, int flag)
	{
		return bitField |= 1u << flag;
	}

	public static ushort AddBitFlag(ushort bitField, int flag)
	{
		return bitField |= (ushort)(1 << flag);
	}

	public static byte AddBitFlag(byte bitField, int flag)
	{
		return bitField |= (byte)(1 << flag);
	}

	public static ulong RemoveBitFlag(ulong bitField, int flag)
	{
		return bitField &= ~(1uL << flag);
	}

	public static uint RemoveBitFlag(uint bitField, int flag)
	{
		return bitField &= ~(1u << flag);
	}

	public static ushort RemoveBitFlag(ushort bitField, int flag)
	{
		return bitField &= ~(ushort)(1 << flag);
	}

	public static byte RemoveBitFlag(byte bitField, int flag)
	{
		return bitField &= ~(byte)(1 << flag);
	}

	public static bool HasBitFlag(ulong bitField, int flag)
	{
		return (bitField & 1uL << flag) > 0uL;
	}

	public static bool HasBitFlag(uint bitField, int flag)
	{
		return (bitField & 1u << flag) > 0u;
	}

	public static bool HasBitFlag(ushort bitField, int flag)
	{
		return (bitField & (ushort)(1 << flag)) > 0;
	}

	public static bool HasBitFlag(byte bitField, int flag)
	{
		return (bitField & (byte)(1 << flag)) > 0;
	}

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

	private static string toStringWithLength(ulong bitField, int length)
	{
		StringBuilder stringBuilder = new StringBuilder(length);
		for (int i = length - 1; i >= 0; i--)
		{
			stringBuilder.Append((!BitField.HasBitFlag(bitField, i)) ? "0" : "1");
		}
		return stringBuilder.ToString();
	}

	public static string ToString(ulong bitField)
	{
		return BitField.toStringWithLength(bitField, 64);
	}

	public static string ToString(uint bitField)
	{
		return BitField.toStringWithLength((ulong)bitField, 32);
	}

	public static string ToString(ushort bitField)
	{
		return BitField.toStringWithLength((ulong)bitField, 16);
	}

	public static string ToString(byte bitField)
	{
		return BitField.toStringWithLength((ulong)bitField, 8);
	}
}
