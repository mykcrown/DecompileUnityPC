using System;

// Token: 0x02000594 RID: 1428
[Serializable]
public struct PhysicsBoolOverride : IPhysicsValueOverride<bool>, IPhysicsValueOverride
{
	// Token: 0x06002038 RID: 8248 RVA: 0x000A2D25 File Offset: 0x000A1125
	public PhysicsBoolOverride(bool isOverriden = false, bool value = false)
	{
		this.isOverriden = isOverriden;
		this.value = value;
	}

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06002039 RID: 8249 RVA: 0x000A2D35 File Offset: 0x000A1135
	// (set) Token: 0x0600203A RID: 8250 RVA: 0x000A2D3D File Offset: 0x000A113D
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

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x0600203B RID: 8251 RVA: 0x000A2D46 File Offset: 0x000A1146
	// (set) Token: 0x0600203C RID: 8252 RVA: 0x000A2D4E File Offset: 0x000A114E
	public bool Value
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

	// Token: 0x0600203D RID: 8253 RVA: 0x000A2D57 File Offset: 0x000A1157
	public bool GetValueOrDefault(bool defaultValue)
	{
		return (!this.isOverriden) ? defaultValue : this.value;
	}

	// Token: 0x040019BB RID: 6587
	public bool isOverriden;

	// Token: 0x040019BC RID: 6588
	public bool value;
}
