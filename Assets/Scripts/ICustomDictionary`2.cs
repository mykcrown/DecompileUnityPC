// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICustomDictionary<TKey, TValue>
{
	TValue this[TKey key]
	{
		get;
		set;
	}

	void Add(TKey key, TValue value);

	bool ContainsKey(TKey key);

	bool Remove(TKey key);

	bool TryGetValue(TKey key, out TValue value);
}
