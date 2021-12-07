using System;
using FixedPoint;

// Token: 0x020003A9 RID: 937
[Serializable]
public class LedgeGrabBox
{
	// Token: 0x06001421 RID: 5153 RVA: 0x00071A1B File Offset: 0x0006FE1B
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x04000D67 RID: 3431
	public FixedRect rect = new FixedRect(-1, 3, 1, (Fixed)1.5);
}
