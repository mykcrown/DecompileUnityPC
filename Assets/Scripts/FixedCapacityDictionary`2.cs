// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class FixedCapacityDictionary<TKey, TValue> : ICustomDictionary<TKey, TValue>
{
	private Dictionary<TKey, TValue> dict;

	public TValue this[TKey key]
	{
		get
		{
			return this.dict[key];
		}
		set
		{
			this.dict[key] = value;
		}
	}

	public int Count
	{
		get
		{
			return this.dict.Count;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	public FixedCapacityDictionary(int capacity)
	{
		this.dict = new Dictionary<TKey, TValue>(capacity);
	}

	public FixedCapacityDictionary(int capacity, IEqualityComparer<TKey> comparer)
	{
		this.dict = new Dictionary<TKey, TValue>(capacity, comparer);
	}

	public Dictionary<TKey, TValue> GetEnumerableList()
	{
		return this.dict;
	}

	public void Add(KeyValuePair<TKey, TValue> item)
	{
		this.dict.Add(item.Key, item.Value);
	}

	public void Add(TKey key, TValue value)
	{
		this.Add(new KeyValuePair<TKey, TValue>(key, value));
	}

	public void Clear()
	{
		this.dict.Clear();
	}

	public bool ContainsKey(TKey key)
	{
		return this.dict.ContainsKey(key);
	}

	public void CopyTo(FixedCapacityDictionary<TKey, TValue> targetDict)
	{
		targetDict.Clear();
		foreach (TKey current in this.dict.Keys)
		{
			targetDict[current] = this.dict[current];
		}
	}

	public bool Remove(TKey key)
	{
		return this.dict.Remove(key);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		return this.dict.TryGetValue(key, out value);
	}
}
