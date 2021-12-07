using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FixedPoint;
using IconsServer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008A9 RID: 2217
public class BattleUI : GameScreen
{
	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x06003785 RID: 14213 RVA: 0x00103BDC File Offset: 0x00101FDC
	// (set) Token: 0x06003786 RID: 14214 RVA: 0x00103BE4 File Offset: 0x00101FE4
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x06003787 RID: 14215 RVA: 0x00103BED File Offset: 0x00101FED
	// (set) Token: 0x06003788 RID: 14216 RVA: 0x00103BF5 File Offset: 0x00101FF5
	[Inject]
	public IVictoryPoseController victoryPoseController { get; set; }

	// Token: 0x17000D7E RID: 3454
	// (get) Token: 0x06003789 RID: 14217 RVA: 0x00103BFE File Offset: 0x00101FFE
	// (set) Token: 0x0600378A RID: 14218 RVA: 0x00103C06 File Offset: 0x00102006
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17000D7F RID: 3455
	// (get) Token: 0x0600378B RID: 14219 RVA: 0x00103C0F File Offset: 0x0010200F
	// (set) Token: 0x0600378C RID: 14220 RVA: 0x00103C17 File Offset: 0x00102017
	[Inject]
	public IExitGame terminateGame { get; set; }

	// Token: 0x17000D80 RID: 3456
	// (get) Token: 0x0600378D RID: 14221 RVA: 0x00103C20 File Offset: 0x00102020
	// (set) Token: 0x0600378E RID: 14222 RVA: 0x00103C28 File Offset: 0x00102028
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000D81 RID: 3457
	// (get) Token: 0x0600378F RID: 14223 RVA: 0x00103C31 File Offset: 0x00102031
	// (set) Token: 0x06003790 RID: 14224 RVA: 0x00103C39 File Offset: 0x00102039
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x17000D82 RID: 3458
	// (get) Token: 0x06003791 RID: 14225 RVA: 0x00103C42 File Offset: 0x00102042
	// (set) Token: 0x06003792 RID: 14226 RVA: 0x00103C4A File Offset: 0x0010204A
	[Inject]
	public IVictoryPoseHelper victoryPoseHelper { get; set; }

	// Token: 0x17000D83 RID: 3459
	// (get) Token: 0x06003793 RID: 14227 RVA: 0x00103C53 File Offset: 0x00102053
	// (set) Token: 0x06003794 RID: 14228 RVA: 0x00103C5B File Offset: 0x0010205B
	[Inject]
	public IVideoSettingsUtility videoSettingsUtility { get; set; }

	// Token: 0x17000D84 RID: 3460
	// (get) Token: 0x06003795 RID: 14229 RVA: 0x00103C64 File Offset: 0x00102064
	// (set) Token: 0x06003796 RID: 14230 RVA: 0x00103C6C File Offset: 0x0010206C
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x17000D85 RID: 3461
	// (get) Token: 0x06003797 RID: 14231 RVA: 0x00103C75 File Offset: 0x00102075
	// (set) Token: 0x06003798 RID: 14232 RVA: 0x00103C7D File Offset: 0x0010207D
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x06003799 RID: 14233 RVA: 0x00103C86 File Offset: 0x00102086
	// (set) Token: 0x0600379A RID: 14234 RVA: 0x00103C8E File Offset: 0x0010208E
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x0600379B RID: 14235 RVA: 0x00103C97 File Offset: 0x00102097
	// (set) Token: 0x0600379C RID: 14236 RVA: 0x00103C9F File Offset: 0x0010209F
	[Inject]
	public ISceneController sceneController { get; set; }

	// Token: 0x17000D88 RID: 3464
	// (get) Token: 0x0600379D RID: 14237 RVA: 0x00103CA8 File Offset: 0x001020A8
	// (set) Token: 0x0600379E RID: 14238 RVA: 0x00103CB0 File Offset: 0x001020B0
	[Inject]
	public ScreenshotModel screenshotModel { get; set; }

	// Token: 0x17000D89 RID: 3465
	// (get) Token: 0x0600379F RID: 14239 RVA: 0x00103CB9 File Offset: 0x001020B9
	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	// Token: 0x17000D8A RID: 3466
	// (get) Token: 0x060037A0 RID: 14240 RVA: 0x00103CC6 File Offset: 0x001020C6
	public override bool AlwaysHideMouse
	{
		get
		{
			return !this.isPaused;
		}
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x00103CD8 File Offset: 0x001020D8
	public override void LoadPayload(Payload payload)
	{
		base.LoadPayload(payload);
		this.StandardTimer.gameObject.SetActive(false);
		this.CrewsTimer.gameObject.SetActive(false);
		this.screenshot.gameObject.SetActive(false);
		if (base.config.uiuxSettings.emotiveStartup)
		{
			this.centerTextGroup = this.TopTextGroup;
			this.centerText = this.TopText;
			this.TopTextGroup.SetActive(true);
			this.CenterTextGroup.SetActive(false);
		}
		else
		{
			this.centerTextGroup = this.CenterTextGroup;
			this.centerText = this.CenterText;
			this.CenterTextGroup.SetActive(true);
			this.TopTextGroup.SetActive(false);
		}
		if (this.gamePayload != null)
		{
			GameModeSettings settings = base.gameDataManager.GameModeData.GetDataByType(this.gamePayload.battleConfig.mode).settings;
			if (settings.usesPlayerBarUI)
			{
				this.playerBar = UnityEngine.Object.Instantiate<GameObject>(this.playerBarPrefab).GetComponent<PlayerGUIBar>();
				this.playerBar.Initialize(this.gamePayload.battleConfig, this.gamePayload.players);
				this.playerBar.transform.SetParent(this.playerBarContainer.transform, false);
			}
			if (settings.usesTeamBarUI)
			{
				if (base.config.uiuxSettings.crewsGuiType == CrewBattleGuiType.VERSION1)
				{
					this.crewsGui = UnityEngine.Object.Instantiate<CrewsGUI>(this.crewsGuiPrefabV1);
				}
				else if (base.config.uiuxSettings.crewsGuiType == CrewBattleGuiType.VERSION2)
				{
					this.crewsGui = UnityEngine.Object.Instantiate<CrewsGUI>(this.crewsGuiPrefabV2);
				}
				else
				{
					this.crewsGui = UnityEngine.Object.Instantiate<CrewsGUIV3>(this.crewsGuiPrefabV3);
				}
				this.crewsGui.Initialize(this.gamePayload.battleConfig, this.gamePayload.players);
				this.crewsGui.Transform.SetParent(this.teamBarContainer.transform, false);
			}
			this.CrewsBottomBar.IsActive = settings.usesCrewBattleBottomBar;
			if (settings.usesCrewBattleBottomBar)
			{
				this.CrewsBottomBar.Initialize(this.gamePayload.battleConfig, this.gamePayload.players);
			}
			this.useCrewsTimer = settings.usesTeamBarUI;
			if (settings.usesFloatyNamesUI)
			{
				this.floatyNameManager.Init(base.gameObject, base.gameController.currentGame);
			}
			if (settings.usesOffScreenArrows)
			{
				this.arrowManager.Init(base.gameController.currentGame, this.offscreenArrowPrefab, this.playerTextureCameraPrefab);
			}
		}
		this.updateTimers();
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x00103F7C File Offset: 0x0010237C
	public override void OnAddedToHeirarchy()
	{
		base.OnAddedToHeirarchy();
		this.updateTimers();
		if (this.pauseScreen == null)
		{
			this.pauseScreen = UnityEngine.Object.Instantiate<GameObject>(this.PauseScreenPrefab, base.transform, false).GetComponent<PauseScreen>();
			this.pauseScreen.gameObject.SetActive(false);
		}
		base.events.Subscribe(typeof(GamePausedEvent), new Events.EventHandler(this.onPause));
		base.events.Subscribe(typeof(GameEndEvent), new Events.EventHandler(this.onGameEnd));
		base.events.Subscribe(typeof(TransitionToVictoryPoseCommand), new Events.EventHandler(this.onTransitionToVictoryPose));
		base.events.Subscribe(typeof(PlayerLeaveMatchCommand), new Events.EventHandler(this.onPlayerLeaveMatch));
		base.signalBus.GetSignal<RoundCountSignal>().AddListener(new Action<int>(this.onRoundCountDisplay));
		base.signalBus.GetSignal<UpdatePauseScreenOnline>().AddListener(new Action<bool>(this.onShowPauseScreen));
		this.centerText.text = string.Empty;
		this.CenterTextBgNumbers.SetActive(false);
		this.CenterTextBg.SetActive(false);
		this.updateCenterText();
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x001040BE File Offset: 0x001024BE
	protected override void Update()
	{
		if (this.victoryPoseController == null)
		{
			return;
		}
		this.updateCenterText();
		this.victoryPoseController.Update();
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x001040DD File Offset: 0x001024DD
	public override void Deactivate()
	{
		this.victoryPoseController.Clear();
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x001040EC File Offset: 0x001024EC
	private void updateCenterText()
	{
		if (base.gameController == null || base.gameController.currentGame == null)
		{
			return;
		}
		this.centerTextGroup.SetActive(false);
		if (this.roundCountDisplayFrames > 0)
		{
			this.centerTextGroup.SetActive(true);
			this.centerText.text = "GAUNTLET ROUND " + this.roundCountDisplay;
		}
		else if (this.endEvent != null)
		{
			if (this.endEvent.winners != null)
			{
				this.centerTextGroup.SetActive(true);
				if (base.gameController.currentGame.BattleSettings.rules == GameRules.Time)
				{
					this.centerText.text = base.localization.GetText("ui.battle.center.time");
				}
				else
				{
					this.centerText.text = base.localization.GetText("ui.battle.center.game");
				}
			}
		}
		else if (!this.isPaused)
		{
			Fixed one = (!this.devConfig.disableCountdown) ? base.config.uiuxSettings.countdownIntervalSeconds : Fixed.Zero;
			int num = (int)(one * WTime.fps);
			int num2 = base.config.uiuxSettings.countdownAmt + 2;
			if (base.gameController.currentGame.Frame < num * num2)
			{
				int num3 = base.gameController.currentGame.Frame / num;
				if (num3 <= 0)
				{
					this.centerTextGroup.SetActive(false);
				}
				else
				{
					if (base.gameController.currentGame.Frame >= num3 * num && this.countdownIndex < num3)
					{
						this.incrementCountdownText(this.centerText, num3, num2);
						this.countdownIndex = num3;
					}
					if (this.countdownIndex != 0)
					{
						this.centerTextGroup.SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x001042D4 File Offset: 0x001026D4
	private void incrementCountdownText(TextMeshProUGUI countdownText, int count, int maxCount)
	{
		if (count >= maxCount - 1)
		{
			AudioManager audioManager = base.audioManager;
			AudioRequest audioRequest = new AudioRequest(this.startSound, null);
			audioManager.PlayGameSound(audioRequest.MaintainPitch());
			countdownText.text = base.localization.GetText("ui.battle.center.fight");
			if (!base.config.uiuxSettings.emotiveStartup)
			{
				base.events.Broadcast(new PlayAnnouncementCommand(AnnouncementType.Battle_Countdown_Go));
			}
		}
		else if (count > 0)
		{
			int num = maxCount - 1 - count;
			string type = AnnouncementType.None;
			if (!base.config.uiuxSettings.emotiveStartup)
			{
				switch (count)
				{
				case 1:
					type = AnnouncementType.Battle_Countdown_3;
					goto IL_D1;
				case 2:
					type = AnnouncementType.Battle_Countdown_2;
					goto IL_D1;
				}
				type = AnnouncementType.Battle_Countdown_1;
				IL_D1:
				base.events.Broadcast(new PlayAnnouncementCommand(type));
			}
			countdownText.text = num.ToString();
			AudioManager audioManager2 = base.audioManager;
			AudioRequest audioRequest2 = new AudioRequest(this.countdownSound, null);
			audioManager2.PlayGameSound(audioRequest2.MaintainPitch());
		}
	}

	// Token: 0x060037A7 RID: 14247 RVA: 0x001043F7 File Offset: 0x001027F7
	private void onGameEnd(GameEvent message)
	{
		this.endEvent = (GameEndEvent)message;
		if (this.endEvent.winners != null)
		{
		}
		this.updateCenterText();
		if (this.dialog != null)
		{
			this.dialog.Close();
		}
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x00104438 File Offset: 0x00102838
	private void onPlayerLeaveMatch(GameEvent message)
	{
		if (this.dialog == null)
		{
			string key = "dialog.Battle.promptLeaveMatch.body";
			if (this.gamePayload.customLobbyMatch)
			{
				if (this.customLobby.IsInLobby && this.customLobby.IsLobbyLeader)
				{
					key = "dialog.Battle.promptLeaveMatchLobbyLeader.body";
				}
				else
				{
					key = "dialog.Battle.promptLeaveMatchAndLobby.body";
				}
			}
			this.dialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("dialog.Battle.promptLeaveMatch.title"), base.localization.GetText(key), base.localization.GetText("dialog.Battle.promptLeaveMatch.confirm"), base.localization.GetText("dialog.cancel"));
			this.dialog.ConfirmCallback = new Action(this.onPlayerLeaveMatchConfirm);
			this.dialog.CloseCallback = new Action(this.onPlayerLeaveMatchClose);
		}
		else
		{
			this.dialog.Close();
			this.dialog = null;
		}
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x0010452F File Offset: 0x0010292F
	private void onPlayerLeaveMatchConfirm()
	{
		base.gameController.currentGame.ForfeitGame(ScreenType.VictoryGUI);
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x00104542 File Offset: 0x00102942
	private void onPlayerLeaveMatchClose()
	{
		this.dialog = null;
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x0010454C File Offset: 0x0010294C
	private void onTransitionToVictoryPose(GameEvent message)
	{
		TransitionToVictoryPoseCommand command = (TransitionToVictoryPoseCommand)message;
		this.screenshotModel.SaveScreenshot(delegate
		{
			this.screenshot.texture = this.screenshotModel.GetScreenshot();
			this.screenshot.gameObject.SetActive(true);
			this.terminateGame.DestroyGameManager();
			if (this.config.uiuxSettings.skipVictoryPoseScreen)
			{
				this.loadVictoryScreen(command.Payload);
			}
			else if (command.Payload.victors.Count == 0)
			{
				this.loadVictoryScreen(command.Payload);
			}
			else
			{
				this.sceneController.LoadVictoryPoseScene(delegate
				{
					DOTween.To(() => this.BlackScreenUI.alpha, delegate(float x)
					{
						this.BlackScreenUI.alpha = x;
					}, 1f, 0.2f).SetEase(Ease.InSine).OnComplete(delegate
					{
						this.screenshot.gameObject.SetActive(false);
						this.StartCoroutine(this.playVictoryPoseTransitions(command.Payload));
					});
				});
			}
		});
	}

	// Token: 0x17000D8B RID: 3467
	// (get) Token: 0x060037AC RID: 14252 RVA: 0x00104589 File Offset: 0x00102989
	private bool CanAdvanceToVictoryScreen
	{
		get
		{
			return !base.battleServerAPI.IsConnected || base.battleServerAPI.ReceivedMatchResults;
		}
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x001045AC File Offset: 0x001029AC
	private IEnumerator playVictoryPoseTransitions(VictoryScreenPayload victoryPayload)
	{
		this.BattleInProgressUI.alpha = 0f;
		this.BlackScreenUI.alpha = 1f;
		this.VictoryPoseUI.alpha = 1f;
		GameModeData modeData = base.gameDataManager.GameModeData.GetDataByType(victoryPayload.gamePayload.battleConfig.mode);
		this.VictoryPoseTitle.text = ScreenTextHelper.GetTitleText(victoryPayload, base.localization, modeData);
		this.VictoryPoseSubtitle.text = base.localization.GetText("gameRules.subtitle." + victoryPayload.gamePayload.battleConfig.mode);
		this.playSound(victoryPayload, modeData);
		foreach (Camera camera in Camera.allCameras)
		{
			this.videoSettingsUtility.ApplyToCamera(camera);
		}
		DOTween.To(() => this.BlackScreenUI.alpha, delegate(float x)
		{
			this.BlackScreenUI.alpha = x;
		}, 0f, 0.2f).SetEase(Ease.OutSine);
		this.victoryPoseController.InitVictoryPose(victoryPayload, base.gameDataManager);
		float unskippableDuration;
		float totalDuration;
		this.getVictoryPoseDurations(victoryPayload, out unskippableDuration, out totalDuration);
		float failsafeTime = totalDuration + 3f;
		float startTime = Time.time;
		for (;;)
		{
			float timeSinceStart = Time.time - startTime;
			if (this.CanAdvanceToVictoryScreen || timeSinceStart >= failsafeTime)
			{
				if (timeSinceStart > unskippableDuration && this.anyButtonPressed)
				{
					break;
				}
				if (timeSinceStart > totalDuration)
				{
					break;
				}
			}
			this.anyButtonPressed = false;
			yield return null;
		}
		this.loadVictoryScreen(victoryPayload);
		yield break;
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x001045D0 File Offset: 0x001029D0
	private void loadVictoryScreen(VictoryScreenPayload victoryPayload)
	{
		this.screenshotModel.SaveScreenshot(delegate
		{
			this.screenshot.texture = this.screenshotModel.GetScreenshot();
			this.screenshot.gameObject.SetActive(true);
			this.terminateGame.ExitGameMode(delegate
			{
				if (victoryPayload.nextScreen == ScreenType.VictoryGUI)
				{
					this.events.Broadcast(new LoadScreenCommand(ScreenType.VictoryGUI, victoryPayload, ScreenUpdateType.Next));
				}
				else
				{
					ScreenType nextScreen = victoryPayload.nextScreen;
					if (nextScreen != ScreenType.CharacterSelect)
					{
						if (nextScreen != ScreenType.SelectStage)
						{
							this.richPresence.SetPresence(null, null, null, null);
						}
						else
						{
							this.enterNewGame.InitPayload(GameStartType.FreePlay, victoryPayload.gamePayload);
							this.richPresence.SetPresence("InStageSelect", null, null, null);
						}
					}
					else
					{
						this.enterNewGame.InitPayload(GameStartType.FreePlay, victoryPayload.gamePayload);
						this.richPresence.SetPresence("InCharacterSelect", null, null, null);
					}
					this.events.Broadcast(new LoadScreenCommand(victoryPayload.nextScreen, null, ScreenUpdateType.Next));
				}
			});
		});
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x00104608 File Offset: 0x00102A08
	private void playSound(VictoryScreenPayload victoryPayload, GameModeData modeData)
	{
		string text = AnnouncementType.None;
		if (modeData.settings.usesTeams && victoryPayload.winningTeams.Count > 0)
		{
			switch (victoryPayload.winningTeams[0])
			{
			case TeamNum.Team1:
				text = AnnouncementType.Victory_Winner_Team1;
				break;
			case TeamNum.Team2:
				text = AnnouncementType.Victory_Winner_Team2;
				break;
			case TeamNum.Team3:
				text = AnnouncementType.Victory_Winner_Team3;
				break;
			case TeamNum.Team4:
				text = AnnouncementType.Victory_Winner_Team4;
				break;
			}
		}
		else if (victoryPayload.victors.Count > 0)
		{
			switch (victoryPayload.victors[0])
			{
			case PlayerNum.Player1:
				text = AnnouncementType.Victory_Winner_Player1;
				break;
			case PlayerNum.Player2:
				text = AnnouncementType.Victory_Winner_Player2;
				break;
			case PlayerNum.Player3:
				text = AnnouncementType.Victory_Winner_Player3;
				break;
			case PlayerNum.Player4:
				text = AnnouncementType.Victory_Winner_Player4;
				break;
			case PlayerNum.Player5:
				text = AnnouncementType.Victory_Winner_Player5;
				break;
			case PlayerNum.Player6:
				text = AnnouncementType.Victory_Winner_Player6;
				break;
			}
		}
		if (text != AnnouncementType.None)
		{
			base.events.Broadcast(new PlayAnnouncementCommand(text));
		}
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x00104739 File Offset: 0x00102B39
	private void onRoundCountDisplay(int count)
	{
		this.roundCountDisplay = count;
		this.roundCountDisplayFrames = 180;
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x00104750 File Offset: 0x00102B50
	public override void TickFrame()
	{
		base.TickFrame();
		if (!this.isPaused)
		{
			if (this.playerBar != null)
			{
				this.playerBar.TickFrame();
			}
			if (this.crewsGui != null)
			{
				this.crewsGui.TickFrame();
			}
			this.CrewsBottomBar.TickFrame();
			if (this.roundCountDisplayFrames > 0)
			{
				this.roundCountDisplayFrames--;
			}
		}
		this.updateTimers();
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x001047CB File Offset: 0x00102BCB
	private void updateTimers()
	{
		if (this.useCrewsTimer)
		{
			this.updateTimerDisplay(this.CrewsTimer, this.CrewsTimerDecimalReduce);
		}
		else
		{
			this.updateTimerDisplay(this.StandardTimer, this.StandardTimerDecimalReduce);
		}
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x00104804 File Offset: 0x00102C04
	private void updateTimerDisplay(TextMeshProUGUI theTimer, int decimalSizeReduce)
	{
		if (base.gameController.currentGame != null)
		{
			if (this.shouldDisplayTimer())
			{
				float currentSeconds = base.gameController.currentGame.CurrentGameMode.CurrentSeconds;
				this.timeString.Reset();
				TimeUtil.FormatTime(currentSeconds, ref this.timeString, -decimalSizeReduce, true);
				theTimer.SetCharArray(this.timeString.arr, 0, this.timeString.len);
				theTimer.gameObject.SetActive(true);
			}
			else
			{
				theTimer.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x0010489C File Offset: 0x00102C9C
	private bool shouldDisplayTimer()
	{
		return base.gameController.currentGame.CurrentGameMode.ShouldDisplayTimer && !this.isPaused && !this.centerTextGroup.activeSelf;
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x001048DC File Offset: 0x00102CDC
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (base.events != null)
		{
			base.events.Unsubscribe(typeof(GameEndEvent), new Events.EventHandler(this.onGameEnd));
			base.events.Unsubscribe(typeof(GamePausedEvent), new Events.EventHandler(this.onPause));
			base.events.Unsubscribe(typeof(TransitionToVictoryPoseCommand), new Events.EventHandler(this.onTransitionToVictoryPose));
			base.events.Unsubscribe(typeof(PlayerLeaveMatchCommand), new Events.EventHandler(this.onPlayerLeaveMatch));
			base.signalBus.GetSignal<RoundCountSignal>().RemoveListener(new Action<int>(this.onRoundCountDisplay));
			base.signalBus.GetSignal<UpdatePauseScreenOnline>().RemoveListener(new Action<bool>(this.onShowPauseScreen));
		}
		if (this.floatyNameManager != null)
		{
			this.floatyNameManager.Destroy();
			this.floatyNameManager = null;
		}
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x001049DC File Offset: 0x00102DDC
	private void onShowPauseScreen(bool paused)
	{
		if (this.PauseScreenPrefab != null)
		{
			if (paused)
			{
				this.isPaused = true;
				this.pauseScreen.gameObject.SetActive(true);
				UIInputModule uiinputModule = base.uiManager.CurrentInputModule as UIInputModule;
				uiinputModule.ScreenDelegate = this.pauseScreen;
				uiinputModule.ButtonDelegate = this.pauseScreen;
				uiinputModule.SupressButtonsPressedThisFrame();
				this.pauseScreen.Initialize(PlayerNum.None);
				this.FirstSelected = this.pauseScreen.FirstSelected;
				this.updateCenterText();
			}
			else
			{
				this.isPaused = false;
				this.FirstSelected = this.pauseScreen.PreviousSelected;
				UIInputModule uiinputModule2 = base.uiManager.CurrentInputModule as UIInputModule;
				uiinputModule2.ScreenDelegate = null;
				uiinputModule2.ButtonDelegate = null;
				if (this.pauseScreen != null)
				{
					this.pauseScreen.gameObject.SetActive(false);
				}
			}
			base.uiManager.OnUpdateMouseMode();
		}
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x00104AD4 File Offset: 0x00102ED4
	private void onPause(GameEvent message)
	{
		GamePausedEvent gamePausedEvent = message as GamePausedEvent;
		bool paused = gamePausedEvent.paused;
		if (this.PauseScreenPrefab != null)
		{
			if (paused)
			{
				this.isPaused = true;
				this.pauseScreen = UnityEngine.Object.Instantiate<GameObject>(this.PauseScreenPrefab, base.transform, false).GetComponent<PauseScreen>();
				UIInputModule uiinputModule = base.uiManager.CurrentInputModule as UIInputModule;
				uiinputModule.ScreenDelegate = this.pauseScreen;
				uiinputModule.ButtonDelegate = this.pauseScreen;
				uiinputModule.SupressButtonsPressedThisFrame();
				this.pauseScreen.Initialize(gamePausedEvent.player);
				this.FirstSelected = this.pauseScreen.FirstSelected;
				this.updateCenterText();
			}
			else
			{
				this.isPaused = false;
				this.FirstSelected = this.pauseScreen.PreviousSelected;
				UIInputModule uiinputModule2 = base.uiManager.CurrentInputModule as UIInputModule;
				uiinputModule2.ScreenDelegate = null;
				uiinputModule2.ButtonDelegate = null;
				if (this.pauseScreen != null)
				{
					UnityEngine.Object.Destroy(this.pauseScreen.gameObject);
				}
			}
			base.uiManager.OnUpdateMouseMode();
		}
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x00104BE9 File Offset: 0x00102FE9
	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		this.anyButtonPressed = true;
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x00104BF8 File Offset: 0x00102FF8
	public void getVictoryPoseDurations(VictoryScreenPayload victoryPayload, out float unskippableDuration, out float totalDuration)
	{
		unskippableDuration = 0f;
		totalDuration = 0f;
		foreach (PlayerNum playerNum in victoryPayload.victors)
		{
			PlayerSelectionInfo player = victoryPayload.gamePayload.players.GetPlayer(playerNum);
			EquippableItem equippedVictoryPoseItem = this.victoryPoseHelper.getEquippedVictoryPoseItem(player, victoryPayload.gamePayload);
			VictoryPoseData victoryPoseData = this.itemLoader.LoadAsset<VictoryPoseData>(equippedVictoryPoseItem);
			if (victoryPoseData != null)
			{
				unskippableDuration = Mathf.Max(unskippableDuration, victoryPoseData.minDuration);
				totalDuration = Mathf.Max(totalDuration, victoryPoseData.maxDuration);
			}
		}
	}

	// Token: 0x040025BF RID: 9663
	public AudioData countdownSound;

	// Token: 0x040025C0 RID: 9664
	public AudioData startSound;

	// Token: 0x040025C1 RID: 9665
	public TextMeshProUGUI StandardTimer;

	// Token: 0x040025C2 RID: 9666
	public TextMeshProUGUI CrewsTimer;

	// Token: 0x040025C3 RID: 9667
	public int StandardTimerDecimalReduce = 22;

	// Token: 0x040025C4 RID: 9668
	public int CrewsTimerDecimalReduce = 14;

	// Token: 0x040025C5 RID: 9669
	public TextMeshProUGUI CenterText;

	// Token: 0x040025C6 RID: 9670
	public TextMeshProUGUI TopText;

	// Token: 0x040025C7 RID: 9671
	public GameObject CenterTextGroup;

	// Token: 0x040025C8 RID: 9672
	public GameObject CenterTextBg;

	// Token: 0x040025C9 RID: 9673
	public GameObject CenterTextBgNumbers;

	// Token: 0x040025CA RID: 9674
	public GameObject TopTextGroup;

	// Token: 0x040025CB RID: 9675
	public GameObject PauseScreenPrefab;

	// Token: 0x040025CC RID: 9676
	public GameObject playerBarPrefab;

	// Token: 0x040025CD RID: 9677
	public Transform playerBarContainer;

	// Token: 0x040025CE RID: 9678
	public Transform teamBarContainer;

	// Token: 0x040025CF RID: 9679
	private PlayerGUIBar playerBar;

	// Token: 0x040025D0 RID: 9680
	public CrewsGUI crewsGuiPrefabV1;

	// Token: 0x040025D1 RID: 9681
	public CrewsGUI crewsGuiPrefabV2;

	// Token: 0x040025D2 RID: 9682
	public CrewsGUIV3 crewsGuiPrefabV3;

	// Token: 0x040025D3 RID: 9683
	private ICrewsGUI crewsGui;

	// Token: 0x040025D4 RID: 9684
	public CrewBattleBottomBar CrewsBottomBar;

	// Token: 0x040025D5 RID: 9685
	public FloatyNameManager floatyNameManager;

	// Token: 0x040025D6 RID: 9686
	public OffscreenArrowManager arrowManager;

	// Token: 0x040025D7 RID: 9687
	public GameObject offscreenArrowPrefab;

	// Token: 0x040025D8 RID: 9688
	public Camera playerTextureCameraPrefab;

	// Token: 0x040025D9 RID: 9689
	public RawImage screenshot;

	// Token: 0x040025DA RID: 9690
	public CanvasGroup BattleInProgressUI;

	// Token: 0x040025DB RID: 9691
	public CanvasGroup VictoryPoseUI;

	// Token: 0x040025DC RID: 9692
	public CanvasGroup BlackScreenUI;

	// Token: 0x040025DD RID: 9693
	public TextMeshProUGUI VictoryPoseTitle;

	// Token: 0x040025DE RID: 9694
	public TextMeshProUGUI VictoryPoseSubtitle;

	// Token: 0x040025DF RID: 9695
	private bool anyButtonPressed;

	// Token: 0x040025E0 RID: 9696
	private bool isPaused;

	// Token: 0x040025E1 RID: 9697
	private PauseScreen pauseScreen;

	// Token: 0x040025E2 RID: 9698
	private bool useCrewsTimer;

	// Token: 0x040025E3 RID: 9699
	private GameObject centerTextGroup;

	// Token: 0x040025E4 RID: 9700
	private TextMeshProUGUI centerText;

	// Token: 0x040025E5 RID: 9701
	private int countdownIndex;

	// Token: 0x040025E6 RID: 9702
	private GameEndEvent endEvent;

	// Token: 0x040025E7 RID: 9703
	private StaticString timeString = new StaticString(64);

	// Token: 0x040025E8 RID: 9704
	private GenericDialog dialog;

	// Token: 0x040025E9 RID: 9705
	private int roundCountDisplay;

	// Token: 0x040025EA RID: 9706
	private int roundCountDisplayFrames;
}
