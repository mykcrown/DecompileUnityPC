using System;
using UnityEngine;

// Token: 0x02000B61 RID: 2913
public class StatTracker
{
	// Token: 0x0600544C RID: 21580 RVA: 0x001B1B4C File Offset: 0x001AFF4C
	public StatTracker()
	{
		this.Sum = 0f;
		this.TotalValues = 0;
	}

	// Token: 0x17001377 RID: 4983
	// (get) Token: 0x0600544D RID: 21581 RVA: 0x001B1B66 File Offset: 0x001AFF66
	// (set) Token: 0x0600544E RID: 21582 RVA: 0x001B1B6E File Offset: 0x001AFF6E
	public float MaximumValue { get; private set; }

	// Token: 0x17001378 RID: 4984
	// (get) Token: 0x0600544F RID: 21583 RVA: 0x001B1B77 File Offset: 0x001AFF77
	// (set) Token: 0x06005450 RID: 21584 RVA: 0x001B1B7F File Offset: 0x001AFF7F
	public float MinimumValue { get; private set; }

	// Token: 0x17001379 RID: 4985
	// (get) Token: 0x06005451 RID: 21585 RVA: 0x001B1B88 File Offset: 0x001AFF88
	public float AverageValue
	{
		get
		{
			return (this.TotalValues <= 0) ? 0f : (this.Sum / (float)this.TotalValues);
		}
	}

	// Token: 0x1700137A RID: 4986
	// (get) Token: 0x06005452 RID: 21586 RVA: 0x001B1BAE File Offset: 0x001AFFAE
	// (set) Token: 0x06005453 RID: 21587 RVA: 0x001B1BB6 File Offset: 0x001AFFB6
	public float Sum { get; private set; }

	// Token: 0x1700137B RID: 4987
	// (get) Token: 0x06005454 RID: 21588 RVA: 0x001B1BBF File Offset: 0x001AFFBF
	// (set) Token: 0x06005455 RID: 21589 RVA: 0x001B1BC7 File Offset: 0x001AFFC7
	public int TotalValues { get; private set; }

	// Token: 0x06005456 RID: 21590 RVA: 0x001B1BD0 File Offset: 0x001AFFD0
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

	// Token: 0x06005457 RID: 21591 RVA: 0x001B1C3B File Offset: 0x001B003B
	public void Reset()
	{
		this.Sum = 0f;
		this.TotalValues = 0;
	}
}
