using System;

// Token: 0x020006D4 RID: 1748
public class AutoJoin : IAutoJoin
{
	// Token: 0x17000AC1 RID: 2753
	// (get) Token: 0x06002BDA RID: 11226 RVA: 0x000E486B File Offset: 0x000E2C6B
	// (set) Token: 0x06002BDB RID: 11227 RVA: 0x000E4873 File Offset: 0x000E2C73
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000AC2 RID: 2754
	// (get) Token: 0x06002BDC RID: 11228 RVA: 0x000E487C File Offset: 0x000E2C7C
	// (set) Token: 0x06002BDD RID: 11229 RVA: 0x000E4884 File Offset: 0x000E2C84
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000AC3 RID: 2755
	// (get) Token: 0x06002BDE RID: 11230 RVA: 0x000E488D File Offset: 0x000E2C8D
	// (set) Token: 0x06002BDF RID: 11231 RVA: 0x000E4895 File Offset: 0x000E2C95
	public string LobbyName { get; private set; }

	// Token: 0x17000AC4 RID: 2756
	// (get) Token: 0x06002BE0 RID: 11232 RVA: 0x000E489E File Offset: 0x000E2C9E
	// (set) Token: 0x06002BE1 RID: 11233 RVA: 0x000E48A6 File Offset: 0x000E2CA6
	public string LobbyPassword { get; private set; }

	// Token: 0x17000AC5 RID: 2757
	// (get) Token: 0x06002BE2 RID: 11234 RVA: 0x000E48AF File Offset: 0x000E2CAF
	// (set) Token: 0x06002BE3 RID: 11235 RVA: 0x000E48B7 File Offset: 0x000E2CB7
	public ulong LobbySteamID { get; private set; }

	// Token: 0x06002BE4 RID: 11236 RVA: 0x000E48C0 File Offset: 0x000E2CC0
	[PostConstruct]
	public void Init()
	{
		this.events.Subscribe(typeof(OnStartJoinLobbyCommand), new Events.EventHandler(this.onStartJoinLobby));
		this.events.Subscribe(typeof(OnStartJoinSteamLobbyCommand), new Events.EventHandler(this.onStartJoinSteamLobby));
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x000E490F File Offset: 0x000E2D0F
	public void Clear()
	{
		this.LobbyName = null;
		this.LobbyPassword = null;
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x000E491F File Offset: 0x000E2D1F
	public bool AutoJoinIfSet()
	{
		if (this.LobbyName != null)
		{
			this.signalBus.Dispatch(AutoJoin.AUTOJOIN);
			return true;
		}
		return false;
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x000E4940 File Offset: 0x000E2D40
	private void onStartJoinLobby(GameEvent message)
	{
		OnStartJoinLobbyCommand onStartJoinLobbyCommand = message as OnStartJoinLobbyCommand;
		this.LobbyName = onStartJoinLobbyCommand.LobbyName;
		this.LobbyPassword = onStartJoinLobbyCommand.LobbyPassword;
		this.signalBus.Dispatch(AutoJoin.AUTOJOIN);
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x000E497C File Offset: 0x000E2D7C
	private void onStartJoinSteamLobby(GameEvent message)
	{
		OnStartJoinSteamLobbyCommand onStartJoinSteamLobbyCommand = message as OnStartJoinSteamLobbyCommand;
		this.LobbySteamID = onStartJoinSteamLobbyCommand.SteamLobbyID;
	}

	// Token: 0x04001F4B RID: 8011
	public static string AUTOJOIN = "AutoJoin.JOINLOBBY";
}
