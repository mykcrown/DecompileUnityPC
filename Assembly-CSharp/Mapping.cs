using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000B1B RID: 2843
public class Mapping<TFirst, TSecond> : IEnumerable<TFirst>, IEnumerable
{
	// Token: 0x06005177 RID: 20855 RVA: 0x001525F3 File Offset: 0x001509F3
	public Mapping()
	{
		this.firstToSecond = new Dictionary<TFirst, TSecond>();
		this.secondToFirst = new Dictionary<TSecond, TFirst>();
	}

	// Token: 0x06005178 RID: 20856 RVA: 0x00152611 File Offset: 0x00150A11
	public Mapping(Mapping<TFirst, TSecond> other)
	{
		this.firstToSecond = new Dictionary<TFirst, TSecond>(other.firstToSecond);
		this.secondToFirst = new Dictionary<TSecond, TFirst>(other.secondToFirst);
	}

	// Token: 0x06005179 RID: 20857 RVA: 0x0015263B File Offset: 0x00150A3B
	public bool Add(TFirst first, TSecond second)
	{
		if (!this.ContainsFirst(first) && !this.ContainsSecond(second))
		{
			this.firstToSecond.Add(first, second);
			this.secondToFirst.Add(second, first);
			return true;
		}
		return false;
	}

	// Token: 0x0600517A RID: 20858 RVA: 0x00152674 File Offset: 0x00150A74
	public bool RemoveByFirst(TFirst first)
	{
		if (this.ContainsFirst(first))
		{
			TSecond key = this.firstToSecond[first];
			this.firstToSecond.Remove(first);
			this.secondToFirst.Remove(key);
			return true;
		}
		return false;
	}

	// Token: 0x0600517B RID: 20859 RVA: 0x001526B8 File Offset: 0x00150AB8
	public bool RemoveBySecond(TSecond second)
	{
		if (this.ContainsSecond(second))
		{
			TFirst key = this.secondToFirst[second];
			this.secondToFirst.Remove(second);
			this.firstToSecond.Remove(key);
			return true;
		}
		return false;
	}

	// Token: 0x0600517C RID: 20860 RVA: 0x001526FB File Offset: 0x00150AFB
	public TSecond GetSecond(TFirst first)
	{
		return this.firstToSecond[first];
	}

	// Token: 0x0600517D RID: 20861 RVA: 0x00152709 File Offset: 0x00150B09
	public TFirst GetFirst(TSecond second)
	{
		return this.secondToFirst[second];
	}

	// Token: 0x1700130C RID: 4876
	public TSecond this[TFirst key]
	{
		get
		{
			return this.GetSecond(key);
		}
	}

	// Token: 0x1700130D RID: 4877
	public TFirst this[TSecond val]
	{
		get
		{
			return this.GetFirst(val);
		}
	}

	// Token: 0x06005180 RID: 20864 RVA: 0x00152729 File Offset: 0x00150B29
	public void Clear()
	{
		this.firstToSecond.Clear();
		this.secondToFirst.Clear();
	}

	// Token: 0x06005181 RID: 20865 RVA: 0x00152741 File Offset: 0x00150B41
	public bool ContainsFirst(TFirst first)
	{
		return first != null && this.firstToSecond.ContainsKey(first);
	}

	// Token: 0x06005182 RID: 20866 RVA: 0x0015275D File Offset: 0x00150B5D
	public bool ContainsSecond(TSecond value)
	{
		return value != null && this.secondToFirst.ContainsKey(value);
	}

	// Token: 0x06005183 RID: 20867 RVA: 0x00152779 File Offset: 0x00150B79
	public bool AreEitherInMapping(TFirst first, TSecond second)
	{
		return this.ContainsFirst(first) || this.ContainsSecond(second);
	}

	// Token: 0x06005184 RID: 20868 RVA: 0x00152794 File Offset: 0x00150B94
	public bool AreMappedTogether(TFirst first, TSecond second)
	{
		if (this.ContainsFirst(first) && this.ContainsSecond(second))
		{
			TSecond tsecond = this.firstToSecond[first];
			if (tsecond.Equals(second))
			{
				TFirst tfirst = this.secondToFirst[second];
				return tfirst.Equals(first);
			}
		}
		return false;
	}

	// Token: 0x1700130E RID: 4878
	// (get) Token: 0x06005185 RID: 20869 RVA: 0x00152801 File Offset: 0x00150C01
	public int Count
	{
		get
		{
			return this.firstToSecond.Count;
		}
	}

	// Token: 0x06005186 RID: 20870 RVA: 0x0015280E File Offset: 0x00150C0E
	public Dictionary<TFirst, TSecond> Raw()
	{
		return this.firstToSecond;
	}

	// Token: 0x06005187 RID: 20871 RVA: 0x00152816 File Offset: 0x00150C16
	public IEnumerator<TFirst> GetEnumerator()
	{
		return this.firstToSecond.Keys.GetEnumerator();
	}

	// Token: 0x06005188 RID: 20872 RVA: 0x0015282D File Offset: 0x00150C2D
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x04003476 RID: 13430
	private Dictionary<TFirst, TSecond> firstToSecond;

	// Token: 0x04003477 RID: 13431
	private Dictionary<TSecond, TFirst> secondToFirst;
}
