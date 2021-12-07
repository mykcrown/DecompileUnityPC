using System;

// Token: 0x02000A5F RID: 2655
[Serializable]
public class SetStageStateRequest : UIEvent, IUIRequest
{
	// Token: 0x06004D09 RID: 19721 RVA: 0x001458EE File Offset: 0x00143CEE
	public SetStageStateRequest(StageID stageID, StageState state)
	{
		this.stageID = stageID;
		this.state = state;
	}

	// Token: 0x1700123D RID: 4669
	// (get) Token: 0x06004D0A RID: 19722 RVA: 0x00145904 File Offset: 0x00143D04
	// (set) Token: 0x06004D0B RID: 19723 RVA: 0x0014590C File Offset: 0x00143D0C
	public StageID stageID { get; private set; }

	// Token: 0x1700123E RID: 4670
	// (get) Token: 0x06004D0C RID: 19724 RVA: 0x00145915 File Offset: 0x00143D15
	// (set) Token: 0x06004D0D RID: 19725 RVA: 0x0014591D File Offset: 0x00143D1D
	public StageState state { get; private set; }
}
