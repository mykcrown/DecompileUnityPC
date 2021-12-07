using System;

// Token: 0x02000593 RID: 1427
[Serializable]
public struct PhysicsIntOverride : IPhysicsValueOverride<int>, IPhysicsValueOverride
{
	// Token: 0x06002032 RID: 8242 RVA: 0x000A2CDA File Offset: 0x000A10DA
	public PhysicsIntOverride(bool isOverriden = false, int value = 0)
	{
		this.isOverriden = isOverriden;
		this.value = value;
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06002033 RID: 8243 RVA: 0x000A2CEA File Offset: 0x000A10EA
	// (set) Token: 0x06002034 RID: 8244 RVA: 0x000A2CF2 File Offset: 0x000A10F2
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

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06002035 RID: 8245 RVA: 0x000A2CFB File Offset: 0x000A10FB
	// (set) Token: 0x06002036 RID: 8246 RVA: 0x000A2D03 File Offset: 0x000A1103
	public int Value
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

	// Token: 0x06002037 RID: 8247 RVA: 0x000A2D0C File Offset: 0x000A110C
	public int GetValueOrDefault(int defaultValue)
	{
		return (!this.isOverriden) ? defaultValue : this.value;
	}

	// Token: 0x040019B9 RID: 6585
	public bool isOverriden;

	// Token: 0x040019BA RID: 6586
	public int value;
}
