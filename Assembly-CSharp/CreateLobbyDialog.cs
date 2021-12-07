using System;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009A3 RID: 2467
public class CreateLobbyDialog : BaseWindow
{
	// Token: 0x17000FFB RID: 4091
	// (get) Token: 0x0600437D RID: 17277 RVA: 0x0012A1DE File Offset: 0x001285DE
	// (set) Token: 0x0600437E RID: 17278 RVA: 0x0012A1E6 File Offset: 0x001285E6
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x17000FFC RID: 4092
	// (get) Token: 0x0600437F RID: 17279 RVA: 0x0012A1EF File Offset: 0x001285EF
	// (set) Token: 0x06004380 RID: 17280 RVA: 0x0012A1F7 File Offset: 0x001285F7
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000FFD RID: 4093
	// (get) Token: 0x06004381 RID: 17281 RVA: 0x0012A200 File Offset: 0x00128600
	// (set) Token: 0x06004382 RID: 17282 RVA: 0x0012A208 File Offset: 0x00128608
	[Inject]
	public LobbyNameValidator lobbyNameValidator { get; set; }

	// Token: 0x17000FFE RID: 4094
	// (get) Token: 0x06004383 RID: 17283 RVA: 0x0012A211 File Offset: 0x00128611
	// (set) Token: 0x06004384 RID: 17284 RVA: 0x0012A219 File Offset: 0x00128619
	[Inject]
	public LobbyPasswordValidator lobbyPasswordValidator { get; set; }

	// Token: 0x06004385 RID: 17285 RVA: 0x0012A224 File Offset: 0x00128624
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
		CursorTargetButton closeButton = this.CloseButton;
		closeButton.ClickCallback = (Action<CursorTargetButton>)Delegate.Combine(closeButton.ClickCallback, new Action<CursorTargetButton>(this.closeClicked));
		this.inputButtons.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.bottomButtons.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.bottomButtons.LandingPoint = this.ConfirmButton;
		base.injector.Inject(this.NameInputField);
		this.NameInputField.Init(this.lobbyNameValidator);
		this.NameInputField.UseValidation = false;
		this.NameInputField.TargetInput.characterLimit = this.gameDataManager.ConfigData.lobbySettings.maxNameLength;
		ValidatableTextEntry nameInputField = this.NameInputField;
		nameInputField.EndEditCallback = (Action)Delegate.Combine(nameInputField.EndEditCallback, new Action(this.onEndEditName));
		ValidatableTextEntry nameInputField2 = this.NameInputField;
		nameInputField2.EnterCallback = (Action)Delegate.Combine(nameInputField2.EnterCallback, new Action(this.confirm));
		this.dropdownList = base.injector.GetInstance<MenuItemList>();
		base.injector.Inject(this.LobbyTypeDropdown);
		this.dropdownList.AddButton(this.LobbyTypeDropdown.Button, new Action(this.openLobbyTypeDropDown));
		DropdownElement lobbyTypeDropdown = this.LobbyTypeDropdown;
		lobbyTypeDropdown.ValueSelected = (Action<int>)Delegate.Combine(lobbyTypeDropdown.ValueSelected, new Action<int>(this.lobbyTypeSelected));
		this.LobbyTypeDropdown.Initialize(this.getLobbyTypeNames(), this.DropdownContainer, null);
		this.LobbyTypeDropdown.Button.DisableType = ButtonAnimator.VisualDisableType.Grey;
		DropdownElement lobbyTypeDropdown2 = this.LobbyTypeDropdown;
		lobbyTypeDropdown2.OnDropdownClosed = (Action)Delegate.Combine(lobbyTypeDropdown2.OnDropdownClosed, new Action(delegate()
		{
			this.onDropdownClosed(this.LobbyTypeDropdown);
		}));
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

	// Token: 0x06004386 RID: 17286 RVA: 0x0012A60C File Offset: 0x00128A0C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.signalBus.RemoveListener(CustomLobbyController.UPDATED, new Action(this.onUpdate));
		base.signalBus.RemoveListener(CustomLobbyController.EVENT, new Action(this.onLobbyEvent));
		base.signalBus.RemoveListener(UIManager.SCREEN_CLOSED, new Action(this.onScreenClosed));
	}

	// Token: 0x06004387 RID: 17287 RVA: 0x0012A673 File Offset: 0x00128A73
	private void openLobbyTypeDropDown()
	{
		this.dropdownList.Lock();
		this.LobbyTypeDropdown.Open();
		this.LobbyTypeDropdown.AutoSelectElement();
	}

	// Token: 0x06004388 RID: 17288 RVA: 0x0012A696 File Offset: 0x00128A96
	private void lobbyTypeSelected(int value)
	{
		this.LobbyTypeDropdown.Close();
		this.lobbyType = this.lobbyListMap[value];
		this.LobbyTypeDropdown.SetValue(value);
	}

	// Token: 0x06004389 RID: 17289 RVA: 0x0012A6C1 File Offset: 0x00128AC1
	private void onDropdownClosed(DropdownElement dropdown)
	{
		this.dropdownList.Unlock();
		this.dropdownList.AutoSelect(dropdown.Button);
	}

	// Token: 0x0600438A RID: 17290 RVA: 0x0012A6E0 File Offset: 0x00128AE0
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

	// Token: 0x0600438B RID: 17291 RVA: 0x0012A778 File Offset: 0x00128B78
	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		if (((UIInputModule)base.uiManager.CurrentInputModule).CurrentInputField != null)
		{
			this.inputButtons.Disable();
			this.bottomButtons.Disable();
		}
	}

	// Token: 0x0600438C RID: 17292 RVA: 0x0012A7B8 File Offset: 0x00128BB8
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

	// Token: 0x0600438D RID: 17293 RVA: 0x0012A809 File Offset: 0x00128C09
	private void onScreenClosed()
	{
		this.Close();
	}

	// Token: 0x0600438E RID: 17294 RVA: 0x0012A811 File Offset: 0x00128C11
	private void onLobbyEvent()
	{
		this.errorTitle = this.getServerErrorTitle(this.customLobby.LastEvent);
		this.errorBody = this.getServerErrorBody(this.customLobby.LastEvent);
		this.onUpdate();
	}

	// Token: 0x0600438F RID: 17295 RVA: 0x0012A847 File Offset: 0x00128C47
	private void onCreateFailed()
	{
		this.errorTitle = this.getServerErrorTitle(LobbyEvent.CreateSystemError);
		this.errorBody = this.getServerErrorBody(LobbyEvent.CreateSystemError);
		this.onUpdate();
	}

	// Token: 0x06004390 RID: 17296 RVA: 0x0012A86C File Offset: 0x00128C6C
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

	// Token: 0x06004391 RID: 17297 RVA: 0x0012A8F0 File Offset: 0x00128CF0
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

	// Token: 0x06004392 RID: 17298 RVA: 0x0012A948 File Offset: 0x00128D48
	private bool isNullSelection()
	{
		return this.inputButtons.CurrentSelection == null && this.bottomButtons.CurrentSelection == null;
	}

	// Token: 0x06004393 RID: 17299 RVA: 0x0012A974 File Offset: 0x00128D74
	private void nameInputClicked()
	{
		base.uiManager.CurrentInputModule.SetSelectedInputField(this.NameInputField.TargetInput);
		this.inputButtons.Disable();
		this.bottomButtons.Disable();
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x0012A9A7 File Offset: 0x00128DA7
	private void lobbyTypeClicked()
	{
		this.dropdownList.Lock();
		this.LobbyTypeDropdown.Open();
		this.dropdownList.AutoSelect(this.dropdownList.GetButtons()[0]);
	}

	// Token: 0x06004395 RID: 17301 RVA: 0x0012A9D7 File Offset: 0x00128DD7
	private void confirmClicked()
	{
		this.confirm();
	}

	// Token: 0x06004396 RID: 17302 RVA: 0x0012A9DF File Offset: 0x00128DDF
	private void cancelClicked()
	{
		this.Close();
	}

	// Token: 0x06004397 RID: 17303 RVA: 0x0012A9E7 File Offset: 0x00128DE7
	private void closeClicked(CursorTargetButton button)
	{
		this.Close();
	}

	// Token: 0x06004398 RID: 17304 RVA: 0x0012A9F0 File Offset: 0x00128DF0
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

	// Token: 0x06004399 RID: 17305 RVA: 0x0012AA58 File Offset: 0x00128E58
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

	// Token: 0x0600439A RID: 17306 RVA: 0x0012ABD0 File Offset: 0x00128FD0
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

	// Token: 0x0600439B RID: 17307 RVA: 0x0012AD48 File Offset: 0x00129148
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

	// Token: 0x04002CFB RID: 11515
	public TextMeshProUGUI Title;

	// Token: 0x04002CFC RID: 11516
	public MenuItemButton NameInputButton;

	// Token: 0x04002CFD RID: 11517
	public ValidatableTextEntry NameInputField;

	// Token: 0x04002CFE RID: 11518
	public TextMeshProUGUI ServerErrorTitle;

	// Token: 0x04002CFF RID: 11519
	public TextMeshProUGUI ServerErrorBody;

	// Token: 0x04002D00 RID: 11520
	public MenuItemButton ConfirmButton;

	// Token: 0x04002D01 RID: 11521
	public CursorTargetButton CloseButton;

	// Token: 0x04002D02 RID: 11522
	private bool isCreate;

	// Token: 0x04002D03 RID: 11523
	private MenuItemList dropdownList;

	// Token: 0x04002D04 RID: 11524
	public DropdownElement LobbyTypeDropdown;

	// Token: 0x04002D05 RID: 11525
	private ELobbyType lobbyType;

	// Token: 0x04002D06 RID: 11526
	public Transform DropdownContainer;

	// Token: 0x04002D07 RID: 11527
	private MenuItemList inputButtons;

	// Token: 0x04002D08 RID: 11528
	private MenuItemList bottomButtons;

	// Token: 0x04002D09 RID: 11529
	private string errorTitle;

	// Token: 0x04002D0A RID: 11530
	private string errorBody;

	// Token: 0x04002D0B RID: 11531
	private List<ELobbyType> lobbyListMap = new List<ELobbyType>();
}
