using System;
using InControl;

// Token: 0x0200069C RID: 1692
[Serializable]
public class DefaultInputBinding : ICloneable
{
	// Token: 0x06002A07 RID: 10759 RVA: 0x000DE030 File Offset: 0x000DC430
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x04001E0F RID: 7695
	public ButtonPress button;

	// Token: 0x04001E10 RID: 7696
	public InputControlType controlType;

	// Token: 0x04001E11 RID: 7697
	public Key defaultP1Key;

	// Token: 0x04001E12 RID: 7698
	public Key defaultP2Key;
}
