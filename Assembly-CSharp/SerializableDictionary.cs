using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02000B5C RID: 2908
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	// Token: 0x06005434 RID: 21556 RVA: 0x000651F1 File Offset: 0x000635F1
	public SerializableDictionary()
	{
	}

	// Token: 0x06005435 RID: 21557 RVA: 0x0006520F File Offset: 0x0006360F
	public SerializableDictionary(int capacity) : base(capacity)
	{
	}

	// Token: 0x06005436 RID: 21558 RVA: 0x0006522E File Offset: 0x0006362E
	public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
	{
	}

	// Token: 0x06005437 RID: 21559 RVA: 0x0006524E File Offset: 0x0006364E
	public SerializableDictionary(SerializableDictionary<TKey, TValue> other) : base(other)
	{
	}

	// Token: 0x06005438 RID: 21560 RVA: 0x0006526D File Offset: 0x0006366D
	public SerializableDictionary(Dictionary<TKey, TValue> other) : base(other)
	{
	}

	// Token: 0x06005439 RID: 21561 RVA: 0x0006528C File Offset: 0x0006368C
	public SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x0600543A RID: 21562 RVA: 0x000652AC File Offset: 0x000636AC
	public object ShallowClone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x0600543B RID: 21563 RVA: 0x000652B4 File Offset: 0x000636B4
	public void OnBeforeSerialize()
	{
		this.keys.Clear();
		this.values.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
		{
			this.keys.Add(keyValuePair.Key);
			this.values.Add(keyValuePair.Value);
		}
	}

	// Token: 0x0600543C RID: 21564 RVA: 0x00065340 File Offset: 0x00063740
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

	// Token: 0x0400355B RID: 13659
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	// Token: 0x0400355C RID: 13660
	[SerializeField]
	private List<TValue> values = new List<TValue>();
}
