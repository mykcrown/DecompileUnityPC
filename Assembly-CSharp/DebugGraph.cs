using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B7A RID: 2938
public class DebugGraph : ITickable
{
	// Token: 0x060054C1 RID: 21697 RVA: 0x001B2F78 File Offset: 0x001B1378
	public DebugGraph(string name, int frameCount, DebugGraphConfig config)
	{
		if (frameCount < 2)
		{
			throw new ArgumentException();
		}
		if (config.width <= 0f)
		{
			config.width = NetGraphVisualizer.MasterGraphWidth;
		}
		if (config.height <= 0f)
		{
			config.height = NetGraphVisualizer.MasterGraphHeight;
		}
		this.Name = name;
		this.frameCount = frameCount;
		this.config = config;
		this.values = new List<float>(this.frameCount);
		this.averages = new List<float>(this.frameCount);
		this.valuePoints = new List<Vector2>(this.frameCount);
		this.averagePoints = new List<Vector2>(this.frameCount);
		for (int i = 0; i < frameCount; i++)
		{
			this.values.Add(0f);
			this.averages.Add(0f);
			this.valuePoints.Add(Vector2.zero);
			this.averagePoints.Add(Vector2.zero);
		}
		this.resetLimits();
	}

	// Token: 0x17001387 RID: 4999
	// (get) Token: 0x060054C2 RID: 21698 RVA: 0x001B3083 File Offset: 0x001B1483
	// (set) Token: 0x060054C3 RID: 21699 RVA: 0x001B308B File Offset: 0x001B148B
	public bool IsShown { get; private set; }

	// Token: 0x17001388 RID: 5000
	// (get) Token: 0x060054C4 RID: 21700 RVA: 0x001B3094 File Offset: 0x001B1494
	// (set) Token: 0x060054C5 RID: 21701 RVA: 0x001B309C File Offset: 0x001B149C
	public string Name { get; private set; }

	// Token: 0x17001389 RID: 5001
	// (get) Token: 0x060054C6 RID: 21702 RVA: 0x001B30A5 File Offset: 0x001B14A5
	private bool isDynamic
	{
		get
		{
			return this.config.viewStyle != DebugGraphViewStyle.Fixed;
		}
	}

	// Token: 0x1700138A RID: 5002
	// (get) Token: 0x060054C7 RID: 21703 RVA: 0x001B30B8 File Offset: 0x001B14B8
	private float minViewValue
	{
		get
		{
			return (!this.isDynamic) ? this.config.minValue : this.viewMin;
		}
	}

	// Token: 0x1700138B RID: 5003
	// (get) Token: 0x060054C8 RID: 21704 RVA: 0x001B30DB File Offset: 0x001B14DB
	private float maxViewValue
	{
		get
		{
			return (!this.isDynamic) ? this.config.maxValue : this.viewMax;
		}
	}

	// Token: 0x060054C9 RID: 21705 RVA: 0x001B3100 File Offset: 0x001B1500
	public void SetFrameCount(int frameCount)
	{
		int num = this.frameCount;
		this.frameCount = frameCount;
		List<float> list = new List<float>(this.frameCount);
		List<float> list2 = new List<float>(this.frameCount);
		List<Vector2> list3 = new List<Vector2>(this.frameCount);
		List<Vector2> list4 = new List<Vector2>(this.frameCount);
		for (int i = 0; i < this.frameCount - num; i++)
		{
			list.Add(0f);
			list2.Add(0f);
			list3.Add(Vector2.zero);
			list4.Add(Vector2.zero);
		}
		for (int j = 0; j < Math.Min(this.frameCount, num); j++)
		{
			list.Add(this.values[j]);
			list2.Add(this.averages[j]);
			list3.Add(this.valuePoints[j]);
			list4.Add(this.averagePoints[j]);
		}
		this.values = list;
		this.averages = list2;
		this.valuePoints = list3;
		this.averagePoints = list4;
	}

	// Token: 0x060054CA RID: 21706 RVA: 0x001B3221 File Offset: 0x001B1621
	private void resetLimits()
	{
		this.minValue = float.MaxValue;
		this.maxValue = float.MinValue;
	}

	// Token: 0x060054CB RID: 21707 RVA: 0x001B323C File Offset: 0x001B163C
	public Vector2 ToGraphSpace(float value, int frameIndex)
	{
		float x = (float)frameIndex / (float)this.frameCount * this.config.width + this.config.screenOffset.x;
		float num = (value - this.minViewValue) / (this.maxViewValue - this.minViewValue);
		float y = num * this.config.height + this.config.screenOffset.y;
		return new Vector2(x, y);
	}

	// Token: 0x060054CC RID: 21708 RVA: 0x001B32B0 File Offset: 0x001B16B0
	public void ReportValue(float value)
	{
		if (!this.IsShown)
		{
			return;
		}
		for (int i = 0; i < this.values.Count - 1; i++)
		{
			this.values[i] = this.values[i + 1];
			this.averages[i] = this.averages[i + 1];
		}
		this.values[this.values.Count - 1] = value;
		if (this.config.viewStyle == DebugGraphViewStyle.DynamicRecent)
		{
			this.resetLimits();
		}
		float num = 0f;
		foreach (float num2 in this.values)
		{
			float num3 = num2;
			num += num3;
			if (num3 < this.minValue)
			{
				this.minValue = num3;
			}
			if (num3 > this.maxValue)
			{
				this.maxValue = num3;
			}
		}
		float value2 = num / (float)this.values.Count;
		this.averages[this.values.Count - 1] = value2;
		float num4 = this.maxValue - this.minValue;
		this.targetViewMin = this.minValue;
		this.targetViewMax = this.maxValue;
		if (num4 < this.config.requiredViewSize)
		{
			float num5 = (this.config.requiredViewSize - num4) / 2f;
			this.targetViewMin -= num5;
			this.targetViewMax += num5;
		}
		this.RecalculateLines();
	}

	// Token: 0x060054CD RID: 21709 RVA: 0x001B3464 File Offset: 0x001B1864
	public void RecalculateLines()
	{
		for (int i = 0; i < this.values.Count; i++)
		{
			this.valuePoints[i] = this.ToGraphSpace(this.values[i], i);
			this.averagePoints[i] = this.ToGraphSpace(this.averages[i], i);
		}
	}

	// Token: 0x060054CE RID: 21710 RVA: 0x001B34CC File Offset: 0x001B18CC
	public void TickFrame()
	{
		if (!this.IsShown)
		{
			return;
		}
		this.viewMin = Mathf.Lerp(this.viewMin, this.targetViewMin, 0.1f);
		this.viewMax = Mathf.Lerp(this.viewMax, this.targetViewMax, 0.1f);
	}

	// Token: 0x060054CF RID: 21711 RVA: 0x001B351D File Offset: 0x001B191D
	public void Draw()
	{
		if (!this.IsShown)
		{
			return;
		}
		if ((this.config.graphStyle & DebugGraphStyle.Average) > DebugGraphStyle.None)
		{
		}
		if ((this.config.graphStyle & DebugGraphStyle.Value) > DebugGraphStyle.None)
		{
		}
	}

	// Token: 0x060054D0 RID: 21712 RVA: 0x001B3551 File Offset: 0x001B1951
	public void Show()
	{
		this.IsShown = true;
	}

	// Token: 0x060054D1 RID: 21713 RVA: 0x001B355A File Offset: 0x001B195A
	public void Hide()
	{
		this.IsShown = false;
	}

	// Token: 0x040035B5 RID: 13749
	public DebugGraphConfig config;

	// Token: 0x040035B6 RID: 13750
	private int frameCount;

	// Token: 0x040035B7 RID: 13751
	private float minValue;

	// Token: 0x040035B8 RID: 13752
	private float maxValue;

	// Token: 0x040035B9 RID: 13753
	private float targetViewMin;

	// Token: 0x040035BA RID: 13754
	private float targetViewMax;

	// Token: 0x040035BB RID: 13755
	private float viewMin;

	// Token: 0x040035BC RID: 13756
	private float viewMax;

	// Token: 0x040035BD RID: 13757
	private List<float> values;

	// Token: 0x040035BE RID: 13758
	private List<float> averages;

	// Token: 0x040035BF RID: 13759
	private List<Vector2> valuePoints;

	// Token: 0x040035C0 RID: 13760
	private List<Vector2> averagePoints;
}
