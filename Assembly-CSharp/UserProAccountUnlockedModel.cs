using System;

// Token: 0x0200076D RID: 1901
public class UserProAccountUnlockedModel : IUserProAccountUnlockedModel
{
	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x06002F1E RID: 12062 RVA: 0x000ECCFA File Offset: 0x000EB0FA
	// (set) Token: 0x06002F1F RID: 12063 RVA: 0x000ECD02 File Offset: 0x000EB102
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x06002F20 RID: 12064 RVA: 0x000ECD0B File Offset: 0x000EB10B
	// (set) Token: 0x06002F21 RID: 12065 RVA: 0x000ECD13 File Offset: 0x000EB113
	[Inject]
	public IUserProAccountSource source { get; set; }

	// Token: 0x06002F22 RID: 12066 RVA: 0x000ECD1C File Offset: 0x000EB11C
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserProAccountUnlockedModel.SOURCE_UPDATED, new Action(this.update));
		this.update();
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x000ECD40 File Offset: 0x000EB140
	private void update()
	{
		this.isUnlocked = this.source.IsUnlocked();
		this.signalBus.Dispatch(UserProAccountUnlockedModel.UPDATED);
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x000ECD63 File Offset: 0x000EB163
	public bool IsUnlocked()
	{
		return this.isUnlocked;
	}

	// Token: 0x06002F25 RID: 12069 RVA: 0x000ECD6B File Offset: 0x000EB16B
	public void SetUnlocked()
	{
		this.isUnlocked = true;
		this.signalBus.Dispatch(UserProAccountUnlockedModel.UPDATED);
	}

	// Token: 0x040020ED RID: 8429
	public static string UPDATED = "UserProAccountUnlockedModel.UPDATED";

	// Token: 0x040020EE RID: 8430
	public static string SOURCE_UPDATED = "UserProAccountUnlockedModel.SOURCE_UPDATED";

	// Token: 0x040020F1 RID: 8433
	private bool isUnlocked;
}
