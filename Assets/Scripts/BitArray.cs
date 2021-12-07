// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

public class BitArray : IEquatable<global::BitArray>, IList<bool>, ICollection<bool>, IEnumerable<bool>, IEnumerable
{
	private sealed class _GetEnumerator_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<bool>
	{
		internal int _index___0;

		internal global::BitArray _this;

		internal bool _current;

		internal bool _disposing;

		internal int _PC;

		bool IEnumerator<bool>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _GetEnumerator_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._index___0 = 0;
				break;
			case 1u:
				this._index___0++;
				break;
			default:
				return false;
			}
			if (this._index___0 < this._this.Count)
			{
				this._current = this._this[this._index___0];
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const int BITS_PER_CHUNK = 8;

	private byte[] chunks;

	public int Count
	{
		get;
		private set;
	}

	public int Capacity
	{
		get;
		private set;
	}

	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

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

	public BitArray(IList<bool> values) : this(values.Count, false)
	{
		this.AddRange(values);
	}

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

	public BitArray(global::BitArray other) : this(other.Capacity, false)
	{
		this.Count = other.Count;
		for (int i = 0; i < other.chunks.Length; i++)
		{
			this.chunks[i] = other.chunks[i];
		}
	}

	public BitArray(byte[] bytes) : this(bytes.Length * 8, false)
	{
		for (int i = 0; i < this.chunks.Length; i++)
		{
			this.chunks[i] = bytes[i];
		}
		this.Count = this.Capacity;
	}

	private int capacityToChunkCount(int capacity)
	{
		return capacity / 8 + 1;
	}

	public void Resize(int capacity)
	{
		if (capacity < this.Count)
		{
			throw new ArgumentException("New capacity cannot be less than current count.");
		}
		this.Capacity = capacity;
		Array.Resize<byte>(ref this.chunks, this.capacityToChunkCount(capacity));
	}

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

	private void closeGap(int index, int size)
	{
		for (int i = index; i < this.Count - size; i++)
		{
			this[i] = this[i + size];
		}
		this.Count -= size;
	}

	private void openGap(int index, int size)
	{
		this.Count += size;
		for (int i = this.Count - 1; i >= index + size; i--)
		{
			this[i] = this[i - size];
		}
	}

	private void resizeIfSpaceNeeded(int requestedSpace)
	{
		if (this.Count + requestedSpace >= this.Capacity)
		{
			this.Resize(Math.Max(this.Capacity * 2, this.Count + requestedSpace));
		}
	}

	public void Add(bool value)
	{
		if (this.Count >= this.Capacity)
		{
			this.Resize(this.Capacity * 2);
		}
		this.unsafeSet(this.Count, value);
		this.Count++;
	}

	public void ReserveCapacity()
	{
		this.Reserve(this.Capacity - this.Count);
	}

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

	public void AddPrepend(bool value)
	{
		this.Insert(0, value);
	}

	public void RemoveAt(int index)
	{
		if (index < 0 || index >= this.Count)
		{
			throw new IndexOutOfRangeException();
		}
		this.closeGap(index, 1);
	}

	public void RemoveFirst()
	{
		this.RemoveAt(0);
	}

	public void RemoveLast()
	{
		this.RemoveAt(this.Count - 1);
	}

	public void Set(int index, bool value)
	{
		if (index < 0 || index >= this.Count)
		{
			throw new IndexOutOfRangeException();
		}
		this.unsafeSet(index, value);
	}

	public void AddRange(IList<bool> values)
	{
		int count = this.Count;
		this.Reserve(values.Count);
		foreach (bool current in values)
		{
			this.unsafeSet(count++, current);
		}
	}

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

	public void AddBytePattern(byte pattern, int bitsOfBytes = 8)
	{
		int count = this.Count;
		this.Reserve(bitsOfBytes);
		for (int i = 0; i < bitsOfBytes; i++)
		{
			this.unsafeSet(count + i, BitField.HasBitFlag(pattern, i));
		}
	}

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

	public void WriteShort(int index, ushort pattern)
	{
		this.WriteByte(index, (byte)((pattern & 255) >> 0), 8);
		this.WriteByte(index + 8, (byte)((pattern & 65280) >> 8), 8);
	}

	public void WriteInt(int index, uint pattern)
	{
		this.WriteByte(index, (byte)((pattern & 255u) >> 0), 8);
		this.WriteByte(index + 8, (byte)((pattern & 65280u) >> 8), 8);
		this.WriteByte(index + 16, (byte)((pattern & 16711680u) >> 16), 8);
		this.WriteByte(index + 24, (byte)((pattern & 4278190080u) >> 24), 8);
	}

	public void WriteLong(int index, ulong pattern)
	{
		this.WriteByte(index, (byte)((pattern & 255uL) >> 0), 8);
		this.WriteByte(index + 8, (byte)((pattern & 65280uL) >> 8), 8);
		this.WriteByte(index + 16, (byte)((pattern & 16711680uL) >> 16), 8);
		this.WriteByte(index + 24, (byte)((pattern & (ulong)(-16777216)) >> 24), 8);
		this.WriteByte(index + 32, (byte)((pattern & 1095216660480uL) >> 32), 8);
		this.WriteByte(index + 40, (byte)((pattern & 280375465082880uL) >> 40), 8);
		this.WriteByte(index + 48, (byte)((pattern & 71776119061217280uL) >> 48), 8);
		this.WriteByte(index + 56, (byte)((pattern & 18374686479671623680uL) >> 56), 8);
	}

	public void WriteByte(byte pattern)
	{
		this.WriteByte(0, pattern, 8);
	}

	public void WriteShort(ushort pattern)
	{
		this.WriteShort(0, pattern);
	}

	public void WriteInt(uint pattern)
	{
		this.WriteInt(0, pattern);
	}

	public void WriteLong(ulong pattern)
	{
		this.WriteLong(0, pattern);
	}

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

	public void Clear()
	{
		for (int i = 0; i < this.chunks.Length; i++)
		{
			this.chunks[i] = 0;
		}
		this.Count = 0;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(this.Count);
		for (int i = 0; i < this.Count; i++)
		{
			stringBuilder.Append((!this[i]) ? '0' : '1');
		}
		return stringBuilder.ToString();
	}

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

	public bool[] ToBoolArray()
	{
		bool[] array = new bool[this.Count];
		for (int i = 0; i < this.Count; i++)
		{
			array[i] = this[i];
		}
		return array;
	}

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

	public bool Contains(bool item)
	{
		return this.IndexOf(item) >= 0;
	}

	public void CopyTo(bool[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

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

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	public IEnumerator<bool> GetEnumerator()
	{
		global::BitArray._GetEnumerator_c__Iterator0 _GetEnumerator_c__Iterator = new global::BitArray._GetEnumerator_c__Iterator0();
		_GetEnumerator_c__Iterator._this = this;
		return _GetEnumerator_c__Iterator;
	}

	public byte[] GetBytes()
	{
		return this.chunks;
	}

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
		ulong num = 0uL;
		int num2 = 0;
		while (num2 < chunkCount && num2 < this.chunks.Length)
		{
			num += (ulong)this.chunks[num2 + chunkIndex] << 8 * num2;
			num2++;
		}
		return num;
	}

	public byte ToByte()
	{
		if (this.Count > 8)
		{
			throw new InsufficientBitSpaceException();
		}
		return (byte)this.combineChunks(0, 1);
	}

	public ushort ToShort()
	{
		if (this.Count > 16)
		{
			throw new InsufficientBitSpaceException();
		}
		return (ushort)this.combineChunks(0, 2);
	}

	public uint ToInt()
	{
		if (this.Count > 32)
		{
			throw new InsufficientBitSpaceException();
		}
		return (uint)this.combineChunks(0, 4);
	}

	public ulong ToLong()
	{
		if (this.Count > 64)
		{
			throw new InsufficientBitSpaceException();
		}
		return this.combineChunks(0, 8);
	}
}
