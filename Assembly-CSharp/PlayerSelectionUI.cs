using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020008F6 RID: 2294
public class PlayerSelectionUI : ClientBehavior
{
	// Token: 0x17000E21 RID: 3617
	// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x00112EE6 File Offset: 0x001112E6
	// (set) Token: 0x06003AE2 RID: 15074 RVA: 0x00112EEE File Offset: 0x001112EE
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x17000E22 RID: 3618
	// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x00112EF7 File Offset: 0x001112F7
	// (set) Token: 0x06003AE4 RID: 15076 RVA: 0x00112EFF File Offset: 0x001112FF
	[Inject]
	public ICharacterSelectModel model { private get; set; }

	// Token: 0x17000E23 RID: 3619
	// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x00112F08 File Offset: 0x00111308
	// (set) Token: 0x06003AE6 RID: 15078 RVA: 0x00112F10 File Offset: 0x00111310
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x17000E24 RID: 3620
	// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x00112F19 File Offset: 0x00111319
	// (set) Token: 0x06003AE8 RID: 15080 RVA: 0x00112F21 File Offset: 0x00111321
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x17000E25 RID: 3621
	// (get) Token: 0x06003AE9 RID: 15081 RVA: 0x00112F2A File Offset: 0x0011132A
	// (set) Token: 0x06003AEA RID: 15082 RVA: 0x00112F32 File Offset: 0x00111332
	[Inject]
	public IMainThreadTimer timer { private get; set; }

	// Token: 0x17000E26 RID: 3622
	// (get) Token: 0x06003AEB RID: 15083 RVA: 0x00112F3B File Offset: 0x0011133B
	// (set) Token: 0x06003AEC RID: 15084 RVA: 0x00112F43 File Offset: 0x00111343
	[Inject]
	public IUITextHelper textHelper { private get; set; }

	// Token: 0x17000E27 RID: 3623
	// (get) Token: 0x06003AED RID: 15085 RVA: 0x00112F4C File Offset: 0x0011134C
	// (set) Token: 0x06003AEE RID: 15086 RVA: 0x00112F54 File Offset: 0x00111354
	[Inject]
	public IUserInputManager userInputManager { private get; set; }

	// Token: 0x17000E28 RID: 3624
	// (get) Token: 0x06003AEF RID: 15087 RVA: 0x00112F5D File Offset: 0x0011135D
	// (set) Token: 0x06003AF0 RID: 15088 RVA: 0x00112F65 File Offset: 0x00111365
	[Inject]
	public ISettingsTabsModel settingsTabModel { private get; set; }

	// Token: 0x17000E29 RID: 3625
	// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x00112F6E File Offset: 0x0011136E
	// (set) Token: 0x06003AF2 RID: 15090 RVA: 0x00112F76 File Offset: 0x00111376
	[Inject]
	public IEnterNewGame enterNewGame { private get; set; }

	// Token: 0x17000E2A RID: 3626
	// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x00112F7F File Offset: 0x0011137F
	// (set) Token: 0x06003AF4 RID: 15092 RVA: 0x00112F87 File Offset: 0x00111387
	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel { private get; set; }

	// Token: 0x17000E2B RID: 3627
	// (get) Token: 0x06003AF5 RID: 15093 RVA: 0x00112F90 File Offset: 0x00111390
	// (set) Token: 0x06003AF6 RID: 15094 RVA: 0x00112F98 File Offset: 0x00111398
	[Inject]
	public IUnlockCharacter unlockCharacter { private get; set; }

	// Token: 0x17000E2C RID: 3628
	// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x00112FA1 File Offset: 0x001113A1
	// (set) Token: 0x06003AF8 RID: 15096 RVA: 0x00112FA9 File Offset: 0x001113A9
	[Inject]
	public IUnlockProAccount unlockProAccount { private get; set; }

	// Token: 0x17000E2D RID: 3629
	// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x00112FB2 File Offset: 0x001113B2
	// (set) Token: 0x06003AFA RID: 15098 RVA: 0x00112FBA File Offset: 0x001113BA
	[Inject]
	public IUserInventory inventory { private get; set; }

	// Token: 0x17000E2E RID: 3630
	// (get) Token: 0x06003AFB RID: 15099 RVA: 0x00112FC3 File Offset: 0x001113C3
	// (set) Token: 0x06003AFC RID: 15100 RVA: 0x00112FCB File Offset: 0x001113CB
	[Inject]
	public IEquipmentModel equipmentModel { private get; set; }

	// Token: 0x17000E2F RID: 3631
	// (get) Token: 0x06003AFD RID: 15101 RVA: 0x00112FD4 File Offset: 0x001113D4
	// (set) Token: 0x06003AFE RID: 15102 RVA: 0x00112FDC File Offset: 0x001113DC
	[Inject]
	public IUnlockCharacterFlow unlockCharacterFlow { private get; set; }

	// Token: 0x17000E30 RID: 3632
	// (get) Token: 0x06003AFF RID: 15103 RVA: 0x00112FE5 File Offset: 0x001113E5
	// (set) Token: 0x06003B00 RID: 15104 RVA: 0x00112FED File Offset: 0x001113ED
	[Inject]
	public IUnlockEquipmentFlow unlockEquipmentFlow { private get; set; }

	// Token: 0x17000E31 RID: 3633
	// (get) Token: 0x06003B01 RID: 15105 RVA: 0x00112FF6 File Offset: 0x001113F6
	// (set) Token: 0x06003B02 RID: 15106 RVA: 0x00112FFE File Offset: 0x001113FE
	[Inject]
	public IEquipFlow equipFlow { private get; set; }

	// Token: 0x17000E32 RID: 3634
	// (get) Token: 0x06003B03 RID: 15107 RVA: 0x00113007 File Offset: 0x00111407
	// (set) Token: 0x06003B04 RID: 15108 RVA: 0x0011300F File Offset: 0x0011140F
	[Inject]
	public IDetailedUnlockCharacterFlow unlockFlow { private get; set; }

	// Token: 0x17000E33 RID: 3635
	// (get) Token: 0x06003B05 RID: 15109 RVA: 0x00113018 File Offset: 0x00111418
	// (set) Token: 0x06003B06 RID: 15110 RVA: 0x00113020 File Offset: 0x00111420
	[Inject]
	public IBattleServerStagingAPI battleServerStagingAPI { private get; set; }

	// Token: 0x17000E34 RID: 3636
	// (get) Token: 0x06003B07 RID: 15111 RVA: 0x00113029 File Offset: 0x00111429
	// (set) Token: 0x06003B08 RID: 15112 RVA: 0x00113031 File Offset: 0x00111431
	[Inject]
	public CharacterSelectCalculator characterSelectCalculator { private get; set; }

	// Token: 0x17000E35 RID: 3637
	// (get) Token: 0x06003B09 RID: 15113 RVA: 0x0011303A File Offset: 0x0011143A
	// (set) Token: 0x06003B0A RID: 15114 RVA: 0x00113042 File Offset: 0x00111442
	[Inject]
	public GameEnvironmentData environment { private get; set; }

	// Token: 0x17000E36 RID: 3638
	// (get) Token: 0x06003B0B RID: 15115 RVA: 0x0011304B File Offset: 0x0011144B
	// (set) Token: 0x06003B0C RID: 15116 RVA: 0x00113053 File Offset: 0x00111453
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17000E37 RID: 3639
	// (get) Token: 0x06003B0D RID: 15117 RVA: 0x0011305C File Offset: 0x0011145C
	public PlayerType Type
	{
		get
		{
			return (this.playerInfo == null) ? PlayerType.None : this.playerInfo.type;
		}
	}

	// Token: 0x17000E38 RID: 3640
	// (get) Token: 0x06003B0E RID: 15118 RVA: 0x0011307A File Offset: 0x0011147A
	public PlayerNum PlayerNum
	{
		get
		{
			return (this.playerInfo == null) ? PlayerNum.None : this.playerInfo.playerNum;
		}
	}

	// Token: 0x17000E39 RID: 3641
	// (get) Token: 0x06003B0F RID: 15119 RVA: 0x00113099 File Offset: 0x00111499
	public TeamNum Team
	{
		get
		{
			return (this.playerInfo == null) ? TeamNum.None : this.playerInfo.team;
		}
	}

	// Token: 0x17000E3A RID: 3642
	// (get) Token: 0x06003B10 RID: 15120 RVA: 0x001130B8 File Offset: 0x001114B8
	// (set) Token: 0x06003B11 RID: 15121 RVA: 0x001130C0 File Offset: 0x001114C0
	public bool IsDisplayed { get; set; }

	// Token: 0x06003B12 RID: 15122 RVA: 0x001130CC File Offset: 0x001114CC
	public override void Awake()
	{
		base.Awake();
		this.unlockDiamond = delegate()
		{
			this.unlockDiamondFn();
		};
		base.events.Subscribe(typeof(HighlightCharacterEvent), new Events.EventHandler(this.onCharacterHighlighted));
		base.events.Subscribe(typeof(PlayerSelectionInfoChangedEvent), new Events.EventHandler(this.onPlayerSelectionInfoChanged));
		base.events.Subscribe(typeof(GameModeChangedEvent), new Events.EventHandler(this.onGameModeChanged));
		this.InputSettingsWindow.OnCloseRequest = new Action(this.closeInputSettings);
		this.PlayerProfileWindow.OnCloseRequest = new Action(this.closePlayerProfile);
	}

	// Token: 0x06003B13 RID: 15123 RVA: 0x00113184 File Offset: 0x00111584
	public void Initialize(PlayerSelectionInfo player, GameModeData data, bool isOnlineBlindPick)
	{
		this.isOnlineBlindPick = isOnlineBlindPick;
		if (isOnlineBlindPick)
		{
			this.characterSelectScene = base.uiAdapter.GetUIScene<OnlineBlindPickScene3D>();
		}
		else
		{
			this.characterSelectScene = base.uiAdapter.GetUIScene<CharacterSelectScene3D>();
		}
		this.currentModeData = data;
		this.setCharacterPortrait(null, null);
		this.playerNum = player.playerNum;
		this.textHelper.TrackText(this.PlayerName, new Action(this.syncPlayerNamePositions));
		this.updatePlayerInfo(player);
		this.InputSettingsWindow.Initialize(this.playerNum);
		this.PlayerProfileWindow.Initialize(this.playerNum);
		if (this.PlayerDiamondButton != null)
		{
			this.PlayerDiamondButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickPlayerDiamond);
			this.PlayerDiamondButton.HighlightCallback = new Action<CursorTargetButton>(this.onSelectPlayerDiamond);
			this.PlayerDiamondButton.UnhighlightCallback = new Action<CursorTargetButton>(this.onDeselectPlayerDiamond);
			base.signalBus.GetSignal<PlayerCursorNotIdleSignal>().AddListener(new Action<PlayerNum>(this.onNotIdle));
			base.signalBus.GetSignal<PlayerCursorStatusSignal>().AddListener(new Action<PlayerCursor>(this.onCursorUpdate));
		}
		this.TeamButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickTeamIndicatorButton);
		this.CycleSkinsButton.UseOverrideHighlightSound = true;
		this.CycleSkinsButton.OverrideHighlightSound = AudioData.Empty;
		this.CycleSkinsButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickPortrait);
		if (this.PlayerNameButton != null)
		{
			this.PlayerNameButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickPlayerName);
		}
		if (this.CharacterNameButton != null)
		{
			this.CharacterNameButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickCharacterName);
		}
		if (isOnlineBlindPick)
		{
			this.DisabledPlayerButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
			this.DisabledPlayerButton.DisableDuration = 0.075f;
			this.DisabledPlayerButton.SetInteractable(false);
			this.PlayerButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickedPlayerButton);
			this.PlayerButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
			this.PlayerButton.DisableDuration = 0.075f;
		}
	}

	// Token: 0x06003B14 RID: 15124 RVA: 0x001133B0 File Offset: 0x001117B0
	protected override void removeAllListeners()
	{
		if (this.PlayerDiamondButton != null)
		{
			base.signalBus.GetSignal<PlayerCursorNotIdleSignal>().RemoveListener(new Action<PlayerNum>(this.onNotIdle));
			base.signalBus.GetSignal<PlayerCursorStatusSignal>().RemoveListener(new Action<PlayerCursor>(this.onCursorUpdate));
		}
		base.removeAllListeners();
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x0011340C File Offset: 0x0011180C
	private void Update()
	{
		this.updatePlayerButton();
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x00113414 File Offset: 0x00111814
	public void UpdatePayload()
	{
		this.updateAll();
	}

	// Token: 0x06003B17 RID: 15127 RVA: 0x0011341C File Offset: 0x0011181C
	public override void OnDestroy()
	{
		this.clear3dDisplay();
		base.OnDestroy();
		base.events.Unsubscribe(typeof(HighlightCharacterEvent), new Events.EventHandler(this.onCharacterHighlighted));
		base.events.Unsubscribe(typeof(PlayerSelectionInfoChangedEvent), new Events.EventHandler(this.onPlayerSelectionInfoChanged));
		base.events.Unsubscribe(typeof(GameModeChangedEvent), new Events.EventHandler(this.onGameModeChanged));
	}

	// Token: 0x06003B18 RID: 15128 RVA: 0x00113498 File Offset: 0x00111898
	private void onClickCharacterName(CursorTargetButton target, PointerEventData eventData)
	{
		int portId = 0;
		PlayerInputPort portWithPlayerNum = this.userInputManager.GetPortWithPlayerNum(this.playerNum);
		if (portWithPlayerNum != null)
		{
			portId = portWithPlayerNum.Id;
		}
		InputSettingsPayload inputSettingsPayload = new InputSettingsPayload();
		inputSettingsPayload.portId = portId;
		this.settingsTabModel.Current = SettingsTab.CONTROLS;
		base.events.Broadcast(new LoadScreenCommand(ScreenType.SettingsScreen, inputSettingsPayload, ScreenUpdateType.Next));
	}

	// Token: 0x06003B19 RID: 15129 RVA: 0x001134F8 File Offset: 0x001118F8
	private void onClickPlayerName(CursorTargetButton target, PointerEventData eventData)
	{
		if (this.PlayerProfileWindow.gameObject.activeInHierarchy)
		{
			this.closePlayerProfile();
		}
		else
		{
			this.openPlayerProfile();
		}
	}

	// Token: 0x06003B1A RID: 15130 RVA: 0x00113520 File Offset: 0x00111920
	private void openPlayerProfile()
	{
		this.PlayerProfileWindow.gameObject.SetActive(true);
		this.PlayerProfileWindow.OnOpened();
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x0011353E File Offset: 0x0011193E
	private void closePlayerProfile()
	{
		this.PlayerProfileWindow.gameObject.SetActive(false);
	}

	// Token: 0x06003B1C RID: 15132 RVA: 0x00113554 File Offset: 0x00111954
	private void onClickPortrait(CursorTargetButton target, PointerEventData eventData)
	{
		if (this.locked)
		{
			return;
		}
		UICSSSceneCharacter characterDisplay = this.characterSelectScene.GetCharacterDisplay(this.playerNum);
		if (characterDisplay == null || characterDisplay.CharacterDisplay == null || characterDisplay.CharacterDisplay.IsCharacterSwapping)
		{
			return;
		}
		int clickedCharacterIndex = this.characterSelectScene.GetClickedCharacterIndex(this.playerNum, eventData.position);
		bool flag = false;
		if (clickedCharacterIndex != -1 && this.currentCharacter != null && this.characterDataHelper.GetLinkedCharacters(this.currentCharacter).Length > 1)
		{
			if (this.characterDataHelper.GetPrimary(this.currentCharacter) == this.currentCharacter)
			{
				if (clickedCharacterIndex == 1)
				{
					flag = true;
				}
			}
			else if (clickedCharacterIndex == 0)
			{
				flag = true;
			}
		}
		if (flag)
		{
			base.events.Broadcast(new CycleCharacterIndex(this.playerNum));
		}
		else
		{
			base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
			base.events.Broadcast(new CyclePlayerSkinRequest(this.playerNum, 1));
		}
	}

	// Token: 0x06003B1D RID: 15133 RVA: 0x00113674 File Offset: 0x00111A74
	private void updatePlayerType()
	{
		if (this.playerInfo.type == PlayerType.None)
		{
			this.ActiveMode.SetActive(false);
			this.InactiveMode.SetActive(true);
		}
		else
		{
			if (this.PlayerNameButton != null)
			{
				this.PlayerNameButton.RequireAuthorization(this.playerNum);
				if (this.playerInfo.type == PlayerType.Human)
				{
					this.PlayerNameButton.RequireAuthorization(PlayerNum.All);
				}
			}
			if (this.CharacterNameButton != null)
			{
				this.CharacterNameButton.RequireAuthorization(this.playerNum);
				if (this.playerInfo.type == PlayerType.Human)
				{
					this.CharacterNameButton.RequireAuthorization(PlayerNum.All);
				}
			}
			this.ActiveMode.SetActive(true);
			this.InactiveMode.SetActive(false);
		}
		if (this.PlayerDiamondButton != null)
		{
			if (this.playerInfo.type == PlayerType.Human)
			{
				this.DiamondArrow.SetActive(false);
				this.refreshPlayerDiamondLock();
				this.TeamButton.RequireAuthorization(this.playerNum);
				this.TeamButton.RequireAuthorization(PlayerNum.All);
				this.CycleSkinsButton.RequireAuthorization(this.playerNum);
				this.CycleSkinsButton.RequireAuthorization(PlayerNum.All);
			}
			else
			{
				this.DiamondArrow.SetActive(false);
				this.timer.CancelTimeout(this.unlockDiamond);
				this.PlayerDiamondButton.DisableAuthorization();
				this.TeamButton.DisableAuthorization();
				this.CycleSkinsButton.DisableAuthorization();
			}
		}
		this.syncPlayerNameState();
	}

	// Token: 0x06003B1E RID: 15134 RVA: 0x001137F9 File Offset: 0x00111BF9
	private void onNotIdle(PlayerNum playerNum)
	{
		if (playerNum == this.playerNum)
		{
			if (this.Type == PlayerType.Human)
			{
				this.refreshPlayerDiamondLock();
			}
			else
			{
				this.unlockDiamond();
			}
		}
	}

	// Token: 0x06003B1F RID: 15135 RVA: 0x00113828 File Offset: 0x00111C28
	private void onCursorUpdate(PlayerCursor cursor)
	{
		if (!cursor.IsInteracting)
		{
			this.PlayerDiamondButton.RemovePlayerSelect(cursor.Player);
		}
	}

	// Token: 0x06003B20 RID: 15136 RVA: 0x00113848 File Offset: 0x00111C48
	private void refreshPlayerDiamondLock()
	{
		this.PlayerDiamondButton.RequireAuthorization(this.playerNum);
		if (this.userInputManager.GetDeviceWithPlayerNum(this.playerNum) is KeyboardInputDevice)
		{
			this.PlayerDiamondButton.RequireAuthorization(PlayerNum.All);
		}
		this.timer.CancelTimeout(this.unlockDiamond);
		this.timer.SetTimeout((int)(base.gameDataManager.ConfigData.uiuxSettings.characterSelectIdleSeconds * 1000), this.unlockDiamond);
	}

	// Token: 0x06003B21 RID: 15137 RVA: 0x001138D3 File Offset: 0x00111CD3
	private void unlockDiamondFn()
	{
		this.timer.CancelTimeout(this.unlockDiamond);
		this.PlayerDiamondButton.DisableAuthorization();
	}

	// Token: 0x06003B22 RID: 15138 RVA: 0x001138F4 File Offset: 0x00111CF4
	private Sprite getDiamondHoverSprite()
	{
		switch (this.getDiamondAction())
		{
		case DiamondAction.ADD_CPU:
			return this.PlayerDiamondCPUAddSprite;
		case DiamondAction.REMOVE_CPU:
			return this.PlayerDiamondCPURemoveSprite;
		case DiamondAction.OPEN_OPTIONS:
			return this.PlayerDiamondHoverSprite;
		}
		return this.PlayerDiamondHoverSprite;
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x00113940 File Offset: 0x00111D40
	private void updatePlayerNumText()
	{
		if (this.PlayerDiamondButton != null && this.PlayerNumText != null)
		{
			if (this.PlayerDiamondButton.IsManualHighlight)
			{
				this.PlayerDiamond.sprite = this.getDiamondHoverSprite();
				this.PlayerNumText.gameObject.SetActive(false);
			}
			else
			{
				if (this.playerInfo.type != PlayerType.Human)
				{
					this.color = UIColor.Grey;
				}
				else
				{
					this.color = PlayerUtil.GetUIColor(this.playerNum, this.Team, PlayerType.Human, this.currentModeData.settings.usesTeams);
				}
				this.PlayerDiamond.sprite = this.PlayerDiamondSprites.GetSprite(this.color);
				string playerNumText = PlayerUtil.GetPlayerNumText(this.localization, this.playerInfo);
				this.PlayerNumText.gameObject.SetActive(true);
				this.PlayerNumText.text = playerNumText;
				this.PlayerNumText.color = PlayerUtil.GetColorFromUIColor(this.color);
			}
		}
		this.updatePlayerName();
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x00113A54 File Offset: 0x00111E54
	private void showProAccountSign()
	{
		string priceString = this.unlockProAccount.GetPriceString();
		this.ProAccountBody.text = this.localization.GetText("ui.store.characters.proAccount.body", priceString);
		this.ProAccountSignage.gameObject.SetActive(true);
	}

	// Token: 0x06003B25 RID: 15141 RVA: 0x00113A9C File Offset: 0x00111E9C
	private void updatePlayerButton()
	{
		if (!this.isOnlineBlindPick)
		{
			return;
		}
		this.PlayerButtonObject.SetActive(false);
		this.DisabledPlayerButtonObject.SetActive(false);
		this.CharacterNameText.gameObject.SetActive(true);
		this.CharacterNameDisabledText.gameObject.SetActive(false);
		this.BuyCharacterText.gameObject.SetActive(false);
		this.BuySkinText.gameObject.SetActive(false);
		this.ReadyText.gameObject.SetActive(false);
		this.ProAccountSignage.gameObject.SetActive(false);
		bool interactable = Time.realtimeSinceStartup < this.battleServerStagingAPI.PurchaseEndTime;
		switch (this.getPlayerButtonMode())
		{
		case PlayerButtonMode.READY:
			this.PlayerButtonObject.SetActive(true);
			this.ReadyText.gameObject.SetActive(true);
			if (this.locked)
			{
				this.ReadyText.text = this.localization.GetText("ui.onlineBlindPick.IsLocked");
				this.PlayerButton.SetInteractable(false);
			}
			else
			{
				this.ReadyText.text = this.localization.GetText("ui.onlineBlindPick.LockIn");
				this.PlayerButton.SetInteractable(true);
			}
			break;
		case PlayerButtonMode.BUY_CHARACTER:
		{
			this.PlayerButtonObject.SetActive(true);
			this.BuyCharacterText.gameObject.SetActive(true);
			string softPriceString = this.unlockCharacter.GetSoftPriceString(this.currentCharacter.characterID);
			this.BuyCharacterText.text = this.localization.GetText("ui.characterSelect.purchase.character", softPriceString);
			this.PlayerButton.SetInteractable(interactable);
			this.showProAccountSign();
			break;
		}
		case PlayerButtonMode.BUY_SKIN:
		{
			this.PlayerButtonObject.SetActive(true);
			this.BuySkinText.gameObject.SetActive(true);
			int price = this.equipmentModel.GetItemFromSkinKey(this.currentSkin.uniqueKey).price;
			this.BuySkinText.text = this.localization.GetText("ui.characterSelect.purchase.skin", this.localization.GetSoftPriceString(price));
			this.PlayerButton.SetInteractable(interactable);
			break;
		}
		case PlayerButtonMode.NOTOWNED_DISABLED:
			this.PlayerButtonObject.SetActive(false);
			this.DisabledPlayerButtonObject.SetActive(true);
			this.CharacterNameText.gameObject.SetActive(false);
			this.CharacterNameDisabledText.gameObject.SetActive(true);
			this.PlayerButton.SetInteractable(false);
			this.showProAccountSign();
			break;
		}
	}

	// Token: 0x06003B26 RID: 15142 RVA: 0x00113D1C File Offset: 0x0011211C
	private void updateSkinText()
	{
		if (!(this.previousCharacter == this.currentCharacter) || !(this.previousCharacter != null) || !(this.previousSkin != this.currentSkin) || this.previousSkin != null)
		{
		}
		this.previousCharacter = this.currentCharacter;
		this.previousSkin = this.currentSkin;
	}

	// Token: 0x06003B27 RID: 15143 RVA: 0x00113D90 File Offset: 0x00112190
	private void updatePlayerName()
	{
		string text = string.Empty;
		PlayerType type = this.Type;
		if (type != PlayerType.Human)
		{
			if (type == PlayerType.CPU)
			{
				text = this.localization.GetText("ui.characterSelect.cpuNumDisplay");
			}
		}
		else if (this.playerInfo.curProfile != null && this.playerInfo.curProfile.profileName != null)
		{
			text = this.playerInfo.curProfile.profileName;
		}
		else if (this.playerInfo.curProfile != null && this.playerInfo.curProfile.localName != null)
		{
			text = this.playerInfo.curProfile.localName;
		}
		else
		{
			text = this.localization.GetText("ui.characterSelect.playerName", PlayerUtil.GetIntFromPlayerNum(this.playerNum, false) + string.Empty);
		}
		this.textHelper.UpdateText(this.PlayerName, text);
	}

	// Token: 0x06003B28 RID: 15144 RVA: 0x00113E8C File Offset: 0x0011228C
	private void syncPlayerNameState()
	{
		if (this.PlayerNameArrow == null)
		{
			return;
		}
		if (this.Type == PlayerType.CPU)
		{
			this.PlayerNameArrow.gameObject.SetActive(false);
		}
		else
		{
			this.PlayerNameArrow.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003B29 RID: 15145 RVA: 0x00113EE0 File Offset: 0x001122E0
	private void syncPlayerNamePositions()
	{
		if (this.PlayerName == null || this.PlayerNameArrow == null)
		{
			return;
		}
		this.syncPlayerNameState();
		if (this.PlayerNameArrow.gameObject.activeSelf)
		{
			float num = this.PlayerName.renderedWidth - this.PlayerNameDefaultWidth;
			if (!this.hasAdjustedPlayerNameArrow)
			{
				this.playerNameArrowOriginalPosition = this.PlayerNameArrow.transform.localPosition;
			}
			Vector3 localPosition = this.playerNameArrowOriginalPosition;
			localPosition.x += num;
			this.PlayerNameArrow.transform.localPosition = localPosition;
			this.hasAdjustedPlayerNameArrow = true;
		}
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x00113F90 File Offset: 0x00112390
	private void onCharacterHighlighted(GameEvent message)
	{
		HighlightCharacterEvent highlightCharacterEvent = message as HighlightCharacterEvent;
		if (highlightCharacterEvent.playerNum != this.playerNum)
		{
			return;
		}
		CharacterSelectModel.HighlightInfo highlightInfo = this.model.GetHighlightInfo(this.playerNum);
		if (highlightCharacterEvent.characterDef != null)
		{
			highlightInfo.characterID = highlightCharacterEvent.characterDef.characterID;
			if (!this.isExistingSkinValid(highlightCharacterEvent.characterDef))
			{
				SkinSelectMode selectMode = (!this.isOnlineBlindPick) ? SkinSelectMode.Offline : SkinSelectMode.Online;
				SkinDefinition availableEquippedOrDefaultSkin = this.model.GetAvailableEquippedOrDefaultSkin(highlightCharacterEvent.characterDef, this.playerNum, this.enterNewGame.GamePayload.players, selectMode);
				if (availableEquippedOrDefaultSkin != null)
				{
					highlightInfo.skinIDs[highlightInfo.characterID] = availableEquippedOrDefaultSkin.uniqueKey;
				}
			}
		}
		else
		{
			highlightInfo.characterID = CharacterID.None;
		}
		this.updateAll();
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x0011406C File Offset: 0x0011246C
	private bool isExistingSkinValid(CharacterDefinition characterDef)
	{
		if (characterDef.isRandom)
		{
			return true;
		}
		CharacterSelectModel.HighlightInfo highlightInfo = this.model.GetHighlightInfo(this.playerNum);
		if (highlightInfo.skinIDs.ContainsKey(highlightInfo.characterID))
		{
			string text = highlightInfo.skinIDs[highlightInfo.characterID];
			if (text != null)
			{
				SkinDefinition skinDefinition = this.characterDataHelper.GetSkinDefinition(characterDef.characterID, text);
				SkinSelectMode selectMode = (!this.isOnlineBlindPick) ? SkinSelectMode.Offline : SkinSelectMode.Online;
				if (this.model.IsSkinAvailable(characterDef.characterID, skinDefinition, this.playerNum, this.enterNewGame.GamePayload.players, selectMode))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003B2C RID: 15148 RVA: 0x0011411D File Offset: 0x0011251D
	private CharacterID getCurrentCharacterId()
	{
		if (this.currentCharacter != null)
		{
			return this.currentCharacter.characterID;
		}
		return CharacterID.None;
	}

	// Token: 0x06003B2D RID: 15149 RVA: 0x00114140 File Offset: 0x00112540
	private void onPlayerSelectionInfoChanged(GameEvent message)
	{
		PlayerSelectionInfoChangedEvent playerSelectionInfoChangedEvent = message as PlayerSelectionInfoChangedEvent;
		PlayerSelectionInfo info = playerSelectionInfoChangedEvent.info;
		if (info.playerNum == this.playerNum)
		{
			this.updatePlayerInfo(info);
		}
	}

	// Token: 0x06003B2E RID: 15150 RVA: 0x00114173 File Offset: 0x00112573
	private void updatePlayerInfo(PlayerSelectionInfo info)
	{
		this.playerInfo = info;
		this.updateAll();
	}

	// Token: 0x06003B2F RID: 15151 RVA: 0x00114182 File Offset: 0x00112582
	private void updateAll()
	{
		this.updateSwap();
		this.updatePlayerType();
		this.updateCharacterPortrait();
		this.updateBg();
		this.updateColors();
		this.updatePlayerNumText();
		this.updatePlayerButton();
		this.updateSkinText();
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x001141B4 File Offset: 0x001125B4
	private void updateSwap()
	{
		if (this.playerInfo.characterID == CharacterID.None)
		{
			this.currentCharacter = null;
		}
		else
		{
			CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(this.playerInfo.characterID);
			CharacterDefinition characterDefinition2 = this.currentCharacter;
			this.currentCharacter = this.characterDataHelper.GetLinkedCharacters(characterDefinition)[this.playerInfo.characterIndex];
			this.characterIndexSwapped = (characterDefinition2 != this.currentCharacter && this.characterDataHelper.LinkedCharactersContains(this.characterDataHelper.GetLinkedCharacters(characterDefinition), characterDefinition2));
			this.currentSkin = null;
			if (this.currentCharacter != null && !this.currentCharacter.isRandom)
			{
				this.currentSkin = this.characterDataHelper.GetSkinDefinition(this.playerInfo.characterID, this.playerInfo.skinKey);
			}
		}
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x0011429C File Offset: 0x0011269C
	private void updateBg()
	{
		this.BackgroundReady.SetActive(false);
		this.BackgroundUnready.SetActive(false);
		if (this.currentCharacter == null)
		{
			this.characterSelectScene.SetReadyState(this.playerNum, false);
		}
		else
		{
			this.characterSelectScene.SetReadyState(this.playerNum, true);
		}
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x001142FC File Offset: 0x001126FC
	private void updateCharacterPortrait()
	{
		if (this.currentCharacter != null)
		{
			this.setCharacterPortrait(this.currentCharacter, this.currentSkin);
		}
		else
		{
			CharacterSelectModel.HighlightInfo highlightInfo = this.model.GetHighlightInfo(this.playerNum);
			if (highlightInfo.characterID != CharacterID.None)
			{
				SkinDefinition skin = null;
				CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(highlightInfo.characterID);
				if (!characterDefinition.isRandom && highlightInfo.skinIDs.ContainsKey(highlightInfo.characterID))
				{
					string skinKey = highlightInfo.skinIDs[highlightInfo.characterID];
					skin = this.characterDataHelper.GetSkinDefinition(highlightInfo.characterID, skinKey);
				}
				this.setCharacterPortrait(characterDefinition, skin);
			}
			else
			{
				this.setCharacterPortrait(null, null);
			}
		}
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x001143C0 File Offset: 0x001127C0
	private void setCharacterPortrait(CharacterDefinition characterDef, SkinDefinition skin)
	{
		bool flag = false;
		if (characterDef == null)
		{
			this.CharacterNameText.text = string.Empty;
			if (this.CharacterNameDisabledText)
			{
				this.CharacterNameDisabledText.text = string.Empty;
			}
		}
		else if (this.playerInfo.type != PlayerType.None && !this.playerInfo.isSpectator)
		{
			if (skin == null)
			{
				skin = this.skinDataManager.GetDefaultSkin(characterDef.characterID);
			}
			string text = this.characterDataHelper.GetDisplayName(characterDef.characterID);
			bool flag2 = !this.characterUnlockModel.IsUnlockedInGameMode(characterDef.characterID, this.currentModeData.Type);
			if (flag2)
			{
				text += " <sprite=0>";
			}
			this.CharacterNameText.text = text;
			if (this.CharacterNameDisabledText)
			{
				this.CharacterNameDisabledText.text = text;
			}
			flag = true;
		}
		if (!flag)
		{
			this.clear3dDisplay();
		}
		else
		{
			float num = (float)this.characterSelectCalculator.GetMaxPlayers();
			float num2 = (1f + num) / 2f;
			float num3 = (num - 1f) / 2f;
			float positionIndex = ((float)PlayerUtil.GetIntFromPlayerNum(this.playerNum, false) - num2) / num3;
			bool useCameraLensingCompensation = !this.isOnlineBlindPick && (this.playerNum == PlayerNum.Player1 || this.playerNum == PlayerNum.Player4);
			this.characterSelectScene.SetCharacter(this.playerNum, characterDef.characterID, skin, positionIndex, this.CharacterDisplay3DV2.transform, useCameraLensingCompensation);
			this.updateBg();
			if (this.characterIndexSwapped)
			{
				CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(this.characterDataHelper.GetPrimary(characterDef));
				int index = this.characterDataHelper.LinkedCharactersindex(linkedCharacters, characterDef);
				this.characterSelectScene.ChangeFrontCharIndex(this.playerNum, index);
				List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(characterDef, CharacterDefaultAnimationKey.CHARACTER_SELECT_IDLE);
				List<WavedashAnimationData> allDefaultAnimations2 = this.characterDataHelper.GetAllDefaultAnimations(characterDef, CharacterDefaultAnimationKey.SWAP_IN_ANIMATION);
				List<UISceneCharacterAnimRequest> list = new List<UISceneCharacterAnimRequest>();
				for (int i = 0; i < allDefaultAnimations2.Count; i++)
				{
					list.Add(new UISceneCharacterAnimRequest
					{
						animData = allDefaultAnimations2[i],
						type = UISceneCharacterAnimRequest.AnimRequestType.AnimData,
						mode = UISceneCharacter.AnimationMode.TRANSITION
					});
				}
				this.characterSelectScene.SetDefaultAnimations(this.playerNum, allDefaultAnimations);
				this.characterSelectScene.PlayTransition(this.playerNum, list);
			}
		}
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x00114642 File Offset: 0x00112A42
	private void clear3dDisplay()
	{
		this.characterSelectScene.HideCharacter(this.playerNum);
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x00114658 File Offset: 0x00112A58
	private void onGameModeChanged(GameEvent message)
	{
		GameModeChangedEvent gameModeChangedEvent = message as GameModeChangedEvent;
		this.currentModeData = gameModeChangedEvent.data;
		this.updateColors();
		this.updatePlayerNumText();
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x00114684 File Offset: 0x00112A84
	public void TweenIn(float time, Vector3 target)
	{
		this.killMotionTween();
		this._motionTween = DOTween.To(() => base.transform.position, delegate(Vector3 valueIn)
		{
			base.transform.position = valueIn;
		}, target, time).SetEase(Ease.OutSine).OnComplete(delegate
		{
			this.killMotionTween();
		});
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x001146D4 File Offset: 0x00112AD4
	public void TweenOut(float time, Vector3 target, bool setActive)
	{
		this.killMotionTween();
		this._motionTween = DOTween.To(() => this.transform.position, delegate(Vector3 valueIn)
		{
			this.transform.position = valueIn;
		}, target, time).SetEase(Ease.OutSine).OnComplete(delegate
		{
			this.killMotionTween();
			this.gameObject.SetActive(setActive);
		});
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x00114737 File Offset: 0x00112B37
	private void killMotionTween()
	{
		if (this._motionTween != null && this._motionTween.IsPlaying())
		{
			this._motionTween.Kill(false);
		}
		this._motionTween = null;
	}

	// Token: 0x06003B39 RID: 15161 RVA: 0x00114768 File Offset: 0x00112B68
	private void updateColors()
	{
		if (this.currentModeData.settings.usesTeams && this.playerInfo.type != PlayerType.None && this.Team != TeamNum.None && !base.battleServerAPI.IsConnected)
		{
			this.showTeamDisplay();
			UIColor uicolorFromTeam = PlayerUtil.GetUIColorFromTeam(this.Team);
			this.TeamColorDisplay.sprite = this.TeamBgSprites.GetSprite(uicolorFromTeam);
			this.TeamText.text = this.localization.GetText("team." + uicolorFromTeam.ToString());
			this.TeamText.color = PlayerUtil.GetLightColorFromUIColor(uicolorFromTeam);
		}
		else
		{
			this.hideTeamDisplay();
		}
	}

	// Token: 0x17000E3B RID: 3643
	// (get) Token: 0x06003B3A RID: 15162 RVA: 0x0011482A File Offset: 0x00112C2A
	// (set) Token: 0x06003B3B RID: 15163 RVA: 0x00114832 File Offset: 0x00112C32
	public float TeamDisplayAlpha
	{
		get
		{
			return this._teamDisplayAlpha;
		}
		set
		{
			this._teamDisplayAlpha = value;
			this.TeamDisplay.GetComponent<CanvasGroup>().alpha = this._teamDisplayAlpha;
		}
	}

	// Token: 0x06003B3C RID: 15164 RVA: 0x00114854 File Offset: 0x00112C54
	private void showTeamDisplay()
	{
		if (!this.isTeamDisplayShown)
		{
			this.isTeamDisplayShown = true;
			this.TeamDisplay.SetActive(true);
			this.killTeamDisplayTween();
			this._teamDisplayTween = DOTween.To(new DOGetter<float>(this.get_TeamDisplayAlpha), delegate(float x)
			{
				this.TeamDisplayAlpha = x;
			}, 1f, 0.1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killTeamDisplayTween));
		}
	}

	// Token: 0x06003B3D RID: 15165 RVA: 0x001148CC File Offset: 0x00112CCC
	private void hideTeamDisplay()
	{
		if (this.isTeamDisplayShown)
		{
			this.isTeamDisplayShown = false;
			this.killTeamDisplayTween();
			this._teamDisplayTween = DOTween.To(new DOGetter<float>(this.get_TeamDisplayAlpha), delegate(float x)
			{
				this.TeamDisplayAlpha = x;
			}, 0f, 0.1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.onHideTeamDisplayComplete));
		}
	}

	// Token: 0x06003B3E RID: 15166 RVA: 0x00114935 File Offset: 0x00112D35
	private void onHideTeamDisplayComplete()
	{
		this.TeamDisplay.SetActive(false);
		this.killTeamDisplayTween();
	}

	// Token: 0x06003B3F RID: 15167 RVA: 0x00114949 File Offset: 0x00112D49
	private void killTeamDisplayTween()
	{
		if (this._teamDisplayTween != null && this._teamDisplayTween.IsPlaying())
		{
			this._teamDisplayTween.Kill(false);
		}
		this._teamDisplayTween = null;
	}

	// Token: 0x06003B40 RID: 15168 RVA: 0x00114979 File Offset: 0x00112D79
	private void onClickTeamIndicatorButton(CursorTargetButton target, PointerEventData eventData)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_teamCycle, 0f);
		base.events.Broadcast(new CyclePlayerTeamRequest(this.playerNum, 1));
	}

	// Token: 0x06003B41 RID: 15169 RVA: 0x001149A4 File Offset: 0x00112DA4
	private void onClickPlayerDiamond(CursorTargetButton target, PointerEventData eventData)
	{
		switch (this.getDiamondAction())
		{
		case DiamondAction.ADD_CPU:
			this.addCpu();
			break;
		case DiamondAction.REMOVE_CPU:
			this.removePlayer();
			break;
		case DiamondAction.REMOVE_PLAYER:
			this.removePlayer();
			break;
		case DiamondAction.OPEN_OPTIONS:
			this.openInputSettings();
			break;
		}
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x001149FF File Offset: 0x00112DFF
	private void addCpu()
	{
		base.events.Broadcast(new SetPlayerTypeRequest(this.playerNum, PlayerType.CPU, true));
		base.events.Broadcast(new SelectCharacterRequest(this.playerNum));
	}

	// Token: 0x06003B43 RID: 15171 RVA: 0x00114A2F File Offset: 0x00112E2F
	private void removePlayer()
	{
		base.events.Broadcast(new SetPlayerTypeRequest(this.playerNum, PlayerType.None, true));
		base.events.Broadcast(new SelectCharacterRequest(this.playerNum, CharacterID.None));
	}

	// Token: 0x06003B44 RID: 15172 RVA: 0x00114A60 File Offset: 0x00112E60
	private void onSelectPlayerDiamond(CursorTargetButton target)
	{
		this.updatePlayerNumText();
	}

	// Token: 0x06003B45 RID: 15173 RVA: 0x00114A68 File Offset: 0x00112E68
	private void onDeselectPlayerDiamond(CursorTargetButton target)
	{
		this.updatePlayerNumText();
	}

	// Token: 0x06003B46 RID: 15174 RVA: 0x00114A70 File Offset: 0x00112E70
	private void openInputSettings()
	{
		this.InputSettingsWindow.gameObject.SetActive(true);
	}

	// Token: 0x06003B47 RID: 15175 RVA: 0x00114A83 File Offset: 0x00112E83
	private void closeInputSettings()
	{
		this.InputSettingsWindow.gameObject.SetActive(false);
	}

	// Token: 0x06003B48 RID: 15176 RVA: 0x00114A98 File Offset: 0x00112E98
	private void onClickedPlayerButton(CursorTargetButton button, PointerEventData data)
	{
		PlayerButtonMode playerButtonMode = this.getPlayerButtonMode();
		switch (playerButtonMode)
		{
		case PlayerButtonMode.NONE:
			Debug.LogError("Should not be able to click PlayerButton.");
			break;
		case PlayerButtonMode.READY:
			if (this.ReadyClicked != null)
			{
				this.ReadyClicked();
			}
			break;
		case PlayerButtonMode.BUY_CHARACTER:
			if (this.isOnlineBlindPick)
			{
				this.unlockCharacterFlow.StartWithTimer(this.currentCharacter.characterID, new Action(this.updateAll), this.battleServerStagingAPI.PurchaseEndTime);
			}
			else
			{
				this.unlockCharacterFlow.Start(this.currentCharacter.characterID, new Action(this.updateAll));
			}
			break;
		case PlayerButtonMode.BUY_SKIN:
		{
			EquippableItem item = this.equipmentModel.GetItemFromSkinKey(this.currentSkin.uniqueKey);
			Action equipCallback = delegate()
			{
				this.equipFlow.Start(item, this.currentCharacter.characterID);
			};
			if (this.isOnlineBlindPick)
			{
				this.unlockEquipmentFlow.StartWithTimer(item, new Action(this.updateAll), equipCallback, this.battleServerStagingAPI.PurchaseEndTime);
			}
			else
			{
				this.unlockEquipmentFlow.Start(item, new Action(this.updateAll), equipCallback);
			}
			break;
		}
		}
	}

	// Token: 0x06003B49 RID: 15177 RVA: 0x00114BE7 File Offset: 0x00112FE7
	public bool OnCancelPressed()
	{
		if (this.InputSettingsWindow.gameObject.activeInHierarchy)
		{
			this.closeInputSettings();
			return true;
		}
		return false;
	}

	// Token: 0x06003B4A RID: 15178 RVA: 0x00114C07 File Offset: 0x00113007
	public void LockedIn(bool locked)
	{
		this.locked = locked;
		this.updateAll();
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x00114C18 File Offset: 0x00113018
	private DiamondAction getDiamondAction()
	{
		PlayerType type = this.Type;
		if (type == PlayerType.CPU)
		{
			return DiamondAction.REMOVE_CPU;
		}
		if (type != PlayerType.None)
		{
			return DiamondAction.REMOVE_PLAYER;
		}
		return DiamondAction.ADD_CPU;
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x00114C44 File Offset: 0x00113044
	private PlayerButtonMode getPlayerButtonMode()
	{
		if (this.currentCharacter == null)
		{
			return PlayerButtonMode.NONE;
		}
		if (!this.characterUnlockModel.IsUnlockedInGameMode(this.currentCharacter.characterID, this.currentModeData.Type))
		{
			return PlayerButtonMode.NOTOWNED_DISABLED;
		}
		if (!this.inventory.HasSkin(this.currentSkin))
		{
			return PlayerButtonMode.NOTOWNED_DISABLED;
		}
		return PlayerButtonMode.READY;
	}

	// Token: 0x04002896 RID: 10390
	public GameObject ActiveMode;

	// Token: 0x04002897 RID: 10391
	public GameObject InactiveMode;

	// Token: 0x04002898 RID: 10392
	public Image PlayerDiamond;

	// Token: 0x04002899 RID: 10393
	public CursorTargetButton PlayerDiamondButton;

	// Token: 0x0400289A RID: 10394
	public ColorSpriteContainer PlayerDiamondSprites;

	// Token: 0x0400289B RID: 10395
	public Sprite PlayerDiamondHoverSprite;

	// Token: 0x0400289C RID: 10396
	public Sprite PlayerDiamondCPURemoveSprite;

	// Token: 0x0400289D RID: 10397
	public Sprite PlayerDiamondCPUAddSprite;

	// Token: 0x0400289E RID: 10398
	public TextMeshProUGUI PlayerNumText;

	// Token: 0x0400289F RID: 10399
	public TextMeshProUGUI PlayerName;

	// Token: 0x040028A0 RID: 10400
	public Image PlayerNameArrow;

	// Token: 0x040028A1 RID: 10401
	public CursorTargetButton PlayerNameButton;

	// Token: 0x040028A2 RID: 10402
	public CursorTargetButton CharacterNameButton;

	// Token: 0x040028A3 RID: 10403
	public TextMeshProUGUI CharacterNameText;

	// Token: 0x040028A4 RID: 10404
	public TextMeshProUGUI CharacterNameDisabledText;

	// Token: 0x040028A5 RID: 10405
	public GameObject BackgroundReady;

	// Token: 0x040028A6 RID: 10406
	public GameObject BackgroundUnready;

	// Token: 0x040028A7 RID: 10407
	public GameObject DiamondArrow;

	// Token: 0x040028A8 RID: 10408
	public InputSettingsWindow InputSettingsWindow;

	// Token: 0x040028A9 RID: 10409
	public PlayerProfileWindow PlayerProfileWindow;

	// Token: 0x040028AA RID: 10410
	public CursorTargetButton CycleSkinsButton;

	// Token: 0x040028AB RID: 10411
	public UnbindText SkinText;

	// Token: 0x040028AC RID: 10412
	public GameObject CharacterDisplay3DV2;

	// Token: 0x040028AD RID: 10413
	public GameObject TeamDisplay;

	// Token: 0x040028AE RID: 10414
	public TextMeshProUGUI TeamText;

	// Token: 0x040028AF RID: 10415
	public Image TeamColorDisplay;

	// Token: 0x040028B0 RID: 10416
	public CursorTargetButton TeamButton;

	// Token: 0x040028B1 RID: 10417
	public ColorSpriteContainer TeamBgSprites;

	// Token: 0x040028B2 RID: 10418
	public Material FlashMaterial;

	// Token: 0x040028B3 RID: 10419
	public float PlayerNameDefaultWidth;

	// Token: 0x040028B4 RID: 10420
	[Space(5f)]
	public GameObject PlayerButtonObject;

	// Token: 0x040028B5 RID: 10421
	public GameObject DisabledPlayerButtonObject;

	// Token: 0x040028B6 RID: 10422
	public CursorTargetButton PlayerButton;

	// Token: 0x040028B7 RID: 10423
	public CursorTargetButton DisabledPlayerButton;

	// Token: 0x040028B8 RID: 10424
	public TextMeshProUGUI BuyCharacterText;

	// Token: 0x040028B9 RID: 10425
	public TextMeshProUGUI BuySkinText;

	// Token: 0x040028BA RID: 10426
	public TextMeshProUGUI ReadyText;

	// Token: 0x040028BB RID: 10427
	public GameObject ProAccountSignage;

	// Token: 0x040028BC RID: 10428
	public TextMeshProUGUI ProAccountBody;

	// Token: 0x040028BD RID: 10429
	public Action ReadyClicked;

	// Token: 0x040028BF RID: 10431
	private PlayerSelectionInfo playerInfo;

	// Token: 0x040028C0 RID: 10432
	private Tweener _teamDisplayTween;

	// Token: 0x040028C1 RID: 10433
	private float _teamDisplayAlpha;

	// Token: 0x040028C2 RID: 10434
	private bool isTeamDisplayShown = true;

	// Token: 0x040028C3 RID: 10435
	private Tweener _motionTween;

	// Token: 0x040028C4 RID: 10436
	private PlayerNum playerNum = PlayerNum.None;

	// Token: 0x040028C5 RID: 10437
	private UIColor color;

	// Token: 0x040028C6 RID: 10438
	private bool hasAdjustedPlayerNameArrow;

	// Token: 0x040028C7 RID: 10439
	private Vector3 playerNameArrowOriginalPosition;

	// Token: 0x040028C8 RID: 10440
	private bool characterIndexSwapped;

	// Token: 0x040028C9 RID: 10441
	private GameModeData currentModeData;

	// Token: 0x040028CA RID: 10442
	private CharacterDefinition currentCharacter;

	// Token: 0x040028CB RID: 10443
	private CharacterDefinition previousCharacter;

	// Token: 0x040028CC RID: 10444
	private SkinDefinition currentSkin;

	// Token: 0x040028CD RID: 10445
	private SkinDefinition previousSkin;

	// Token: 0x040028CE RID: 10446
	private CharacterSelectScene3D characterSelectScene;

	// Token: 0x040028CF RID: 10447
	private bool isOnlineBlindPick;

	// Token: 0x040028D0 RID: 10448
	private bool locked;

	// Token: 0x040028D1 RID: 10449
	private Action unlockDiamond;
}
