// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoinLobbyDialog : BaseWindow
{
	public Transform itemList;

	public LobbyGameButton itemEntryPrefab;

	public MenuItemButton NameInputButton;

	public ValidatableTextEntry NameInputField;

	public TextMeshProUGUI ServerErrorTitle;

	public TextMeshProUGUI ServerErrorBody;

	public CursorTargetButton CloseButton;

	public MenuItemList inputButtons;

	public MenuItemList bottomButtons;

	public MenuItemButton JoinFriendButton;

	public MenuItemButton RefreshLobbyListButton;

	public string errorTitle;

	public string errorBody;

	private List<LobbyGameButton> unfilteredLobbyButtons = new List<LobbyGameButton>();

	private static Action<EChatRoomEnterResponse> __f__am_cache0;

	[Inject]
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public SteamManager steamManager
	{
		get;
		set;
	}

	[Inject]
	public LobbyNameValidator lobbyNameValidator
	{
		get;
		set;
	}

	public void Initialize()
	{
		this.inputButtons = base.injector.GetInstance<MenuItemList>();
		this.inputButtons.AddButton(this.NameInputButton, new Action(this.nameInputClicked));
		CursorTargetButton expr_34 = this.CloseButton;
		expr_34.ClickCallback = (Action<CursorTargetButton>)Delegate.Combine(expr_34.ClickCallback, new Action<CursorTargetButton>(this.closeClicked));
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
		ValidatableTextEntry expr_174 = this.NameInputField;
		expr_174.EndEditCallback = (Action)Delegate.Combine(expr_174.EndEditCallback, new Action(this.onEndEditName));
		ValidatableTextEntry expr_19B = this.NameInputField;
		expr_19B.EnterCallback = (Action)Delegate.Combine(expr_19B.EnterCallback, new Action(this.confirm));
		ValidatableTextEntry expr_1C2 = this.NameInputField;
		expr_1C2.ValueChangedCallback = (Action)Delegate.Combine(expr_1C2.ValueChangedCallback, new Action(this.onFilterUpdate));
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

	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.timer.CancelTimeout(new Action(this.refreshList));
		base.signalBus.RemoveListener(SteamManager.STEAM_LOBBY_LIST_UPDATED, new Action(this.onListUpdate));
		base.signalBus.RemoveListener(CustomLobbyController.UPDATED, new Action(this.onUpdate));
		base.signalBus.RemoveListener(CustomLobbyController.EVENT, new Action(this.onLobbyEvent));
		base.signalBus.RemoveListener(UIManager.SCREEN_CLOSED, new Action(this.onScreenClosed));
	}

	public void clearLobbyList()
	{
		foreach (LobbyGameButton current in this.unfilteredLobbyButtons)
		{
			this.inputButtons.RemoveButton(current.button);
			current.DestroySelf();
		}
		this.unfilteredLobbyButtons.Clear();
	}

	public void onListUpdate()
	{
		this.clearLobbyList();
		this.inputButtons.ClearButtons();
		this.inputButtons.AddButton(this.NameInputButton, new Action(this.nameInputClicked));
		foreach (SteamManager.LobbyListDataEntry current in this.steamManager.LobbyList)
		{
			LobbyGameButton lobbyGameButton = UnityEngine.Object.Instantiate<LobbyGameButton>(this.itemEntryPrefab, this.itemList);
			lobbyGameButton.Initialize(current.SteamId, current.LobbyName, current.PlayerCount, current.PlayerLimit, this);
			this.unfilteredLobbyButtons.Add(lobbyGameButton);
			bool buttonActive = string.IsNullOrEmpty(this.NameInputField.Text) || current.LobbyName.ToLower().Contains(this.NameInputField.Text.ToLower());
			lobbyGameButton.SetButtonActive(buttonActive);
		}
		this.inputButtons.LandingPoint = this.inputButtons.GetButtons()[this.inputButtons.GetButtons().Length - 1];
		this.bottomButtons.AddEdgeNavigation(MoveDirection.Up, this.inputButtons);
		this.inputButtons.Initialize();
	}

	public void onFilterUpdate()
	{
		foreach (LobbyGameButton current in this.unfilteredLobbyButtons)
		{
			bool buttonActive = string.IsNullOrEmpty(this.NameInputField.Text) || current.gameName.text.ToLower().Contains(this.NameInputField.Text.ToLower());
			current.SetButtonActive(buttonActive);
		}
	}

	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		if (((UIInputModule)base.uiManager.CurrentInputModule).CurrentInputField != null)
		{
			this.inputButtons.Disable();
		}
	}

	public void OnUpdate()
	{
		this.ServerErrorTitle.text = base.localization.GetText(this.errorTitle);
		this.ServerErrorBody.text = base.localization.GetText(this.errorBody);
	}

	private void onUpdate()
	{
		this.ServerErrorTitle.text = this.errorTitle;
		this.ServerErrorBody.text = this.errorBody;
		if (this.customLobby.IsInLobby)
		{
			this.Close();
		}
	}

	private void onScreenClosed()
	{
		this.Close();
	}

	private void onLobbyEvent()
	{
		this.errorTitle = this.getServerErrorTitle(this.customLobby.LastEvent);
		this.errorBody = this.getServerErrorBody(this.customLobby.LastEvent);
		this.onUpdate();
	}

	private void confirm()
	{
		this.steamManager.RequestLobbyList();
	}

	private void onEndEditName()
	{
		this.onFilterUpdate();
		if (this.isNullSelection())
		{
			this.inputButtons.AutoSelect(this.NameInputButton);
		}
	}

	private bool isNullSelection()
	{
		return this.inputButtons.CurrentSelection == null;
	}

	private void nameInputClicked()
	{
		base.uiManager.CurrentInputModule.SetSelectedInputField(this.NameInputField.TargetInput);
		this.inputButtons.Disable();
	}

	private void joinFriendClicked()
	{
		this.steamManager.JoinFriendLobby();
	}

	private void refreshLobbyListClicked()
	{
		this.refreshList();
	}

	private void refreshList()
	{
		this.steamManager.RequestLobbyList();
		base.timer.SetOrReplaceTimeout(8000, new Action(this.refreshList));
	}

	private void confirmClicked()
	{
		this.confirm();
	}

	private void cancelClicked()
	{
		this.Close();
	}

	private void closeClicked(CursorTargetButton button)
	{
		this.Close();
	}

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
		case LobbyEvent.CreateFailedInQueued:
		case LobbyEvent.CreateFailedBadParams:
			IL_22:
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
					UnityEngine.Debug.LogError("No title text for lobby error: " + error);
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
		goto IL_22;
	}

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
		case LobbyEvent.CreateFailedInQueued:
		case LobbyEvent.CreateFailedBadParams:
			IL_22:
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
					UnityEngine.Debug.LogError("No body text for lobby error: " + error);
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
		goto IL_22;
	}

	public void JoinCustomLobby(ulong steamLobbyID)
	{
		SteamManager arg_24_0 = this.steamManager;
		if (JoinLobbyDialog.__f__am_cache0 == null)
		{
			JoinLobbyDialog.__f__am_cache0 = new Action<EChatRoomEnterResponse>(JoinLobbyDialog._JoinCustomLobby_m__0);
		}
		arg_24_0.JoinLobby(steamLobbyID, JoinLobbyDialog.__f__am_cache0);
	}

	private static void _JoinCustomLobby_m__0(EChatRoomEnterResponse response)
	{
		if (response == EChatRoomEnterResponse.k_EChatRoomEnterResponseFull)
		{
			UnityEngine.Debug.LogError("Full room");
		}
		else if (response != EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
		{
			UnityEngine.Debug.LogError("failed room entry");
		}
	}
}
