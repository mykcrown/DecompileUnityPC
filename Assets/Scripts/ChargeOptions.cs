// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class ChargeOptions : ICloneable
{
	public bool canCharge;

	public int chargeBeginFrame = 1;

	public int chargeReleaseFrame = 2;

	public bool hasOverrideChargeConfig;

	public ChargeConfig overrideChargeConfig = new ChargeConfig();

	public object Clone()
	{
		return CloneUtil.SlowDeepClone<ChargeOptions>(this);
	}
}
