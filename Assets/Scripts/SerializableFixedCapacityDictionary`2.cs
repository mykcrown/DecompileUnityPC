// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableFixedCapacityDictionary<TKey, TValue> : FixedCapacityDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	[SerializeField]
	private List<TValue> values = new List<TValue>();

	public SerializableFixedCapacityDictionary(int capacity) : base(capacity)
	{
	}

	public object ShallowClone()
	{
		return base.MemberwiseClone();
	}

	public void OnBeforeSerialize()
	{
		this.keys.Clear();
		this.values.Clear();
		foreach (KeyValuePair<TKey, TValue> current in base.GetEnumerableList())
		{
			this.keys.Add(current.Key);
			this.values.Add(current.Value);
		}
	}

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
}
