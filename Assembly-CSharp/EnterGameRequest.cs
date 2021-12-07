using System;

// Token: 0x02000A61 RID: 2657
[Serializable]
public class EnterGameRequest : GameEvent, IUIRequest
{
	// Token: 0x06004D13 RID: 19731 RVA: 0x0014595E File Offset: 0x00143D5E
	public EnterGameRequest(StageID stageID, bool confirmed)
	{
		this.stageID = stageID;
		this.confirmed = confirmed;
	}

	// Token: 0x17001241 RID: 4673
	// (get) Token: 0x06004D14 RID: 19732 RVA: 0x00145974 File Offset: 0x00143D74
	// (set) Token: 0x06004D15 RID: 19733 RVA: 0x0014597C File Offset: 0x00143D7C
	public StageID stageID { get; private set; }

	// Token: 0x17001242 RID: 4674
	// (get) Token: 0x06004D16 RID: 19734 RVA: 0x00145985 File Offset: 0x00143D85
	// (set) Token: 0x06004D17 RID: 19735 RVA: 0x0014598D File Offset: 0x00143D8D
	public bool confirmed { get; private set; }
}
