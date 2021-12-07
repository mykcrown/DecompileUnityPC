// Decompile from assembly: Assembly-CSharp.dll

using System;

public class AutoJoin : IAutoJoin
{
	public static string AUTOJOIN = "AutoJoin.JOINLOBBY";

	[Inject]
	public IEvents events
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

	public string LobbyName
	{
		get;
		private set;
	}

	public string LobbyPassword
	{
		get;
		private set;
	}

	public ulong LobbySteamID
	{
		get;
		private set;
	}

	[PostConstruct]
	public void Init()
	{
		this.events.Subscribe(typeof(OnStartJoinLobbyCommand), new Events.EventHandler(this.onStartJoinLobby));
		this.events.Subscribe(typeof(OnStartJoinSteamLobbyCommand), new Events.EventHandler(this.onStartJoinSteamLobby));
	}

	public void Clear()
	{
		this.LobbyName = null;
		this.LobbyPassword = null;
	}

	public bool AutoJoinIfSet()
	{
		if (this.LobbyName != null)
		{
			this.signalBus.Dispatch(AutoJoin.AUTOJOIN);
			return true;
		}
		return false;
	}

	private void onStartJoinLobby(GameEvent message)
	{
		OnStartJoinLobbyCommand onStartJoinLobbyCommand = message as OnStartJoinLobbyCommand;
		this.LobbyName = onStartJoinLobbyCommand.LobbyName;
		this.LobbyPassword = onStartJoinLobbyCommand.LobbyPassword;
		this.signalBus.Dispatch(AutoJoin.AUTOJOIN);
	}

	private void onStartJoinSteamLobby(GameEvent message)
	{
		OnStartJoinSteamLobbyCommand onStartJoinSteamLobbyCommand = message as OnStartJoinSteamLobbyCommand;
		this.LobbySteamID = onStartJoinSteamLobbyCommand.SteamLobbyID;
	}
}
