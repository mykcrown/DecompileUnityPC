// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class CustomLobbyEventNotifier : ICustomLobbyEventNotifier
{
	private List<LobbyEvent> unhandledLobbyEvents = new List<LobbyEvent>();

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialog
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IUIAdapter uIAdapter
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UIScreenAdapter.SCREEN_CHANGED, new Action(this.onScreenChanged));
		this.signalBus.AddListener(CustomLobbyController.EVENT, new Action(this.onCustomLobbyEvent));
	}

	private void onScreenChanged()
	{
		this.handleCustomLobbyEvents();
	}

	private bool shouldTriggerEvent(ScreenType currentScreen, LobbyEvent lobbyEvent)
	{
		return (currentScreen == ScreenType.CustomLobbyScreen || currentScreen == ScreenType.MainMenu) && lobbyEvent == LobbyEvent.DestroyedOwnerLeft;
	}

	private void tryHandleCustomLobbyEvent(LobbyEvent lobbyEvent)
	{
		if (this.shouldTriggerEvent(this.uIAdapter.CurrentScreen, lobbyEvent))
		{
			this.triggerLobbyEvent(lobbyEvent);
		}
		else
		{
			this.unhandledLobbyEvents.Insert(0, lobbyEvent);
		}
	}

	private void handleCustomLobbyEvents()
	{
		ScreenType currentScreen = this.uIAdapter.CurrentScreen;
		if (currentScreen == ScreenType.CustomLobbyScreen || currentScreen == ScreenType.MainMenu)
		{
			for (int i = this.unhandledLobbyEvents.Count - 1; i >= 0; i--)
			{
				LobbyEvent lobbyEvent = this.unhandledLobbyEvents[i];
				if (this.shouldTriggerEvent(currentScreen, lobbyEvent))
				{
					this.triggerLobbyEvent(lobbyEvent);
					this.unhandledLobbyEvents.RemoveAt(i);
				}
			}
		}
	}

	private void onCustomLobbyEvent()
	{
		LobbyEvent lastEvent = this.customLobby.LastEvent;
		if (lastEvent == LobbyEvent.DestroyedOwnerLeft)
		{
			this.tryHandleCustomLobbyEvent(this.customLobby.LastEvent);
		}
	}

	public void ClearCustomLobbyEventList()
	{
		this.unhandledLobbyEvents.Clear();
	}

	private void triggerLobbyEvent(LobbyEvent lobbyEvent)
	{
		if (lobbyEvent == LobbyEvent.DestroyedOwnerLeft)
		{
			this.dialog.ShowOneButtonDialog(this.localization.GetText("ui.customLobby.serverMessage.ownerLeft.title"), this.localization.GetText("ui.customLobby.serverMessage.ownerLeft.body"), this.localization.GetText("ui.customLobby.serverMessage.ownerLeft.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
	}
}
