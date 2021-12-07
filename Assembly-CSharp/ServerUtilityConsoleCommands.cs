using System;
using IconsServer;

// Token: 0x020002F4 RID: 756
public class ServerUtilityConsoleCommands
{
	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x0600106F RID: 4207 RVA: 0x00060979 File Offset: 0x0005ED79
	// (set) Token: 0x06001070 RID: 4208 RVA: 0x00060981 File Offset: 0x0005ED81
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06001071 RID: 4209 RVA: 0x0006098A File Offset: 0x0005ED8A
	// (set) Token: 0x06001072 RID: 4210 RVA: 0x00060992 File Offset: 0x0005ED92
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x06001073 RID: 4211 RVA: 0x0006099B File Offset: 0x0005ED9B
	public void Init()
	{
		this.devConsole.AddCommand(new Action(this.networkTest), "server", "test", "Endpoint for network path testing.");
		this.networkListeners();
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x000609C9 File Offset: 0x0005EDC9
	private void networkListeners()
	{
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x000609CB File Offset: 0x0005EDCB
	private void onOpenLootboxes(ServerEvent evt)
	{
		this.devConsole.PrintLn("Ack event received");
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x000609DD File Offset: 0x0005EDDD
	private void networkTest()
	{
		this.devConsole.PrintLn("EXECUTE NETWORK TEST");
	}
}
