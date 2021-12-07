using System;

// Token: 0x02000A57 RID: 2647
public class LoadInputSettingEvent : GameEvent
{
	// Token: 0x06004D01 RID: 19713 RVA: 0x0014588B File Offset: 0x00143C8B
	public LoadInputSettingEvent(InputSettingsData data)
	{
		this.data = data;
	}

	// Token: 0x04003290 RID: 12944
	public InputSettingsData data;
}
