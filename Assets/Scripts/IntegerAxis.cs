// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class IntegerAxis
{
	public static readonly int PRECISION_BITS = 7;

	public static readonly int AXIS_RESOLUTION = 1 << IntegerAxis.PRECISION_BITS;

	public static readonly int NEGATIVE_FLAG = MathUtil.IntLog2(IntegerAxis.AXIS_RESOLUTION);

	public static readonly int BITS_PER_AXIS = IntegerAxis.NEGATIVE_FLAG + 1;

	public static readonly int VALUE_MASK = IntegerAxis.AXIS_RESOLUTION - 1;

	public static readonly int MAX_VALUE = IntegerAxis.AXIS_RESOLUTION - 1;

	public static readonly int MIN_VALUE = -(IntegerAxis.AXIS_RESOLUTION - 1);

	public static readonly int TOTAL_ZONES = IntegerAxis.MAX_VALUE - IntegerAxis.MIN_VALUE + 1;

	private int value;

	public Fixed Value
	{
		get
		{
			return this.value / IntegerAxis.MAX_VALUE;
		}
	}

	public int RawIntegerValue
	{
		get
		{
			return this.value;
		}
	}

	public void Set(int value)
	{
		this.value = Mathf.Clamp(value, IntegerAxis.MIN_VALUE, IntegerAxis.MAX_VALUE);
	}

	public void Set(float value)
	{
		this.Set((Fixed)((double)value));
	}

	public void Set(Fixed fixedValue)
	{
		fixedValue = FixedMath.Clamp(fixedValue, -1, 1) * IntegerAxis.MAX_VALUE;
		int num = (this.value <= 0) ? FixedMath.Ceil(fixedValue) : ((int)fixedValue);
		this.Set(num);
	}

	public void Set(IntegerAxis value)
	{
		this.value = value.value;
	}
}
