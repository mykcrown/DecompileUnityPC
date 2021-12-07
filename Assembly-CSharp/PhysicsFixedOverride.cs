using System;
using FixedPoint;

// Token: 0x02000592 RID: 1426
[Serializable]
public struct PhysicsFixedOverride : IPhysicsValueOverride<Fixed>, IPhysicsValueOverride
{
	// Token: 0x0600202C RID: 8236 RVA: 0x000A2C8F File Offset: 0x000A108F
	public PhysicsFixedOverride(bool isOverriden = false, Fixed value = default(Fixed))
	{
		this.isOverriden = isOverriden;
		this.value = value;
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x0600202D RID: 8237 RVA: 0x000A2C9F File Offset: 0x000A109F
	// (set) Token: 0x0600202E RID: 8238 RVA: 0x000A2CA7 File Offset: 0x000A10A7
	public bool IsOverriden
	{
		get
		{
			return this.isOverriden;
		}
		set
		{
			this.isOverriden = value;
		}
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x0600202F RID: 8239 RVA: 0x000A2CB0 File Offset: 0x000A10B0
	// (set) Token: 0x06002030 RID: 8240 RVA: 0x000A2CB8 File Offset: 0x000A10B8
	public Fixed Value
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000A2CC1 File Offset: 0x000A10C1
	public Fixed GetValueOrDefault(Fixed defaultValue)
	{
		return (!this.isOverriden) ? defaultValue : this.value;
	}

	// Token: 0x040019B7 RID: 6583
	public bool isOverriden;

	// Token: 0x040019B8 RID: 6584
	public Fixed value;
}
