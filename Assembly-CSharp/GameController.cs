using System;
using System.Collections.Generic;
using IconsServer;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class GameController
{
	// Token: 0x060017DB RID: 6107 RVA: 0x0007F37D File Offset: 0x0007D77D
	public GameController()
	{
		GameController.instance = this;
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x060017DC RID: 6108 RVA: 0x0007F38B File Offset: 0x0007D78B
	// (set) Token: 0x060017DD RID: 6109 RVA: 0x0007F393 File Offset: 0x0007D793
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x060017DE RID: 6110 RVA: 0x0007F39C File Offset: 0x0007D79C
	// (set) Token: 0x060017DF RID: 6111 RVA: 0x0007F3A4 File Offset: 0x0007D7A4
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x060017E0 RID: 6112 RVA: 0x0007F3AD File Offset: 0x0007D7AD
	// (set) Token: 0x060017E1 RID: 6113 RVA: 0x0007F3B5 File Offset: 0x0007D7B5
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x060017E2 RID: 6114 RVA: 0x0007F3BE File Offset: 0x0007D7BE
	// (set) Token: 0x060017E3 RID: 6115 RVA: 0x0007F3C6 File Offset: 0x0007D7C6
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x060017E4 RID: 6116 RVA: 0x0007F3CF File Offset: 0x0007D7CF
	// (set) Token: 0x060017E5 RID: 6117 RVA: 0x0007F3D7 File Offset: 0x0007D7D7
	[Inject]
	public IPreviousCrashDetector previousCrashDetector { get; set; }

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x060017E6 RID: 6118 RVA: 0x0007F3E0 File Offset: 0x0007D7E0
	// (set) Token: 0x060017E7 RID: 6119 RVA: 0x0007F3E8 File Offset: 0x0007D7E8
	[Inject]
	public StageMusicMap stageMusicMap { get; set; }

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x060017E8 RID: 6120 RVA: 0x0007F3F1 File Offset: 0x0007D7F1
	// (set) Token: 0x060017E9 RID: 6121 RVA: 0x0007F3F9 File Offset: 0x0007D7F9
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x060017EA RID: 6122 RVA: 0x0007F402 File Offset: 0x0007D802
	// (set) Token: 0x060017EB RID: 6123 RVA: 0x0007F40A File Offset: 0x0007D80A
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x060017EC RID: 6124 RVA: 0x0007F413 File Offset: 0x0007D813
	// (set) Token: 0x060017ED RID: 6125 RVA: 0x0007F41B File Offset: 0x0007D81B
	[Inject]
	public ICustomLobbyController lobbyController { get; set; }

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x060017EE RID: 6126 RVA: 0x0007F424 File Offset: 0x0007D824
	// (set) Token: 0x060017EF RID: 6127 RVA: 0x0007F42C File Offset: 0x0007D82C
	public GameManager currentGame { get; private set; }

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x060017F0 RID: 6128 RVA: 0x0007F435 File Offset: 0x0007D835
	public static bool IsMatchRunning
	{
		get
		{
			return GameController.instance.MatchIsRunning;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x060017F1 RID: 6129 RVA: 0x0007F441 File Offset: 0x0007D841
	// (set) Token: 0x060017F2 RID: 6130 RVA: 0x0007F449 File Offset: 0x0007D849
	public long matchStartTime { get; private set; }

	// Token: 0x060017F3 RID: 6131 RVA: 0x0007F452 File Offset: 0x0007D852
	public void EndPreload()
	{
		if (this.preloader != null)
		{
			this.preloader.Destroy();
			this.preloader = null;
		}
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x0007F474 File Offset: 0x0007D874
	public void ClientReadyToStartGame()
	{
		if (!this.battleServerAPI.IsConnected)
		{
			this.previousCrashDetector.UpdateStatus(PreviousCrashDetector.GameStatus.LocalGame);
			this.beginMatch();
		}
		else
		{
			this.previousCrashDetector.UpdateStatus(PreviousCrashDetector.GameStatus.OnlineGame);
			this.battleServerAPI.Listen<MatchBeginEvent>(new ServerEventHandler(this.onMatchBegin));
			this.battleServerAPI.StageLoaded();
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x060017F5 RID: 6133 RVA: 0x0007F4D8 File Offset: 0x0007D8D8
	public bool MatchIsRunning
	{
		get
		{
			return !(this.currentGame == null) && this.currentGame.IsRunning;
		}
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x0007F4FC File Offset: 0x0007D8FC
	private void onMatchBegin(ServerEvent message)
	{
		Debug.Log("ON MATCH BEGIN");
		this.battleServerAPI.Unsubscribe<MatchBeginEvent>(new ServerEventHandler(this.onMatchBegin));
		MatchBeginEvent matchBeginEvent = message as MatchBeginEvent;
		this.matchStartTime = matchBeginEvent.matchStartTime;
		this.beginMatch();
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x0007F543 File Offset: 0x0007D943
	private void beginMatch()
	{
		this.uiAdapter.ShowPreloaded();
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x0007F550 File Offset: 0x0007D950
	private void updateMatchRichPresence()
	{
		string loc = "presence.Local";
		string portraitKey = string.Empty;
		if (this.battleServerAPI.IsConnected)
		{
			if (this.lobbyController.IsInLobby)
			{
				loc = "presence.CustomLobby";
			}
			PlayerController playerController = this.currentGame.GetPlayerController(this.battleServerAPI.GetPrimaryLocalPlayer);
			if (playerController != null)
			{
				CharacterData characterData = playerController.CharacterData;
				portraitKey = characterData.characterName;
			}
		}
		else
		{
			List<PlayerController> players = this.currentGame.GetPlayers();
			foreach (PlayerController playerController2 in players)
			{
				if (playerController2.Reference.Type == PlayerType.Human)
				{
					CharacterData characterData2 = playerController2.CharacterData;
					portraitKey = characterData2.characterName;
					break;
				}
			}
			if (this.currentGame.IsTrainingMode)
			{
				loc = "presence.Training";
			}
		}
		this.richPresence.SetPresence("Playing", loc, portraitKey, null);
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x0007F668 File Offset: 0x0007DA68
	public void OnGameSynced()
	{
		this.audioManager.PlayMusic(this.stageMusicMap.GetSoundKey(this.currentGame.stageData.stageID));
		this.updateMatchRichPresence();
		this.uiAdapter.OnGameSynced();
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x0007F6A1 File Offset: 0x0007DAA1
	public void SetCurrentGame(GameManager gameManager)
	{
		this.currentGame = gameManager;
		if (this.currentGame == null)
		{
			this.previousCrashDetector.UpdateStatus(PreviousCrashDetector.GameStatus.Menu);
		}
		else
		{
			this.previousCrashDetector.UpdateStatus(PreviousCrashDetector.GameStatus.GameLoad);
		}
	}

	// Token: 0x0400124F RID: 4687
	public GameAssetPreloader preloader;

	// Token: 0x04001250 RID: 4688
	private static GameController instance;
}
