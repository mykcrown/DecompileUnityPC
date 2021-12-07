using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009B4 RID: 2484
public class OnlineBlindPickScreen : GameScreen
{
	// Token: 0x17001038 RID: 4152
	// (get) Token: 0x060044AD RID: 17581 RVA: 0x0012E128 File Offset: 0x0012C528
	// (set) Token: 0x060044AE RID: 17582 RVA: 0x0012E130 File Offset: 0x0012C530
	[Inject]
	public IOnlineBlindPickScreenAPI api { get; set; }

	// Token: 0x17001039 RID: 4153
	// (get) Token: 0x060044AF RID: 17583 RVA: 0x0012E139 File Offset: 0x0012C539
	// (set) Token: 0x060044B0 RID: 17584 RVA: 0x0012E141 File Offset: 0x0012C541
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x1700103A RID: 4154
	// (get) Token: 0x060044B1 RID: 17585 RVA: 0x0012E14A File Offset: 0x0012C54A
	// (set) Token: 0x060044B2 RID: 17586 RVA: 0x0012E152 File Offset: 0x0012C552
	[Inject]
	public UIPreload3DAssets preload3dAssets { get; set; }

	// Token: 0x1700103B RID: 4155
	// (get) Token: 0x060044B3 RID: 17587 RVA: 0x0012E15B File Offset: 0x0012C55B
	// (set) Token: 0x060044B4 RID: 17588 RVA: 0x0012E163 File Offset: 0x0012C563
	[Inject]
	public IBattleServerStagingAPI battleServerStagingAPI { get; set; }

	// Token: 0x1700103C RID: 4156
	// (get) Token: 0x060044B5 RID: 17589 RVA: 0x0012E16C File Offset: 0x0012C56C
	// (set) Token: 0x060044B6 RID: 17590 RVA: 0x0012E174 File Offset: 0x0012C574
	[Inject]
	public ITokenManager tokenManager { get; set; }

	// Token: 0x1700103D RID: 4157
	// (get) Token: 0x060044B7 RID: 17591 RVA: 0x0012E17D File Offset: 0x0012C57D
	// (set) Token: 0x060044B8 RID: 17592 RVA: 0x0012E185 File Offset: 0x0012C585
	[Inject]
	public IUIAdapter adapter { get; set; }

	// Token: 0x1700103E RID: 4158
	// (get) Token: 0x060044B9 RID: 17593 RVA: 0x0012E18E File Offset: 0x0012C58E
	// (set) Token: 0x060044BA RID: 17594 RVA: 0x0012E196 File Offset: 0x0012C596
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x1700103F RID: 4159
	// (get) Token: 0x060044BB RID: 17595 RVA: 0x0012E19F File Offset: 0x0012C59F
	// (set) Token: 0x060044BC RID: 17596 RVA: 0x0012E1A7 File Offset: 0x0012C5A7
	[Inject]
	public IPlayerJoinGameController playerJoinGameController { get; set; }

	// Token: 0x17001040 RID: 4160
	// (get) Token: 0x060044BD RID: 17597 RVA: 0x0012E1B0 File Offset: 0x0012C5B0
	// (set) Token: 0x060044BE RID: 17598 RVA: 0x0012E1B8 File Offset: 0x0012C5B8
	[Inject]
	public ICharacterSelectSharedFunctions characterSelectFunctions { get; set; }

	// Token: 0x17001041 RID: 4161
	// (get) Token: 0x060044BF RID: 17599 RVA: 0x0012E1C1 File Offset: 0x0012C5C1
	// (set) Token: 0x060044C0 RID: 17600 RVA: 0x0012E1C9 File Offset: 0x0012C5C9
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x17001042 RID: 4162
	// (get) Token: 0x060044C1 RID: 17601 RVA: 0x0012E1D2 File Offset: 0x0012C5D2
	// (set) Token: 0x060044C2 RID: 17602 RVA: 0x0012E1DA File Offset: 0x0012C5DA
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17001043 RID: 4163
	// (get) Token: 0x060044C3 RID: 17603 RVA: 0x0012E1E3 File Offset: 0x0012C5E3
	// (set) Token: 0x060044C4 RID: 17604 RVA: 0x0012E1EB File Offset: 0x0012C5EB
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x17001044 RID: 4164
	// (get) Token: 0x060044C5 RID: 17605 RVA: 0x0012E1F4 File Offset: 0x0012C5F4
	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	// Token: 0x17001045 RID: 4165
	// (get) Token: 0x060044C6 RID: 17606 RVA: 0x0012E201 File Offset: 0x0012C601
	private GameMode rules
	{
		get
		{
			return (GameMode)this.gamePayload.battleConfig.settings[BattleSettingType.Mode];
		}
	}

	// Token: 0x17001046 RID: 4166
	// (get) Token: 0x060044C7 RID: 17607 RVA: 0x0012E219 File Offset: 0x0012C619
	private GameModeData modeData
	{
		get
		{
			return base.gameDataManager.GameModeData.GetDataByType(this.rules);
		}
	}

	// Token: 0x060044C8 RID: 17608 RVA: 0x0012E234 File Offset: 0x0012C634
	public override void Awake()
	{
		base.Awake();
		this.playersUI = base.GetComponent<OnlineBlindPickPlayerOrganizer>();
		base.injector.Inject(this.playersUI);
		ICharacterSelectSharedFunctions characterSelectFunctions = this.characterSelectFunctions;
		List<CharacterSelectionPortrait> list = this.characterSelectionPortraits;
		Func<PlayerNum, IPlayerCursor> findPlayerCursor = new Func<PlayerNum, IPlayerCursor>(base.findPlayerCursor);
		Func<PlayerNum, Vector2> getCursorDefaultPosition = new Func<PlayerNum, Vector2>(this.getCursorDefaultPosition);
		if (OnlineBlindPickScreen.f__mg_cache0 == null)
		{
			OnlineBlindPickScreen.f__mg_cache0 = new Func<UnityEngine.Object, UnityEngine.Object>(UnityEngine.Object.Instantiate);
		}
		Func<UnityEngine.Object, UnityEngine.Object> instantiate = OnlineBlindPickScreen.f__mg_cache0;
		if (OnlineBlindPickScreen.f__mg_cache1 == null)
		{
			OnlineBlindPickScreen.f__mg_cache1 = new Action<UnityEngine.Object>(UnityEngine.Object.Destroy);
		}
		characterSelectFunctions.Init(list, findPlayerCursor, getCursorDefaultPosition, instantiate, OnlineBlindPickScreen.f__mg_cache1, new ShouldTokenClickCallback(this.shouldTokenClick), this.TokenSpace, this.TokenDrop, this.TokenDefaultLocation, this.TokenGroup, this.PlayerTokenPrefab);
	}

	// Token: 0x060044C9 RID: 17609 RVA: 0x0012E2F4 File Offset: 0x0012C6F4
	public override void OnAddedToHeirarchy()
	{
		base.OnAddedToHeirarchy();
		this.tokenManager.Init(new Func<PlayerNum, IPlayerCursor>(base.findPlayerCursor));
		this.tokenManager.Reset();
		this.TokenDrop.UseOverrideHighlightSound = true;
		this.TokenDrop.OverrideHighlightSound = AudioData.Empty;
		this.TokenDrop.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClickTokenDrop);
		foreach (PlayerNum playerNum in base.battleServerAPI.LocalPlayerNumIDs)
		{
			this.createCursor(playerNum);
		}
		this.backButton = base.addBackButtonForCursorScreen(this.BackButtonAnchor, this.BackButtonPrefab).GetComponentInChildren<CursorTargetButton>();
		this.backButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
		this.backButton.DisableDuration = 0.075f;
		this.ParticipantMode.SetActive(!this.api.IsSpectator);
		this.SpectatorMode.SetActive(this.api.IsSpectator);
	}

	// Token: 0x060044CA RID: 17610 RVA: 0x0012E41C File Offset: 0x0012C81C
	protected override void Update()
	{
		base.Update();
		this.playerJoinGameController.DoUpdate();
		this.updateTimerDisplay(this.selectionTimerDecimalReduce);
		foreach (PlayerToken token in this.tokenManager.GetAll())
		{
			this.updateTokenVisibility(token, true);
		}
	}

	// Token: 0x060044CB RID: 17611 RVA: 0x0012E472 File Offset: 0x0012C872
	private bool shouldTokenClick()
	{
		return !this.api.LockedIn;
	}

	// Token: 0x060044CC RID: 17612 RVA: 0x0012E484 File Offset: 0x0012C884
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

	// Token: 0x060044CD RID: 17613 RVA: 0x0012E4BC File Offset: 0x0012C8BC
	private void releaseToken(PlayerSelectionInfo playerInfo)
	{
		PlayerToken playerToken = this.tokenManager.GetPlayerToken(playerInfo.playerNum);
		this.tokenManager.GrabToken(playerInfo.playerNum, playerToken, 0f);
		this.updateTokenVisibility(playerToken, true);
	}

	// Token: 0x060044CE RID: 17614 RVA: 0x0012E4FC File Offset: 0x0012C8FC
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

	// Token: 0x060044CF RID: 17615 RVA: 0x0012E57C File Offset: 0x0012C97C
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

	// Token: 0x060044D0 RID: 17616 RVA: 0x0012E637 File Offset: 0x0012CA37
	private void onClickTokenDrop(CursorTargetButton target, PointerEventData eventData)
	{
		this.characterSelectFunctions.OnClickTokenDrop(target, eventData, new ShouldDisplayToken(this.shouldDisplayToken), this.gamePayload);
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x0012E658 File Offset: 0x0012CA58
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

	// Token: 0x060044D2 RID: 17618 RVA: 0x0012E67B File Offset: 0x0012CA7B
	private bool shouldDisplayToken(PlayerToken token)
	{
		return token.AttachToCursor == null || this.characterSelectFunctions.shouldDisplayToken(token.AttachToCursor);
	}

	// Token: 0x060044D3 RID: 17619 RVA: 0x0012E6A1 File Offset: 0x0012CAA1
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

	// Token: 0x060044D4 RID: 17620 RVA: 0x0012E6D0 File Offset: 0x0012CAD0
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

	// Token: 0x060044D5 RID: 17621 RVA: 0x0012E834 File Offset: 0x0012CC34
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

	// Token: 0x060044D6 RID: 17622 RVA: 0x0012EA24 File Offset: 0x0012CE24
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

	// Token: 0x060044D7 RID: 17623 RVA: 0x0012EB08 File Offset: 0x0012CF08
	private void populateScreen()
	{
		this.selectionEndTime = this.battleServerStagingAPI.SelectionEndTime;
		string text = string.Empty;
		this.gamePayload.battleConfig.Load(this.GetInitialSettings());
		LobbyPlayerData lobbyPlayerData = null;
		foreach (KeyValuePair<ulong, LobbyPlayerData> keyValuePair in this.api.GetLobbyPlayers())
		{
			if (keyValuePair.Key == this.api.MyUserID)
			{
				lobbyPlayerData = keyValuePair.Value;
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
			foreach (LobbyPlayerData lobbyPlayerData2 in this.api.GetLobbyPlayers().Values)
			{
				bool flag = false;
				if (!this.api.IsTeams)
				{
					if (lobbyPlayerData2 != lobbyPlayerData)
					{
						flag = true;
					}
				}
				else if (lobbyPlayerData2.team != lobbyPlayerData.team)
				{
					flag = true;
				}
				if (flag)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += ", ";
					}
					text += lobbyPlayerData2.name;
				}
			}
			this.PlayingVsText.text = base.localization.GetText("ui.onlineBlindPick.playingVs", text);
		}
		base.lockInput();
		base.tweenInItem(this.TweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.SpectatorTweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.TweenInLeft, -1, 0, 0.25f, 0f, delegate
		{
			base.unlockInput();
		});
	}

	// Token: 0x060044D8 RID: 17624 RVA: 0x0012ED20 File Offset: 0x0012D120
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

	// Token: 0x060044D9 RID: 17625 RVA: 0x0012EE64 File Offset: 0x0012D264
	public override void OnAdvance1Pressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
		base.events.Broadcast(new CyclePlayerSkinRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), 1));
	}

	// Token: 0x060044DA RID: 17626 RVA: 0x0012EEB1 File Offset: 0x0012D2B1
	public override void OnRightStickUpPressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		base.events.Broadcast(new CycleCharacterIndex(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)));
	}

	// Token: 0x060044DB RID: 17627 RVA: 0x0012EEE0 File Offset: 0x0012D2E0
	public override void OnRightStickDownPressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		base.events.Broadcast(new CycleCharacterIndex(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)));
	}

	// Token: 0x060044DC RID: 17628 RVA: 0x0012EF10 File Offset: 0x0012D310
	public override void OnPrevious1Pressed(IPlayerCursor cursor)
	{
		if (this.api.LockedIn)
		{
			return;
		}
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
		base.events.Broadcast(new CyclePlayerSkinRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), -1));
	}

	// Token: 0x060044DD RID: 17629 RVA: 0x0012EF5D File Offset: 0x0012D35D
	public override void OnStartPressed(IPlayerCursor cursor)
	{
		this.GoToNextScreen();
	}

	// Token: 0x060044DE RID: 17630 RVA: 0x0012EF68 File Offset: 0x0012D368
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

	// Token: 0x060044DF RID: 17631 RVA: 0x0012EFDC File Offset: 0x0012D3DC
	public override void GoToNextScreen()
	{
		this.api.LockInSelection();
	}

	// Token: 0x060044E0 RID: 17632 RVA: 0x0012EFEC File Offset: 0x0012D3EC
	protected override void postStartAndPayload()
	{
		base.postStartAndPayload();
		this.api.OnScreenShown();
		base.listen(OnlineBlindPickScreenAPI.UPDATED, new Action(this.onUpdate));
		base.signalBus.GetSignal<SetPlayerDeviceSignal>().AddListener(new Action<int, IInputDevice>(this.onSetPlayerDevice));
		this.onUpdate();
	}

	// Token: 0x060044E1 RID: 17633 RVA: 0x0012F043 File Offset: 0x0012D443
	private void onSetPlayerDevice(int portId, IInputDevice device)
	{
		this.onUpdate();
	}

	// Token: 0x060044E2 RID: 17634 RVA: 0x0012F04B File Offset: 0x0012D44B
	public override void OnDestroy()
	{
		this.playerJoinGameController.Deactivate();
		base.signalBus.GetSignal<SetPlayerDeviceSignal>().RemoveListener(new Action<int, IInputDevice>(this.onSetPlayerDevice));
		this.api.OnScreenDestroyed();
		base.OnDestroy();
	}

	// Token: 0x060044E3 RID: 17635 RVA: 0x0012F088 File Offset: 0x0012D488
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

	// Token: 0x060044E4 RID: 17636 RVA: 0x0012F248 File Offset: 0x0012D648
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

	// Token: 0x060044E5 RID: 17637 RVA: 0x0012F2F3 File Offset: 0x0012D6F3
	private void checkForLockedInEvent()
	{
		if (this.api.LockedIn != this.wasLockedIn)
		{
			this.wasLockedIn = this.api.LockedIn;
			this.playersUI.LockedIn(this.wasLockedIn);
		}
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x0012F330 File Offset: 0x0012D730
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

	// Token: 0x060044E7 RID: 17639 RVA: 0x0012F389 File Offset: 0x0012D789
	private void updateBackButton()
	{
		this.backButton.SetInteractable(!this.api.LockedIn);
	}

	// Token: 0x060044E8 RID: 17640 RVA: 0x0012F3A4 File Offset: 0x0012D7A4
	private bool portraitCanInteract()
	{
		return !this.api.LockedIn;
	}

	// Token: 0x04002DBF RID: 11711
	public GameObject BackButtonPrefab;

	// Token: 0x04002DC0 RID: 11712
	public Transform BackButtonAnchor;

	// Token: 0x04002DC1 RID: 11713
	public GameObject ParticipantMode;

	// Token: 0x04002DC2 RID: 11714
	public GameObject SpectatorMode;

	// Token: 0x04002DC3 RID: 11715
	public TextMeshProUGUI SelectFighterText;

	// Token: 0x04002DC4 RID: 11716
	public TextMeshProUGUI PlayingVsText;

	// Token: 0x04002DC5 RID: 11717
	public TextMeshProUGUI TimerText;

	// Token: 0x04002DC6 RID: 11718
	private bool didFirstLoad;

	// Token: 0x04002DC7 RID: 11719
	private float selectionEndTime;

	// Token: 0x04002DC8 RID: 11720
	private int selectionTimerDecimalReduce = 22;

	// Token: 0x04002DC9 RID: 11721
	public GameObject CharacterPortraitPrefab;

	// Token: 0x04002DCA RID: 11722
	public int CharacterTopRowCount;

	// Token: 0x04002DCB RID: 11723
	public HorizontalLayoutGroup CharacterListTop;

	// Token: 0x04002DCC RID: 11724
	public HorizontalLayoutGroup CharacterListBottom;

	// Token: 0x04002DCD RID: 11725
	private List<CharacterSelectionPortrait> characterSelectionPortraits = new List<CharacterSelectionPortrait>();

	// Token: 0x04002DCE RID: 11726
	private CharacterDefinition[] characters;

	// Token: 0x04002DCF RID: 11727
	private OnlineBlindPickPlayerOrganizer playersUI;

	// Token: 0x04002DD0 RID: 11728
	public GameObject PlayerTokenPrefab;

	// Token: 0x04002DD1 RID: 11729
	public GameObject TokenGroup;

	// Token: 0x04002DD2 RID: 11730
	public GameObject TokenSpace;

	// Token: 0x04002DD3 RID: 11731
	public CursorTargetButton TokenDrop;

	// Token: 0x04002DD4 RID: 11732
	public GameObject TokenDefaultLocation;

	// Token: 0x04002DD5 RID: 11733
	private bool wasLockedIn;

	// Token: 0x04002DD6 RID: 11734
	public TextMeshProUGUI stageText;

	// Token: 0x04002DD7 RID: 11735
	public GameObject StageItemPrefab;

	// Token: 0x04002DD8 RID: 11736
	public GridLayoutGroup StageLayoutGroup;

	// Token: 0x04002DD9 RID: 11737
	private List<StageData> stages;

	// Token: 0x04002DDA RID: 11738
	private CursorTargetButton backButton;

	// Token: 0x04002DDB RID: 11739
	public GameObject TweenInLeft;

	// Token: 0x04002DDC RID: 11740
	public GameObject TweenInRight;

	// Token: 0x04002DDD RID: 11741
	public GameObject SpectatorTweenInRight;

	// Token: 0x04002DDE RID: 11742
	private GameLoadPayload previousPayload;

	// Token: 0x04002DDF RID: 11743
	[CompilerGenerated]
	private static Func<UnityEngine.Object, UnityEngine.Object> f__mg_cache0;

	// Token: 0x04002DE0 RID: 11744
	[CompilerGenerated]
	private static Action<UnityEngine.Object> f__mg_cache1;
}
