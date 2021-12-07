using System;

// Token: 0x02000A60 RID: 2656
[Serializable]
public class SelectStageRequest : GameEvent, IUIRequest
{
	// Token: 0x06004D0E RID: 19726 RVA: 0x00145926 File Offset: 0x00143D26
	public SelectStageRequest(StageID stageID, bool confirmed)
	{
		this.stageID = stageID;
		this.confirmed = confirmed;
	}

	// Token: 0x1700123F RID: 4671
	// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0014593C File Offset: 0x00143D3C
	// (set) Token: 0x06004D10 RID: 19728 RVA: 0x00145944 File Offset: 0x00143D44
	public StageID stageID { get; private set; }

	// Token: 0x17001240 RID: 4672
	// (get) Token: 0x06004D11 RID: 19729 RVA: 0x0014594D File Offset: 0x00143D4D
	// (set) Token: 0x06004D12 RID: 19730 RVA: 0x00145955 File Offset: 0x00143D55
	public bool confirmed { get; private set; }
}
