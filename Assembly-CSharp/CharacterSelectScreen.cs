using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020008DE RID: 2270
public class CharacterSelectScreen : GameScreen
{
	// Token: 0x17000DDC RID: 3548
	// (get) Token: 0x06003995 RID: 14741 RVA: 0x0010DD0D File Offset: 0x0010C10D
	// (set) Token: 0x06003996 RID: 14742 RVA: 0x0010DD15 File Offset: 0x0010C115
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000DDD RID: 3549
	// (get) Token: 0x06003997 RID: 14743 RVA: 0x0010DD1E File Offset: 0x0010C11E
	// (set) Token: 0x06003998 RID: 14744 RVA: 0x0010DD26 File Offset: 0x0010C126
	[Inject]
	public CharacterSelectCalculator characterSelectCalculator { get; set; }

	// Token: 0x17000DDE RID: 3550
	// (get) Token: 0x06003999 RID: 14745 RVA: 0x0010DD2F File Offset: 0x0010C12F
	// (set) Token: 0x0600399A RID: 14746 RVA: 0x0010DD37 File Offset: 0x0010C137
	[Inject]
	public CharacterSelectModel api { get; set; }

	// Token: 0x17000DDF RID: 3551
	// (get) Token: 0x0600399B RID: 14747 RVA: 0x0010DD40 File Offset: 0x0010C140
	// (set) Token: 0x0600399C RID: 14748 RVA: 0x0010DD48 File Offset: 0x0010C148
	[Inject]
	public ITokenManager tokenManager { get; set; }

	// Token: 0x17000DE0 RID: 3552
	// (get) Token: 0x0600399D RID: 14749 RVA: 0x0010DD51 File Offset: 0x0010C151
	// (set) Token: 0x0600399E RID: 14750 RVA: 0x0010DD59 File Offset: 0x0010C159
	[Inject]
	public IMainOptionsCalculator optionsCalculator { get; set; }

	// Token: 0x17000DE1 RID: 3553
	// (get) Token: 0x0600399F RID: 14751 RVA: 0x0010DD62 File Offset: 0x0010C162
	// (set) Token: 0x060039A0 RID: 14752 RVA: 0x0010DD6A File Offset: 0x0010C16A
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000DE2 RID: 3554
	// (get) Token: 0x060039A1 RID: 14753 RVA: 0x0010DD73 File Offset: 0x0010C173
	// (set) Token: 0x060039A2 RID: 14754 RVA: 0x0010DD7B File Offset: 0x0010C17B
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x17000DE3 RID: 3555
	// (get) Token: 0x060039A3 RID: 14755 RVA: 0x0010DD84 File Offset: 0x0010C184
	// (set) Token: 0x060039A4 RID: 14756 RVA: 0x0010DD8C File Offset: 0x0010C18C
	[Inject]
	public UIPreload3DAssets preload3dAssets { get; set; }

	// Token: 0x17000DE4 RID: 3556
	// (get) Token: 0x060039A5 RID: 14757 RVA: 0x0010DD95 File Offset: 0x0010C195
	// (set) Token: 0x060039A6 RID: 14758 RVA: 0x0010DD9D File Offset: 0x0010C19D
	[Inject]
	public IPlayerJoinGameController playerJoinGameController { get; set; }

	// Token: 0x17000DE5 RID: 3557
	// (get) Token: 0x060039A7 RID: 14759 RVA: 0x0010DDA6 File Offset: 0x0010C1A6
	// (set) Token: 0x060039A8 RID: 14760 RVA: 0x0010DDAE File Offset: 0x0010C1AE
	[Inject]
	public new UserAudioSettings userAudioSettings { get; set; }

	// Token: 0x17000DE6 RID: 3558
	// (get) Token: 0x060039A9 RID: 14761 RVA: 0x0010DDB7 File Offset: 0x0010C1B7
	// (set) Token: 0x060039AA RID: 14762 RVA: 0x0010DDBF File Offset: 0x0010C1BF
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17000DE7 RID: 3559
	// (get) Token: 0x060039AB RID: 14763 RVA: 0x0010DDC8 File Offset: 0x0010C1C8
	// (set) Token: 0x060039AC RID: 14764 RVA: 0x0010DDD0 File Offset: 0x0010C1D0
	[Inject]
	public ICharacterSelectSharedFunctions characterSelectFunctions { get; set; }

	// Token: 0x17000DE8 RID: 3560
	// (get) Token: 0x060039AD RID: 14765 RVA: 0x0010DDD9 File Offset: 0x0010C1D9
	// (set) Token: 0x060039AE RID: 14766 RVA: 0x0010DDE1 File Offset: 0x0010C1E1
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x17000DE9 RID: 3561
	// (get) Token: 0x060039AF RID: 14767 RVA: 0x0010DDEA File Offset: 0x0010C1EA
	// (set) Token: 0x060039B0 RID: 14768 RVA: 0x0010DDF2 File Offset: 0x0010C1F2
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x17000DEA RID: 3562
	// (get) Token: 0x060039B1 RID: 14769 RVA: 0x0010DDFB File Offset: 0x0010C1FB
	// (set) Token: 0x060039B2 RID: 14770 RVA: 0x0010DE03 File Offset: 0x0010C203
	[Inject]
	public IApplicationFramerateManager framerateManager { get; set; }

	// Token: 0x17000DEB RID: 3563
	// (get) Token: 0x060039B3 RID: 14771 RVA: 0x0010DE0C File Offset: 0x0010C20C
	private PlayerSelectionList players
	{
		get
		{
			return this.gamePayload.players;
		}
	}

	// Token: 0x17000DEC RID: 3564
	// (get) Token: 0x060039B4 RID: 14772 RVA: 0x0010DE19 File Offset: 0x0010C219
	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	// Token: 0x17000DED RID: 3565
	// (get) Token: 0x060039B5 RID: 14773 RVA: 0x0010DE26 File Offset: 0x0010C226
	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000DEE RID: 3566
	// (get) Token: 0x060039B6 RID: 14774 RVA: 0x0010DE29 File Offset: 0x0010C229
	private GameMode rules
	{
		get
		{
			return (GameMode)this.gamePayload.battleConfig.settings[BattleSettingType.Mode];
		}
	}

	// Token: 0x17000DEF RID: 3567
	// (get) Token: 0x060039B7 RID: 14775 RVA: 0x0010DE41 File Offset: 0x0010C241
	private GameModeData modeData
	{
		get
		{
			return base.gameDataManager.GameModeData.GetDataByType(this.rules);
		}
	}

	// Token: 0x17000DF0 RID: 3568
	// (get) Token: 0x060039B8 RID: 14776 RVA: 0x0010DE59 File Offset: 0x0010C259
	private TeamMode teamMode
	{
		get
		{
			return (!this.modeData.settings.usesTeams) ? TeamMode.None : this.modeData.settings.teamMode;
		}
	}

	// Token: 0x060039B9 RID: 14777 RVA: 0x0010DE88 File Offset: 0x0010C288
	public override void Awake()
	{
		base.Awake();
		this.api.ClearHighlightInfoSelectedSkins();
		this.characterSelectScene = base.uiAdapter.GetUIScene<CharacterSelectScene3D>();
		this.characterSelectScene.ResetAllPlayers();
		ICharacterSelectSharedFunctions characterSelectFunctions = this.characterSelectFunctions;
		List<CharacterSelectionPortrait> list = this.characterSelectionPortraits;
		Func<PlayerNum, IPlayerCursor> findPlayerCursor = new Func<PlayerNum, IPlayerCursor>(base.findPlayerCursor);
		Func<PlayerNum, Vector2> getCursorDefaultPosition = new Func<PlayerNum, Vector2>(this.getCursorDefaultPosition);
		if (CharacterSelectScreen.<>f__mg$cache0 == null)
		{
			CharacterSelectScreen.<>f__mg$cache0 = new Func<UnityEngine.Object, UnityEngine.Object>(UnityEngine.Object.Instantiate);
		}
		Func<UnityEngine.Object, UnityEngine.Object> instantiate = CharacterSelectScreen.<>f__mg$cache0;
		if (CharacterSelectScreen.<>f__mg$cache1 == null)
		{
			CharacterSelectScreen.<>f__mg$cache1 = new Action<UnityEngine.Object>(UnityEngine.Object.Destroy);
		}
		characterSelectFunctions.Init(list, findPlayerCursor, getCursorDefaultPosition, instantiate, CharacterSelectScreen.<>f__mg$cache1, null, this.TokenSpace, this.TokenDrop, this.TokenDefaultLocation, this.TokenGroup, this.PlayerTokenPrefab);
		base.injector.Inject(this.MoreOptionsWindow);
		this.playersUI = base.GetComponent<CharacterSelectPlayerOrganizer>();
		base.injector.Inject(this.playersUI);
	}

	// Token: 0x060039BA RID: 14778 RVA: 0x0010DF74 File Offset: 0x0010C374
	protected override void Update()
	{
		base.Update();
		this.playerJoinGameController.DoUpdate();
		foreach (PlayerToken token in this.tokenManager.GetAll())
		{
			this.updateTokenVisibility(token, true);
		}
	}

	// Token: 0x060039BB RID: 14779 RVA: 0x0010DFBE File Offset: 0x0010C3BE
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

	// Token: 0x060039BC RID: 14780 RVA: 0x0010DFE4 File Offset: 0x0010C3E4
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

	// Token: 0x060039BD RID: 14781 RVA: 0x0010E038 File Offset: 0x0010C438
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

	// Token: 0x060039BE RID: 14782 RVA: 0x0010E0CC File Offset: 0x0010C4CC
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

	// Token: 0x060039BF RID: 14783 RVA: 0x0010E1D3 File Offset: 0x0010C5D3
	public override void OnDestroy()
	{
		if (this.tokenManager != null)
		{
			this.tokenManager.ReleaseFunctions();
		}
		this.playerJoinGameController.Deactivate();
		base.OnDestroy();
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x0010E1FC File Offset: 0x0010C5FC
	protected override Vector2 getCursorDefaultPosition(PlayerNum playerNum)
	{
		return this.CursorDefault.transform.position;
	}

	// Token: 0x060039C1 RID: 14785 RVA: 0x0010E214 File Offset: 0x0010C614
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

	// Token: 0x060039C2 RID: 14786 RVA: 0x0010E314 File Offset: 0x0010C714
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

	// Token: 0x060039C3 RID: 14787 RVA: 0x0010E354 File Offset: 0x0010C754
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

	// Token: 0x060039C4 RID: 14788 RVA: 0x0010E4EC File Offset: 0x0010C8EC
	private bool noPlayersActive()
	{
		IEnumerator enumerator = ((IEnumerable)this.gamePayload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
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

	// Token: 0x060039C5 RID: 14789 RVA: 0x0010E564 File Offset: 0x0010C964
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

	// Token: 0x060039C6 RID: 14790 RVA: 0x0010E63C File Offset: 0x0010CA3C
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

	// Token: 0x060039C7 RID: 14791 RVA: 0x0010E66A File Offset: 0x0010CA6A
	private void onClickTokenDrop(CursorTargetButton target, PointerEventData eventData)
	{
		this.characterSelectFunctions.OnClickTokenDrop(target, eventData, new ShouldDisplayToken(this.shouldDisplayToken), this.gamePayload);
	}

	// Token: 0x060039C8 RID: 14792 RVA: 0x0010E68B File Offset: 0x0010CA8B
	public void OnStartButton()
	{
		base.events.Broadcast(new NextScreenRequest());
	}

	// Token: 0x060039C9 RID: 14793 RVA: 0x0010E69D File Offset: 0x0010CA9D
	public override void OnAdvance1Pressed(IPlayerCursor cursor)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
		base.events.Broadcast(new CyclePlayerSkinRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), 1));
	}

	// Token: 0x060039CA RID: 14794 RVA: 0x0010E6CE File Offset: 0x0010CACE
	public override void OnPrevious1Pressed(IPlayerCursor cursor)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_skinCycle, 0f);
		base.events.Broadcast(new CyclePlayerSkinRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), -1));
	}

	// Token: 0x060039CB RID: 14795 RVA: 0x0010E6FF File Offset: 0x0010CAFF
	public override void OnAdvance2Pressed(IPlayerCursor cursor)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_teamCycle, 0f);
		base.events.Broadcast(new CyclePlayerTeamRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), 1));
	}

	// Token: 0x060039CC RID: 14796 RVA: 0x0010E730 File Offset: 0x0010CB30
	public override void OnPrevious2Pressed(IPlayerCursor cursor)
	{
		base.audioManager.PlayMenuSound(SoundKey.characterSelect_teamCycle, 0f);
		base.events.Broadcast(new CyclePlayerTeamRequest(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false), -1));
	}

	// Token: 0x060039CD RID: 14797 RVA: 0x0010E761 File Offset: 0x0010CB61
	public override void OnRightStickUpPressed(IPlayerCursor cursor)
	{
		base.events.Broadcast(new CycleCharacterIndex(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)));
	}

	// Token: 0x060039CE RID: 14798 RVA: 0x0010E77F File Offset: 0x0010CB7F
	public override void OnRightStickDownPressed(IPlayerCursor cursor)
	{
		base.events.Broadcast(new CycleCharacterIndex(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)));
	}

	// Token: 0x060039CF RID: 14799 RVA: 0x0010E7A0 File Offset: 0x0010CBA0
	private void populateScreen()
	{
		base.lockInput();
		this.characterSelectionPortraits.Clear();
		List<CharacterDefinition> list = this.characterSelectFunctions.SortCharacters(this.characters, this.modeData.Type);
		foreach (CharacterDefinition characterDef in list)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CharacterPortraitPrefab);
			gameObject.transform.SetParent(this.CharacterList.transform, false);
			CharacterSelectionPortrait component = gameObject.GetComponent<CharacterSelectionPortrait>();
			base.injector.Inject(component);
			component.Init(characterDef, this.gamePayload.players, this.modeData);
			this.characterSelectionPortraits.Add(component);
		}
		this.playersUI.Setup(this.modeData);
		this.CharacterList.Redraw();
		this.tweenIn();
	}

	// Token: 0x060039D0 RID: 14800 RVA: 0x0010E8A0 File Offset: 0x0010CCA0
	private void tweenIn()
	{
		this.OptionsBar.alpha = 0f;
		base.tweenInItem(this.SwipeIn2, 0, -1, 0.25f, 0f, null);
		base.tweenInItem(this.SwipeIn3, 0, -1, 0.25f, 0f, null);
		base.tweenInItem(this.SwipeIn1, 0, 1, 0.25f, 0f, delegate
		{
			this.onAnimationsComplete();
		});
		DOTween.To(() => this.OptionsBar.alpha, delegate(float valueIn)
		{
			this.OptionsBar.alpha = valueIn;
		}, 1f, 0.1f).SetDelay(0.25f).SetEase(Ease.OutSine);
	}

	// Token: 0x060039D1 RID: 14801 RVA: 0x0010E94B File Offset: 0x0010CD4B
	private void onAnimationsComplete()
	{
		base.unlockInput();
	}

	// Token: 0x060039D2 RID: 14802 RVA: 0x0010E953 File Offset: 0x0010CD53
	public override void GoToPreviousScreen()
	{
		if (!base.gameDataManager.ConfigData.uiuxSettings.DemoModeEnabled)
		{
			base.GoToPreviousScreen();
		}
	}

	// Token: 0x060039D3 RID: 14803 RVA: 0x0010E975 File Offset: 0x0010CD75
	public override void GoToNextScreen()
	{
		base.events.Broadcast(new NextScreenRequest());
	}

	// Token: 0x060039D4 RID: 14804 RVA: 0x0010E987 File Offset: 0x0010CD87
	public override void UpdatePayload(Payload payload)
	{
		this.onUpdate();
	}

	// Token: 0x060039D5 RID: 14805 RVA: 0x0010E990 File Offset: 0x0010CD90
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
		foreach (CharacterSelectionPortrait characterSelectionPortrait in this.characterSelectionPortraits)
		{
			characterSelectionPortrait.UpdatePlayerList(gamePayload.players);
		}
		this.playersUI.UpdatePayload();
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x0010EA7C File Offset: 0x0010CE7C
	private void updatePlayButton()
	{
		bool flag = this.api.IsValidPayload(this.modeData, this.gamePayload);
		if (flag != this.PlayButton.gameObject.activeInHierarchy)
		{
			this.killPlayButtonTween();
			if (flag)
			{
				this.PlayButton.gameObject.SetActive(true);
				this.playButtonTween = DOTween.To(new DOGetter<float>(this.get_playButtonFade), delegate(float valueIn)
				{
					this.playButtonFade = valueIn;
				}, 1f, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killPlayButtonTween));
			}
			else
			{
				this.PlayButton.gameObject.SetActive(false);
				this.playButtonTween = DOTween.To(new DOGetter<float>(this.get_playButtonFade), delegate(float valueIn)
				{
					this.playButtonFade = valueIn;
				}, 0.5f, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killPlayButtonTween));
			}
		}
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x0010EB72 File Offset: 0x0010CF72
	private void killPlayButtonTween()
	{
		if (this.playButtonTween != null && this.playButtonTween.IsPlaying())
		{
			this.playButtonTween.Kill(false);
		}
		this.playButtonTween = null;
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x0010EBA4 File Offset: 0x0010CFA4
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

	// Token: 0x060039D9 RID: 14809 RVA: 0x0010ECCC File Offset: 0x0010D0CC
	private bool isCursorActive(PlayerSelectionInfo newInfo)
	{
		return newInfo.type == PlayerType.Human;
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x0010ECDC File Offset: 0x0010D0DC
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

	// Token: 0x060039DB RID: 14811 RVA: 0x0010ED54 File Offset: 0x0010D154
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

	// Token: 0x060039DC RID: 14812 RVA: 0x0010EDE0 File Offset: 0x0010D1E0
	private void processGameModeInfoDelta(BattleSettings newSettings, BattleSettings oldSettings)
	{
		this.broadcastDisabledPlayerSlots(newSettings.mode, oldSettings.mode);
		this.setGameMode(newSettings.mode, newSettings.rules);
		foreach (OptionGUI optionGUI in this.optionsLeft)
		{
			optionGUI.UpdatePayload(this.gamePayload);
		}
		foreach (OptionGUI optionGUI2 in this.optionsRight)
		{
			optionGUI2.UpdatePayload(this.gamePayload);
		}
		this.MoreOptionsWindow.UpdatePayload(this.gamePayload, newSettings.mode, newSettings.rules);
	}

	// Token: 0x060039DD RID: 14813 RVA: 0x0010EED4 File Offset: 0x0010D2D4
	private void updateOptionsGuis()
	{
		MainOptionsList leftRightOptions = this.optionsCalculator.GetLeftRightOptions(this.currentGameMode, this.currentGameRules);
		this.updateOptionSide(this.optionsLeft, leftRightOptions.LeftSide, this.LeftSideOptions);
		this.updateOptionSide(this.optionsRight, leftRightOptions.RightSide, this.RightSideOptions);
	}

	// Token: 0x060039DE RID: 14814 RVA: 0x0010EF2C File Offset: 0x0010D32C
	private void updateOptionSide(List<OptionGUI> existing, List<OptionDescription> newList, HorizontalLayoutGroup theParent)
	{
		if (!this.isEquivalentOptions(existing, newList))
		{
			foreach (OptionGUI optionGUI in existing)
			{
				UnityEngine.Object.Destroy(optionGUI.gameObject);
			}
			existing.Clear();
			bool flag = theParent == this.RightSideOptions;
			if (flag && this.moreOptions != null)
			{
				this.moreOptions.transform.SetParent(null, false);
			}
			foreach (OptionDescription desc in newList)
			{
				OptionGUI component = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab).GetComponent<OptionGUI>();
				base.injector.Inject(component);
				component.LoadFromDesc(desc);
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

	// Token: 0x060039DF RID: 14815 RVA: 0x0010F06C File Offset: 0x0010D46C
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

	// Token: 0x060039E0 RID: 14816 RVA: 0x0010F0D5 File Offset: 0x0010D4D5
	private void onMoreOptions(CursorTargetButton target, PointerEventData eventData)
	{
		this.openMoreOptions();
		this.MoreOptionsWindow.OnCloseRequest = new Action<bool>(this.closeMoreOptions);
	}

	// Token: 0x060039E1 RID: 14817 RVA: 0x0010F0F4 File Offset: 0x0010D4F4
	private void openMoreOptions()
	{
		if (!this.isMoreOptionsOpen)
		{
			this.isMoreOptionsOpen = true;
			this.MoreOptionsWindow.gameObject.SetActive(true);
			this.MoreOptionsWindow.OnOpened();
			this.killMoreOptionsTween();
			this.MoreOptionsWindow.Alpha = 0f;
			this.moreOptionsTween = DOTween.To(() => this.MoreOptionsWindow.Alpha, delegate(float valueIn)
			{
				this.MoreOptionsWindow.Alpha = valueIn;
			}, 1f, 0.1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killMoreOptionsTween));
			base.events.Broadcast(new MoreOptionsWasOpened());
		}
	}

	// Token: 0x060039E2 RID: 14818 RVA: 0x0010F19C File Offset: 0x0010D59C
	private void closeMoreOptions(bool revertChanges)
	{
		if (this.isMoreOptionsOpen)
		{
			this.isMoreOptionsOpen = false;
			this.MoreOptionsWindow.OnClosed();
			this.killMoreOptionsTween();
			this.moreOptionsTween = DOTween.To(() => this.MoreOptionsWindow.Alpha, delegate(float valueIn)
			{
				this.MoreOptionsWindow.Alpha = valueIn;
			}, 0f, 0.1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.moreOptionsFadeOutComplete));
			base.events.Broadcast(new MoreOptionsWasClosed(revertChanges));
		}
	}

	// Token: 0x060039E3 RID: 14819 RVA: 0x0010F221 File Offset: 0x0010D621
	private void moreOptionsFadeOutComplete()
	{
		this.MoreOptionsWindow.gameObject.SetActive(false);
		this.killMoreOptionsTween();
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x0010F23A File Offset: 0x0010D63A
	private void killMoreOptionsTween()
	{
		if (this.moreOptionsTween != null && this.moreOptionsTween.IsPlaying())
		{
			this.moreOptionsTween.Kill(false);
		}
		this.moreOptionsTween = null;
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x0010F26C File Offset: 0x0010D66C
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

	// Token: 0x060039E6 RID: 14822 RVA: 0x0010F2C0 File Offset: 0x0010D6C0
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

	// Token: 0x17000DF1 RID: 3569
	// (get) Token: 0x060039E7 RID: 14823 RVA: 0x0010F315 File Offset: 0x0010D715
	// (set) Token: 0x060039E8 RID: 14824 RVA: 0x0010F31D File Offset: 0x0010D71D
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

	// Token: 0x040027C5 RID: 10181
	public HorizontalLayoutGroup CharacterList;

	// Token: 0x040027C6 RID: 10182
	public GameObject TokenGroup;

	// Token: 0x040027C7 RID: 10183
	public GameObject CursorDefault;

	// Token: 0x040027C8 RID: 10184
	public GameObject TokenSpace;

	// Token: 0x040027C9 RID: 10185
	public CursorTargetButton TokenDrop;

	// Token: 0x040027CA RID: 10186
	public HorizontalLayoutGroup LeftSideOptions;

	// Token: 0x040027CB RID: 10187
	public HorizontalLayoutGroup RightSideOptions;

	// Token: 0x040027CC RID: 10188
	public MoreOptionsWindow MoreOptionsWindow;

	// Token: 0x040027CD RID: 10189
	public CursorTargetButton PlayButton;

	// Token: 0x040027CE RID: 10190
	public Image PlayButtonImage;

	// Token: 0x040027CF RID: 10191
	public TextMeshProUGUI PlayButtonText;

	// Token: 0x040027D0 RID: 10192
	public GameObject TokenDefaultLocation;

	// Token: 0x040027D1 RID: 10193
	public CanvasGroup OptionsBar;

	// Token: 0x040027D2 RID: 10194
	public GameObject SwipeIn1;

	// Token: 0x040027D3 RID: 10195
	public GameObject SwipeIn2;

	// Token: 0x040027D4 RID: 10196
	public GameObject SwipeIn3;

	// Token: 0x040027D5 RID: 10197
	public GameObject BackButtonStub;

	// Token: 0x040027D6 RID: 10198
	public GameObject BackButtonPrefab;

	// Token: 0x040027D7 RID: 10199
	public GameObject CharacterPortraitPrefab;

	// Token: 0x040027D8 RID: 10200
	public GameObject PlayerTokenPrefab;

	// Token: 0x040027D9 RID: 10201
	public GameObject OptionPrefab;

	// Token: 0x040027DA RID: 10202
	public PlayerSelectionUI PlayerPortraitPrefab;

	// Token: 0x040027DB RID: 10203
	public GameObject MoreOptionsPrefab;

	// Token: 0x040027DC RID: 10204
	public ScreenType PreviousScreen;

	// Token: 0x040027DD RID: 10205
	public ScreenType NextScreen;

	// Token: 0x040027DE RID: 10206
	private CharacterSelectPlayerOrganizer playersUI;

	// Token: 0x040027DF RID: 10207
	private GameMode currentGameMode;

	// Token: 0x040027E0 RID: 10208
	private GameRules currentGameRules;

	// Token: 0x040027E1 RID: 10209
	private List<OptionGUI> optionsLeft = new List<OptionGUI>();

	// Token: 0x040027E2 RID: 10210
	private List<OptionGUI> optionsRight = new List<OptionGUI>();

	// Token: 0x040027E3 RID: 10211
	private GameObject moreOptions;

	// Token: 0x040027E4 RID: 10212
	private float _playButtonFade;

	// Token: 0x040027E5 RID: 10213
	private Tweener playButtonTween;

	// Token: 0x040027E6 RID: 10214
	private bool isMoreOptionsOpen;

	// Token: 0x040027E7 RID: 10215
	private Tweener moreOptionsTween;

	// Token: 0x040027E8 RID: 10216
	private GameLoadPayload previousPayload;

	// Token: 0x040027E9 RID: 10217
	private CharacterSelectScene3D characterSelectScene;

	// Token: 0x040027EA RID: 10218
	private CharacterDefinition[] characters;

	// Token: 0x040027EB RID: 10219
	private List<CharacterSelectionPortrait> characterSelectionPortraits = new List<CharacterSelectionPortrait>();

	// Token: 0x040027EC RID: 10220
	[CompilerGenerated]
	private static Func<UnityEngine.Object, UnityEngine.Object> <>f__mg$cache0;

	// Token: 0x040027ED RID: 10221
	[CompilerGenerated]
	private static Action<UnityEngine.Object> <>f__mg$cache1;
}
