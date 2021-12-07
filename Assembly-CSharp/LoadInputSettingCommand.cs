using System;

// Token: 0x02000A56 RID: 2646
public class LoadInputSettingCommand : GameEvent
{
	// Token: 0x06004D00 RID: 19712 RVA: 0x0014587C File Offset: 0x00143C7C
	public LoadInputSettingCommand(InputSettingsData data)
	{
		this.data = data;
	}

	// Token: 0x0400328F RID: 12943
	public InputSettingsData data;
}
