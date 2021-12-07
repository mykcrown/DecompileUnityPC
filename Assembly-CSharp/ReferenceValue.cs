using System;

// Token: 0x02000B58 RID: 2904
public class ReferenceValue<T>
{
	// Token: 0x0600541F RID: 21535 RVA: 0x001B1024 File Offset: 0x001AF424
	public ReferenceValue()
	{
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x001B102C File Offset: 0x001AF42C
	public ReferenceValue(T value)
	{
		this.Value = value;
	}

	// Token: 0x06005421 RID: 21537 RVA: 0x001B103B File Offset: 0x001AF43B
	public ReferenceValue(ReferenceValue<T> value)
	{
		this.Value = value.Value;
	}

	// Token: 0x17001375 RID: 4981
	// (get) Token: 0x06005422 RID: 21538 RVA: 0x001B104F File Offset: 0x001AF44F
	// (set) Token: 0x06005423 RID: 21539 RVA: 0x001B1057 File Offset: 0x001AF457
	public T Value { get; set; }
}
