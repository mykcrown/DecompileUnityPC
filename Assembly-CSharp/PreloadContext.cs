using System;
using System.Collections.Generic;

// Token: 0x0200048B RID: 1163
public class PreloadContext
{
	// Token: 0x06001938 RID: 6456 RVA: 0x00084054 File Offset: 0x00082454
	public bool AlreadyChecked(object obj)
	{
		bool flag = this.alreadyChecked.Contains(obj);
		if (!flag)
		{
			this.alreadyChecked.Add(obj);
		}
		return flag;
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x00084082 File Offset: 0x00082482
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

	// Token: 0x0600193A RID: 6458 RVA: 0x000840C1 File Offset: 0x000824C1
	public void OneLoadComplete()
	{
		if (this.isCanceled)
		{
			return;
		}
		this.currentLoadCount--;
		this.checkComplete();
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x000840E3 File Offset: 0x000824E3
	public float GetProgress()
	{
		if (this.isCanceled)
		{
			return 0f;
		}
		return (float)((this.totalLoadCount - this.currentLoadCount) / this.totalLoadCount);
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x0008410B File Offset: 0x0008250B
	public bool IsComplete()
	{
		return !this.isCanceled && this.currentLoadCount <= 0 && this.isStarted;
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x0008412D File Offset: 0x0008252D
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

	// Token: 0x0600193E RID: 6462 RVA: 0x00084168 File Offset: 0x00082568
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

	// Token: 0x0600193F RID: 6463 RVA: 0x0008419C File Offset: 0x0008259C
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

	// Token: 0x04001302 RID: 4866
	public Dictionary<PreloadDef, int> list = new Dictionary<PreloadDef, int>();

	// Token: 0x04001303 RID: 4867
	public HashSet<object> alreadyChecked = new HashSet<object>();

	// Token: 0x04001304 RID: 4868
	public ThreeTierQualityLevel particleQuality;

	// Token: 0x04001305 RID: 4869
	public int multiplier = 1;

	// Token: 0x04001306 RID: 4870
	private int currentLoadCount;

	// Token: 0x04001307 RID: 4871
	private int totalLoadCount;

	// Token: 0x04001308 RID: 4872
	private Action callback;

	// Token: 0x04001309 RID: 4873
	private bool isCanceled;

	// Token: 0x0400130A RID: 4874
	private bool isStarted;
}
