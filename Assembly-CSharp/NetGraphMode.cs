using System;

// Token: 0x02000B76 RID: 2934
public enum NetGraphMode
{
	// Token: 0x0400359A RID: 13722
	Off,
	// Token: 0x0400359B RID: 13723
	CPU,
	// Token: 0x0400359C RID: 13724
	Memory,
	// Token: 0x0400359D RID: 13725
	Network = 4,
	// Token: 0x0400359E RID: 13726
	Physics = 8,
	// Token: 0x0400359F RID: 13727
	Default = 7,
	// Token: 0x040035A0 RID: 13728
	Performance = 3,
	// Token: 0x040035A1 RID: 13729
	On = -1
}
