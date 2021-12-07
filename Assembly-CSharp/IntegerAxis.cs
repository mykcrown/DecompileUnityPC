using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000B08 RID: 2824
public class IntegerAxis
{
	// Token: 0x17001304 RID: 4868
	// (get) Token: 0x0600511F RID: 20767 RVA: 0x00151703 File Offset: 0x0014FB03
	public Fixed Value
	{
		get
		{
			return this.value / IntegerAxis.MAX_VALUE;
		}
	}

	// Token: 0x17001305 RID: 4869
	// (get) Token: 0x06005120 RID: 20768 RVA: 0x0015171A File Offset: 0x0014FB1A
	public int RawIntegerValue
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x06005121 RID: 20769 RVA: 0x00151722 File Offset: 0x0014FB22
	public void Set(int value)
	{
		this.value = Mathf.Clamp(value, IntegerAxis.MIN_VALUE, IntegerAxis.MAX_VALUE);
	}

	// Token: 0x06005122 RID: 20770 RVA: 0x0015173A File Offset: 0x0014FB3A
	public void Set(float value)
	{
		this.Set((Fixed)((double)value));
	}

	// Token: 0x06005123 RID: 20771 RVA: 0x0015174C File Offset: 0x0014FB4C
	public void Set(Fixed fixedValue)
	{
		fixedValue = FixedMath.Clamp(fixedValue, -1, 1) * IntegerAxis.MAX_VALUE;
		int num = (this.value <= 0) ? FixedMath.Ceil(fixedValue) : ((int)fixedValue);
		this.Set(num);
	}

	// Token: 0x06005124 RID: 20772 RVA: 0x0015179C File Offset: 0x0014FB9C
	public void Set(IntegerAxis value)
	{
		this.value = value.value;
	}

	// Token: 0x04003450 RID: 13392
	public static readonly int PRECISION_BITS = 7;

	// Token: 0x04003451 RID: 13393
	public static readonly int AXIS_RESOLUTION = 1 << IntegerAxis.PRECISION_BITS;

	// Token: 0x04003452 RID: 13394
	public static readonly int NEGATIVE_FLAG = MathUtil.IntLog2(IntegerAxis.AXIS_RESOLUTION);

	// Token: 0x04003453 RID: 13395
	public static readonly int BITS_PER_AXIS = IntegerAxis.NEGATIVE_FLAG + 1;

	// Token: 0x04003454 RID: 13396
	public static readonly int VALUE_MASK = IntegerAxis.AXIS_RESOLUTION - 1;

	// Token: 0x04003455 RID: 13397
	public static readonly int MAX_VALUE = IntegerAxis.AXIS_RESOLUTION - 1;

	// Token: 0x04003456 RID: 13398
	public static readonly int MIN_VALUE = -(IntegerAxis.AXIS_RESOLUTION - 1);

	// Token: 0x04003457 RID: 13399
	public static readonly int TOTAL_ZONES = IntegerAxis.MAX_VALUE - IntegerAxis.MIN_VALUE + 1;

	// Token: 0x04003458 RID: 13400
	private int value;
}
