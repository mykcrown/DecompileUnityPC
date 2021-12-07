using System;

// Token: 0x02000662 RID: 1634
[Serializable]
public class StaleEntry : CloneableObject
{
	// Token: 0x060027F7 RID: 10231 RVA: 0x000C287F File Offset: 0x000C0C7F
	public StaleEntry()
	{
	}

	// Token: 0x060027F8 RID: 10232 RVA: 0x000C2887 File Offset: 0x000C0C87
	public StaleEntry(MoveLabel label, string name, int uid)
	{
		this.label = label;
		this.name = name;
		this.uid = uid;
	}

	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x060027F9 RID: 10233 RVA: 0x000C28A4 File Offset: 0x000C0CA4
	// (set) Token: 0x060027FA RID: 10234 RVA: 0x000C28AC File Offset: 0x000C0CAC
	public MoveLabel label { get; private set; }

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x060027FB RID: 10235 RVA: 0x000C28B5 File Offset: 0x000C0CB5
	// (set) Token: 0x060027FC RID: 10236 RVA: 0x000C28BD File Offset: 0x000C0CBD
	public string name { get; private set; }

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x060027FD RID: 10237 RVA: 0x000C28C6 File Offset: 0x000C0CC6
	// (set) Token: 0x060027FE RID: 10238 RVA: 0x000C28CE File Offset: 0x000C0CCE
	public int uid { get; private set; }
}
