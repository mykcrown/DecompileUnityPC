// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LoadInputSettingCommand : GameEvent
{
	public InputSettingsData data;

	public LoadInputSettingCommand(InputSettingsData data)
	{
		this.data = data;
	}
}
