using System;
using System.Collections.Generic;

// Token: 0x0200091B RID: 2331
public class CustomLobbyEventNotifier : ICustomLobbyEventNotifier
{
	// Token: 0x17000E6E RID: 3694
	// (get) Token: 0x06003C7F RID: 15487 RVA: 0x00118D23 File Offset: 0x00117123
	// (set) Token: 0x06003C80 RID: 15488 RVA: 0x00118D2B File Offset: 0x0011712B
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000E6F RID: 3695
	// (get) Token: 0x06003C81 RID: 15489 RVA: 0x00118D34 File Offset: 0x00117134
	// (set) Token: 0x06003C82 RID: 15490 RVA: 0x00118D3C File Offset: 0x0011713C
	[Inject]
	public IDialogController dialog { get; set; }

	// Token: 0x17000E70 RID: 3696
	// (get) Token: 0x06003C83 RID: 15491 RVA: 0x00118D45 File Offset: 0x00117145
	// (set) Token: 0x06003C84 RID: 15492 RVA: 0x00118D4D File Offset: 0x0011714D
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000E71 RID: 3697
	// (get) Token: 0x06003C85 RID: 15493 RVA: 0x00118D56 File Offset: 0x00117156
	// (set) Token: 0x06003C86 RID: 15494 RVA: 0x00118D5E File Offset: 0x0011715E
	[Inject]
	public IUIAdapter uIAdapter { get; set; }

	// Token: 0x17000E72 RID: 3698
	// (get) Token: 0x06003C87 RID: 15495 RVA: 0x00118D67 File Offset: 0x00117167
	// (set) Token: 0x06003C88 RID: 15496 RVA: 0x00118D6F File Offset: 0x0011716F
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x06003C89 RID: 15497 RVA: 0x00118D78 File Offset: 0x00117178
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UIScreenAdapter.SCREEN_CHANGED, new Action(this.onScreenChanged));
		this.signalBus.AddListener(CustomLobbyController.EVENT, new Action(this.onCustomLobbyEvent));
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x00118DB2 File Offset: 0x001171B2
	private void onScreenChanged()
	{
		this.handleCustomLobbyEvents();
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x00118DBA File Offset: 0x001171BA
	private bool shouldTriggerEvent(ScreenType currentScreen, LobbyEvent lobbyEvent)
	{
		return (currentScreen == ScreenType.CustomLobbyScreen || currentScreen == ScreenType.MainMenu) && lobbyEvent == LobbyEvent.DestroyedOwnerLeft;
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x00118DD4 File Offset: 0x001171D4
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

	// Token: 0x06003C8D RID: 15501 RVA: 0x00118E08 File Offset: 0x00117208
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

	// Token: 0x06003C8E RID: 15502 RVA: 0x00118E7C File Offset: 0x0011727C
	private void onCustomLobbyEvent()
	{
		LobbyEvent lastEvent = this.customLobby.LastEvent;
		if (lastEvent == LobbyEvent.DestroyedOwnerLeft)
		{
			this.tryHandleCustomLobbyEvent(this.customLobby.LastEvent);
		}
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x00118EB8 File Offset: 0x001172B8
	public void ClearCustomLobbyEventList()
	{
		this.unhandledLobbyEvents.Clear();
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x00118EC8 File Offset: 0x001172C8
	private void triggerLobbyEvent(LobbyEvent lobbyEvent)
	{
		if (lobbyEvent == LobbyEvent.DestroyedOwnerLeft)
		{
			this.dialog.ShowOneButtonDialog(this.localization.GetText("ui.customLobby.serverMessage.ownerLeft.title"), this.localization.GetText("ui.customLobby.serverMessage.ownerLeft.body"), this.localization.GetText("ui.customLobby.serverMessage.ownerLeft.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
	}

	// Token: 0x04002973 RID: 10611
	private List<LobbyEvent> unhandledLobbyEvents = new List<LobbyEvent>();
}
