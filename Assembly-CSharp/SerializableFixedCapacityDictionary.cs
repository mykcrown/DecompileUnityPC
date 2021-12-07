using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B5D RID: 2909
[Serializable]
public class SerializableFixedCapacityDictionary<TKey, TValue> : FixedCapacityDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	// Token: 0x0600543D RID: 21565 RVA: 0x001B1722 File Offset: 0x001AFB22
	public SerializableFixedCapacityDictionary(int capacity) : base(capacity)
	{
	}

	// Token: 0x0600543E RID: 21566 RVA: 0x001B1741 File Offset: 0x001AFB41
	public object ShallowClone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x0600543F RID: 21567 RVA: 0x001B174C File Offset: 0x001AFB4C
	public void OnBeforeSerialize()
	{
		this.keys.Clear();
		this.values.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in base.GetEnumerableList())
		{
			this.keys.Add(keyValuePair.Key);
			this.values.Add(keyValuePair.Value);
		}
	}

	// Token: 0x06005440 RID: 21568 RVA: 0x001B17DC File Offset: 0x001AFBDC
	public void OnAfterDeserialize()
	{
		base.Clear();
		if (this.keys.Count != this.values.Count)
		{
			throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.", Array.Empty<object>()));
		}
		for (int i = 0; i < this.keys.Count; i++)
		{
			base.Add(this.keys[i], this.values[i]);
		}
	}

	// Token: 0x0400355D RID: 13661
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	// Token: 0x0400355E RID: 13662
	[SerializeField]
	private List<TValue> values = new List<TValue>();
}
