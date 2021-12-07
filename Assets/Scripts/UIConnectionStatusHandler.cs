// Decompile from assembly: Assembly-CSharp.dll

using System;

public class UIConnectionStatusHandler : IConnectionStatusHandler
{
	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager connectionManager
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
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IOfflineModeDetector offlineMode
	{
		get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
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

	[Inject]
	public IExitGame terminateGame
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

	public void Startup()
	{
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.onServerStatusUpdate));
		IBattleServerAPI expr_22 = this.battleServerAPI;
		expr_22.OnLeftRoom = (Action<bool>)Delegate.Combine(expr_22.OnLeftRoom, new Action<bool>(this.onLeftRoom));
	}

	private void onLeftRoom(bool setIncomplete)
	{
		this.userInputManager.ResetPlayerMapping();
		if (setIncomplete)
		{
			this.exitBattle();
			if (this.lobbyController.IsInLobby)
			{
				this.richPresence.SetPresence("InCustomLobby", null, null, null);
				this.events.Broadcast(new LoadScreenCommand(ScreenType.CustomLobbyScreen, null, ScreenUpdateType.Next));
			}
			else if (!this.uiAdapter.AtOrGoingTo(ScreenType.MainMenu) && !this.uiAdapter.AtOrGoingTo(ScreenType.LoginScreen))
			{
				this.richPresence.SetPresence(null, null, null, null);
				this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
			}
		}
		this.onServerStatusUpdate();
	}

	private void onServerStatusUpdate()
	{
		if (this.offlineMode.IsOfflineMode())
		{
			return;
		}
		if ((!this.connectionManager.IsConnectedToAuth || !this.connectionManager.IsCoreConnectionReady) && this.uiAdapter.CurrentScreen != ScreenType.None && this.uiAdapter.CurrentScreen != ScreenType.LoginScreen)
		{
			this.exitBattle();
			this.richPresence.SetPresence(null, null, null, null);
			this.events.Broadcast(new LoadScreenCommand(ScreenType.LoginScreen, null, ScreenUpdateType.Next));
		}
	}

	private void exitBattle()
	{
		if (this.isInBattle())
		{
			this.terminateGame.InstantTerminate();
		}
	}

	private bool isInBattle()
	{
		return this.uiAdapter.CurrentScreen == ScreenType.BattleGUI || this.uiAdapter.CurrentScreen == ScreenType.LoadingBattle;
	}
}
