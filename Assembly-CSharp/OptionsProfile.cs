using System;

// Token: 0x020003E0 RID: 992
[Serializable]
public class OptionsProfile
{
	// Token: 0x0600155C RID: 5468 RVA: 0x000760D4 File Offset: 0x000744D4
	public OptionsProfile Clone()
	{
		return new OptionsProfile
		{
			settings = this.settings.Clone(),
			isDefault = this.isDefault,
			id = this.id,
			name = this.name
		};
	}

	// Token: 0x04000F1F RID: 3871
	public BattleSettings settings;

	// Token: 0x04000F20 RID: 3872
	public bool isDefault;

	// Token: 0x04000F21 RID: 3873
	public string id = "__default";

	// Token: 0x04000F22 RID: 3874
	public string name;
}
