using System;

// Token: 0x02000A5C RID: 2652
public class DeactivateUIInputModuleCommand : GameEvent
{
	// Token: 0x06004D06 RID: 19718 RVA: 0x001458C8 File Offset: 0x00143CC8
	public DeactivateUIInputModuleCommand(UIInputModuleType type)
	{
		this.type = type;
	}

	// Token: 0x04003293 RID: 12947
	public UIInputModuleType type;
}
