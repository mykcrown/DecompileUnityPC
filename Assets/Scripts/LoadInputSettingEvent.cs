// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LoadInputSettingEvent : GameEvent
{
	public InputSettingsData data;

	public LoadInputSettingEvent(InputSettingsData data)
	{
		this.data = data;
	}
}
