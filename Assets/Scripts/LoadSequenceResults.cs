// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class LoadSequenceResults
{
	public DataLoadStatus status;

	public Dictionary<Type, object> dict = new Dictionary<Type, object>();

	public void AddData(Type theType, object data)
	{
		if (this.dict.ContainsKey(theType))
		{
			throw new UnityException("Duplicate type key " + theType + ", talk to msiegel");
		}
		this.dict[theType] = data;
	}

	public object FindByType<T>()
	{
		Type typeFromHandle = typeof(T);
		return this.dict[typeFromHandle];
	}
}
