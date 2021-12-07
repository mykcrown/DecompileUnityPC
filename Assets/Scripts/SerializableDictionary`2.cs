// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	[SerializeField]
	private List<TValue> values = new List<TValue>();

	public SerializableDictionary()
	{
	}

	public SerializableDictionary(int capacity) : base(capacity)
	{
	}

	public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
	{
	}

	public SerializableDictionary(SerializableDictionary<TKey, TValue> other) : base(other)
	{
	}

	public SerializableDictionary(Dictionary<TKey, TValue> other) : base(other)
	{
	}

	public SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
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
		foreach (KeyValuePair<TKey, TValue> current in this)
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
