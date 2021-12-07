using System;

// Token: 0x02000AD6 RID: 2774
public class PlayAnnouncementCommand : GameEvent
{
	// Token: 0x060050B7 RID: 20663 RVA: 0x0015067D File Offset: 0x0014EA7D
	public PlayAnnouncementCommand(string type)
	{
		this.AnnouncementType = type;
	}

	// Token: 0x170012F6 RID: 4854
	// (get) Token: 0x060050B8 RID: 20664 RVA: 0x0015068C File Offset: 0x0014EA8C
	// (set) Token: 0x060050B9 RID: 20665 RVA: 0x00150694 File Offset: 0x0014EA94
	public string AnnouncementType { get; private set; }
}
