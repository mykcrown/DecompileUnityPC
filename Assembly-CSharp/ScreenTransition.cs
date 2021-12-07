using System;

// Token: 0x020009CD RID: 2509
public class ScreenTransition
{
	// Token: 0x06004641 RID: 17985 RVA: 0x00132AA7 File Offset: 0x00130EA7
	public ScreenTransition(ScreenTransitionType type)
	{
		this.type = type;
	}

	// Token: 0x06004642 RID: 17986 RVA: 0x00132AC1 File Offset: 0x00130EC1
	public ScreenTransition SetDelay(float delay)
	{
		this.delay = delay;
		return this;
	}

	// Token: 0x06004643 RID: 17987 RVA: 0x00132ACB File Offset: 0x00130ECB
	public ScreenTransition SetTime(float time)
	{
		this.time = time;
		return this;
	}

	// Token: 0x04002E6F RID: 11887
	public ScreenTransitionType type;

	// Token: 0x04002E70 RID: 11888
	public float time = 0.15f;

	// Token: 0x04002E71 RID: 11889
	public float delay;
}
