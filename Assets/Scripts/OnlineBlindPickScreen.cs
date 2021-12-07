// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnlineBlindPickScreen : GameScreen
{
	public GameObject BackButtonPrefab;

	public Transform BackButtonAnchor;

	public GameObject ParticipantMode;

	public GameObject SpectatorMode;

	public TextMeshProUGUI SelectFighterText;

	public TextMeshProUGUI PlayingVsText;

	public TextMeshProUGUI TimerText;

	private bool didFirstLoad;

	private float selectionEndTime;

	private int selectionTimerDecimalReduce = 22;

	public GameObject CharacterPortraitPrefab;

	public int CharacterTopRowCount;

	public HorizontalLayoutGroup CharacterListTop;

	public HorizontalLayoutGroup CharacterListBottom;

	private List<CharacterSelectionPortrait> characterSelectionPortraits = new List<CharacterSelectionPortrait>();

	private CharacterDefinition[] characters;

	private OnlineBlindPickPlayerOrganizer playersUI;

	public GameObject PlayerTokenPrefab;

	public GameObject TokenGroup;

	public GameObject TokenSpace;

	public CursorTargetButton TokenDrop;

	public GameObject TokenDefaultLocation;

	private bool wasLockedIn;

	public TextMeshProUGUI stageText;

	public GameObject StageItemPrefab;

	public GridLayoutGroup StageLayoutGroup;

	private List<StageData> stages;

	private CursorTargetButton backButton;

	public GameObject TweenInLeft;

	public GameObject TweenInRight;

	public GameObject SpectatorTweenInRight;

	private GameLoadPayload previousPayload;

	private static Func<UnityEngine.Object, UnityEngine.Object> __f__mg_cache0;

	private static Action<UnityEngine.Object> __f__mg_cache1;

	[Inject]
	public IOnlineBlindPickScreenAPI api
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
	public IBattleServerStagingAPI battleServerStagingAPI
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
	public IUIAdapter adapter
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
	public IPlayerJoinGameController playerJoinGameController
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
	public IEnterNewGame enterNewGame
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

	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
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

	public override void Awake()
	{
		base.Awake();
		this.playersUI = base.GetComponent<OnlineBlindPickPlayerOrganizer>();
		base.injector.Inject(this.playersUI);
		ICharacterSelectSharedFunctions arg_AC_0 = this.characterSelectFunctions;
		List<CharacterSelectionPortrait> arg_AC_1 = this.characterSelectionPortraits;
		Func<PlayerNum, IPlayerCursor> arg_AC_2 = new Func<PlayerNum, IPlayerCursor>(base.findPlayerCursor);
		Func<PlayerNum, Vector2> arg_AC_3 = new Func<PlayerNum, Vector2>(this.getCursorDefaultPosition);
		if (OnlineBlindPickScreen.__f__mg_cache0 == null)
		{
			OnlineBlindPickScreen.__f__mg_cache0 = new Func<UnityEngine.Object, UnityEngine.Object>(UnityEngine.Object.Instantiate);
		}
		Func<UnityEngine.Object, UnityEngine.Object> arg_AC_4 = OnlineBlindPickScreen.__f__mg_cache0;
		if (OnlineBlindPickScreen.__f__mg_cache1 == null)
		{
			OnlineBlindPickScreen.__f__mg_cache1 = new Action<UnityEngine.Object>(UnityEngine.Object.Destroy);
		}
		arg_AC_0.Init(arg_AC_1, arg_AC_2, arg_AC_3, arg_AC_4, OnlineBlindPickScreen.__f__mg_cache1, new ShouldTokenClickCallback(this.shouldTokenClick), this.TokenSpace, this.TokenDrop, this.TokenDefaultLocation, this.TokenGroup, this.PlayerTokenPrefab);
	}

	public override void OnAddedToHeirarchy()
	{
		base.OnAddedToHeirarchy();
		this.tokenManager.Init(new Func<PlayerNum, IPlayerCursor>(base.findPlayerCursor));
		this.tokenManager.Reset();
		this.TokenDrop.UseOverrideHighlightSound = true;
		this.TokenDrop.OverrideHighlightSound = AudioData.Empty;
		this.TokenDrop.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickTokenDrop);
		foreach (PlayerNum current in base.battleServerAPI.LocalPlayerNumIDs)
		{
			this.createCursor(current);
		}
		this.backButton = base.addBackButtonForCursorScreen(this.BackButtonAnchor, this.BackButtonPrefab).GetComponentInChildren<CursorTargetButton>();
		this.backButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
		this.backButton.DisableDuration = 0.075f;
		this.ParticipantMode.SetActive(!this.api.IsSpectator);
		this.SpectatorMode.SetActive(this.api.IsSpectator);
	}

	protected override void Update()
	{
		base.Update();
		this.playerJoinGameController.DoUpdate();
		this.updateTimerDisplay(this.selectionTimerDecimalReduce);
		PlayerToken[] all = this.tokenManager.GetAll();
		for (int i = 0; i < all.Length; i++)
		{
			PlayerToken token = all[i];
			this.updateTokenVisibility(token, true);
		}
	}

	private bool shouldTokenClick()
	{
		return !this.api.LockedIn;
	}

	public override void OnCancelPressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		if (!this.playersUI.OnCancelPressed(cursor))
		{
			this.tokenModeCancelPressed(cursor);
		}
	}

	private void releaseToken(PlayerSelectionInfo playerInfo)
	{
		PlayerToken playerToken = this.tokenManager.GetPlayerToken(playerInfo.playerNum);
		this.tokenManager.GrabToken(playerInfo.playerNum, playerToken, 0f);
		this.updateTokenVisibility(playerToken, true);
	}

	private void tokenModeCancelPressed(IPlayerCursor cursor)
	{
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
			if (PlayerUtil.GetIntFromPlayerNum(playerSelectionInfo.playerNum, false) == cursor.PointerId && playerSelectionInfo.characterID != CharacterID.None)
			{
				this.releaseToken(playerSelectionInfo);
				base.events.Broadcast(new SelectCharacterRequest(playerSelectionInfo.playerNum, CharacterID.None));
			}
		}
	}

	public override void OnAltSubmitPressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
			if (playerSelectionInfo.playerNum == base.battleServerAPI.GetPrimaryLocalPlayer)
			{
				this.characterSelectFunctions.onAltSubmit(playerSelectionInfo, new ShouldDisplayToken(this.shouldDisplayToken));
			}
		}
		PlayerNum playerNumFromPointer = PlayerUtil.GetPlayerNumFromPointer(cursor.PointerId);
		if (playerNumFromPointer == PlayerNum.All)
		{
			PlayerToken playerToken = this.tokenManager.GetPlayerToken(base.battleServerAPI.GetPrimaryLocalPlayer);
			this.tokenManager.GrabToken(playerNumFromPointer, playerToken, 0f);
		}
	}

	private void onClickTokenDrop(CursorTargetButton target, PointerEventData eventData)
	{
		this.characterSelectFunctions.OnClickTokenDrop(target, eventData, new ShouldDisplayToken(this.shouldDisplayToken), this.gamePayload);
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
		return token.AttachToCursor == null || this.characterSelectFunctions.shouldDisplayToken(token.AttachToCursor);
	}

	public override void LoadPayload(Payload payload)
	{
		base.LoadPayload(payload);
		if (!this.didFirstLoad)
		{
			this.didFirstLoad = true;
			this.populateScreen();
			this.preload3dAssets.PreloadForScene(null);
		}
	}

	private void populateCharacterSelect()
	{
		this.characters = this.characterLists.GetLegalCharacters();
		this.characterSelectionPortraits.Clear();
		List<CharacterDefinition> list = this.characterSelectFunctions.SortCharacters(this.characters, this.modeData.Type);
		for (int i = 0; i < list.Count; i++)
		{
			CharacterDefinition characterDef = list[i];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CharacterPortraitPrefab);
			if (i < this.CharacterTopRowCount)
			{
				gameObject.transform.SetParent(this.CharacterListTop.transform, false);
			}
			else
			{
				gameObject.transform.SetParent(this.CharacterListBottom.transform, false);
			}
			CharacterSelectionPortrait component = gameObject.GetComponent<CharacterSelectionPortrait>();
			base.injector.Inject(component);
			component.Init(characterDef, this.gamePayload.players, this.modeData);
			component.canInteractCallback = new CharacterSelectionPortrait.CanInteractCallback(this.portraitCanInteract);
			this.characterSelectionPortraits.Add(component);
		}
		if (this.api.IsSpectator)
		{
			this.playersUI.Setup(this.modeData, this.api.GetLobbyPlayers());
		}
		else
		{
			this.playersUI.Setup(this.modeData, base.battleServerAPI.LocalPlayerNumIDs);
		}
		this.CharacterListTop.Redraw();
		this.CharacterListBottom.Redraw();
	}

	private void populateStages()
	{
		if (this.battleServerStagingAPI.NumMatches > 1)
		{
			this.stageText.text = base.localization.GetText("ui.onlineBlindPick.stagetitle", "<font=\"LeagueSpartan-Bold SDF\"><size=26>" + base.localization.GetText("ui.onlineBlindPick.NofN", (this.battleServerStagingAPI.CurrentMatchIndex + 1).ToString(), this.battleServerStagingAPI.NumMatches.ToString()) + "</size></font>");
		}
		else
		{
			this.stageText.text = base.localization.GetText("ui.onlineBlindPick.stagetitle", string.Empty);
		}
		this.stages = base.gameDataManager.StageData.GetDataByIDs(this.battleServerStagingAPI.Stages);
		for (int i = 0; i < this.stages.Count; i++)
		{
			StageData stageData = this.stages[i];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.StageItemPrefab);
			gameObject.transform.SetParent(this.StageLayoutGroup.transform, false);
			OnlineBlindPickStageDisplay componentInChildren = gameObject.GetComponentInChildren<OnlineBlindPickStageDisplay>();
			base.injector.Inject(componentInChildren);
			componentInChildren.Init(stageData);
			componentInChildren.WithoutWorldIconTitle.text = "<align=\"center\">" + componentInChildren.WithoutWorldIconTitle.text + "</align>";
			componentInChildren.WithWorldIconTitle.text = "<align=\"center\">" + componentInChildren.WithoutWorldIconTitle.text + "</align>";
			if (i == this.battleServerStagingAPI.CurrentMatchIndex)
			{
				this.gamePayload.stage = stageData.stageID;
				this.gamePayload.isConfirmed = true;
				componentInChildren.SetMode(OnlineBlindPickStageDisplayMode.Current);
			}
			else if (i < this.battleServerStagingAPI.CurrentMatchIndex)
			{
				componentInChildren.SetMode(OnlineBlindPickStageDisplayMode.Played);
			}
			else
			{
				componentInChildren.SetMode(OnlineBlindPickStageDisplayMode.Upcoming);
			}
		}
	}

	private Dictionary<BattleSettingType, int> GetInitialSettings()
	{
		return new Dictionary<BattleSettingType, int>
		{
			{
				BattleSettingType.Lives,
				this.battleServerStagingAPI.NumLives
			},
			{
				BattleSettingType.Time,
				this.battleServerStagingAPI.MatchTime
			},
			{
				BattleSettingType.Mode,
				(int)this.battleServerStagingAPI.MatchGameMode
			},
			{
				BattleSettingType.Rules,
				(int)this.battleServerStagingAPI.MatchRules
			},
			{
				BattleSettingType.Pause,
				1
			},
			{
				BattleSettingType.Teams,
				0
			},
			{
				BattleSettingType.TeamAttack,
				(!this.battleServerStagingAPI.TeamAttack) ? 0 : 1
			},
			{
				BattleSettingType.CrewBattle_Lives,
				this.battleServerStagingAPI.NumLives
			},
			{
				BattleSettingType.CrewBattle_Time,
				this.battleServerStagingAPI.MatchTime
			},
			{
				BattleSettingType.CrewBattle_TeamAttack,
				(!this.battleServerStagingAPI.TeamAttack) ? 0 : 1
			},
			{
				BattleSettingType.Assists,
				this.battleServerStagingAPI.AssistCount
			}
		};
	}

	private void populateScreen()
	{
		this.selectionEndTime = this.battleServerStagingAPI.SelectionEndTime;
		string text = string.Empty;
		this.gamePayload.battleConfig.Load(this.GetInitialSettings());
		LobbyPlayerData lobbyPlayerData = null;
		foreach (KeyValuePair<ulong, LobbyPlayerData> current in this.api.GetLobbyPlayers())
		{
			if (current.Key == this.api.MyUserID)
			{
				lobbyPlayerData = current.Value;
				break;
			}
		}
		this.populateCharacterSelect();
		this.populateStages();
		if (this.api.IsSpectator)
		{
			this.PlayingVsText.text = base.localization.GetText("ui.onlineBlindPick.waiting");
		}
		else
		{
			foreach (LobbyPlayerData current2 in this.api.GetLobbyPlayers().Values)
			{
				bool flag = false;
				if (!this.api.IsTeams)
				{
					if (current2 != lobbyPlayerData)
					{
						flag = true;
					}
				}
				else if (current2.team != lobbyPlayerData.team)
				{
					flag = true;
				}
				if (flag)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += ", ";
					}
					text += current2.name;
				}
			}
			this.PlayingVsText.text = base.localization.GetText("ui.onlineBlindPick.playingVs", text);
		}
		base.lockInput();
		base.tweenInItem(this.TweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.SpectatorTweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.TweenInLeft, -1, 0, 0.25f, 0f, new Action(this._populateScreen_m__0));
	}

	private void updateTimerDisplay(int decimalSizeReduce)
	{
		float num = Math.Max(0f, this.selectionEndTime - Time.realtimeSinceStartup);
		StringBuilder stringBuilder = new StringBuilder();
		if (num > 5f)
		{
			this.TimerText.color = Color.white;
			TimeUtil.FormatTime((float)((int)Math.Floor((double)num)), stringBuilder, 0, true);
			this.TimerText.text = stringBuilder.ToString();
		}
		else
		{
			float num2 = 1f - num % 1f;
			this.TimerText.color = new Color(1f, num2, num2);
			TimeUtil.FormatTime(num, stringBuilder, -decimalSizeReduce, true);
			this.TimerText.text = stringBuilder.ToString();
			if (num == 0f && !this.api.LockedIn)
			{
				for (int i = 0; i < this.gamePayload.players.Length; i++)
				{
					PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
					if (base.battleServerAPI.IsLocalPlayer(playerSelectionInfo.playerNum) && playerSelectionInfo.characterID == CharacterID.None)
					{
						base.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientAFKSelection));
						return;
					}
				}
				this.onUpdate();
				this.api.LockInSelection();
			}
		}
	}

	public override void OnAdvance1Pressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
		base.events.Broadcast(new CyclePlayerSkinRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), 1));
	}

	public override void OnRightStickUpPressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		base.events.Broadcast(new CycleCharacterIndex(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)));
	}

	public override void OnRightStickDownPressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		base.events.Broadcast(new CycleCharacterIndex(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)));
	}

	public override void OnPrevious1Pressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
		base.events.Broadcast(new CyclePlayerSkinRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), -1));
	}

	public override void OnStartPressed(IPlayerCursor cursor)
	{
		this.GoToNextScreen();
	}

	public override void GoToPreviousScreen()
	{
		if (!this.api.LockedIn)
		{
			this.api.LeaveRoom();
		}
		else
		{
			base.dialogController.ShowOneButtonDialog(base.localization.GetText("dialog.OnlineGame.LockedIn.title"), base.localization.GetText("dialog.OnlineGame.LockedIn.body"), base.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
	}

	public override void GoToNextScreen()
	{
		this.api.LockInSelection();
	}

	protected override void postStartAndPayload()
	{
		base.postStartAndPayload();
		this.api.OnScreenShown();
		base.listen(OnlineBlindPickScreenAPI.UPDATED, new Action(this.onUpdate));
		base.signalBus.GetSignal<SetPlayerDeviceSignal>().AddListener(new Action<int, IInputDevice>(this.onSetPlayerDevice));
		this.onUpdate();
	}

	private void onSetPlayerDevice(int portId, IInputDevice device)
	{
		this.onUpdate();
	}

	public override void OnDestroy()
	{
		this.playerJoinGameController.Deactivate();
		base.signalBus.GetSignal<SetPlayerDeviceSignal>().RemoveListener(new Action<int, IInputDevice>(this.onSetPlayerDevice));
		this.api.OnScreenDestroyed();
		base.OnDestroy();
	}

	private void onUpdate()
	{
		if (this.api.IsSpectator)
		{
			this.SelectFighterText.text = base.localization.GetText("ui.onlineBlindPick.spectating");
		}
		else
		{
			this.SelectFighterText.text = base.localization.GetText("ui.onlineBlindPick.selectFighter");
		}
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			base.events.Broadcast(new PlayerSelectionInfoChangedEvent(this.gamePayload.players[i]));
		}
		GameLoadPayload gamePayload = this.gamePayload;
		GameLoadPayload gameLoadPayload = this.previousPayload;
		this.previousPayload = this.gamePayload.Clone();
		for (int j = 0; j < gamePayload.players.Length; j++)
		{
			PlayerSelectionInfo oldInfo = (gameLoadPayload == null) ? null : gameLoadPayload.players[j];
			PlayerSelectionInfo newInfo = (gamePayload == null) ? null : gamePayload.players[j];
			this.characterSelectFunctions.PlayPlayerSelectionSounds(oldInfo, newInfo);
		}
		this.updateTokens();
		this.updateBackButton();
		this.updateCursorState();
		for (int k = 0; k < this.characterSelectionPortraits.Count; k++)
		{
			this.characterSelectionPortraits[k].UpdatePlayerList(this.gamePayload.players);
		}
		this.playersUI.UpdatePayload();
		this.playerJoinGameController.Activate();
		if (this.api.CanStartGame)
		{
			base.events.Broadcast(new LoadScreenCommand(ScreenType.LoadingBattle, this.payload, ScreenUpdateType.Next));
			this.richPresence.SetPresence("Loading", null, null, null);
		}
		this.checkForLockedInEvent();
	}

	private void updateCursorState()
	{
		for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.All; playerNum++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.FindPlayerInfo(playerNum);
			if (playerSelectionInfo != null)
			{
				IPlayerCursor playerCursor = base.findPlayerCursor(playerSelectionInfo.playerNum);
				if (playerCursor != null)
				{
					PlayerInputPort portWithPlayerNum = this.userInputManager.GetPortWithPlayerNum(playerSelectionInfo.playerNum);
					bool flag = portWithPlayerNum != null && portWithPlayerNum.Device != null;
					if (flag)
					{
						if (playerCursor.IsHidden)
						{
							base.showCursor(playerCursor);
							playerCursor.ResetPosition(this.getCursorDefaultPosition(playerSelectionInfo.playerNum));
						}
					}
					else
					{
						base.hideCursor(playerCursor);
					}
				}
			}
		}
	}

	private void checkForLockedInEvent()
	{
		if (this.api.LockedIn != this.wasLockedIn)
		{
			this.wasLockedIn = this.api.LockedIn;
			this.playersUI.LockedIn(this.wasLockedIn);
		}
	}

	private void updateTokens()
	{
		for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.All; playerNum++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.FindPlayerInfo(playerNum);
			if (playerSelectionInfo != null)
			{
				this.characterSelectFunctions.updateTokenState(playerSelectionInfo, playerSelectionInfo.type != PlayerType.None && base.battleServerAPI.IsLocalPlayer(playerNum));
			}
		}
	}

	private void updateBackButton()
	{
		this.backButton.SetInteractable(!this.api.LockedIn);
	}

	private bool portraitCanInteract()
	{
		return !this.api.LockedIn;
	}

	private void _populateScreen_m__0()
	{
		base.unlockInput();
	}
}
