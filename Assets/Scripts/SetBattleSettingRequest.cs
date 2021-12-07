// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class SetBattleSettingRequest : UIEvent, IUIRequest
{
	public int value;

	public BattleSettingType settingType;

	public SetBattleSettingRequest(BattleSettingType settingType, int value)
	{
		this.settingType = settingType;
		this.value = value;
	}
}
