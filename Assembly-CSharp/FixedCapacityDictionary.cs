using System;
using System.Collections.Generic;

// Token: 0x02000AA7 RID: 2727
public class FixedCapacityDictionary<TKey, TValue> : ICustomDictionary<TKey, TValue>
{
	// Token: 0x06005010 RID: 20496 RVA: 0x0014E48D File Offset: 0x0014C88D
	public FixedCapacityDictionary(int capacity)
	{
		this.dict = new Dictionary<TKey, TValue>(capacity);
	}

	// Token: 0x06005011 RID: 20497 RVA: 0x0014E4A1 File Offset: 0x0014C8A1
	public FixedCapacityDictionary(int capacity, IEqualityComparer<TKey> comparer)
	{
		this.dict = new Dictionary<TKey, TValue>(capacity, comparer);
	}

	// Token: 0x06005012 RID: 20498 RVA: 0x0014E4B6 File Offset: 0x0014C8B6
	public Dictionary<TKey, TValue> GetEnumerableList()
	{
		return this.dict;
	}

	// Token: 0x170012E2 RID: 4834
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

	// Token: 0x170012E3 RID: 4835
	// (get) Token: 0x06005015 RID: 20501 RVA: 0x0014E4DB File Offset: 0x0014C8DB
	public int Count
	{
		get
		{
			return this.dict.Count;
		}
	}

	// Token: 0x170012E4 RID: 4836
	// (get) Token: 0x06005016 RID: 20502 RVA: 0x0014E4E8 File Offset: 0x0014C8E8
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06005017 RID: 20503 RVA: 0x0014E4EB File Offset: 0x0014C8EB
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		this.dict.Add(item.Key, item.Value);
	}

	// Token: 0x06005018 RID: 20504 RVA: 0x0014E506 File Offset: 0x0014C906
	public void Add(TKey key, TValue value)
	{
		this.Add(new KeyValuePair<TKey, TValue>(key, value));
	}

	// Token: 0x06005019 RID: 20505 RVA: 0x0014E515 File Offset: 0x0014C915
	public void Clear()
	{
		this.dict.Clear();
	}

	// Token: 0x0600501A RID: 20506 RVA: 0x0014E522 File Offset: 0x0014C922
	public bool ContainsKey(TKey key)
	{
		return this.dict.ContainsKey(key);
	}

	// Token: 0x0600501B RID: 20507 RVA: 0x0014E530 File Offset: 0x0014C930
	public void CopyTo(FixedCapacityDictionary<TKey, TValue> targetDict)
	{
		targetDict.Clear();
		foreach (TKey key in this.dict.Keys)
		{
			targetDict[key] = this.dict[key];
		}
	}

	// Token: 0x0600501C RID: 20508 RVA: 0x0014E5A4 File Offset: 0x0014C9A4
	public bool Remove(TKey key)
	{
		return this.dict.Remove(key);
	}

	// Token: 0x0600501D RID: 20509 RVA: 0x0014E5B2 File Offset: 0x0014C9B2
	public bool TryGetValue(TKey key, out TValue value)
	{
		return this.dict.TryGetValue(key, out value);
	}

	// Token: 0x040033AF RID: 13231
	private Dictionary<TKey, TValue> dict;
}
