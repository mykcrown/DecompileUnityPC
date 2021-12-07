using System;

// Token: 0x02000A4D RID: 2637
[Serializable]
public class LoadBattleSettings : UIEvent, IUIRequest
{
	// Token: 0x06004CF8 RID: 19704 RVA: 0x001457CB File Offset: 0x00143BCB
	public LoadBattleSettings(BattleSettings settings)
	{
		this.settings = settings;
	}

	// Token: 0x0400327A RID: 12922
	public BattleSettings settings;
}
