using System;

// Token: 0x02000A5B RID: 2651
public class SetUIInputModuleCommand : GameEvent
{
	// Token: 0x06004D05 RID: 19717 RVA: 0x001458B9 File Offset: 0x00143CB9
	public SetUIInputModuleCommand(UIInputModuleType type)
	{
		this.type = type;
	}

	// Token: 0x04003292 RID: 12946
	public UIInputModuleType type;
}
