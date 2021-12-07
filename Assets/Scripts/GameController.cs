// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
	public GameAssetPreloader preloader;

	private static GameController instance;

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IPreviousCrashDetector previousCrashDetector
	{
		get;
		set;
	}

	[Inject]
	public StageMusicMap stageMusicMap
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public IRichPresence richPresence
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController lobbyController
	{
		get;
		set;
	}

	public GameManager currentGame
	{
		get;
		private set;
	}

	public static bool IsMatchRunning
	{
		get
		{
			return GameController.instance.MatchIsRunning;
		}
	}

	public long matchStartTime
	{
		get;
		private set;
	}

	public bool MatchIsRunning
	{
		get
		{
			return !(this.currentGame == null) && this.currentGame.IsRunning;
		}
	}

	public GameController()
	{
		GameController.instance = this;
	}

	public void EndPreload()
	{
		if (this.preloader != null)
		{
			this.preloader.Destroy();
			this.preloader = null;
		}
	}

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

	private void onMatchBegin(ServerEvent message)
	{
		UnityEngine.Debug.Log("ON MATCH BEGIN");
		this.battleServerAPI.Unsubscribe<MatchBeginEvent>(new ServerEventHandler(this.onMatchBegin));
		MatchBeginEvent matchBeginEvent = message as MatchBeginEvent;
		this.matchStartTime = matchBeginEvent.matchStartTime;
		this.beginMatch();
	}

	private void beginMatch()
	{
		this.uiAdapter.ShowPreloaded();
	}

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
			foreach (PlayerController current in players)
			{
				if (current.Reference.Type == PlayerType.Human)
				{
					CharacterData characterData2 = current.CharacterData;
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

	public void OnGameSynced()
	{
		this.audioManager.PlayMusic(this.stageMusicMap.GetSoundKey(this.currentGame.stageData.stageID));
		this.updateMatchRichPresence();
		this.uiAdapter.OnGameSynced();
	}

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
}
