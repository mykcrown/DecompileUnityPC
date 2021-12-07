using System;

// Token: 0x02000502 RID: 1282
[Serializable]
public class LedgeGrabEnableData : ICloneable
{
	// Token: 0x06001BDD RID: 7133 RVA: 0x0008D17C File Offset: 0x0008B57C
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x0400163B RID: 5691
	public int startFrame;

	// Token: 0x0400163C RID: 5692
	public int endFrame;
}
