// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FixedPoint;
using IconsServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : GameScreen
{
	private sealed class _onTransitionToVictoryPose_c__AnonStorey1
	{
		internal TransitionToVictoryPoseCommand command;

		internal BattleUI _this;

		internal void __m__0()
		{
			this._this.screenshot.texture = this._this.screenshotModel.GetScreenshot();
			this._this.screenshot.gameObject.SetActive(true);
			this._this.terminateGame.DestroyGameManager();
			if (this._this.config.uiuxSettings.skipVictoryPoseScreen)
			{
				this._this.loadVictoryScreen(this.command.Payload);
			}
			else if (this.command.Payload.victors.Count == 0)
			{
				this._this.loadVictoryScreen(this.command.Payload);
			}
			else
			{
				this._this.sceneController.LoadVictoryPoseScene(new Action(this.__m__1));
			}
		}

		internal void __m__1()
		{
			DOTween.To(new DOGetter<float>(this.__m__2), new DOSetter<float>(this.__m__3), 1f, 0.2f).SetEase(Ease.InSine).OnComplete(new TweenCallback(this.__m__4));
		}

		internal float __m__2()
		{
			return this._this.BlackScreenUI.alpha;
		}

		internal void __m__3(float x)
		{
			this._this.BlackScreenUI.alpha = x;
		}

		internal void __m__4()
		{
			this._this.screenshot.gameObject.SetActive(false);
			this._this.StartCoroutine(this._this.playVictoryPoseTransitions(this.command.Payload));
		}
	}

	private sealed class _playVictoryPoseTransitions_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal VictoryScreenPayload victoryPayload;

		internal GameModeData _modeData___0;

		internal Camera[] _locvar0;

		internal int _locvar1;

		internal float _unskippableDuration___0;

		internal float _totalDuration___0;

		internal float _failsafeTime___0;

		internal float _startTime___0;

		internal float _timeSinceStart___1;

		internal BattleUI _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _playVictoryPoseTransitions_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.BattleInProgressUI.alpha = 0f;
				this._this.BlackScreenUI.alpha = 1f;
				this._this.VictoryPoseUI.alpha = 1f;
				this._modeData___0 = this._this.gameDataManager.GameModeData.GetDataByType(this.victoryPayload.gamePayload.battleConfig.mode);
				this._this.VictoryPoseTitle.text = ScreenTextHelper.GetTitleText(this.victoryPayload, this._this.localization, this._modeData___0);
				this._this.VictoryPoseSubtitle.text = this._this.localization.GetText("gameRules.subtitle." + this.victoryPayload.gamePayload.battleConfig.mode);
				this._this.playSound(this.victoryPayload, this._modeData___0);
				this._locvar0 = Camera.allCameras;
				this._locvar1 = 0;
				while (this._locvar1 < this._locvar0.Length)
				{
					Camera camera = this._locvar0[this._locvar1];
					this._this.videoSettingsUtility.ApplyToCamera(camera);
					this._locvar1++;
				}
				DOTween.To(new DOGetter<float>(this.__m__0), new DOSetter<float>(this.__m__1), 0f, 0.2f).SetEase(Ease.OutSine);
				this._this.victoryPoseController.InitVictoryPose(this.victoryPayload, this._this.gameDataManager);
				this._this.getVictoryPoseDurations(this.victoryPayload, out this._unskippableDuration___0, out this._totalDuration___0);
				this._failsafeTime___0 = this._totalDuration___0 + 3f;
				this._startTime___0 = Time.time;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._timeSinceStart___1 = Time.time - this._startTime___0;
			if (this._this.CanAdvanceToVictoryScreen || this._timeSinceStart___1 >= this._failsafeTime___0)
			{
				if (this._timeSinceStart___1 <= this._unskippableDuration___0 || !this._this.anyButtonPressed)
				{
					if (this._timeSinceStart___1 <= this._totalDuration___0)
					{
						goto IL_266;
					}
				}
				this._this.loadVictoryScreen(this.victoryPayload);
				this._PC = -1;
				return false;
			}
			IL_266:
			this._this.anyButtonPressed = false;
			this._current = null;
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}

		internal float __m__0()
		{
			return this._this.BlackScreenUI.alpha;
		}

		internal void __m__1(float x)
		{
			this._this.BlackScreenUI.alpha = x;
		}
	}

	private sealed class _loadVictoryScreen_c__AnonStorey2
	{
		internal VictoryScreenPayload victoryPayload;

		internal BattleUI _this;

		internal void __m__0()
		{
			this._this.screenshot.texture = this._this.screenshotModel.GetScreenshot();
			this._this.screenshot.gameObject.SetActive(true);
			this._this.terminateGame.ExitGameMode(new Action(this.__m__1));
		}

		internal void __m__1()
		{
			if (this.victoryPayload.nextScreen == ScreenType.VictoryGUI)
			{
				this._this.events.Broadcast(new LoadScreenCommand(ScreenType.VictoryGUI, this.victoryPayload, ScreenUpdateType.Next));
			}
			else
			{
				ScreenType nextScreen = this.victoryPayload.nextScreen;
				if (nextScreen != ScreenType.CharacterSelect)
				{
					if (nextScreen != ScreenType.SelectStage)
					{
						this._this.richPresence.SetPresence(null, null, null, null);
					}
					else
					{
						this._this.enterNewGame.InitPayload(GameStartType.FreePlay, this.victoryPayload.gamePayload);
						this._this.richPresence.SetPresence("InStageSelect", null, null, null);
					}
				}
				else
				{
					this._this.enterNewGame.InitPayload(GameStartType.FreePlay, this.victoryPayload.gamePayload);
					this._this.richPresence.SetPresence("InCharacterSelect", null, null, null);
				}
				this._this.events.Broadcast(new LoadScreenCommand(this.victoryPayload.nextScreen, null, ScreenUpdateType.Next));
			}
		}
	}

	public AudioData countdownSound;

	public AudioData startSound;

	public TextMeshProUGUI StandardTimer;

	public TextMeshProUGUI CrewsTimer;

	public int StandardTimerDecimalReduce = 22;

	public int CrewsTimerDecimalReduce = 14;

	public TextMeshProUGUI CenterText;

	public TextMeshProUGUI TopText;

	public GameObject CenterTextGroup;

	public GameObject CenterTextBg;

	public GameObject CenterTextBgNumbers;

	public GameObject TopTextGroup;

	public GameObject PauseScreenPrefab;

	public GameObject playerBarPrefab;

	public Transform playerBarContainer;

	public Transform teamBarContainer;

	private PlayerGUIBar playerBar;

	public CrewsGUI crewsGuiPrefabV1;

	public CrewsGUI crewsGuiPrefabV2;

	public CrewsGUIV3 crewsGuiPrefabV3;

	private ICrewsGUI crewsGui;

	public CrewBattleBottomBar CrewsBottomBar;

	public FloatyNameManager floatyNameManager;

	public OffscreenArrowManager arrowManager;

	public GameObject offscreenArrowPrefab;

	public Camera playerTextureCameraPrefab;

	public RawImage screenshot;

	public CanvasGroup BattleInProgressUI;

	public CanvasGroup VictoryPoseUI;

	public CanvasGroup BlackScreenUI;

	public TextMeshProUGUI VictoryPoseTitle;

	public TextMeshProUGUI VictoryPoseSubtitle;

	private bool anyButtonPressed;

	private bool isPaused;

	private PauseScreen pauseScreen;

	private bool useCrewsTimer;

	private GameObject centerTextGroup;

	private TextMeshProUGUI centerText;

	private int countdownIndex;

	private GameEndEvent endEvent;

	private StaticString timeString = new StaticString(64);

	private GenericDialog dialog;

	private int roundCountDisplay;

	private int roundCountDisplayFrames;

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public IVictoryPoseController victoryPoseController
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
	public IExitGame terminateGame
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IItemLoader itemLoader
	{
		get;
		set;
	}

	[Inject]
	public IVictoryPoseHelper victoryPoseHelper
	{
		get;
		set;
	}

	[Inject]
	public IVideoSettingsUtility videoSettingsUtility
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
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

	[Inject]
	public ISceneController sceneController
	{
		get;
		set;
	}

	[Inject]
	public ScreenshotModel screenshotModel
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

	public override bool AlwaysHideMouse
	{
		get
		{
			return !this.isPaused;
		}
	}

	private bool CanAdvanceToVictoryScreen
	{
		get
		{
			return !base.battleServerAPI.IsConnected || base.battleServerAPI.ReceivedMatchResults;
		}
	}

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

	protected override void Update()
	{
		if (this.victoryPoseController == null)
		{
			return;
		}
		this.updateCenterText();
		this.victoryPoseController.Update();
	}

	public override void Deactivate()
	{
		this.victoryPoseController.Clear();
	}

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

	private void incrementCountdownText(TextMeshProUGUI countdownText, int count, int maxCount)
	{
		if (count >= maxCount - 1)
		{
			AudioManager arg_24_0 = base.audioManager;
			AudioRequest audioRequest = new AudioRequest(this.startSound, null);
			arg_24_0.PlayGameSound(audioRequest.MaintainPitch());
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
			AudioManager arg_110_0 = base.audioManager;
			AudioRequest audioRequest2 = new AudioRequest(this.countdownSound, null);
			arg_110_0.PlayGameSound(audioRequest2.MaintainPitch());
		}
	}

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

	private void onPlayerLeaveMatchConfirm()
	{
		base.gameController.currentGame.ForfeitGame(ScreenType.VictoryGUI);
	}

	private void onPlayerLeaveMatchClose()
	{
		this.dialog = null;
	}

	private void onTransitionToVictoryPose(GameEvent message)
	{
		BattleUI._onTransitionToVictoryPose_c__AnonStorey1 _onTransitionToVictoryPose_c__AnonStorey = new BattleUI._onTransitionToVictoryPose_c__AnonStorey1();
		_onTransitionToVictoryPose_c__AnonStorey._this = this;
		_onTransitionToVictoryPose_c__AnonStorey.command = (TransitionToVictoryPoseCommand)message;
		this.screenshotModel.SaveScreenshot(new Action(_onTransitionToVictoryPose_c__AnonStorey.__m__0));
	}

	private IEnumerator playVictoryPoseTransitions(VictoryScreenPayload victoryPayload)
	{
		BattleUI._playVictoryPoseTransitions_c__Iterator0 _playVictoryPoseTransitions_c__Iterator = new BattleUI._playVictoryPoseTransitions_c__Iterator0();
		_playVictoryPoseTransitions_c__Iterator.victoryPayload = victoryPayload;
		_playVictoryPoseTransitions_c__Iterator._this = this;
		return _playVictoryPoseTransitions_c__Iterator;
	}

	private void loadVictoryScreen(VictoryScreenPayload victoryPayload)
	{
		BattleUI._loadVictoryScreen_c__AnonStorey2 _loadVictoryScreen_c__AnonStorey = new BattleUI._loadVictoryScreen_c__AnonStorey2();
		_loadVictoryScreen_c__AnonStorey.victoryPayload = victoryPayload;
		_loadVictoryScreen_c__AnonStorey._this = this;
		this.screenshotModel.SaveScreenshot(new Action(_loadVictoryScreen_c__AnonStorey.__m__0));
	}

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

	private void onRoundCountDisplay(int count)
	{
		this.roundCountDisplay = count;
		this.roundCountDisplayFrames = 180;
	}

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

	private bool shouldDisplayTimer()
	{
		return base.gameController.currentGame.CurrentGameMode.ShouldDisplayTimer && !this.isPaused && !this.centerTextGroup.activeSelf;
	}

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

	private void onShowPauseScreen(bool paused)
	{
		if (this.PauseScreenPrefab != null)
		{
			if (paused)
			{
				this.isPaused = true;
				this.pauseScreen.gameObject.SetActive(true);
				UIInputModule uIInputModule = base.uiManager.CurrentInputModule as UIInputModule;
				uIInputModule.ScreenDelegate = this.pauseScreen;
				uIInputModule.ButtonDelegate = this.pauseScreen;
				uIInputModule.SupressButtonsPressedThisFrame();
				this.pauseScreen.Initialize(PlayerNum.None);
				this.FirstSelected = this.pauseScreen.FirstSelected;
				this.updateCenterText();
			}
			else
			{
				this.isPaused = false;
				this.FirstSelected = this.pauseScreen.PreviousSelected;
				UIInputModule uIInputModule2 = base.uiManager.CurrentInputModule as UIInputModule;
				uIInputModule2.ScreenDelegate = null;
				uIInputModule2.ButtonDelegate = null;
				if (this.pauseScreen != null)
				{
					this.pauseScreen.gameObject.SetActive(false);
				}
			}
			base.uiManager.OnUpdateMouseMode();
		}
	}

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
				UIInputModule uIInputModule = base.uiManager.CurrentInputModule as UIInputModule;
				uIInputModule.ScreenDelegate = this.pauseScreen;
				uIInputModule.ButtonDelegate = this.pauseScreen;
				uIInputModule.SupressButtonsPressedThisFrame();
				this.pauseScreen.Initialize(gamePausedEvent.player);
				this.FirstSelected = this.pauseScreen.FirstSelected;
				this.updateCenterText();
			}
			else
			{
				this.isPaused = false;
				this.FirstSelected = this.pauseScreen.PreviousSelected;
				UIInputModule uIInputModule2 = base.uiManager.CurrentInputModule as UIInputModule;
				uIInputModule2.ScreenDelegate = null;
				uIInputModule2.ButtonDelegate = null;
				if (this.pauseScreen != null)
				{
					UnityEngine.Object.Destroy(this.pauseScreen.gameObject);
				}
			}
			base.uiManager.OnUpdateMouseMode();
		}
	}

	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		this.anyButtonPressed = true;
	}

	public void getVictoryPoseDurations(VictoryScreenPayload victoryPayload, out float unskippableDuration, out float totalDuration)
	{
		unskippableDuration = 0f;
		totalDuration = 0f;
		foreach (PlayerNum current in victoryPayload.victors)
		{
			PlayerSelectionInfo player = victoryPayload.gamePayload.players.GetPlayer(current);
			EquippableItem equippedVictoryPoseItem = this.victoryPoseHelper.getEquippedVictoryPoseItem(player, victoryPayload.gamePayload);
			VictoryPoseData victoryPoseData = this.itemLoader.LoadAsset<VictoryPoseData>(equippedVictoryPoseItem);
			if (victoryPoseData != null)
			{
				unskippableDuration = Mathf.Max(unskippableDuration, victoryPoseData.minDuration);
				totalDuration = Mathf.Max(totalDuration, victoryPoseData.maxDuration);
			}
		}
	}
}
