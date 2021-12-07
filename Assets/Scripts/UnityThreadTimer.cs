// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityThreadTimer : IMainThreadTimer
{
	private List<CallbackInfo> list = new List<CallbackInfo>(10);

	private CallbackInfoPool pool = new CallbackInfoPool();

	private List<Action> endOfFrame = new List<Action>(10);

	private List<Action> willExecute = new List<Action>(10);

	private CallbackInfo[] callbackArray;

	private int listLen;

	private int x;

	private int willExecuteLen;

	private int currentTime;

	private bool processWillExecute;

	private bool callEndOfFrame;

	[PostConstruct]
	public void Init()
	{
		ProxyMono proxyMono = ProxyMono.CreateNew("UnityThreadTimer");
		ProxyMono expr_0C = proxyMono;
		expr_0C.OnUpdate = (Action)Delegate.Combine(expr_0C.OnUpdate, new Action(this.Update));
		ProxyMono expr_2E = proxyMono;
		expr_2E.OnLateUpdate = (Action)Delegate.Combine(expr_2E.OnLateUpdate, new Action(this.LateUpdate));
	}

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

	private void LateUpdate()
	{
		if (this.callEndOfFrame)
		{
			this.willExecute.Clear();
			foreach (Action current in this.endOfFrame)
			{
				this.willExecute.Add(current);
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

	public void SetOrReplaceTimeout(int time, Action callback)
	{
		foreach (CallbackInfo current in this.list)
		{
			if (current.Callback == callback)
			{
				current.Time = (int)(Time.realtimeSinceStartup * 1000f) + time;
				return;
			}
		}
		this.SetTimeout(time, callback);
	}

	public void UnblockThread(Action callback)
	{
		this.SetTimeout(1, callback);
	}

	public void NextFrame(Action callback)
	{
		this.SetTimeout(1, callback);
	}

	public void EndOfFrame(Action callback)
	{
		this.endOfFrame.Add(callback);
		this.callEndOfFrame = true;
	}

	public void SetTimeout(int time, Action callback)
	{
		CallbackInfo callbackInfo = this.pool.Get();
		callbackInfo.Time = (int)(Time.realtimeSinceStartup * 1000f) + time;
		callbackInfo.Callback = callback;
		this.list.Add(callbackInfo);
		this.syncOptimizedArray();
	}

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

	private void syncOptimizedArray()
	{
		this.callbackArray = this.list.ToArray();
		this.listLen = this.callbackArray.Length;
	}
}
