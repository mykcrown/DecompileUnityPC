using System;

// Token: 0x02000ABF RID: 2751
[Serializable]
public class Payload : ICloneable
{
	// Token: 0x06005093 RID: 20627 RVA: 0x001289A1 File Offset: 0x00126DA1
	public virtual object Clone()
	{
		return Serialization.DeepClone<Payload>(this);
	}
}
