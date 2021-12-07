// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class PreloadContext
{
	public Dictionary<PreloadDef, int> list = new Dictionary<PreloadDef, int>();

	public HashSet<object> alreadyChecked = new HashSet<object>();

	public ThreeTierQualityLevel particleQuality;

	public int multiplier = 1;

	private int currentLoadCount;

	private int totalLoadCount;

	private Action callback;

	private bool isCanceled;

	private bool isStarted;

	public bool AlreadyChecked(object obj)
	{
		bool flag = this.alreadyChecked.Contains(obj);
		if (!flag)
		{
			this.alreadyChecked.Add(obj);
		}
		return flag;
	}

	public void StartLoad(Action callback)
	{
		if (this.isCanceled)
		{
			return;
		}
		this.callback = callback;
		this.isStarted = true;
		this.currentLoadCount = this.list.Count;
		this.totalLoadCount = this.currentLoadCount;
		this.checkComplete();
	}

	public void OneLoadComplete()
	{
		if (this.isCanceled)
		{
			return;
		}
		this.currentLoadCount--;
		this.checkComplete();
	}

	public float GetProgress()
	{
		if (this.isCanceled)
		{
			return 0f;
		}
		return (float)((this.totalLoadCount - this.currentLoadCount) / this.totalLoadCount);
	}

	public bool IsComplete()
	{
		return !this.isCanceled && this.currentLoadCount <= 0 && this.isStarted;
	}

	public void Cancel()
	{
		this.isCanceled = true;
		this.isStarted = false;
		this.callback = null;
		this.currentLoadCount = 0;
		this.totalLoadCount = 0;
		this.list.Clear();
		this.alreadyChecked.Clear();
	}

	private void checkComplete()
	{
		if (this.currentLoadCount == 0)
		{
			Action action = this.callback;
			this.callback = null;
			if (action != null)
			{
				action();
			}
		}
	}

	public void Register(PreloadDef newDef, int newPoolSize = 4)
	{
		if (newDef.obj == null)
		{
			return;
		}
		int num;
		if (this.list.TryGetValue(newDef, out num))
		{
			if (num < newPoolSize)
			{
				this.list[newDef] = newPoolSize;
			}
		}
		else
		{
			this.list.Add(newDef, newPoolSize);
		}
	}
}
