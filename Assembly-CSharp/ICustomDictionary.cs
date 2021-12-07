using System;

// Token: 0x02000AA9 RID: 2729
public interface ICustomDictionary<TKey, TValue>
{
	// Token: 0x170012E5 RID: 4837
	TValue this[TKey key]
	{
		get;
		set;
	}

	// Token: 0x06005027 RID: 20519
	void Add(TKey key, TValue value);

	// Token: 0x06005028 RID: 20520
	bool ContainsKey(TKey key);

	// Token: 0x06005029 RID: 20521
	bool Remove(TKey key);

	// Token: 0x0600502A RID: 20522
	bool TryGetValue(TKey key, out TValue value);
}
