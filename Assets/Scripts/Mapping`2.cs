// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class Mapping<TFirst, TSecond> : IEnumerable<TFirst>, IEnumerable
{
	private Dictionary<TFirst, TSecond> firstToSecond;

	private Dictionary<TSecond, TFirst> secondToFirst;

	public TSecond this[TFirst key]
	{
		get
		{
			return this.GetSecond(key);
		}
	}

	public TFirst this[TSecond val]
	{
		get
		{
			return this.GetFirst(val);
		}
	}

	public int Count
	{
		get
		{
			return this.firstToSecond.Count;
		}
	}

	public Mapping()
	{
		this.firstToSecond = new Dictionary<TFirst, TSecond>();
		this.secondToFirst = new Dictionary<TSecond, TFirst>();
	}

	public Mapping(Mapping<TFirst, TSecond> other)
	{
		this.firstToSecond = new Dictionary<TFirst, TSecond>(other.firstToSecond);
		this.secondToFirst = new Dictionary<TSecond, TFirst>(other.secondToFirst);
	}

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

	public TSecond GetSecond(TFirst first)
	{
		return this.firstToSecond[first];
	}

	public TFirst GetFirst(TSecond second)
	{
		return this.secondToFirst[second];
	}

	public void Clear()
	{
		this.firstToSecond.Clear();
		this.secondToFirst.Clear();
	}

	public bool ContainsFirst(TFirst first)
	{
		return first != null && this.firstToSecond.ContainsKey(first);
	}

	public bool ContainsSecond(TSecond value)
	{
		return value != null && this.secondToFirst.ContainsKey(value);
	}

	public bool AreEitherInMapping(TFirst first, TSecond second)
	{
		return this.ContainsFirst(first) || this.ContainsSecond(second);
	}

	public bool AreMappedTogether(TFirst first, TSecond second)
	{
		int arg_60_0;
		if (this.ContainsFirst(first) && this.ContainsSecond(second))
		{
			TSecond tSecond = this.firstToSecond[first];
			if (tSecond.Equals(second))
			{
				TFirst tFirst = this.secondToFirst[second];
				arg_60_0 = (tFirst.Equals(first) ? 1 : 0);
				return arg_60_0 != 0;
			}
		}
		arg_60_0 = 0;
		return arg_60_0 != 0;
	}

	public Dictionary<TFirst, TSecond> Raw()
	{
		return this.firstToSecond;
	}

	public IEnumerator<TFirst> GetEnumerator()
	{
		return this.firstToSecond.Keys.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
}
