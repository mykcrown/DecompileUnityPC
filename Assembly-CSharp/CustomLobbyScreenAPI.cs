using System;
using System.Collections.Generic;
using Steamworks;

// Token: 0x0200091E RID: 2334
public class CustomLobbyScreenAPI : ICustomLobbyScreenAPI
{
	// Token: 0x17000E7E RID: 3710
	// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x0011A338 File Offset: 0x00118738
	// (set) Token: 0x06003CC7 RID: 15559 RVA: 0x0011A340 File Offset: 0x00118740
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x17000E7F RID: 3711
	// (get) Token: 0x06003CC8 RID: 15560 RVA: 0x0011A349 File Offset: 0x00118749
	// (set) Token: 0x06003CC9 RID: 15561 RVA: 0x0011A351 File Offset: 0x00118751
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000E80 RID: 3712
	// (get) Token: 0x06003CCA RID: 15562 RVA: 0x0011A35A File Offset: 0x0011875A
	public bool IsInLobby
	{
		get
		{
			return this.customLobby.IsInLobby;
		}
	}

	// Token: 0x17000E81 RID: 3713
	// (get) Token: 0x06003CCB RID: 15563 RVA: 0x0011A367 File Offset: 0x00118767
	public string LobbyName
	{
		get
		{
			return this.customLobby.LobbyName;
		}
	}

	// Token: 0x17000E82 RID: 3714
	// (get) Token: 0x06003CCC RID: 15564 RVA: 0x0011A374 File Offset: 0x00118774
	public string LobbyPassword
	{
		get
		{
			return this.customLobby.LobbyPassword;
		}
	}

	// Token: 0x17000E83 RID: 3715
	// (get) Token: 0x06003CCD RID: 15565 RVA: 0x0011A381 File Offset: 0x00118781
	public StageID StageID
	{
		get
		{
			return this.customLobby.StageID;
		}
	}

	// Token: 0x17000E84 RID: 3716
	// (get) Token: 0x06003CCE RID: 15566 RVA: 0x0011A38E File Offset: 0x0011878E
	public LobbyGameMode ModeID
	{
		get
		{
			return this.customLobby.ModeID;
		}
	}

	// Token: 0x17000E85 RID: 3717
	// (get) Token: 0x06003CCF RID: 15567 RVA: 0x0011A39B File Offset: 0x0011879B
	public bool IsTeams
	{
		get
		{
			return this.customLobby.IsTeams;
		}
	}

	// Token: 0x17000E86 RID: 3718
	// (get) Token: 0x06003CD0 RID: 15568 RVA: 0x0011A3A8 File Offset: 0x001187A8
	public bool IsLobbyLeader
	{
		get
		{
			return this.customLobby.IsLobbyLeader;
		}
	}

	// Token: 0x17000E87 RID: 3719
	// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x0011A3B5 File Offset: 0x001187B5
	public ulong HostUserId
	{
		get
		{
			return this.customLobby.HostUserId;
		}
	}

	// Token: 0x06003CD2 RID: 15570 RVA: 0x0011A3C2 File Offset: 0x001187C2
	public bool IsValidPlayerConfiguration()
	{
		return this.customLobby.IsValidPlayerConfiguration();
	}

	// Token: 0x06003CD3 RID: 15571 RVA: 0x0011A3CF File Offset: 0x001187CF
	public bool IsAllPlayersReadyForCSS()
	{
		return this.customLobby.IsAllPlayersReadyForCSS();
	}

	// Token: 0x17000E88 RID: 3720
	// (get) Token: 0x06003CD4 RID: 15572 RVA: 0x0011A3DC File Offset: 0x001187DC
	public bool IsLobbyInMatch
	{
		get
		{
			return this.customLobby.IsLobbyInMatch;
		}
	}

	// Token: 0x17000E89 RID: 3721
	// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x0011A3E9 File Offset: 0x001187E9
	public Dictionary<ulong, LobbyPlayerData> Players
	{
		get
		{
			return this.customLobby.Players;
		}
	}

	// Token: 0x17000E8A RID: 3722
	// (get) Token: 0x06003CD6 RID: 15574 RVA: 0x0011A3F6 File Offset: 0x001187F6
	public LobbyEvent LastEvent
	{
		get
		{
			return this.lobbyEvent;
		}
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x0011A400 File Offset: 0x00118800
	public void Initialize()
	{
		this.lobbyEvent = LobbyEvent.None;
		this.allowUpdates = true;
		this.signalBus.AddListener(CustomLobbyController.UPDATED, new Action(this.lobbyUpdated));
		this.signalBus.AddListener(CustomLobbyController.EVENT, new Action(this.lobbyError));
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x0011A453 File Offset: 0x00118853
	public void OnDestroy()
	{
		this.signalBus.RemoveListener(CustomLobbyController.UPDATED, new Action(this.lobbyUpdated));
		this.signalBus.RemoveListener(CustomLobbyController.EVENT, new Action(this.lobbyError));
	}

	// Token: 0x06003CD9 RID: 15577 RVA: 0x0011A48D File Offset: 0x0011888D
	public void Create(string lobbyName, string lobbyPass, ELobbyType lobbyType)
	{
		this.customLobby.Create(lobbyName, lobbyPass, lobbyType);
	}

	// Token: 0x06003CDA RID: 15578 RVA: 0x0011A49D File Offset: 0x0011889D
	public void SetStage(StageID stageID)
	{
		this.customLobby.SetStage(stageID);
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x0011A4AB File Offset: 0x001188AB
	public void SetMode(LobbyGameMode modeID)
	{
		this.customLobby.SetMode(modeID);
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x0011A4B9 File Offset: 0x001188B9
	public void ChangePlayer(ulong userID, bool isSpectating, int team)
	{
		this.customLobby.ChangePlayer(userID, isSpectating, team);
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x0011A4C9 File Offset: 0x001188C9
	public void Leave()
	{
		this.customLobby.Leave();
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x0011A4D6 File Offset: 0x001188D6
	public void StartMatch()
	{
		this.customLobby.StartMatch();
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x0011A4E3 File Offset: 0x001188E3
	private void lobbyUpdated()
	{
		this.lobbyEvent = LobbyEvent.None;
		this.dispatchUpdate();
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x0011A4F2 File Offset: 0x001188F2
	private void lobbyError()
	{
		this.lobbyEvent = this.customLobby.LastEvent;
		if (this.lobbyEvent == LobbyEvent.StartingLobbyMatch)
		{
			this.allowUpdates = false;
		}
		this.dispatchUpdate();
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x0011A51F File Offset: 0x0011891F
	private void dispatchUpdate()
	{
		if (this.allowUpdates)
		{
			this.signalBus.Dispatch(CustomLobbyScreenAPI.UPDATED);
		}
	}

	// Token: 0x0400299C RID: 10652
	public static string UPDATED = "CustomLobbyScreenAPI.UPDATED";

	// Token: 0x0400299F RID: 10655
	private LobbyEvent lobbyEvent;

	// Token: 0x040029A0 RID: 10656
	private bool allowUpdates;
}
