// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class LoadBattleSettings : UIEvent, IUIRequest
{
	public BattleSettings settings;

	public LoadBattleSettings(BattleSettings settings)
	{
		this.settings = settings;
	}
}
