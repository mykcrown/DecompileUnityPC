using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C5 RID: 1733
public class LoadSequenceResults
{
	// Token: 0x06002B7F RID: 11135 RVA: 0x000E3086 File Offset: 0x000E1486
	public void AddData(Type theType, object data)
	{
		if (this.dict.ContainsKey(theType))
		{
			throw new UnityException("Duplicate type key " + theType + ", talk to msiegel");
		}
		this.dict[theType] = data;
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x000E30BC File Offset: 0x000E14BC
	public object FindByType<T>()
	{
		Type typeFromHandle = typeof(T);
		return this.dict[typeFromHandle];
	}

	// Token: 0x04001F10 RID: 7952
	public DataLoadStatus status;

	// Token: 0x04001F11 RID: 7953
	public Dictionary<Type, object> dict = new Dictionary<Type, object>();
}
