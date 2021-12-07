using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B67 RID: 2919
internal class CallbackInfoPool
{
	// Token: 0x06005476 RID: 21622 RVA: 0x001B2334 File Offset: 0x001B0734
	public CallbackInfo Get()
	{
		if (this.pool.Count > 0)
		{
			CallbackInfo result = this.pool[0];
			this.pool.RemoveAt(0);
			return result;
		}
		return new CallbackInfo();
	}

	// Token: 0x06005477 RID: 21623 RVA: 0x001B2372 File Offset: 0x001B0772
	public void Release(CallbackInfo value)
	{
		this.pool.Add(value);
		if (this.pool.Count > 1000)
		{
			Debug.LogWarning("!!!ALERT!!!, callback pool should not be this large");
		}
	}

	// Token: 0x0400357D RID: 13693
	private List<CallbackInfo> pool = new List<CallbackInfo>();
}
