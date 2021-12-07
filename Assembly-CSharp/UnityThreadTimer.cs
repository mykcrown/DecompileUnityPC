using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B65 RID: 2917
public class UnityThreadTimer : IMainThreadTimer
{
	// Token: 0x06005465 RID: 21605 RVA: 0x001B1F14 File Offset: 0x001B0314
	[PostConstruct]
	public void Init()
	{
		ProxyMono proxyMono = ProxyMono.CreateNew("UnityThreadTimer");
		ProxyMono proxyMono2 = proxyMono;
		proxyMono2.OnUpdate = (Action)Delegate.Combine(proxyMono2.OnUpdate, new Action(this.Update));
		ProxyMono proxyMono3 = proxyMono;
		proxyMono3.OnLateUpdate = (Action)Delegate.Combine(proxyMono3.OnLateUpdate, new Action(this.LateUpdate));
	}

	// Token: 0x06005466 RID: 21606 RVA: 0x001B1F70 File Offset: 0x001B0370
	private void Update()
	{
		if (this.listLen != 0)
		{
			this.currentTime = (int)(Time.realtimeSinceStartup * 1000f);
			this.willExecute.Clear();
			this.processWillExecute = false;
			this.x = 0;
			while (this.x < this.listLen)
			{
				if (this.callbackArray[this.x].Time <= this.currentTime)
				{
					this.willExecute.Add(this.callbackArray[this.x].Callback);
					this.list.Remove(this.callbackArray[this.x]);
					this.pool.Release(this.callbackArray[this.x]);
					this.processWillExecute = true;
				}
				this.x++;
			}
			if (this.processWillExecute)
			{
				this.syncOptimizedArray();
				this.willExecuteLen = this.willExecute.Count;
				this.x = 0;
				while (this.x < this.willExecuteLen)
				{
					this.willExecute[this.x]();
					this.x++;
				}
			}
		}
	}

	// Token: 0x06005467 RID: 21607 RVA: 0x001B20AC File Offset: 0x001B04AC
	private void LateUpdate()
	{
		if (this.callEndOfFrame)
		{
			this.willExecute.Clear();
			foreach (Action item in this.endOfFrame)
			{
				this.willExecute.Add(item);
			}
			this.endOfFrame.Clear();
			this.callEndOfFrame = false;
			this.willExecuteLen = this.willExecute.Count;
			this.x = 0;
			while (this.x < this.willExecuteLen)
			{
				this.willExecute[this.x]();
				this.x++;
			}
		}
	}

	// Token: 0x06005468 RID: 21608 RVA: 0x001B2188 File Offset: 0x001B0588
	public void SetOrReplaceTimeout(int time, Action callback)
	{
		foreach (CallbackInfo callbackInfo in this.list)
		{
			if (callbackInfo.Callback == callback)
			{
				callbackInfo.Time = (int)(Time.realtimeSinceStartup * 1000f) + time;
				return;
			}
		}
		this.SetTimeout(time, callback);
	}

	// Token: 0x06005469 RID: 21609 RVA: 0x001B2210 File Offset: 0x001B0610
	public void UnblockThread(Action callback)
	{
		this.SetTimeout(1, callback);
	}

	// Token: 0x0600546A RID: 21610 RVA: 0x001B221A File Offset: 0x001B061A
	public void NextFrame(Action callback)
	{
		this.SetTimeout(1, callback);
	}

	// Token: 0x0600546B RID: 21611 RVA: 0x001B2224 File Offset: 0x001B0624
	public void EndOfFrame(Action callback)
	{
		this.endOfFrame.Add(callback);
		this.callEndOfFrame = true;
	}

	// Token: 0x0600546C RID: 21612 RVA: 0x001B223C File Offset: 0x001B063C
	public void SetTimeout(int time, Action callback)
	{
		CallbackInfo callbackInfo = this.pool.Get();
		callbackInfo.Time = (int)(Time.realtimeSinceStartup * 1000f) + time;
		callbackInfo.Callback = callback;
		this.list.Add(callbackInfo);
		this.syncOptimizedArray();
	}

	// Token: 0x0600546D RID: 21613 RVA: 0x001B2284 File Offset: 0x001B0684
	public void CancelTimeout(Action callback)
	{
		bool flag = false;
		for (int i = this.list.Count - 1; i >= 0; i--)
		{
			if (this.list[i].Callback == callback)
			{
				CallbackInfo value = this.list[i];
				this.list.RemoveAt(i);
				this.pool.Release(value);
				flag = true;
			}
		}
		if (flag)
		{
			this.syncOptimizedArray();
		}
	}

	// Token: 0x0600546E RID: 21614 RVA: 0x001B2300 File Offset: 0x001B0700
	private void syncOptimizedArray()
	{
		this.callbackArray = this.list.ToArray();
		this.listLen = this.callbackArray.Length;
	}

	// Token: 0x04003572 RID: 13682
	private List<CallbackInfo> list = new List<CallbackInfo>(10);

	// Token: 0x04003573 RID: 13683
	private CallbackInfoPool pool = new CallbackInfoPool();

	// Token: 0x04003574 RID: 13684
	private List<Action> endOfFrame = new List<Action>(10);

	// Token: 0x04003575 RID: 13685
	private List<Action> willExecute = new List<Action>(10);

	// Token: 0x04003576 RID: 13686
	private CallbackInfo[] callbackArray;

	// Token: 0x04003577 RID: 13687
	private int listLen;

	// Token: 0x04003578 RID: 13688
	private int x;

	// Token: 0x04003579 RID: 13689
	private int willExecuteLen;

	// Token: 0x0400357A RID: 13690
	private int currentTime;

	// Token: 0x0400357B RID: 13691
	private bool processWillExecute;

	// Token: 0x0400357C RID: 13692
	private bool callEndOfFrame;
}
