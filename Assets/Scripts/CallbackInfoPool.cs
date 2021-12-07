// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

internal class CallbackInfoPool
{
	private List<CallbackInfo> pool = new List<CallbackInfo>();

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

	public void Release(CallbackInfo value)
	{
		this.pool.Add(value);
		if (this.pool.Count > 1000)
		{
			UnityEngine.Debug.LogWarning("!!!ALERT!!!, callback pool should not be this large");
		}
	}
}
