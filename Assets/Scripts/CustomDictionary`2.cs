// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class CustomDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ICustomDictionary<TKey, TValue>
{
	TValue ICustomDictionary<TKey, TValue>.get_Item(TKey key)
	{
		return base[key];
	}

	void ICustomDictionary<TKey, TValue>.set_Item(TKey key, TValue value)
	{
		base[key] = value;
	}

	void ICustomDictionary<TKey, TValue>.Add(TKey key, TValue value)
	{
		base.Add(key, value);
	}

	bool ICustomDictionary<TKey, TValue>.ContainsKey(TKey key)
	{
		return base.ContainsKey(key);
	}

	bool ICustomDictionary<TKey, TValue>.Remove(TKey key)
	{
		return base.Remove(key);
	}

	bool ICustomDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
	{
		return base.TryGetValue(key, out value);
	}
}
