using System;
using IconsServer;

// Token: 0x02000775 RID: 1909
public class AccountAPI : IAccountAPI
{
	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x06002F4B RID: 12107 RVA: 0x000ED317 File Offset: 0x000EB717
	// (set) Token: 0x06002F4C RID: 12108 RVA: 0x000ED31F File Offset: 0x000EB71F
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x06002F4D RID: 12109 RVA: 0x000ED328 File Offset: 0x000EB728
	// (set) Token: 0x06002F4E RID: 12110 RVA: 0x000ED330 File Offset: 0x000EB730
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B6F RID: 2927
	// (get) Token: 0x06002F4F RID: 12111 RVA: 0x000ED339 File Offset: 0x000EB739
	// (set) Token: 0x06002F50 RID: 12112 RVA: 0x000ED341 File Offset: 0x000EB741
	public string UserName { get; private set; }

	// Token: 0x17000B70 RID: 2928
	// (get) Token: 0x06002F51 RID: 12113 RVA: 0x000ED34A File Offset: 0x000EB74A
	// (set) Token: 0x06002F52 RID: 12114 RVA: 0x000ED352 File Offset: 0x000EB752
	public ulong ID { get; private set; }

	// Token: 0x06002F53 RID: 12115 RVA: 0x000ED35B File Offset: 0x000EB75B
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(SteamManager.STEAM_INITIALIZED, new Action(this.onUpdateName));
	}

	// Token: 0x06002F54 RID: 12116 RVA: 0x000ED379 File Offset: 0x000EB779
	public void Initialize()
	{
		this.UserName = this.iconsServerAPI.Username;
	}

	// Token: 0x06002F55 RID: 12117 RVA: 0x000ED38C File Offset: 0x000EB78C
	private void onUpdateName()
	{
		this.UserName = this.iconsServerAPI.Username;
	}

	// Token: 0x04002109 RID: 8457
	public const string UPDATED = "AccountAPI.UPDATE";
}
