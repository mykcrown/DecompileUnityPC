// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class StatTracker
{
	public float MaximumValue
	{
		get;
		private set;
	}

	public float MinimumValue
	{
		get;
		private set;
	}

	public float AverageValue
	{
		get
		{
			return (this.TotalValues <= 0) ? 0f : (this.Sum / (float)this.TotalValues);
		}
	}

	public float Sum
	{
		get;
		private set;
	}

	public int TotalValues
	{
		get;
		private set;
	}

	public StatTracker()
	{
		this.Sum = 0f;
		this.TotalValues = 0;
	}

	public void RecordValue(float value)
	{
		if (this.TotalValues == 0)
		{
			this.MaximumValue = value;
			this.MinimumValue = value;
		}
		else
		{
			this.MinimumValue = Mathf.Min(value, this.MinimumValue);
			this.MaximumValue = Mathf.Max(value, this.MaximumValue);
		}
		this.TotalValues++;
		this.Sum += value;
	}

	public void Reset()
	{
		this.Sum = 0f;
		this.TotalValues = 0;
	}
}
