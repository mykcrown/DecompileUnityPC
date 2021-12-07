using System;
using System.Collections.Generic;
using System.IO;

namespace InControl
{
	// Token: 0x0200005B RID: 91
	public struct KeyCombo
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x00014184 File Offset: 0x00012584
		public KeyCombo(params Key[] keys)
		{
			this.includeData = 0UL;
			this.includeSize = 0;
			this.excludeData = 0UL;
			this.excludeSize = 0;
			for (int i = 0; i < keys.Length; i++)
			{
				this.AddInclude(keys[i]);
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000141CC File Offset: 0x000125CC
		private void AddIncludeInt(int key)
		{
			if (this.includeSize == 8)
			{
				return;
			}
			this.includeData |= (ulong)((ulong)((long)key & 255L) << this.includeSize * 8);
			this.includeSize++;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001420B File Offset: 0x0001260B
		private int GetIncludeInt(int index)
		{
			return (int)(this.includeData >> index * 8 & 255UL);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00014222 File Offset: 0x00012622
		[Obsolete("Use KeyCombo.AddInclude instead.")]
		public void Add(Key key)
		{
			this.AddInclude(key);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001422B File Offset: 0x0001262B
		[Obsolete("Use KeyCombo.GetInclude instead.")]
		public Key Get(int index)
		{
			return this.GetInclude(index);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00014234 File Offset: 0x00012634
		public void AddInclude(Key key)
		{
			this.AddIncludeInt((int)key);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00014240 File Offset: 0x00012640
		public Key GetInclude(int index)
		{
			if (index < 0 || index >= this.includeSize)
			{
				throw new IndexOutOfRangeException(string.Concat(new object[]
				{
					"Index ",
					index,
					" is out of the range 0..",
					this.includeSize
				}));
			}
			return (Key)this.GetIncludeInt(index);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001429F File Offset: 0x0001269F
		private void AddExcludeInt(int key)
		{
			if (this.excludeSize == 8)
			{
				return;
			}
			this.excludeData |= (ulong)((ulong)((long)key & 255L) << this.excludeSize * 8);
			this.excludeSize++;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000142DE File Offset: 0x000126DE
		private int GetExcludeInt(int index)
		{
			return (int)(this.excludeData >> index * 8 & 255UL);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000142F5 File Offset: 0x000126F5
		public void AddExclude(Key key)
		{
			this.AddExcludeInt((int)key);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00014300 File Offset: 0x00012700
		public Key GetExclude(int index)
		{
			if (index < 0 || index >= this.excludeSize)
			{
				throw new IndexOutOfRangeException(string.Concat(new object[]
				{
					"Index ",
					index,
					" is out of the range 0..",
					this.excludeSize
				}));
			}
			return (Key)this.GetExcludeInt(index);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001435F File Offset: 0x0001275F
		public static KeyCombo With(params Key[] keys)
		{
			return new KeyCombo(keys);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00014368 File Offset: 0x00012768
		public KeyCombo AndNot(params Key[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				this.AddExclude(keys[i]);
			}
			return this;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00014398 File Offset: 0x00012798
		public void Clear()
		{
			this.includeData = 0UL;
			this.includeSize = 0;
			this.excludeData = 0UL;
			this.excludeSize = 0;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002EE RID: 750 RVA: 0x000143B8 File Offset: 0x000127B8
		[Obsolete("Use KeyCombo.IncludeCount instead.")]
		public int Count
		{
			get
			{
				return this.includeSize;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002EF RID: 751 RVA: 0x000143C0 File Offset: 0x000127C0
		public int IncludeCount
		{
			get
			{
				return this.includeSize;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x000143C8 File Offset: 0x000127C8
		public int ExcludeCount
		{
			get
			{
				return this.excludeSize;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x000143D0 File Offset: 0x000127D0
		public bool IsPressed
		{
			get
			{
				if (this.includeSize == 0)
				{
					return false;
				}
				bool flag = true;
				for (int i = 0; i < this.includeSize; i++)
				{
					int includeInt = this.GetIncludeInt(i);
					flag = (flag && KeyInfo.KeyList[includeInt].IsPressed);
				}
				for (int j = 0; j < this.excludeSize; j++)
				{
					int excludeInt = this.GetExcludeInt(j);
					if (KeyInfo.KeyList[excludeInt].IsPressed)
					{
						return false;
					}
				}
				return flag;
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00014460 File Offset: 0x00012860
		public static KeyCombo Detect(bool modifiersAsKeys)
		{
			KeyCombo result = default(KeyCombo);
			if (modifiersAsKeys)
			{
				for (int i = 5; i < 13; i++)
				{
					if (KeyInfo.KeyList[i].IsPressed)
					{
						result.AddIncludeInt(i);
						return result;
					}
				}
			}
			else
			{
				for (int j = 1; j < 5; j++)
				{
					if (KeyInfo.KeyList[j].IsPressed)
					{
						result.AddIncludeInt(j);
					}
				}
			}
			for (int k = 13; k < KeyInfo.KeyList.Length; k++)
			{
				if (KeyInfo.KeyList[k].IsPressed)
				{
					result.AddIncludeInt(k);
					return result;
				}
			}
			result.Clear();
			return result;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00014524 File Offset: 0x00012924
		public override string ToString()
		{
			string text;
			if (!KeyCombo.cachedStrings.TryGetValue(this.includeData, out text))
			{
				text = string.Empty;
				for (int i = 0; i < this.includeSize; i++)
				{
					if (i != 0)
					{
						text += " ";
					}
					int includeInt = this.GetIncludeInt(i);
					text += KeyInfo.KeyList[includeInt].Name;
				}
			}
			return text;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00014597 File Offset: 0x00012997
		public static bool operator ==(KeyCombo a, KeyCombo b)
		{
			return a.includeData == b.includeData && a.excludeData == b.excludeData;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000145BF File Offset: 0x000129BF
		public static bool operator !=(KeyCombo a, KeyCombo b)
		{
			return a.includeData != b.includeData || a.excludeData != b.excludeData;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000145EC File Offset: 0x000129EC
		public override bool Equals(object other)
		{
			if (other is KeyCombo)
			{
				KeyCombo keyCombo = (KeyCombo)other;
				return this.includeData == keyCombo.includeData && this.excludeData == keyCombo.excludeData;
			}
			return false;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00014634 File Offset: 0x00012A34
		public override int GetHashCode()
		{
			int num = 17;
			num = num * 31 + this.includeData.GetHashCode();
			return num * 31 + this.excludeData.GetHashCode();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00014674 File Offset: 0x00012A74
		internal void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			if (dataFormatVersion == 1)
			{
				this.includeSize = reader.ReadInt32();
				this.includeData = reader.ReadUInt64();
				return;
			}
			if (dataFormatVersion == 2)
			{
				this.includeSize = reader.ReadInt32();
				this.includeData = reader.ReadUInt64();
				this.excludeSize = reader.ReadInt32();
				this.excludeData = reader.ReadUInt64();
				return;
			}
			throw new InControlException("Unknown data format version: " + dataFormatVersion);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000146EE File Offset: 0x00012AEE
		internal void Save(BinaryWriter writer)
		{
			writer.Write(this.includeSize);
			writer.Write(this.includeData);
			writer.Write(this.excludeSize);
			writer.Write(this.excludeData);
		}

		// Token: 0x04000278 RID: 632
		private int includeSize;

		// Token: 0x04000279 RID: 633
		private ulong includeData;

		// Token: 0x0400027A RID: 634
		private int excludeSize;

		// Token: 0x0400027B RID: 635
		private ulong excludeData;

		// Token: 0x0400027C RID: 636
		private static Dictionary<ulong, string> cachedStrings = new Dictionary<ulong, string>();
	}
}
