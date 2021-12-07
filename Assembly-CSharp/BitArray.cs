using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000A9F RID: 2719
public class BitArray : IEquatable<global::BitArray>, IList<bool>, ICollection<bool>, IEnumerable<bool>, IEnumerable
{
	// Token: 0x06004FA6 RID: 20390 RVA: 0x0014CF30 File Offset: 0x0014B330
	public BitArray(int capacity = 32, bool startReserved = false)
	{
		this.Capacity = capacity;
		this.Count = 0;
		this.chunks = new byte[this.capacityToChunkCount(capacity)];
		if (startReserved)
		{
			this.ReserveCapacity();
		}
	}

	// Token: 0x06004FA7 RID: 20391 RVA: 0x0014CF64 File Offset: 0x0014B364
	public BitArray(IList<bool> values) : this(values.Count, false)
	{
		this.AddRange(values);
	}

	// Token: 0x06004FA8 RID: 20392 RVA: 0x0014CF7C File Offset: 0x0014B37C
	public BitArray(string bitString) : this(bitString.Length, false)
	{
		for (int i = 0; i < bitString.Length; i++)
		{
			if (bitString[i] == '1')
			{
				this.Add(true);
			}
			else
			{
				if (bitString[i] != '0')
				{
					throw new ArgumentException("Invalid bit string.  Bit string may only contain '0' or '1' characters.");
				}
				this.Add(false);
			}
		}
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x0014CFEC File Offset: 0x0014B3EC
	public BitArray(global::BitArray other) : this(other.Capacity, false)
	{
		this.Count = other.Count;
		for (int i = 0; i < other.chunks.Length; i++)
		{
			this.chunks[i] = other.chunks[i];
		}
	}

	// Token: 0x06004FAA RID: 20394 RVA: 0x0014D03C File Offset: 0x0014B43C
	public BitArray(byte[] bytes) : this(bytes.Length * 8, false)
	{
		for (int i = 0; i < this.chunks.Length; i++)
		{
			this.chunks[i] = bytes[i];
		}
		this.Count = this.Capacity;
	}

	// Token: 0x170012DD RID: 4829
	// (get) Token: 0x06004FAB RID: 20395 RVA: 0x0014D085 File Offset: 0x0014B485
	// (set) Token: 0x06004FAC RID: 20396 RVA: 0x0014D08D File Offset: 0x0014B48D
	public int Count { get; private set; }

	// Token: 0x170012DE RID: 4830
	// (get) Token: 0x06004FAD RID: 20397 RVA: 0x0014D096 File Offset: 0x0014B496
	// (set) Token: 0x06004FAE RID: 20398 RVA: 0x0014D09E File Offset: 0x0014B49E
	public int Capacity { get; private set; }

	// Token: 0x170012DF RID: 4831
	// (get) Token: 0x06004FAF RID: 20399 RVA: 0x0014D0A7 File Offset: 0x0014B4A7
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004FB0 RID: 20400 RVA: 0x0014D0AA File Offset: 0x0014B4AA
	private int capacityToChunkCount(int capacity)
	{
		return capacity / 8 + 1;
	}

	// Token: 0x06004FB1 RID: 20401 RVA: 0x0014D0B1 File Offset: 0x0014B4B1
	public void Resize(int capacity)
	{
		if (capacity < this.Count)
		{
			throw new ArgumentException("New capacity cannot be less than current count.");
		}
		this.Capacity = capacity;
		Array.Resize<byte>(ref this.chunks, this.capacityToChunkCount(capacity));
	}

	// Token: 0x06004FB2 RID: 20402 RVA: 0x0014D0E4 File Offset: 0x0014B4E4
	private void unsafeSet(int index, bool value)
	{
		int num = index / 8;
		int flag = index - num * 8;
		if (value)
		{
			this.chunks[num] = BitField.AddBitFlag(this.chunks[num], flag);
		}
		else
		{
			this.chunks[num] = BitField.RemoveBitFlag(this.chunks[num], flag);
		}
	}

	// Token: 0x06004FB3 RID: 20403 RVA: 0x0014D134 File Offset: 0x0014B534
	private void closeGap(int index, int size)
	{
		for (int i = index; i < this.Count - size; i++)
		{
			this[i] = this[i + size];
		}
		this.Count -= size;
	}

	// Token: 0x06004FB4 RID: 20404 RVA: 0x0014D178 File Offset: 0x0014B578
	private void openGap(int index, int size)
	{
		this.Count += size;
		for (int i = this.Count - 1; i >= index + size; i--)
		{
			this[i] = this[i - size];
		}
	}

	// Token: 0x06004FB5 RID: 20405 RVA: 0x0014D1BE File Offset: 0x0014B5BE
	private void resizeIfSpaceNeeded(int requestedSpace)
	{
		if (this.Count + requestedSpace >= this.Capacity)
		{
			this.Resize(Math.Max(this.Capacity * 2, this.Count + requestedSpace));
		}
	}

	// Token: 0x06004FB6 RID: 20406 RVA: 0x0014D1EE File Offset: 0x0014B5EE
	public void Add(bool value)
	{
		if (this.Count >= this.Capacity)
		{
			this.Resize(this.Capacity * 2);
		}
		this.unsafeSet(this.Count, value);
		this.Count++;
	}

	// Token: 0x06004FB7 RID: 20407 RVA: 0x0014D22A File Offset: 0x0014B62A
	public void ReserveCapacity()
	{
		this.Reserve(this.Capacity - this.Count);
	}

	// Token: 0x06004FB8 RID: 20408 RVA: 0x0014D240 File Offset: 0x0014B640
	public void Reserve(int amount)
	{
		this.resizeIfSpaceNeeded(amount);
		int count = this.Count;
		this.Count += amount;
		for (int i = count; i < this.Count; i++)
		{
			this[i] = false;
		}
	}

	// Token: 0x06004FB9 RID: 20409 RVA: 0x0014D288 File Offset: 0x0014B688
	public void Insert(int index, bool value)
	{
		if (index < 0 || index > this.Count)
		{
			throw new IndexOutOfRangeException();
		}
		if (this.Count >= this.Capacity)
		{
			this.Resize(this.Capacity * 2);
		}
		this.openGap(index, 1);
		this.unsafeSet(index, value);
	}

	// Token: 0x06004FBA RID: 20410 RVA: 0x0014D2DD File Offset: 0x0014B6DD
	public void AddPrepend(bool value)
	{
		this.Insert(0, value);
	}

	// Token: 0x06004FBB RID: 20411 RVA: 0x0014D2E7 File Offset: 0x0014B6E7
	public void RemoveAt(int index)
	{
		if (index < 0 || index >= this.Count)
		{
			throw new IndexOutOfRangeException();
		}
		this.closeGap(index, 1);
	}

	// Token: 0x06004FBC RID: 20412 RVA: 0x0014D30A File Offset: 0x0014B70A
	public void RemoveFirst()
	{
		this.RemoveAt(0);
	}

	// Token: 0x06004FBD RID: 20413 RVA: 0x0014D313 File Offset: 0x0014B713
	public void RemoveLast()
	{
		this.RemoveAt(this.Count - 1);
	}

	// Token: 0x06004FBE RID: 20414 RVA: 0x0014D323 File Offset: 0x0014B723
	public void Set(int index, bool value)
	{
		if (index < 0 || index >= this.Count)
		{
			throw new IndexOutOfRangeException();
		}
		this.unsafeSet(index, value);
	}

	// Token: 0x06004FBF RID: 20415 RVA: 0x0014D348 File Offset: 0x0014B748
	public void AddRange(IList<bool> values)
	{
		int count = this.Count;
		this.Reserve(values.Count);
		foreach (bool value in values)
		{
			this.unsafeSet(count++, value);
		}
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x0014D3B4 File Offset: 0x0014B7B4
	public void InsertRange(int index, IList<bool> values)
	{
		if (index < 0 || index > this.Count)
		{
			throw new IndexOutOfRangeException();
		}
		int count = values.Count;
		this.resizeIfSpaceNeeded(count);
		this.openGap(index, count);
		for (int i = index; i < index + count; i++)
		{
			this.unsafeSet(i, values[i - index]);
		}
	}

	// Token: 0x06004FC1 RID: 20417 RVA: 0x0014D414 File Offset: 0x0014B814
	public void SetRange(int index, IList<bool> values)
	{
		if (index < 0 || index + values.Count >= this.Count)
		{
			throw new IndexOutOfRangeException();
		}
		for (int i = index; i < index + values.Count; i++)
		{
			this.unsafeSet(i, values[i]);
		}
	}

	// Token: 0x06004FC2 RID: 20418 RVA: 0x0014D468 File Offset: 0x0014B868
	public void RemoveRange(int index, int count)
	{
		if (count < 0 || index < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (index + count > this.Count)
		{
			throw new ArgumentException();
		}
		this.closeGap(index, count);
	}

	// Token: 0x06004FC3 RID: 20419 RVA: 0x0014D49C File Offset: 0x0014B89C
	public void AddBytePattern(byte pattern, int bitsOfBytes = 8)
	{
		int count = this.Count;
		this.Reserve(bitsOfBytes);
		for (int i = 0; i < bitsOfBytes; i++)
		{
			this.unsafeSet(count + i, BitField.HasBitFlag(pattern, i));
		}
	}

	// Token: 0x06004FC4 RID: 20420 RVA: 0x0014D4DC File Offset: 0x0014B8DC
	public void InsertBytePattern(int index, byte pattern, int bitsOfBytes = 0)
	{
		if (index < 0 || index > this.Count)
		{
			throw new ArgumentOutOfRangeException();
		}
		this.resizeIfSpaceNeeded(bitsOfBytes);
		this.openGap(index, bitsOfBytes);
		for (int i = 0; i < bitsOfBytes; i++)
		{
			this.unsafeSet(index + i, BitField.HasBitFlag(pattern, i));
		}
	}

	// Token: 0x06004FC5 RID: 20421 RVA: 0x0014D534 File Offset: 0x0014B934
	public void WriteByte(int index, byte pattern, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > this.Count)
		{
			throw new ArgumentOutOfRangeException();
		}
		for (int i = 0; i < leastSigBits; i++)
		{
			this[index + i] = BitField.HasBitFlag(pattern, i);
		}
	}

	// Token: 0x06004FC6 RID: 20422 RVA: 0x0014D57E File Offset: 0x0014B97E
	public void WriteShort(int index, ushort pattern)
	{
		this.WriteByte(index, (byte)((pattern & 255) >> 0), 8);
		this.WriteByte(index + 8, (byte)((pattern & 65280) >> 8), 8);
	}

	// Token: 0x06004FC7 RID: 20423 RVA: 0x0014D5A8 File Offset: 0x0014B9A8
	public void WriteInt(int index, uint pattern)
	{
		this.WriteByte(index, (byte)((pattern & 255U) >> 0), 8);
		this.WriteByte(index + 8, (byte)((pattern & 65280U) >> 8), 8);
		this.WriteByte(index + 16, (byte)((pattern & 16711680U) >> 16), 8);
		this.WriteByte(index + 24, (byte)((pattern & 4278190080U) >> 24), 8);
	}

	// Token: 0x06004FC8 RID: 20424 RVA: 0x0014D608 File Offset: 0x0014BA08
	public void WriteLong(int index, ulong pattern)
	{
		this.WriteByte(index, (byte)((pattern & 255UL) >> 0), 8);
		this.WriteByte(index + 8, (byte)((pattern & 65280UL) >> 8), 8);
		this.WriteByte(index + 16, (byte)((pattern & 16711680UL) >> 16), 8);
		this.WriteByte(index + 24, (byte)((pattern & (ulong)-16777216) >> 24), 8);
		this.WriteByte(index + 32, (byte)((pattern & 1095216660480UL) >> 32), 8);
		this.WriteByte(index + 40, (byte)((pattern & 280375465082880UL) >> 40), 8);
		this.WriteByte(index + 48, (byte)((pattern & 71776119061217280UL) >> 48), 8);
		this.WriteByte(index + 56, (byte)((pattern & 18374686479671623680UL) >> 56), 8);
	}

	// Token: 0x06004FC9 RID: 20425 RVA: 0x0014D6D3 File Offset: 0x0014BAD3
	public void WriteByte(byte pattern)
	{
		this.WriteByte(0, pattern, 8);
	}

	// Token: 0x06004FCA RID: 20426 RVA: 0x0014D6DE File Offset: 0x0014BADE
	public void WriteShort(ushort pattern)
	{
		this.WriteShort(0, pattern);
	}

	// Token: 0x06004FCB RID: 20427 RVA: 0x0014D6E8 File Offset: 0x0014BAE8
	public void WriteInt(uint pattern)
	{
		this.WriteInt(0, pattern);
	}

	// Token: 0x06004FCC RID: 20428 RVA: 0x0014D6F2 File Offset: 0x0014BAF2
	public void WriteLong(ulong pattern)
	{
		this.WriteLong(0, pattern);
	}

	// Token: 0x06004FCD RID: 20429 RVA: 0x0014D6FC File Offset: 0x0014BAFC
	public byte ReadByte(int index, int leastSigBits = 8)
	{
		if (index < 0 || index + leastSigBits > this.Count)
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
			if (this[i + index])
			{
				b = BitField.AddBitFlag(b, i);
			}
		}
		return b;
	}

	// Token: 0x06004FCE RID: 20430 RVA: 0x0014D764 File Offset: 0x0014BB64
	public bool Get(int index)
	{
		if (index < 0 || index >= this.Count)
		{
			throw new IndexOutOfRangeException();
		}
		int num = index / 8;
		int flag = index - num * 8;
		return BitField.HasBitFlag(this.chunks[num], flag);
	}

	// Token: 0x170012E0 RID: 4832
	public bool this[int index]
	{
		get
		{
			return this.Get(index);
		}
		set
		{
			this.Set(index, value);
		}
	}

	// Token: 0x06004FD1 RID: 20433 RVA: 0x0014D7B8 File Offset: 0x0014BBB8
	public void Clear()
	{
		for (int i = 0; i < this.chunks.Length; i++)
		{
			this.chunks[i] = 0;
		}
		this.Count = 0;
	}

	// Token: 0x06004FD2 RID: 20434 RVA: 0x0014D7F0 File Offset: 0x0014BBF0
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(this.Count);
		for (int i = 0; i < this.Count; i++)
		{
			stringBuilder.Append((!this[i]) ? '0' : '1');
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06004FD3 RID: 20435 RVA: 0x0014D844 File Offset: 0x0014BC44
	public bool Equals(global::BitArray other)
	{
		if (this.Count != other.Count)
		{
			return false;
		}
		for (int i = 0; i < this.Count; i++)
		{
			if (this[i] != other[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004FD4 RID: 20436 RVA: 0x0014D894 File Offset: 0x0014BC94
	public bool[] ToBoolArray()
	{
		bool[] array = new bool[this.Count];
		for (int i = 0; i < this.Count; i++)
		{
			array[i] = this[i];
		}
		return array;
	}

	// Token: 0x06004FD5 RID: 20437 RVA: 0x0014D8D0 File Offset: 0x0014BCD0
	public int IndexOf(bool item)
	{
		for (int i = 0; i < this.Count; i++)
		{
			if (this[i] == item)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06004FD6 RID: 20438 RVA: 0x0014D904 File Offset: 0x0014BD04
	public bool Contains(bool item)
	{
		return this.IndexOf(item) >= 0;
	}

	// Token: 0x06004FD7 RID: 20439 RVA: 0x0014D913 File Offset: 0x0014BD13
	public void CopyTo(bool[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004FD8 RID: 20440 RVA: 0x0014D91C File Offset: 0x0014BD1C
	public bool Remove(bool item)
	{
		int num = this.IndexOf(item);
		if (num >= 0)
		{
			this.RemoveAt(num);
			return true;
		}
		return false;
	}

	// Token: 0x06004FD9 RID: 20441 RVA: 0x0014D942 File Offset: 0x0014BD42
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x06004FDA RID: 20442 RVA: 0x0014D94C File Offset: 0x0014BD4C
	public IEnumerator<bool> GetEnumerator()
	{
		for (int index = 0; index < this.Count; index++)
		{
			yield return this[index];
		}
		yield break;
	}

	// Token: 0x06004FDB RID: 20443 RVA: 0x0014D967 File Offset: 0x0014BD67
	public byte[] GetBytes()
	{
		return this.chunks;
	}

	// Token: 0x06004FDC RID: 20444 RVA: 0x0014D970 File Offset: 0x0014BD70
	private ulong combineChunks(int chunkIndex, int chunkCount)
	{
		if (chunkCount > 8)
		{
			throw new InsufficientBitSpaceException();
		}
		if (chunkIndex < 0 || chunkCount < 0 || chunkIndex >= this.chunks.Length)
		{
			throw new ArgumentOutOfRangeException();
		}
		ulong num = 0UL;
		int num2 = 0;
		while (num2 < chunkCount && num2 < this.chunks.Length)
		{
			num += (ulong)this.chunks[num2 + chunkIndex] << 8 * num2;
			num2++;
		}
		return num;
	}

	// Token: 0x06004FDD RID: 20445 RVA: 0x0014D9E5 File Offset: 0x0014BDE5
	public byte ToByte()
	{
		if (this.Count > 8)
		{
			throw new InsufficientBitSpaceException();
		}
		return (byte)this.combineChunks(0, 1);
	}

	// Token: 0x06004FDE RID: 20446 RVA: 0x0014DA02 File Offset: 0x0014BE02
	public ushort ToShort()
	{
		if (this.Count > 16)
		{
			throw new InsufficientBitSpaceException();
		}
		return (ushort)this.combineChunks(0, 2);
	}

	// Token: 0x06004FDF RID: 20447 RVA: 0x0014DA20 File Offset: 0x0014BE20
	public uint ToInt()
	{
		if (this.Count > 32)
		{
			throw new InsufficientBitSpaceException();
		}
		return (uint)this.combineChunks(0, 4);
	}

	// Token: 0x06004FE0 RID: 20448 RVA: 0x0014DA3E File Offset: 0x0014BE3E
	public ulong ToLong()
	{
		if (this.Count > 64)
		{
			throw new InsufficientBitSpaceException();
		}
		return this.combineChunks(0, 8);
	}

	// Token: 0x040033A4 RID: 13220
	private const int BITS_PER_CHUNK = 8;

	// Token: 0x040033A5 RID: 13221
	private byte[] chunks;
}
