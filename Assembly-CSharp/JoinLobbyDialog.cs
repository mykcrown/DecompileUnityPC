using System;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009A5 RID: 2469
public class JoinLobbyDialog : BaseWindow
{
	// Token: 0x17001000 RID: 4096
	// (get) Token: 0x060043A3 RID: 17315 RVA: 0x0012AE66 File Offset: 0x00129266
	// (set) Token: 0x060043A4 RID: 17316 RVA: 0x0012AE6E File Offset: 0x0012926E
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x17001001 RID: 4097
	// (get) Token: 0x060043A5 RID: 17317 RVA: 0x0012AE77 File Offset: 0x00129277
	// (set) Token: 0x060043A6 RID: 17318 RVA: 0x0012AE7F File Offset: 0x0012927F
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17001002 RID: 4098
	// (get) Token: 0x060043A7 RID: 17319 RVA: 0x0012AE88 File Offset: 0x00129288
	// (set) Token: 0x060043A8 RID: 17320 RVA: 0x0012AE90 File Offset: 0x00129290
	[Inject]
	public SteamManager steamManager { get; set; }

	// Token: 0x17001003 RID: 4099
	// (get) Token: 0x060043A9 RID: 17321 RVA: 0x0012AE99 File Offset: 0x00129299
	// (set) Token: 0x060043AA RID: 17322 RVA: 0x0012AEA1 File Offset: 0x001292A1
	[Inject]
	public LobbyNameValidator lobbyNameValidator { get; set; }

	// Token: 0x060043AB RID: 17323 RVA: 0x0012AEAC File Offset: 0x001292AC
	public void Initialize()
	{
		this.inputButtons = base.injector.GetInstance<MenuItemList>();
		this.inputButtons.AddButton(this.NameInputButton, new Action(this.nameInputClicked));
		CursorTargetButton closeButton = this.CloseButton;
		closeButton.ClickCallback = (Action<CursorTargetButton>)Delegate.Combine(closeButton.ClickCallback, new Action<CursorTargetButton>(this.closeClicked));
		this.inputButtons.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.inputButtons.LandingPoint = this.NameInputButton;
		this.bottomButtons = base.injector.GetInstance<MenuItemList>();
		this.bottomButtons.AddButton(this.RefreshLobbyListButton, new Action(this.refreshLobbyListClicked));
		this.bottomButtons.AddButton(this.JoinFriendButton, new Action(this.joinFriendClicked));
		this.bottomButtons.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.bottomButtons.LandingPoint = this.RefreshLobbyListButton;
		this.inputButtons.AddEdgeNavigation(MoveDirection.Down, this.bottomButtons);
		this.bottomButtons.AddEdgeNavigation(MoveDirection.Up, this.inputButtons);
		this.bottomButtons.Initialize();
		this.NameInputField.Text = string.Empty;
		base.injector.Inject(this.NameInputField);
		this.NameInputField.Init(this.lobbyNameValidator);
		this.NameInputField.UseValidation = false;
		this.NameInputField.TargetInput.characterLimit = this.gameDataManager.ConfigData.lobbySettings.maxNameLength;
		ValidatableTextEntry nameInputField = this.NameInputField;
		nameInputField.EndEditCallback = (Action)Delegate.Combine(nameInputField.EndEditCallback, new Action(this.onEndEditName));
		ValidatableTextEntry nameInputField2 = this.NameInputField;
		nameInputField2.EnterCallback = (Action)Delegate.Combine(nameInputField2.EnterCallback, new Action(this.confirm));
		ValidatableTextEntry nameInputField3 = this.NameInputField;
		nameInputField3.ValueChangedCallback = (Action)Delegate.Combine(nameInputField3.ValueChangedCallback, new Action(this.onFilterUpdate));
		this.errorTitle = string.Empty;
		this.errorBody = string.Empty;
		base.signalBus.AddListener(SteamManager.STEAM_LOBBY_LIST_UPDATED, new Action(this.onListUpdate));
		base.signalBus.AddListener(CustomLobbyController.UPDATED, new Action(this.onUpdate));
		base.signalBus.AddListener(CustomLobbyController.EVENT, new Action(this.onLobbyEvent));
		base.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.onScreenClosed));
		this.clearLobbyList();
		this.nameInputClicked();
		this.refreshList();
		this.onUpdate();
	}

	// Token: 0x060043AC RID: 17324 RVA: 0x0012B13C File Offset: 0x0012953C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.timer.CancelTimeout(new Action(this.refreshList));
		base.signalBus.RemoveListener(SteamManager.STEAM_LOBBY_LIST_UPDATED, new Action(this.onListUpdate));
		base.signalBus.RemoveListener(CustomLobbyController.UPDATED, new Action(this.onUpdate));
		base.signalBus.RemoveListener(CustomLobbyController.EVENT, new Action(this.onLobbyEvent));
		base.signalBus.RemoveListener(UIManager.SCREEN_CLOSED, new Action(this.onScreenClosed));
	}

	// Token: 0x060043AD RID: 17325 RVA: 0x0012B1D8 File Offset: 0x001295D8
	public void clearLobbyList()
	{
		foreach (LobbyGameButton lobbyGameButton in this.unfilteredLobbyButtons)
		{
			this.inputButtons.RemoveButton(lobbyGameButton.button);
			lobbyGameButton.DestroySelf();
		}
		this.unfilteredLobbyButtons.Clear();
	}

	// Token: 0x060043AE RID: 17326 RVA: 0x0012B250 File Offset: 0x00129650
	public void onListUpdate()
	{
		this.clearLobbyList();
		this.inputButtons.ClearButtons();
		this.inputButtons.AddButton(this.NameInputButton, new Action(this.nameInputClicked));
		foreach (SteamManager.LobbyListDataEntry lobbyListDataEntry in this.steamManager.LobbyList)
		{
			LobbyGameButton lobbyGameButton = UnityEngine.Object.Instantiate<LobbyGameButton>(this.itemEntryPrefab, this.itemList);
			lobbyGameButton.Initialize(lobbyListDataEntry.SteamId, lobbyListDataEntry.LobbyName, lobbyListDataEntry.PlayerCount, lobbyListDataEntry.PlayerLimit, this);
			this.unfilteredLobbyButtons.Add(lobbyGameButton);
			bool buttonActive = string.IsNullOrEmpty(this.NameInputField.Text) || lobbyListDataEntry.LobbyName.ToLower().Contains(this.NameInputField.Text.ToLower());
			lobbyGameButton.SetButtonActive(buttonActive);
		}
		this.inputButtons.LandingPoint = this.inputButtons.GetButtons()[this.inputButtons.GetButtons().Length - 1];
		this.bottomButtons.AddEdgeNavigation(MoveDirection.Up, this.inputButtons);
		this.inputButtons.Initialize();
	}

	// Token: 0x060043AF RID: 17327 RVA: 0x0012B398 File Offset: 0x00129798
	public void onFilterUpdate()
	{
		foreach (LobbyGameButton lobbyGameButton in this.unfilteredLobbyButtons)
		{
			bool buttonActive = string.IsNullOrEmpty(this.NameInputField.Text) || lobbyGameButton.gameName.text.ToLower().Contains(this.NameInputField.Text.ToLower());
			lobbyGameButton.SetButtonActive(buttonActive);
		}
	}

	// Token: 0x060043B0 RID: 17328 RVA: 0x0012B434 File Offset: 0x00129834
	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		if (((UIInputModule)base.uiManager.CurrentInputModule).CurrentInputField != null)
		{
			this.inputButtons.Disable();
		}
	}

	// Token: 0x060043B1 RID: 17329 RVA: 0x0012B467 File Offset: 0x00129867
	public void OnUpdate()
	{
		this.ServerErrorTitle.text = base.localization.GetText(this.errorTitle);
		this.ServerErrorBody.text = base.localization.GetText(this.errorBody);
	}

	// Token: 0x060043B2 RID: 17330 RVA: 0x0012B4A1 File Offset: 0x001298A1
	private void onUpdate()
	{
		this.ServerErrorTitle.text = this.errorTitle;
		this.ServerErrorBody.text = this.errorBody;
		if (this.customLobby.IsInLobby)
		{
			this.Close();
		}
	}

	// Token: 0x060043B3 RID: 17331 RVA: 0x0012B4DB File Offset: 0x001298DB
	private void onScreenClosed()
	{
		this.Close();
	}

	// Token: 0x060043B4 RID: 17332 RVA: 0x0012B4E3 File Offset: 0x001298E3
	private void onLobbyEvent()
	{
		this.errorTitle = this.getServerErrorTitle(this.customLobby.LastEvent);
		this.errorBody = this.getServerErrorBody(this.customLobby.LastEvent);
		this.onUpdate();
	}

	// Token: 0x060043B5 RID: 17333 RVA: 0x0012B519 File Offset: 0x00129919
	private void confirm()
	{
		this.steamManager.RequestLobbyList();
	}

	// Token: 0x060043B6 RID: 17334 RVA: 0x0012B526 File Offset: 0x00129926
	private void onEndEditName()
	{
		this.onFilterUpdate();
		if (this.isNullSelection())
		{
			this.inputButtons.AutoSelect(this.NameInputButton);
		}
	}

	// Token: 0x060043B7 RID: 17335 RVA: 0x0012B54A File Offset: 0x0012994A
	private bool isNullSelection()
	{
		return this.inputButtons.CurrentSelection == null;
	}

	// Token: 0x060043B8 RID: 17336 RVA: 0x0012B55D File Offset: 0x0012995D
	private void nameInputClicked()
	{
		base.uiManager.CurrentInputModule.SetSelectedInputField(this.NameInputField.TargetInput);
		this.inputButtons.Disable();
	}

	// Token: 0x060043B9 RID: 17337 RVA: 0x0012B585 File Offset: 0x00129985
	private void joinFriendClicked()
	{
		this.steamManager.JoinFriendLobby();
	}

	// Token: 0x060043BA RID: 17338 RVA: 0x0012B592 File Offset: 0x00129992
	private void refreshLobbyListClicked()
	{
		this.refreshList();
	}

	// Token: 0x060043BB RID: 17339 RVA: 0x0012B59A File Offset: 0x0012999A
	private void refreshList()
	{
		this.steamManager.RequestLobbyList();
		base.timer.SetOrReplaceTimeout(8000, new Action(this.refreshList));
	}

	// Token: 0x060043BC RID: 17340 RVA: 0x0012B5C3 File Offset: 0x001299C3
	private void confirmClicked()
	{
		this.confirm();
	}

	// Token: 0x060043BD RID: 17341 RVA: 0x0012B5CB File Offset: 0x001299CB
	private void cancelClicked()
	{
		this.Close();
	}

	// Token: 0x060043BE RID: 17342 RVA: 0x0012B5D3 File Offset: 0x001299D3
	private void closeClicked(CursorTargetButton button)
	{
		this.Close();
	}

	// Token: 0x060043BF RID: 17343 RVA: 0x0012B5DB File Offset: 0x001299DB
	public override void OnCancelPressed()
	{
		base.OnCancelPressed();
		if (base.uiManager.CurrentInputModule.CurrentInputField != null)
		{
			base.uiManager.CurrentInputModule.SetSelectedInputField(null);
		}
		else
		{
			this.Close();
		}
	}

	// Token: 0x060043C0 RID: 17344 RVA: 0x0012B61C File Offset: 0x00129A1C
	private string getServerErrorTitle(LobbyEvent error)
	{
		switch (error)
		{
		case LobbyEvent.None:
			return string.Empty;
		case LobbyEvent.CreateFailedNameTaken:
			return base.localization.GetText("ui.customLobby.serverError.lobbyNameTaken.title", this.NameInputField.Text);
		case LobbyEvent.CreateFailedInLobby:
			return base.localization.GetText("ui.customLobby.serverError.failedInLobby.title", this.NameInputField.Text);
		default:
			switch (error)
			{
			case LobbyEvent.JoinFailedLobbyFull:
				return base.localization.GetText("ui.customLobby.serverError.lobbyFull.title", this.NameInputField.Text);
			case LobbyEvent.JoinFailedMatchRunning:
				return base.localization.GetText("ui.customLobby.serverError.matchRunning.title", this.NameInputField.Text);
			case LobbyEvent.JoinSystemError:
				return base.localization.GetText("ui.customLobby.serverError.joinSystemError.title", this.NameInputField.Text);
			default:
				if (error == LobbyEvent.JoinFailedInvalidCreds)
				{
					return base.localization.GetText("ui.customLobby.serverError.invalidCredentials.title", this.NameInputField.Text);
				}
				if (error != LobbyEvent.DestroyedOwnerLeft)
				{
					Debug.LogError("No title text for lobby error: " + error);
					return string.Empty;
				}
				return base.localization.GetText("ui.customLobby.serverMessage.ownerLeft.title", this.NameInputField.Text);
			}
			break;
		case LobbyEvent.CreateFailedInMatch:
			return base.localization.GetText("ui.customLobby.serverError.failedInMatch.title", this.NameInputField.Text);
		case LobbyEvent.CreateSystemError:
			return base.localization.GetText("ui.customLobby.serverError.createSystemError.title", this.NameInputField.Text);
		}
	}

	// Token: 0x060043C1 RID: 17345 RVA: 0x0012B794 File Offset: 0x00129B94
	private string getServerErrorBody(LobbyEvent error)
	{
		switch (error)
		{
		case LobbyEvent.None:
			return string.Empty;
		case LobbyEvent.CreateFailedNameTaken:
			return base.localization.GetText("ui.customLobby.serverError.lobbyNameTaken.body", this.NameInputField.Text);
		case LobbyEvent.CreateFailedInLobby:
			return base.localization.GetText("ui.customLobby.serverError.failedInLobby.body", this.NameInputField.Text);
		default:
			switch (error)
			{
			case LobbyEvent.JoinFailedLobbyFull:
				return base.localization.GetText("ui.customLobby.serverError.lobbyFull.body", this.NameInputField.Text);
			case LobbyEvent.JoinFailedMatchRunning:
				return base.localization.GetText("ui.customLobby.serverError.matchRunning.body", this.NameInputField.Text);
			case LobbyEvent.JoinSystemError:
				return base.localization.GetText("ui.customLobby.serverError.joinSystemError.body", this.NameInputField.Text);
			default:
				if (error == LobbyEvent.JoinFailedInvalidCreds)
				{
					return base.localization.GetText("ui.customLobby.serverError.invalidCredentials.body", this.NameInputField.Text);
				}
				if (error != LobbyEvent.DestroyedOwnerLeft)
				{
					Debug.LogError("No body text for lobby error: " + error);
					return string.Empty;
				}
				return base.localization.GetText("ui.customLobby.serverMessage.ownerLeft.body", this.NameInputField.Text);
			}
			break;
		case LobbyEvent.CreateFailedInMatch:
			return base.localization.GetText("ui.customLobby.serverError.failedInMatch.body", this.NameInputField.Text);
		case LobbyEvent.CreateSystemError:
			return base.localization.GetText("ui.customLobby.serverError.createSystemError.body", this.NameInputField.Text);
		}
	}

	// Token: 0x060043C2 RID: 17346 RVA: 0x0012B909 File Offset: 0x00129D09
	public void JoinCustomLobby(ulong steamLobbyID)
	{
		this.steamManager.JoinLobby(steamLobbyID, delegate(EChatRoomEnterResponse response)
		{
			if (response == EChatRoomEnterResponse.k_EChatRoomEnterResponseFull)
			{
				Debug.LogError("Full room");
			}
			else if (response != EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
			{
				Debug.LogError("failed room entry");
			}
		});
	}

	// Token: 0x04002D12 RID: 11538
	public Transform itemList;

	// Token: 0x04002D13 RID: 11539
	public LobbyGameButton itemEntryPrefab;

	// Token: 0x04002D14 RID: 11540
	public MenuItemButton NameInputButton;

	// Token: 0x04002D15 RID: 11541
	public ValidatableTextEntry NameInputField;

	// Token: 0x04002D16 RID: 11542
	public TextMeshProUGUI ServerErrorTitle;

	// Token: 0x04002D17 RID: 11543
	public TextMeshProUGUI ServerErrorBody;

	// Token: 0x04002D18 RID: 11544
	public CursorTargetButton CloseButton;

	// Token: 0x04002D19 RID: 11545
	public MenuItemList inputButtons;

	// Token: 0x04002D1A RID: 11546
	public MenuItemList bottomButtons;

	// Token: 0x04002D1B RID: 11547
	public MenuItemButton JoinFriendButton;

	// Token: 0x04002D1C RID: 11548
	public MenuItemButton RefreshLobbyListButton;

	// Token: 0x04002D1D RID: 11549
	public string errorTitle;

	// Token: 0x04002D1E RID: 11550
	public string errorBody;

	// Token: 0x04002D1F RID: 11551
	private List<LobbyGameButton> unfilteredLobbyButtons = new List<LobbyGameButton>();
}
