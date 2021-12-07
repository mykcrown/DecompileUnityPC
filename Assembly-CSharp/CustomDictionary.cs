using System;
using System.Collections.Generic;

// Token: 0x02000AA8 RID: 2728
public class CustomDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ICustomDictionary<TKey, TValue>
{
	// Token: 0x0600501F RID: 20511 RVA: 0x0014E5C9 File Offset: 0x0014C9C9
	TValue ICustomDictionary<!0, !1>.get_Item(TKey key)
	{
		return base[key];
	}

	// Token: 0x06005020 RID: 20512 RVA: 0x0014E5D2 File Offset: 0x0014C9D2
	void ICustomDictionary<!0, !1>.set_Item(TKey key, TValue value)
	{
		base[key] = value;
	}

	// Token: 0x06005021 RID: 20513 RVA: 0x0014E5DC File Offset: 0x0014C9DC
	void ICustomDictionary<!0, !1>.Add(TKey key, TValue value)
	{
		base.Add(key, value);
	}

	// Token: 0x06005022 RID: 20514 RVA: 0x0014E5E6 File Offset: 0x0014C9E6
	bool ICustomDictionary<!0, !1>.ContainsKey(TKey key)
	{
		return base.ContainsKey(key);
	}

	// Token: 0x06005023 RID: 20515 RVA: 0x0014E5EF File Offset: 0x0014C9EF
	bool ICustomDictionary<!0, !1>.Remove(TKey key)
	{
		return base.Remove(key);
	}

	// Token: 0x06005024 RID: 20516 RVA: 0x0014E5F8 File Offset: 0x0014C9F8
	bool ICustomDictionary<!0, !1>.TryGetValue(TKey key, out TValue value)
	{
		return base.TryGetValue(key, out value);
	}
}
