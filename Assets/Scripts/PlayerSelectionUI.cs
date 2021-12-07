// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelectionUI : ClientBehavior
{
	private sealed class _TweenOut_c__AnonStorey0
	{
		internal bool setActive;

		internal PlayerSelectionUI _this;

		internal Vector3 __m__0()
		{
			return this._this.transform.position;
		}

		internal void __m__1(Vector3 valueIn)
		{
			this._this.transform.position = valueIn;
		}

		internal void __m__2()
		{
			this._this.killMotionTween();
			this._this.gameObject.SetActive(this.setActive);
		}
	}

	private sealed class _onClickedPlayerButton_c__AnonStorey1
	{
		internal EquippableItem item;

		internal PlayerSelectionUI _this;

		internal void __m__0()
		{
			this._this.equipFlow.Start(this.item, this._this.currentCharacter.characterID);
		}
	}

	public GameObject ActiveMode;

	public GameObject InactiveMode;

	public Image PlayerDiamond;

	public CursorTargetButton PlayerDiamondButton;

	public ColorSpriteContainer PlayerDiamondSprites;

	public Sprite PlayerDiamondHoverSprite;

	public Sprite PlayerDiamondCPURemoveSprite;

	public Sprite PlayerDiamondCPUAddSprite;

	public TextMeshProUGUI PlayerNumText;

	public TextMeshProUGUI PlayerName;

	public Image PlayerNameArrow;

	public CursorTargetButton PlayerNameButton;

	public CursorTargetButton CharacterNameButton;

	public TextMeshProUGUI CharacterNameText;

	public TextMeshProUGUI CharacterNameDisabledText;

	public GameObject BackgroundReady;

	public GameObject BackgroundUnready;

	public GameObject DiamondArrow;

	public InputSettingsWindow InputSettingsWindow;

	public PlayerProfileWindow PlayerProfileWindow;

	public CursorTargetButton CycleSkinsButton;

	public UnbindText SkinText;

	public GameObject CharacterDisplay3DV2;

	public GameObject TeamDisplay;

	public TextMeshProUGUI TeamText;

	public Image TeamColorDisplay;

	public CursorTargetButton TeamButton;

	public ColorSpriteContainer TeamBgSprites;

	public Material FlashMaterial;

	public float PlayerNameDefaultWidth;

	[Space(5f)]
	public GameObject PlayerButtonObject;

	public GameObject DisabledPlayerButtonObject;

	public CursorTargetButton PlayerButton;

	public CursorTargetButton DisabledPlayerButton;

	public TextMeshProUGUI BuyCharacterText;

	public TextMeshProUGUI BuySkinText;

	public TextMeshProUGUI ReadyText;

	public GameObject ProAccountSignage;

	public TextMeshProUGUI ProAccountBody;

	public Action ReadyClicked;

	private PlayerSelectionInfo playerInfo;

	private Tweener _teamDisplayTween;

	private float _teamDisplayAlpha;

	private bool isTeamDisplayShown = true;

	private Tweener _motionTween;

	private PlayerNum playerNum = PlayerNum.None;

	private UIColor color;

	private bool hasAdjustedPlayerNameArrow;

	private Vector3 playerNameArrowOriginalPosition;

	private bool characterIndexSwapped;

	private GameModeData currentModeData;

	private CharacterDefinition currentCharacter;

	private CharacterDefinition previousCharacter;

	private SkinDefinition currentSkin;

	private SkinDefinition previousSkin;

	private CharacterSelectScene3D characterSelectScene;

	private bool isOnlineBlindPick;

	private bool locked;

	private Action unlockDiamond;

	[Inject]
	public ILocalization localization
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterSelectModel model
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		private get;
		set;
	}

	[Inject]
	public IUITextHelper textHelper
	{
		private get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		private get;
		set;
	}

	[Inject]
	public ISettingsTabsModel settingsTabModel
	{
		private get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		private get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel
	{
		private get;
		set;
	}

	[Inject]
	public IUnlockCharacter unlockCharacter
	{
		private get;
		set;
	}

	[Inject]
	public IUnlockProAccount unlockProAccount
	{
		private get;
		set;
	}

	[Inject]
	public IUserInventory inventory
	{
		private get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		private get;
		set;
	}

	[Inject]
	public IUnlockCharacterFlow unlockCharacterFlow
	{
		private get;
		set;
	}

	[Inject]
	public IUnlockEquipmentFlow unlockEquipmentFlow
	{
		private get;
		set;
	}

	[Inject]
	public IEquipFlow equipFlow
	{
		private get;
		set;
	}

	[Inject]
	public IDetailedUnlockCharacterFlow unlockFlow
	{
		private get;
		set;
	}

	[Inject]
	public IBattleServerStagingAPI battleServerStagingAPI
	{
		private get;
		set;
	}

	[Inject]
	public CharacterSelectCalculator characterSelectCalculator
	{
		private get;
		set;
	}

	[Inject]
	public GameEnvironmentData environment
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	public PlayerType Type
	{
		get
		{
			return (this.playerInfo == null) ? PlayerType.None : this.playerInfo.type;
		}
	}

	public PlayerNum PlayerNum
	{
		get
		{
			return (this.playerInfo == null) ? PlayerNum.None : this.playerInfo.playerNum;
		}
	}

	public TeamNum Team
	{
		get
		{
			return (this.playerInfo == null) ? TeamNum.None : this.playerInfo.team;
		}
	}

	public bool IsDisplayed
	{
		get;
		set;
	}

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

	public override void Awake()
	{
		base.Awake();
		this.unlockDiamond = new Action(this._Awake_m__0);
		base.events.Subscribe(typeof(HighlightCharacterEvent), new Events.EventHandler(this.onCharacterHighlighted));
		base.events.Subscribe(typeof(PlayerSelectionInfoChangedEvent), new Events.EventHandler(this.onPlayerSelectionInfoChanged));
		base.events.Subscribe(typeof(GameModeChangedEvent), new Events.EventHandler(this.onGameModeChanged));
		this.InputSettingsWindow.OnCloseRequest = new Action(this.closeInputSettings);
		this.PlayerProfileWindow.OnCloseRequest = new Action(this.closePlayerProfile);
	}

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

	protected override void removeAllListeners()
	{
		if (this.PlayerDiamondButton != null)
		{
			base.signalBus.GetSignal<PlayerCursorNotIdleSignal>().RemoveListener(new Action<PlayerNum>(this.onNotIdle));
			base.signalBus.GetSignal<PlayerCursorStatusSignal>().RemoveListener(new Action<PlayerCursor>(this.onCursorUpdate));
		}
		base.removeAllListeners();
	}

	private void Update()
	{
		this.updatePlayerButton();
	}

	public void UpdatePayload()
	{
		this.updateAll();
	}

	public override void OnDestroy()
	{
		this.clear3dDisplay();
		base.OnDestroy();
		base.events.Unsubscribe(typeof(HighlightCharacterEvent), new Events.EventHandler(this.onCharacterHighlighted));
		base.events.Unsubscribe(typeof(PlayerSelectionInfoChangedEvent), new Events.EventHandler(this.onPlayerSelectionInfoChanged));
		base.events.Unsubscribe(typeof(GameModeChangedEvent), new Events.EventHandler(this.onGameModeChanged));
	}

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

	private void openPlayerProfile()
	{
		this.PlayerProfileWindow.gameObject.SetActive(true);
		this.PlayerProfileWindow.OnOpened();
	}

	private void closePlayerProfile()
	{
		this.PlayerProfileWindow.gameObject.SetActive(false);
	}

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

	private void onCursorUpdate(PlayerCursor cursor)
	{
		if (!cursor.IsInteracting)
		{
			this.PlayerDiamondButton.RemovePlayerSelect(cursor.Player);
		}
	}

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

	private void unlockDiamondFn()
	{
		this.timer.CancelTimeout(this.unlockDiamond);
		this.PlayerDiamondButton.DisableAuthorization();
	}

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

	private void showProAccountSign()
	{
		string priceString = this.unlockProAccount.GetPriceString();
		this.ProAccountBody.text = this.localization.GetText("ui.store.characters.proAccount.body", priceString);
		this.ProAccountSignage.gameObject.SetActive(true);
	}

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

	private void updateSkinText()
	{
		if (!(this.previousCharacter == this.currentCharacter) || !(this.previousCharacter != null) || !(this.previousSkin != this.currentSkin) || this.previousSkin != null)
		{
		}
		this.previousCharacter = this.currentCharacter;
		this.previousSkin = this.currentSkin;
	}

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

	private CharacterID getCurrentCharacterId()
	{
		if (this.currentCharacter != null)
		{
			return this.currentCharacter.characterID;
		}
		return CharacterID.None;
	}

	private void onPlayerSelectionInfoChanged(GameEvent message)
	{
		PlayerSelectionInfoChangedEvent playerSelectionInfoChangedEvent = message as PlayerSelectionInfoChangedEvent;
		PlayerSelectionInfo info = playerSelectionInfoChangedEvent.info;
		if (info.playerNum == this.playerNum)
		{
			this.updatePlayerInfo(info);
		}
	}

	private void updatePlayerInfo(PlayerSelectionInfo info)
	{
		this.playerInfo = info;
		this.updateAll();
	}

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

	private void clear3dDisplay()
	{
		this.characterSelectScene.HideCharacter(this.playerNum);
	}

	private void onGameModeChanged(GameEvent message)
	{
		GameModeChangedEvent gameModeChangedEvent = message as GameModeChangedEvent;
		this.currentModeData = gameModeChangedEvent.data;
		this.updateColors();
		this.updatePlayerNumText();
	}

	public void TweenIn(float time, Vector3 target)
	{
		this.killMotionTween();
		this._motionTween = DOTween.To(new DOGetter<Vector3>(this._TweenIn_m__1), new DOSetter<Vector3>(this._TweenIn_m__2), target, time).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this._TweenIn_m__3));
	}

	public void TweenOut(float time, Vector3 target, bool setActive)
	{
		PlayerSelectionUI._TweenOut_c__AnonStorey0 _TweenOut_c__AnonStorey = new PlayerSelectionUI._TweenOut_c__AnonStorey0();
		_TweenOut_c__AnonStorey.setActive = setActive;
		_TweenOut_c__AnonStorey._this = this;
		this.killMotionTween();
		this._motionTween = DOTween.To(new DOGetter<Vector3>(_TweenOut_c__AnonStorey.__m__0), new DOSetter<Vector3>(_TweenOut_c__AnonStorey.__m__1), target, time).SetEase(Ease.OutSine).OnComplete(new TweenCallback(_TweenOut_c__AnonStorey.__m__2));
	}

	private void killMotionTween()
	{
		if (this._motionTween != null && this._motionTween.IsPlaying())
		{
			this._motionTween.Kill(false);
		}
		this._motionTween = null;
	}

	private void updateColors()
	{
		if (this.currentModeData.settings.usesTeams && this.playerInfo.type != PlayerType.None && this.Team != TeamNum.None && !base.battleServerAPI.IsConnected)
		{
			this.showTeamDisplay();
			UIColor uIColorFromTeam = PlayerUtil.GetUIColorFromTeam(this.Team);
			this.TeamColorDisplay.sprite = this.TeamBgSprites.GetSprite(uIColorFromTeam);
			this.TeamText.text = this.localization.GetText("team." + uIColorFromTeam.ToString());
			this.TeamText.color = PlayerUtil.GetLightColorFromUIColor(uIColorFromTeam);
		}
		else
		{
			this.hideTeamDisplay();
		}
	}

	private void showTeamDisplay()
	{
		if (!this.isTeamDisplayShown)
		{
			this.isTeamDisplayShown = true;
			this.TeamDisplay.SetActive(true);
			this.killTeamDisplayTween();
			this._teamDisplayTween = DOTween.To(new DOGetter<float>(this.get_TeamDisplayAlpha), new DOSetter<float>(this._showTeamDisplay_m__4), 1f, 0.1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killTeamDisplayTween));
		}
	}

	private void hideTeamDisplay()
	{
		if (this.isTeamDisplayShown)
		{
			this.isTeamDisplayShown = false;
			this.killTeamDisplayTween();
			this._teamDisplayTween = DOTween.To(new DOGetter<float>(this.get_TeamDisplayAlpha), new DOSetter<float>(this._hideTeamDisplay_m__5), 0f, 0.1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.onHideTeamDisplayComplete));
		}
	}

	private void onHideTeamDisplayComplete()
	{
		this.TeamDisplay.SetActive(false);
		this.killTeamDisplayTween();
	}

	private void killTeamDisplayTween()
	{
		if (this._teamDisplayTween != null && this._teamDisplayTween.IsPlaying())
		{
			this._teamDisplayTween.Kill(false);
		}
		this._teamDisplayTween = null;
	}

	private void onClickTeamIndicatorButton(CursorTargetButton target, PointerEventData eventData)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_teamCycle, 0f);
		base.events.Broadcast(new CyclePlayerTeamRequest(this.playerNum, 1));
	}

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

	private void addCpu()
	{
		base.events.Broadcast(new SetPlayerTypeRequest(this.playerNum, PlayerType.CPU, true));
		base.events.Broadcast(new SelectCharacterRequest(this.playerNum));
	}

	private void removePlayer()
	{
		base.events.Broadcast(new SetPlayerTypeRequest(this.playerNum, PlayerType.None, true));
		base.events.Broadcast(new SelectCharacterRequest(this.playerNum, CharacterID.None));
	}

	private void onSelectPlayerDiamond(CursorTargetButton target)
	{
		this.updatePlayerNumText();
	}

	private void onDeselectPlayerDiamond(CursorTargetButton target)
	{
		this.updatePlayerNumText();
	}

	private void openInputSettings()
	{
		this.InputSettingsWindow.gameObject.SetActive(true);
	}

	private void closeInputSettings()
	{
		this.InputSettingsWindow.gameObject.SetActive(false);
	}

	private void onClickedPlayerButton(CursorTargetButton button, PointerEventData data)
	{
		PlayerButtonMode playerButtonMode = this.getPlayerButtonMode();
		PlayerSelectionUI._onClickedPlayerButton_c__AnonStorey1 _onClickedPlayerButton_c__AnonStorey = new PlayerSelectionUI._onClickedPlayerButton_c__AnonStorey1();
		_onClickedPlayerButton_c__AnonStorey._this = this;
		switch (playerButtonMode)
		{
		case PlayerButtonMode.NONE:
			UnityEngine.Debug.LogError("Should not be able to click PlayerButton.");
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
			_onClickedPlayerButton_c__AnonStorey.item = this.equipmentModel.GetItemFromSkinKey(this.currentSkin.uniqueKey);
			Action equipCallback = new Action(_onClickedPlayerButton_c__AnonStorey.__m__0);
			if (this.isOnlineBlindPick)
			{
				this.unlockEquipmentFlow.StartWithTimer(_onClickedPlayerButton_c__AnonStorey.item, new Action(this.updateAll), equipCallback, this.battleServerStagingAPI.PurchaseEndTime);
			}
			else
			{
				this.unlockEquipmentFlow.Start(_onClickedPlayerButton_c__AnonStorey.item, new Action(this.updateAll), equipCallback);
			}
			break;
		}
		}
	}

	public bool OnCancelPressed()
	{
		if (this.InputSettingsWindow.gameObject.activeInHierarchy)
		{
			this.closeInputSettings();
			return true;
		}
		return false;
	}

	public void LockedIn(bool locked)
	{
		this.locked = locked;
		this.updateAll();
	}

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

	private void _Awake_m__0()
	{
		this.unlockDiamondFn();
	}

	private Vector3 _TweenIn_m__1()
	{
		return base.transform.position;
	}

	private void _TweenIn_m__2(Vector3 valueIn)
	{
		base.transform.position = valueIn;
	}

	private void _TweenIn_m__3()
	{
		this.killMotionTween();
	}

	private void _showTeamDisplay_m__4(float x)
	{
		this.TeamDisplayAlpha = x;
	}

	private void _hideTeamDisplay_m__5(float x)
	{
		this.TeamDisplayAlpha = x;
	}
}
