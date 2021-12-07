// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugGraph : ITickable
{
	public DebugGraphConfig config;

	private int frameCount;

	private float minValue;

	private float maxValue;

	private float targetViewMin;

	private float targetViewMax;

	private float viewMin;

	private float viewMax;

	private List<float> values;

	private List<float> averages;

	private List<Vector2> valuePoints;

	private List<Vector2> averagePoints;

	public bool IsShown
	{
		get;
		private set;
	}

	public string Name
	{
		get;
		private set;
	}

	private bool isDynamic
	{
		get
		{
			return this.config.viewStyle != DebugGraphViewStyle.Fixed;
		}
	}

	private float minViewValue
	{
		get
		{
			return (!this.isDynamic) ? this.config.minValue : this.viewMin;
		}
	}

	private float maxViewValue
	{
		get
		{
			return (!this.isDynamic) ? this.config.maxValue : this.viewMax;
		}
	}

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

	private void resetLimits()
	{
		this.minValue = 3.40282347E+38f;
		this.maxValue = -3.40282347E+38f;
	}

	public Vector2 ToGraphSpace(float value, int frameIndex)
	{
		float x = (float)frameIndex / (float)this.frameCount * this.config.width + this.config.screenOffset.x;
		float num = (value - this.minViewValue) / (this.maxViewValue - this.minViewValue);
		float y = num * this.config.height + this.config.screenOffset.y;
		return new Vector2(x, y);
	}

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
			num += num2;
			if (num2 < this.minValue)
			{
				this.minValue = num2;
			}
			if (num2 > this.maxValue)
			{
				this.maxValue = num2;
			}
		}
		float value2 = num / (float)this.values.Count;
		this.averages[this.values.Count - 1] = value2;
		float num3 = this.maxValue - this.minValue;
		this.targetViewMin = this.minValue;
		this.targetViewMax = this.maxValue;
		if (num3 < this.config.requiredViewSize)
		{
			float num4 = (this.config.requiredViewSize - num3) / 2f;
			this.targetViewMin -= num4;
			this.targetViewMax += num4;
		}
		this.RecalculateLines();
	}

	public void RecalculateLines()
	{
		for (int i = 0; i < this.values.Count; i++)
		{
			this.valuePoints[i] = this.ToGraphSpace(this.values[i], i);
			this.averagePoints[i] = this.ToGraphSpace(this.averages[i], i);
		}
	}

	public void TickFrame()
	{
		if (!this.IsShown)
		{
			return;
		}
		this.viewMin = Mathf.Lerp(this.viewMin, this.targetViewMin, 0.1f);
		this.viewMax = Mathf.Lerp(this.viewMax, this.targetViewMax, 0.1f);
	}

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

	public void Show()
	{
		this.IsShown = true;
	}

	public void Hide()
	{
		this.IsShown = false;
	}
}
