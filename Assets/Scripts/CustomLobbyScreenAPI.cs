// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;

public class CustomLobbyScreenAPI : ICustomLobbyScreenAPI
{
	public static string UPDATED = "CustomLobbyScreenAPI.UPDATED";

	private LobbyEvent lobbyEvent;

	private bool allowUpdates;

	[Inject]
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public bool IsInLobby
	{
		get
		{
			return this.customLobby.IsInLobby;
		}
	}

	public string LobbyName
	{
		get
		{
			return this.customLobby.LobbyName;
		}
	}

	public string LobbyPassword
	{
		get
		{
			return this.customLobby.LobbyPassword;
		}
	}

	public StageID StageID
	{
		get
		{
			return this.customLobby.StageID;
		}
	}

	public LobbyGameMode ModeID
	{
		get
		{
			return this.customLobby.ModeID;
		}
	}

	public bool IsTeams
	{
		get
		{
			return this.customLobby.IsTeams;
		}
	}

	public bool IsLobbyLeader
	{
		get
		{
			return this.customLobby.IsLobbyLeader;
		}
	}

	public ulong HostUserId
	{
		get
		{
			return this.customLobby.HostUserId;
		}
	}

	public bool IsLobbyInMatch
	{
		get
		{
			return this.customLobby.IsLobbyInMatch;
		}
	}

	public Dictionary<ulong, LobbyPlayerData> Players
	{
		get
		{
			return this.customLobby.Players;
		}
	}

	public LobbyEvent LastEvent
	{
		get
		{
			return this.lobbyEvent;
		}
	}

	public bool IsValidPlayerConfiguration()
	{
		return this.customLobby.IsValidPlayerConfiguration();
	}

	public bool IsAllPlayersReadyForCSS()
	{
		return this.customLobby.IsAllPlayersReadyForCSS();
	}

	public void Initialize()
	{
		this.lobbyEvent = LobbyEvent.None;
		this.allowUpdates = true;
		this.signalBus.AddListener(CustomLobbyController.UPDATED, new Action(this.lobbyUpdated));
		this.signalBus.AddListener(CustomLobbyController.EVENT, new Action(this.lobbyError));
	}

	public void OnDestroy()
	{
		this.signalBus.RemoveListener(CustomLobbyController.UPDATED, new Action(this.lobbyUpdated));
		this.signalBus.RemoveListener(CustomLobbyController.EVENT, new Action(this.lobbyError));
	}

	public void Create(string lobbyName, string lobbyPass, ELobbyType lobbyType)
	{
		this.customLobby.Create(lobbyName, lobbyPass, lobbyType);
	}

	public void SetStage(StageID stageID)
	{
		this.customLobby.SetStage(stageID);
	}

	public void SetMode(LobbyGameMode modeID)
	{
		this.customLobby.SetMode(modeID);
	}

	public void ChangePlayer(ulong userID, bool isSpectating, int team)
	{
		this.customLobby.ChangePlayer(userID, isSpectating, team);
	}

	public void Leave()
	{
		this.customLobby.Leave();
	}

	public void StartMatch()
	{
		this.customLobby.StartMatch();
	}

	private void lobbyUpdated()
	{
		this.lobbyEvent = LobbyEvent.None;
		this.dispatchUpdate();
	}

	private void lobbyError()
	{
		this.lobbyEvent = this.customLobby.LastEvent;
		if (this.lobbyEvent == LobbyEvent.StartingLobbyMatch)
		{
			this.allowUpdates = false;
		}
		this.dispatchUpdate();
	}

	private void dispatchUpdate()
	{
		if (this.allowUpdates)
		{
			this.signalBus.Dispatch(CustomLobbyScreenAPI.UPDATED);
		}
	}
}
