using System;

// Token: 0x02000509 RID: 1289
[Serializable]
public class ChargeOptions : ICloneable
{
	// Token: 0x06001BF3 RID: 7155 RVA: 0x0008D530 File Offset: 0x0008B930
	public object Clone()
	{
		return CloneUtil.SlowDeepClone<ChargeOptions>(this);
	}

	// Token: 0x0400166F RID: 5743
	public bool canCharge;

	// Token: 0x04001670 RID: 5744
	public int chargeBeginFrame = 1;

	// Token: 0x04001671 RID: 5745
	public int chargeReleaseFrame = 2;

	// Token: 0x04001672 RID: 5746
	public bool hasOverrideChargeConfig;

	// Token: 0x04001673 RID: 5747
	public ChargeConfig overrideChargeConfig = new ChargeConfig();
}
