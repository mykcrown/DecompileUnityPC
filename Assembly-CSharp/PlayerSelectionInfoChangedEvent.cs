using System;

// Token: 0x02000A4B RID: 2635
public class PlayerSelectionInfoChangedEvent : UIEvent
{
	// Token: 0x06004CF6 RID: 19702 RVA: 0x001457A6 File Offset: 0x00143BA6
	public PlayerSelectionInfoChangedEvent(PlayerSelectionInfo info)
	{
		this.info = info;
	}

	// Token: 0x04003277 RID: 12919
	public PlayerSelectionInfo info;
}
