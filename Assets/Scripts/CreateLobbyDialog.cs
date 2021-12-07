// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateLobbyDialog : BaseWindow
{
	public TextMeshProUGUI Title;

	public MenuItemButton NameInputButton;

	public ValidatableTextEntry NameInputField;

	public TextMeshProUGUI ServerErrorTitle;

	public TextMeshProUGUI ServerErrorBody;

	public MenuItemButton ConfirmButton;

	public CursorTargetButton CloseButton;

	private bool isCreate;

	private MenuItemList dropdownList;

	public DropdownElement LobbyTypeDropdown;

	private ELobbyType lobbyType;

	public Transform DropdownContainer;

	private MenuItemList inputButtons;

	private MenuItemList bottomButtons;

	private string errorTitle;

	private string errorBody;

	private List<ELobbyType> lobbyListMap = new List<ELobbyType>();

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
	public LobbyNameValidator lobbyNameValidator
	{
		get;
		set;
	}

	[Inject]
	public LobbyPasswordValidator lobbyPasswordValidator
	{
		get;
		set;
	}

	public void Initialize(bool isCreate = true)
	{
		this.isCreate = isCreate;
		if (isCreate)
		{
			this.Title.text = base.localization.GetText("ui.lobbyDialog.create.title");
			this.ConfirmButton.TextField.text = base.localization.GetText("ui.lobbyDialog.create.confirm");
		}
		else
		{
			this.Title.text = base.localization.GetText("ui.lobbyDialog.join.title");
			this.ConfirmButton.TextField.text = base.localization.GetText("ui.lobbyDialog.join.confirm");
		}
		this.inputButtons = base.injector.GetInstance<MenuItemList>();
		this.inputButtons.AddButton(this.NameInputButton, new Action(this.nameInputClicked));
		this.bottomButtons = base.injector.GetInstance<MenuItemList>();
		this.bottomButtons.AddButton(this.ConfirmButton, new Action(this.confirmClicked));
		CursorTargetButton expr_EA = this.CloseButton;
		expr_EA.ClickCallback = (Action<CursorTargetButton>)Delegate.Combine(expr_EA.ClickCallback, new Action<CursorTargetButton>(this.closeClicked));
		this.inputButtons.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.bottomButtons.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.bottomButtons.LandingPoint = this.ConfirmButton;
		base.injector.Inject(this.NameInputField);
		this.NameInputField.Init(this.lobbyNameValidator);
		this.NameInputField.UseValidation = false;
		this.NameInputField.TargetInput.characterLimit = this.gameDataManager.ConfigData.lobbySettings.maxNameLength;
		ValidatableTextEntry expr_18F = this.NameInputField;
		expr_18F.EndEditCallback = (Action)Delegate.Combine(expr_18F.EndEditCallback, new Action(this.onEndEditName));
		ValidatableTextEntry expr_1B6 = this.NameInputField;
		expr_1B6.EnterCallback = (Action)Delegate.Combine(expr_1B6.EnterCallback, new Action(this.confirm));
		this.dropdownList = base.injector.GetInstance<MenuItemList>();
		base.injector.Inject(this.LobbyTypeDropdown);
		this.dropdownList.AddButton(this.LobbyTypeDropdown.Button, new Action(this.openLobbyTypeDropDown));
		DropdownElement expr_221 = this.LobbyTypeDropdown;
		expr_221.ValueSelected = (Action<int>)Delegate.Combine(expr_221.ValueSelected, new Action<int>(this.lobbyTypeSelected));
		this.LobbyTypeDropdown.Initialize(this.getLobbyTypeNames(), this.DropdownContainer, null);
		this.LobbyTypeDropdown.Button.DisableType = ButtonAnimator.VisualDisableType.Grey;
		DropdownElement expr_271 = this.LobbyTypeDropdown;
		expr_271.OnDropdownClosed = (Action)Delegate.Combine(expr_271.OnDropdownClosed, new Action(this._Initialize_m__0));
		this.dropdownList.LandingPoint = this.LobbyTypeDropdown.Button;
		this.dropdownList.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.dropdownList.Initialize();
		this.dropdownList.SetButtonEnabled(this.LobbyTypeDropdown.Button, true);
		this.dropdownList.AddEdgeNavigation(MoveDirection.Left, this.bottomButtons);
		this.dropdownList.AddEdgeNavigation(MoveDirection.Down, this.bottomButtons);
		this.inputButtons.AddEdgeNavigation(MoveDirection.Right, this.dropdownList);
		this.inputButtons.AddEdgeNavigation(MoveDirection.Down, this.bottomButtons);
		this.bottomButtons.AddEdgeNavigation(MoveDirection.Up, this.inputButtons);
		this.inputButtons.Initialize();
		this.bottomButtons.Initialize();
		this.errorTitle = string.Empty;
		this.errorBody = string.Empty;
		base.signalBus.AddListener(CustomLobbyController.UPDATED, new Action(this.onUpdate));
		base.signalBus.AddListener(CustomLobbyController.EVENT, new Action(this.onLobbyEvent));
		base.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.onScreenClosed));
		base.signalBus.AddListener(SteamManager.STEAM_LOBBY_FAILED_CREATE, new Action(this.onCreateFailed));
		this.nameInputClicked();
		this.onUpdate();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.signalBus.RemoveListener(CustomLobbyController.UPDATED, new Action(this.onUpdate));
		base.signalBus.RemoveListener(CustomLobbyController.EVENT, new Action(this.onLobbyEvent));
		base.signalBus.RemoveListener(UIManager.SCREEN_CLOSED, new Action(this.onScreenClosed));
	}

	private void openLobbyTypeDropDown()
	{
		this.dropdownList.Lock();
		this.LobbyTypeDropdown.Open();
		this.LobbyTypeDropdown.AutoSelectElement();
	}

	private void lobbyTypeSelected(int value)
	{
		this.LobbyTypeDropdown.Close();
		this.lobbyType = this.lobbyListMap[value];
		this.LobbyTypeDropdown.SetValue(value);
	}

	private void onDropdownClosed(DropdownElement dropdown)
	{
		this.dropdownList.Unlock();
		this.dropdownList.AutoSelect(dropdown.Button);
	}

	private string[] getLobbyTypeNames()
	{
		this.lobbyListMap.Clear();
		string[] names = Enum.GetNames(typeof(ELobbyType));
		List<string> list = new List<string>();
		for (int i = names.Length - 1; i >= 0; i--)
		{
			if (i != 3 && i != 0)
			{
				this.lobbyListMap.Add((ELobbyType)i);
				list.Add(base.localization.GetText("ui.lobbyDialog.lobbyType." + names[i]));
			}
		}
		this.lobbyType = this.lobbyListMap[0];
		return list.ToArray();
	}

	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		if (((UIInputModule)base.uiManager.CurrentInputModule).CurrentInputField != null)
		{
			this.inputButtons.Disable();
			this.bottomButtons.Disable();
		}
	}

	private void onUpdate()
	{
		this.ServerErrorTitle.text = this.errorTitle;
		this.ServerErrorBody.text = this.errorBody;
		this.LobbyTypeDropdown.SetValue(0);
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

	private void onCreateFailed()
	{
		this.errorTitle = this.getServerErrorTitle(LobbyEvent.CreateSystemError);
		this.errorBody = this.getServerErrorBody(LobbyEvent.CreateSystemError);
		this.onUpdate();
	}

	private void confirm()
	{
		if (!this.lobbyNameValidator.Validate(this.NameInputField.Text).IsOk || this.customLobby.IsInLobby)
		{
			this.NameInputField.UseValidation = true;
			return;
		}
		if (this.isCreate)
		{
			this.customLobby.Create(this.NameInputField.Text, string.Empty, this.lobbyType);
		}
	}

	private void onEndEditName()
	{
		if (this.lobbyNameValidator.Validate(this.NameInputField.Text).IsOk)
		{
			this.NameInputField.UseValidation = true;
		}
		if (this.isNullSelection())
		{
			this.inputButtons.AutoSelect(this.NameInputButton);
		}
	}

	private bool isNullSelection()
	{
		return this.inputButtons.CurrentSelection == null && this.bottomButtons.CurrentSelection == null;
	}

	private void nameInputClicked()
	{
		base.uiManager.CurrentInputModule.SetSelectedInputField(this.NameInputField.TargetInput);
		this.inputButtons.Disable();
		this.bottomButtons.Disable();
	}

	private void lobbyTypeClicked()
	{
		this.dropdownList.Lock();
		this.LobbyTypeDropdown.Open();
		this.dropdownList.AutoSelect(this.dropdownList.GetButtons()[0]);
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
		if (this.LobbyTypeDropdown.IsOpen())
		{
			this.LobbyTypeDropdown.Close();
			return;
		}
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

	public void TryJoinLobby(string lobbyName, string lobbyPassword)
	{
		if (lobbyName != null && lobbyPassword != null)
		{
			this.isCreate = false;
			if (this.isCreate)
			{
				this.Title.text = base.localization.GetText("ui.lobbyDialog.create.title");
				this.ConfirmButton.TextField.text = base.localization.GetText("ui.lobbyDialog.create.confirm");
			}
			else
			{
				this.Title.text = base.localization.GetText("ui.lobbyDialog.join.title");
				this.ConfirmButton.TextField.text = base.localization.GetText("ui.lobbyDialog.join.confirm");
			}
			this.NameInputField.Text = lobbyName;
			this.confirm();
		}
	}

	private void _Initialize_m__0()
	{
		this.onDropdownClosed(this.LobbyTypeDropdown);
	}
}
