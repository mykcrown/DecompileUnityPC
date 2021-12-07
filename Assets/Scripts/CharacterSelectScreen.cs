// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectScreen : GameScreen
{
	public HorizontalLayoutGroup CharacterList;

	public GameObject TokenGroup;

	public GameObject CursorDefault;

	public GameObject TokenSpace;

	public CursorTargetButton TokenDrop;

	public HorizontalLayoutGroup LeftSideOptions;

	public HorizontalLayoutGroup RightSideOptions;

	public MoreOptionsWindow MoreOptionsWindow;

	public CursorTargetButton PlayButton;

	public Image PlayButtonImage;

	public TextMeshProUGUI PlayButtonText;

	public GameObject TokenDefaultLocation;

	public CanvasGroup OptionsBar;

	public GameObject SwipeIn1;

	public GameObject SwipeIn2;

	public GameObject SwipeIn3;

	public GameObject BackButtonStub;

	public GameObject BackButtonPrefab;

	public GameObject CharacterPortraitPrefab;

	public GameObject PlayerTokenPrefab;

	public GameObject OptionPrefab;

	public PlayerSelectionUI PlayerPortraitPrefab;

	public GameObject MoreOptionsPrefab;

	public ScreenType PreviousScreen;

	public ScreenType NextScreen;

	private CharacterSelectPlayerOrganizer playersUI;

	private GameMode currentGameMode;

	private GameRules currentGameRules;

	private List<OptionGUI> optionsLeft = new List<OptionGUI>();

	private List<OptionGUI> optionsRight = new List<OptionGUI>();

	private GameObject moreOptions;

	private float _playButtonFade;

	private Tweener playButtonTween;

	private bool isMoreOptionsOpen;

	private Tweener moreOptionsTween;

	private GameLoadPayload previousPayload;

	private CharacterSelectScene3D characterSelectScene;

	private CharacterDefinition[] characters;

	private List<CharacterSelectionPortrait> characterSelectionPortraits = new List<CharacterSelectionPortrait>();

	private static Func<UnityEngine.Object, UnityEngine.Object> __f__mg_cache0;

	private static Action<UnityEngine.Object> __f__mg_cache1;

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public CharacterSelectCalculator characterSelectCalculator
	{
		get;
		set;
	}

	[Inject]
	public CharacterSelectModel api
	{
		get;
		set;
	}

	[Inject]
	public ITokenManager tokenManager
	{
		get;
		set;
	}

	[Inject]
	public IMainOptionsCalculator optionsCalculator
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[Inject]
	public UIPreload3DAssets preload3dAssets
	{
		get;
		set;
	}

	[Inject]
	public IPlayerJoinGameController playerJoinGameController
	{
		get;
		set;
	}

	[Inject]
	public new UserAudioSettings userAudioSettings
	{
		get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	[Inject]
	public ICharacterSelectSharedFunctions characterSelectFunctions
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public IApplicationFramerateManager framerateManager
	{
		get;
		set;
	}

	private PlayerSelectionList players
	{
		get
		{
			return this.gamePayload.players;
		}
	}

	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	private GameMode rules
	{
		get
		{
			return (GameMode)this.gamePayload.battleConfig.settings[BattleSettingType.Mode];
		}
	}

	private GameModeData modeData
	{
		get
		{
			return base.gameDataManager.GameModeData.GetDataByType(this.rules);
		}
	}

	private TeamMode teamMode
	{
		get
		{
			return (!this.modeData.settings.usesTeams) ? TeamMode.None : this.modeData.settings.teamMode;
		}
	}

	private float playButtonFade
	{
		get
		{
			return this._playButtonFade;
		}
		set
		{
			this._playButtonFade = value;
			this.PlayButtonImage.color = new Color(this._playButtonFade, this._playButtonFade, this._playButtonFade);
			this.PlayButtonText.alpha = this._playButtonFade;
		}
	}

	public override void Awake()
	{
		base.Awake();
		this.api.ClearHighlightInfoSelectedSkins();
		this.characterSelectScene = base.uiAdapter.GetUIScene<CharacterSelectScene3D>();
		this.characterSelectScene.ResetAllPlayers();
		ICharacterSelectSharedFunctions arg_AB_0 = this.characterSelectFunctions;
		List<CharacterSelectionPortrait> arg_AB_1 = this.characterSelectionPortraits;
		Func<PlayerNum, IPlayerCursor> arg_AB_2 = new Func<PlayerNum, IPlayerCursor>(base.findPlayerCursor);
		Func<PlayerNum, Vector2> arg_AB_3 = new Func<PlayerNum, Vector2>(this.getCursorDefaultPosition);
		if (CharacterSelectScreen.__f__mg_cache0 == null)
		{
			CharacterSelectScreen.__f__mg_cache0 = new Func<UnityEngine.Object, UnityEngine.Object>(UnityEngine.Object.Instantiate);
		}
		Func<UnityEngine.Object, UnityEngine.Object> arg_AB_4 = CharacterSelectScreen.__f__mg_cache0;
		if (CharacterSelectScreen.__f__mg_cache1 == null)
		{
			CharacterSelectScreen.__f__mg_cache1 = new Action<UnityEngine.Object>(UnityEngine.Object.Destroy);
		}
		arg_AB_0.Init(arg_AB_1, arg_AB_2, arg_AB_3, arg_AB_4, CharacterSelectScreen.__f__mg_cache1, null, this.TokenSpace, this.TokenDrop, this.TokenDefaultLocation, this.TokenGroup, this.PlayerTokenPrefab);
		base.injector.Inject(this.MoreOptionsWindow);
		this.playersUI = base.GetComponent<CharacterSelectPlayerOrganizer>();
		base.injector.Inject(this.playersUI);
	}

	protected override void Update()
	{
		base.Update();
		this.playerJoinGameController.DoUpdate();
		PlayerToken[] all = this.tokenManager.GetAll();
		for (int i = 0; i < all.Length; i++)
		{
			PlayerToken token = all[i];
			this.updateTokenVisibility(token, true);
		}
	}

	private void updateTokenVisibility(PlayerToken token, bool instant)
	{
		if (!this.shouldDisplayToken(token))
		{
			token.SetVisible(false, instant);
		}
		else
		{
			token.SetVisible(true, instant);
		}
	}

	private bool shouldDisplayToken(PlayerToken token)
	{
		if (token.AttachToCursor != null)
		{
			if (!this.characterSelectFunctions.shouldDisplayToken(token.AttachToCursor))
			{
				return false;
			}
			if (this.isMoreOptionsOpen)
			{
				return false;
			}
			if (base.uiManager.GetWindowCount() > 0)
			{
				return false;
			}
		}
		return true;
	}

	public override void LoadPayload(Payload payload)
	{
		bool flag = this.previousPayload == null;
		base.LoadPayload(payload);
		this.previousPayload = this.gamePayload.Clone();
		this.characters = this.characterLists.GetLegalCharacters();
		if (flag)
		{
			string characterSelectionAnnouncement = this.modeData.settings.characterSelectionAnnouncement;
			if (characterSelectionAnnouncement != null && characterSelectionAnnouncement.Length > 0)
			{
				base.events.Broadcast(new PlayAnnouncementCommand(characterSelectionAnnouncement));
			}
			this.populateScreen();
			this.preload3dAssets.PreloadForScene(null);
		}
		this.UpdatePayload(payload);
	}

	public override void OnAddedToHeirarchy()
	{
		base.OnAddedToHeirarchy();
		this.MoreOptionsWindow.gameObject.SetActive(false);
		this.MoreOptionsWindow.Init();
		this.tokenManager.Init(new Func<PlayerNum, IPlayerCursor>(base.findPlayerCursor));
		this.tokenManager.Reset();
		this.TokenDrop.UseOverrideHighlightSound = true;
		this.TokenDrop.OverrideHighlightSound = AudioData.Empty;
		this.TokenDrop.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickTokenDrop);
		GameObject gameObject = base.addBackButtonForCursorScreen(this.BackButtonStub.transform, this.BackButtonPrefab);
		if (base.gameDataManager.ConfigData.uiuxSettings.DemoModeEnabled)
		{
			gameObject.SetActive(false);
		}
		this.UpdatePayload(this.payload);
		this.playerJoinGameController.Activate();
		if (this.userAudioSettings.UseAltMenuMusic())
		{
			base.audioManager.StopMusic(null, 0.5f);
		}
		else
		{
			base.audioManager.PlayMusic(SoundKey.mainMenu_music);
		}
	}

	public override void OnDestroy()
	{
		if (this.tokenManager != null)
		{
			this.tokenManager.ReleaseFunctions();
		}
		this.playerJoinGameController.Deactivate();
		base.OnDestroy();
	}

	protected override Vector2 getCursorDefaultPosition(PlayerNum playerNum)
	{
		return this.CursorDefault.transform.position;
	}

	private void onCharacterSelected(PlayerNum selected, bool unselected)
	{
		PlayerNum playerNum = PlayerNum.None;
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
			if (playerSelectionInfo.characterID == CharacterID.None && playerSelectionInfo.type != PlayerType.None && playerNum == PlayerNum.None)
			{
				playerNum = playerSelectionInfo.playerNum;
			}
		}
		if (this.gamePayload.FindPlayerInfo(selected).type == PlayerType.Human)
		{
			PlayerNum playerNum2 = playerNum;
			if (playerNum2 == PlayerNum.None)
			{
				for (int j = this.gamePayload.players.Length - 1; j >= 0; j--)
				{
					if (this.gamePayload.players[j].characterID != CharacterID.None && this.gamePayload.players[j].type == PlayerType.Human)
					{
						playerNum2 = this.gamePayload.players[j].playerNum;
					}
				}
			}
		}
	}

	public override void OnCancelPressed(IPlayerCursor cursor)
	{
		if (this.isMoreOptionsOpen)
		{
			this.closeMoreOptions(true);
		}
		else if (!this.playersUI.OnCancelPressed(cursor))
		{
			this.tokenModeCancelPressed(cursor);
		}
	}

	private void tokenModeCancelPressed(IPlayerCursor cursor)
	{
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
			if (PlayerUtil.GetIntFromPlayerNum(playerSelectionInfo.playerNum, false) == cursor.PointerId)
			{
				PlayerToken playerToken = this.tokenManager.GetPlayerToken(playerSelectionInfo.playerNum);
				PlayerToken currentlyGrabbing = this.tokenManager.GetCurrentlyGrabbing(playerSelectionInfo.playerNum);
				if (currentlyGrabbing != null && currentlyGrabbing != playerToken)
				{
					PlayerSelectionInfo playerSelectionInfo2 = this.gamePayload.FindPlayerInfo(currentlyGrabbing.PlayerNum);
					if (playerSelectionInfo2 != null && playerSelectionInfo2.type == PlayerType.CPU && playerSelectionInfo2.characterID == CharacterID.None)
					{
						base.events.Broadcast(new SelectCharacterRequest(playerSelectionInfo2.playerNum));
					}
				}
				if (playerSelectionInfo.characterID != CharacterID.None)
				{
					this.tokenManager.GrabToken(playerSelectionInfo.playerNum, playerToken, 0f);
					this.updateTokenVisibility(playerToken, true);
					base.events.Broadcast(new SelectCharacterRequest(playerSelectionInfo.playerNum, CharacterID.None));
				}
				else if (currentlyGrabbing != playerToken)
				{
					this.tokenManager.GrabToken(playerSelectionInfo.playerNum, playerToken, 0f);
					this.updateTokenVisibility(playerToken, true);
				}
				else if (this.noPlayersActive())
				{
					this.GoToPreviousScreen();
				}
				else
				{
					base.events.Broadcast(new SetPlayerTypeRequest(playerSelectionInfo.playerNum, PlayerType.None, true));
					base.events.Broadcast(new SelectCharacterRequest(playerSelectionInfo.playerNum, CharacterID.None));
				}
			}
		}
	}

	private bool noPlayersActive()
	{
		IEnumerator enumerator = ((IEnumerable)this.gamePayload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				if (playerSelectionInfo.type == PlayerType.Human)
				{
					return false;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return true;
	}

	public override void OnAltSubmitPressed(IPlayerCursor cursor)
	{
		if (this.isMoreOptionsOpen)
		{
			return;
		}
		if (!this.devConfig.rightClickAddCPU)
		{
			PlayerNum actingPlayer = this.api.GetActingPlayer(cursor.PointerId);
			for (int i = 0; i < this.gamePayload.players.Length; i++)
			{
				PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
				if (playerSelectionInfo.playerNum == actingPlayer)
				{
					this.characterSelectFunctions.onAltSubmit(playerSelectionInfo, new ShouldDisplayToken(this.shouldDisplayToken));
				}
			}
			PlayerNum playerNumFromPointer = PlayerUtil.GetPlayerNumFromPointer(cursor.PointerId);
			if (playerNumFromPointer == PlayerNum.All)
			{
				actingPlayer = this.api.GetActingPlayer(cursor.PointerId);
				PlayerToken playerToken = this.tokenManager.GetPlayerToken(actingPlayer);
				this.tokenManager.GrabToken(playerNumFromPointer, playerToken, 0f);
			}
		}
	}

	public override void OnStartPressed(IPlayerCursor cursor)
	{
		if (this.isMoreOptionsOpen)
		{
			this.MoreOptionsWindow.OnStartPressed(cursor);
		}
		else
		{
			base.events.Broadcast(new NextScreenRequest());
		}
	}

	private void onClickTokenDrop(CursorTargetButton target, PointerEventData eventData)
	{
		this.characterSelectFunctions.OnClickTokenDrop(target, eventData, new ShouldDisplayToken(this.shouldDisplayToken), this.gamePayload);
	}

	public void OnStartButton()
	{
		base.events.Broadcast(new NextScreenRequest());
	}

	public override void OnAdvance1Pressed(IPlayerCursor cursor)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
		base.events.Broadcast(new CyclePlayerSkinRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), 1));
	}

	public override void OnPrevious1Pressed(IPlayerCursor cursor)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
		base.events.Broadcast(new CyclePlayerSkinRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), -1));
	}

	public override void OnAdvance2Pressed(IPlayerCursor cursor)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_teamCycle, 0f);
		base.events.Broadcast(new CyclePlayerTeamRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), 1));
	}

	public override void OnPrevious2Pressed(IPlayerCursor cursor)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_teamCycle, 0f);
		base.events.Broadcast(new CyclePlayerTeamRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), -1));
	}

	public override void OnRightStickUpPressed(IPlayerCursor cursor)
	{
		base.events.Broadcast(new CycleCharacterIndex(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)));
	}

	public override void OnRightStickDownPressed(IPlayerCursor cursor)
	{
		base.events.Broadcast(new CycleCharacterIndex(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)));
	}

	private void populateScreen()
	{
		base.lockInput();
		this.characterSelectionPortraits.Clear();
		List<CharacterDefinition> list = this.characterSelectFunctions.SortCharacters(this.characters, this.modeData.Type);
		foreach (CharacterDefinition current in list)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CharacterPortraitPrefab);
			gameObject.transform.SetParent(this.CharacterList.transform, false);
			CharacterSelectionPortrait component = gameObject.GetComponent<CharacterSelectionPortrait>();
			base.injector.Inject(component);
			component.Init(current, this.gamePayload.players, this.modeData);
			this.characterSelectionPortraits.Add(component);
		}
		this.playersUI.Setup(this.modeData);
		this.CharacterList.Redraw();
		this.tweenIn();
	}

	private void tweenIn()
	{
		this.OptionsBar.alpha = 0f;
		base.tweenInItem(this.SwipeIn2, 0, -1, 0.25f, 0f, null);
		base.tweenInItem(this.SwipeIn3, 0, -1, 0.25f, 0f, null);
		base.tweenInItem(this.SwipeIn1, 0, 1, 0.25f, 0f, new Action(this._tweenIn_m__0));
		DOTween.To(new DOGetter<float>(this._tweenIn_m__1), new DOSetter<float>(this._tweenIn_m__2), 1f, 0.1f).SetDelay(0.25f).SetEase(Ease.OutSine);
	}

	private void onAnimationsComplete()
	{
		base.unlockInput();
	}

	public override void GoToPreviousScreen()
	{
		if (!base.gameDataManager.ConfigData.uiuxSettings.DemoModeEnabled)
		{
			base.GoToPreviousScreen();
		}
	}

	public override void GoToNextScreen()
	{
		base.events.Broadcast(new NextScreenRequest());
	}

	public override void UpdatePayload(Payload payload)
	{
		this.onUpdate();
	}

	private void onUpdate()
	{
		if (!this.isShown)
		{
			return;
		}
		GameLoadPayload gamePayload = this.gamePayload;
		GameLoadPayload gameLoadPayload = this.previousPayload;
		this.previousPayload = this.gamePayload.Clone();
		for (int i = 0; i < gamePayload.players.Length; i++)
		{
			this.processPlayerSelectionInfoDelta(gamePayload.players[i], gameLoadPayload.players[i]);
		}
		this.processGameModeInfoDelta(gamePayload.battleConfig, gameLoadPayload.battleConfig);
		this.updatePlayButton();
		foreach (CharacterSelectionPortrait current in this.characterSelectionPortraits)
		{
			current.UpdatePlayerList(gamePayload.players);
		}
		this.playersUI.UpdatePayload();
	}

	private void updatePlayButton()
	{
		bool flag = this.api.IsValidPayload(this.modeData, this.gamePayload);
		if (flag != this.PlayButton.gameObject.activeInHierarchy)
		{
			this.killPlayButtonTween();
			if (flag)
			{
				this.PlayButton.gameObject.SetActive(true);
				this.playButtonTween = DOTween.To(new DOGetter<float>(this.get_playButtonFade), new DOSetter<float>(this._updatePlayButton_m__3), 1f, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killPlayButtonTween));
			}
			else
			{
				this.PlayButton.gameObject.SetActive(false);
				this.playButtonTween = DOTween.To(new DOGetter<float>(this.get_playButtonFade), new DOSetter<float>(this._updatePlayButton_m__4), 0.5f, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killPlayButtonTween));
			}
		}
	}

	private void killPlayButtonTween()
	{
		if (this.playButtonTween != null && this.playButtonTween.IsPlaying())
		{
			this.playButtonTween.Kill(false);
		}
		this.playButtonTween = null;
	}

	private void processPlayerSelectionInfoDelta(PlayerSelectionInfo newInfo, PlayerSelectionInfo oldInfo)
	{
		bool flag = false;
		bool flag2 = false;
		if (newInfo.characterID != oldInfo.characterID)
		{
			this.onCharacterSelected(newInfo.playerNum, newInfo.characterID == CharacterID.None);
			this.characterSelectFunctions.syncTokenPosition(newInfo);
			flag |= true;
		}
		flag |= (newInfo.team != oldInfo.team);
		flag |= (newInfo.skinKey != oldInfo.skinKey);
		flag |= (newInfo.characterIndex != oldInfo.characterIndex);
		flag |= (newInfo.type != oldInfo.type);
		if (newInfo.type == PlayerType.Human && oldInfo.type != PlayerType.Human && oldInfo.type == PlayerType.CPU)
		{
			flag2 |= true;
		}
		this.characterSelectFunctions.PlayPlayerSelectionSounds(oldInfo, newInfo);
		bool isActive = this.isCursorActive(newInfo);
		bool isActive2 = newInfo.type != PlayerType.None;
		this.updateCursorState(newInfo, isActive);
		this.characterSelectFunctions.updateTokenState(newInfo, isActive2);
		if (flag)
		{
			base.events.Broadcast(new PlayerSelectionInfoChangedEvent(newInfo));
		}
		if (flag2)
		{
			base.events.Broadcast(new SelectCharacterRequest(oldInfo.playerNum, CharacterID.None));
		}
	}

	private bool isCursorActive(PlayerSelectionInfo newInfo)
	{
		return newInfo.type == PlayerType.Human;
	}

	private void updateCursorState(PlayerSelectionInfo info, bool isActive)
	{
		PlayerNum playerNum = info.playerNum;
		IPlayerCursor playerCursor = base.findPlayerCursor(playerNum);
		if (playerCursor == null && info.type == PlayerType.Human)
		{
			playerCursor = this.createCursor(playerNum);
		}
		if (isActive)
		{
			bool flag = false;
			if (playerCursor != null)
			{
				flag = base.isCursorShown(playerCursor);
				base.showCursor(playerCursor);
			}
			if (!flag)
			{
				playerCursor.ResetPosition(this.getCursorDefaultPosition(playerNum));
			}
		}
		else if (playerCursor != null)
		{
			base.hideCursor(playerCursor);
		}
	}

	private void broadcastDisabledPlayerSlots(GameMode newMode, GameMode oldMode)
	{
		GameModeData dataByType = base.gameDataManager.GameModeData.GetDataByType(oldMode);
		GameModeData dataByType2 = base.gameDataManager.GameModeData.GetDataByType(newMode);
		if (dataByType2.settings.maxPlayerCount < dataByType.settings.maxPlayerCount)
		{
			for (int i = dataByType.settings.maxPlayerCount; i > dataByType2.settings.maxPlayerCount; i--)
			{
				base.events.Broadcast(new SetPlayerTypeRequest(PlayerUtil.GetPlayerNumFromInt(i, false), PlayerType.None, false));
			}
		}
	}

	private void processGameModeInfoDelta(BattleSettings newSettings, BattleSettings oldSettings)
	{
		this.broadcastDisabledPlayerSlots(newSettings.mode, oldSettings.mode);
		this.setGameMode(newSettings.mode, newSettings.rules);
		foreach (OptionGUI current in this.optionsLeft)
		{
			current.UpdatePayload(this.gamePayload);
		}
		foreach (OptionGUI current2 in this.optionsRight)
		{
			current2.UpdatePayload(this.gamePayload);
		}
		this.MoreOptionsWindow.UpdatePayload(this.gamePayload, newSettings.mode, newSettings.rules);
	}

	private void updateOptionsGuis()
	{
		MainOptionsList leftRightOptions = this.optionsCalculator.GetLeftRightOptions(this.currentGameMode, this.currentGameRules);
		this.updateOptionSide(this.optionsLeft, leftRightOptions.LeftSide, this.LeftSideOptions);
		this.updateOptionSide(this.optionsRight, leftRightOptions.RightSide, this.RightSideOptions);
	}

	private void updateOptionSide(List<OptionGUI> existing, List<OptionDescription> newList, HorizontalLayoutGroup theParent)
	{
		if (!this.isEquivalentOptions(existing, newList))
		{
			foreach (OptionGUI current in existing)
			{
				UnityEngine.Object.Destroy(current.gameObject);
			}
			existing.Clear();
			bool flag = theParent == this.RightSideOptions;
			if (flag && this.moreOptions != null)
			{
				this.moreOptions.transform.SetParent(null, false);
			}
			foreach (OptionDescription current2 in newList)
			{
				OptionGUI component = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab).GetComponent<OptionGUI>();
				base.injector.Inject(component);
				component.LoadFromDesc(current2);
				component.UpdatePayload(this.gamePayload);
				existing.Add(component);
				component.transform.SetParent(theParent.transform, false);
			}
			if (flag)
			{
				this.addMoreOptionsButton();
			}
		}
	}

	private void addMoreOptionsButton()
	{
		if (this.moreOptions == null)
		{
			this.moreOptions = UnityEngine.Object.Instantiate<GameObject>(this.MoreOptionsPrefab);
			CursorTargetButton componentInChildren = this.moreOptions.GetComponentInChildren<CursorTargetButton>();
			componentInChildren.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onMoreOptions);
		}
		this.moreOptions.transform.SetParent(this.RightSideOptions.transform, false);
	}

	private void onMoreOptions(CursorTargetButton target, PointerEventData eventData)
	{
		this.openMoreOptions();
		this.MoreOptionsWindow.OnCloseRequest = new Action<bool>(this.closeMoreOptions);
	}

	private void openMoreOptions()
	{
		if (!this.isMoreOptionsOpen)
		{
			this.isMoreOptionsOpen = true;
			this.MoreOptionsWindow.gameObject.SetActive(true);
			this.MoreOptionsWindow.OnOpened();
			this.killMoreOptionsTween();
			this.MoreOptionsWindow.Alpha = 0f;
			this.moreOptionsTween = DOTween.To(new DOGetter<float>(this._openMoreOptions_m__5), new DOSetter<float>(this._openMoreOptions_m__6), 1f, 0.1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killMoreOptionsTween));
			base.events.Broadcast(new MoreOptionsWasOpened());
		}
	}

	private void closeMoreOptions(bool revertChanges)
	{
		if (this.isMoreOptionsOpen)
		{
			this.isMoreOptionsOpen = false;
			this.MoreOptionsWindow.OnClosed();
			this.killMoreOptionsTween();
			this.moreOptionsTween = DOTween.To(new DOGetter<float>(this._closeMoreOptions_m__7), new DOSetter<float>(this._closeMoreOptions_m__8), 0f, 0.1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.moreOptionsFadeOutComplete));
			base.events.Broadcast(new MoreOptionsWasClosed(revertChanges));
		}
	}

	private void moreOptionsFadeOutComplete()
	{
		this.MoreOptionsWindow.gameObject.SetActive(false);
		this.killMoreOptionsTween();
	}

	private void killMoreOptionsTween()
	{
		if (this.moreOptionsTween != null && this.moreOptionsTween.IsPlaying())
		{
			this.moreOptionsTween.Kill(false);
		}
		this.moreOptionsTween = null;
	}

	private bool isEquivalentOptions(List<OptionGUI> existing, List<OptionDescription> newList)
	{
		if (existing.Count != newList.Count)
		{
			return false;
		}
		for (int i = existing.Count - 1; i >= 0; i--)
		{
			if (existing[i].Desc != newList[i])
			{
				return false;
			}
		}
		return true;
	}

	private void setGameMode(GameMode gameMode, GameRules gameRules)
	{
		if (gameRules != this.currentGameRules || gameMode != this.currentGameMode)
		{
			this.currentGameRules = gameRules;
			this.currentGameMode = gameMode;
			this.updateOptionsGuis();
			base.events.Broadcast(new GameModeChangedEvent(this.rules, this.modeData));
		}
	}

	private void _tweenIn_m__0()
	{
		this.onAnimationsComplete();
	}

	private float _tweenIn_m__1()
	{
		return this.OptionsBar.alpha;
	}

	private void _tweenIn_m__2(float valueIn)
	{
		this.OptionsBar.alpha = valueIn;
	}

	private void _updatePlayButton_m__3(float valueIn)
	{
		this.playButtonFade = valueIn;
	}

	private void _updatePlayButton_m__4(float valueIn)
	{
		this.playButtonFade = valueIn;
	}

	private float _openMoreOptions_m__5()
	{
		return this.MoreOptionsWindow.Alpha;
	}

	private void _openMoreOptions_m__6(float valueIn)
	{
		this.MoreOptionsWindow.Alpha = valueIn;
	}

	private float _closeMoreOptions_m__7()
	{
		return this.MoreOptionsWindow.Alpha;
	}

	private void _closeMoreOptions_m__8(float valueIn)
	{
		this.MoreOptionsWindow.Alpha = valueIn;
	}
}
