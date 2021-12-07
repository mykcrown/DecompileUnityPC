using System;

// Token: 0x02000A4C RID: 2636
[Serializable]
public class SetBattleSettingRequest : UIEvent, IUIRequest
{
	// Token: 0x06004CF7 RID: 19703 RVA: 0x001457B5 File Offset: 0x00143BB5
	public SetBattleSettingRequest(BattleSettingType settingType, int value)
	{
		this.settingType = settingType;
		this.value = value;
	}

	// Token: 0x04003278 RID: 12920
	public int value;

	// Token: 0x04003279 RID: 12921
	public BattleSettingType settingType;
}
