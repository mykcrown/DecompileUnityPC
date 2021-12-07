// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class UIScreenAdapter : IUIAdapter
{
	private class Request
	{
		public ScreenType type;

		public Payload payload;
	}

	private sealed class _goToScreen_c__AnonStorey0
	{
		internal InputBlock block;

		internal int thisAttemptId;

		internal GameScreen prefab;

		internal ScreenType type;

		internal Payload payload;

		internal UIScreenAdapter _this;

		internal void __m__0(LoadSequenceResults data)
		{
			this._this.inputBlocker.Release(this.block);
			if (this._this.loadingAttemptId == this.thisAttemptId)
			{
				this._this.incrementLoadingAttemptId();
				this._this.onDataLoadComplete(this.prefab, data, this.type, this.payload);
			}
		}
	}

	private sealed class _onDataLoadComplete_c__AnonStorey2
	{
		internal GameScreen prefab;

		internal LoadSequenceResults data;

		internal ScreenType type;

		internal Payload payload;

		internal UIScreenAdapter _this;
	}

	private sealed class _onDataLoadComplete_c__AnonStorey1
	{
		internal InputBlock block;

		internal int thisAttemptId;

		internal UIScreenAdapter._onDataLoadComplete_c__AnonStorey2 __f__ref_2;

		internal void __m__0()
		{
			this.__f__ref_2._this.inputBlocker.Release(this.block);
			if (this.__f__ref_2._this.loadingAttemptId == this.thisAttemptId)
			{
				this.__f__ref_2._this.loadingScreenType = ScreenType.None;
				this.__f__ref_2._this.incrementLoadingAttemptId();
				this.__f__ref_2._this.postLoadCreateScreen(this.__f__ref_2.prefab, this.__f__ref_2.data, this.__f__ref_2.type, this.__f__ref_2.payload);
			}
		}
	}

	public static string SCREEN_CHANGED = "UIScreenAdapter.SCREEN_CHANGED";

	protected IEvents events;

	private IScreenDisplay display;

	private ScreenType currentScreenType = ScreenType.None;

	private ScreenType previousScreenType = ScreenType.None;

	private GameScreen currentScreen;

	private int loadingAttemptId;

	private ScreenType loadingScreenType = ScreenType.None;

	private Payload payload;

	protected UIScreenController controller;

	private List<Type> controllerSubscribedTypes = new List<Type>();

	private GenericDialog dialog;

	private NetErrorCode netErrorCode;

	private UIScreenAdapter.Request pendingRequest;

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public IUILoaderBindings loadBindings
	{
		get;
		set;
	}

	[Inject]
	public ILoadDataSequence dataLoader
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IInputBlocker inputBlocker
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
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
	public PlayerInputLocator playerInput
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
	public IScreenPermissions screenPermissions
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController lobbyController
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public IPreviousCrashDetector previousCrashDetector
	{
		get;
		set;
	}

	[Inject]
	public ISoundFileManager soundFileManager
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

	public ScreenType CurrentScreen
	{
		get
		{
			return this.currentScreenType;
		}
	}

	public ScreenType PreviousScreen
	{
		get
		{
			return this.previousScreenType;
		}
	}

	public ScreenType LoadingScreen
	{
		get
		{
			return this.loadingScreenType;
		}
	}

	public void Initialize(IEvents events, IScreenDisplay display)
	{
		this.display = display;
		this.events = events;
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.syncSceneDisplay));
		events.Subscribe(typeof(LoadScreenCommand), new Events.EventHandler(this.onLoadScreenCommand));
		events.Subscribe(typeof(DisconnectUpdate), new Events.EventHandler(this.onDisconnect));
	}

	private void onDisconnect(GameEvent message)
	{
		DisconnectUpdate disconnectUpdate = message as DisconnectUpdate;
		if (this.currentScreenType == ScreenType.BattleGUI)
		{
			if (this.gameController.currentGame != null && !this.gameController.currentGame.EndedGame)
			{
				this.gameController.currentGame.ForfeitGame(ScreenType.MainMenu);
			}
		}
		else
		{
			this.events.Broadcast(new LeaveRoomCommand());
			this.attemptLoadScreen((!this.lobbyController.IsInLobby) ? ScreenType.MainMenu : ScreenType.CustomLobbyScreen, this.payload, ScreenUpdateType.Previous);
		}
		if (this.dialog == null)
		{
			this.netErrorCode = disconnectUpdate.errorCode;
			this.dialog = this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.OnlineGame.Error.title"), this.localization.GetText(NetErrorCodeUtil.GetErrorCodeString(disconnectUpdate.errorCode)), this.localization.GetText("dialog.exit"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			this.dialog.CloseCallback = new Action(this._onDisconnect_m__0);
		}
		else if (NetErrorCodeUtil.ShouldReplaceError(this.netErrorCode, disconnectUpdate.errorCode))
		{
			this.netErrorCode = disconnectUpdate.errorCode;
			this.dialog.SetBody(this.localization.GetText(NetErrorCodeUtil.GetErrorCodeString(disconnectUpdate.errorCode)));
		}
	}

	public bool AtOrGoingTo(ScreenType screen)
	{
		return this.loadingScreenType == screen || this.currentScreenType == screen;
	}

	private void onLoadScreenCommand(GameEvent message)
	{
		LoadScreenCommand loadScreenCommand = message as LoadScreenCommand;
		this.attemptLoadScreen(loadScreenCommand.type, loadScreenCommand.payload, loadScreenCommand.updateType);
	}

	private void attemptLoadScreen(ScreenType type, Payload payload, ScreenUpdateType updateType)
	{
		type = this.screenPermissions.GetRedirect(type);
		if (this.currentScreen != null)
		{
			this.playTransitionSound(type, updateType);
		}
		this.timer.CancelTimeout(new Action(this.loadScreenAfterMusicEnd));
		this.pendingRequest = null;
		if (type == ScreenType.StoreScreen)
		{
			this.pendingRequest = new UIScreenAdapter.Request();
			this.pendingRequest.type = type;
			this.pendingRequest.payload = payload;
			float num = 0.35f;
			this.timer.SetTimeout((int)(num * 1000f), new Action(this.loadScreenAfterMusicEnd));
			this.audioManager.StopMusic(null, num);
		}
		else
		{
			this.goToScreen(type, payload);
		}
	}

	private void loadScreenAfterMusicEnd()
	{
		if (this.pendingRequest != null)
		{
			this.goToScreen(this.pendingRequest.type, this.pendingRequest.payload);
		}
	}

	private void playTransitionSound(ScreenType type, ScreenUpdateType updateType)
	{
		AudioData sound = AudioData.Empty;
		if (updateType == ScreenUpdateType.Previous)
		{
			sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.generic_screenBackTransition);
		}
		else if (updateType == ScreenUpdateType.Next)
		{
			switch (type)
			{
			case ScreenType.SelectStage:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.stageSelect_stageSelectScreenOpen);
				goto IL_125;
			case ScreenType.LoadingBattle:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.loadingBattle_loadingBattleScreenOpen);
				goto IL_125;
			case ScreenType.CharacterSelect:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.characterSelect_characterSelectScreenOpen);
				goto IL_125;
			case ScreenType.SettingsScreen:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.settings_settingsScreenOpen);
				goto IL_125;
			case ScreenType.VictoryGUI:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.victoryScreen_victoryScreenOpen);
				goto IL_125;
			case ScreenType.CustomLobbyScreen:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.customLobby_customLobbyScreenOpen);
				goto IL_125;
			case ScreenType.StoreScreen:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.store_storeScreenOpen);
				goto IL_125;
			case ScreenType.OnlineBlindPick:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.characterSelect_onlineBlindPickScreenOpen);
				goto IL_125;
			case ScreenType.PlayerProgression:
				sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.playerProgression_playerProgressionScreenOpen);
				goto IL_125;
			}
			sound = this.soundFileManager.GetSoundAsAudioData(SoundKey.generic_screenForwardTransition);
		}
		IL_125:
		this.audioManager.PlayMenuSound(sound, 0f);
	}

	public void PreloadScreen(ScreenType type)
	{
		if (this.currentScreenType == type || this.loadingScreenType == type)
		{
			return;
		}
		GameScreen screenByType = this.display.GetScreenByType(type);
		this.createScreen(screenByType, type, null);
		this.display.ShowScreen(screenByType, null, null, true);
	}

	public void ShowPreloaded()
	{
		this.events.Broadcast(new AllPlayersReadyMessage());
	}

	public void OnGameSynced()
	{
		this.display.ClearPreviousScreen();
	}

	private void goToScreen(ScreenType type, Payload payload)
	{
		UIScreenAdapter._goToScreen_c__AnonStorey0 _goToScreen_c__AnonStorey = new UIScreenAdapter._goToScreen_c__AnonStorey0();
		_goToScreen_c__AnonStorey.type = type;
		_goToScreen_c__AnonStorey.payload = payload;
		_goToScreen_c__AnonStorey._this = this;
		if (this.currentScreenType == _goToScreen_c__AnonStorey.type || this.loadingScreenType == _goToScreen_c__AnonStorey.type)
		{
			return;
		}
		_goToScreen_c__AnonStorey.prefab = this.display.GetScreenByType(_goToScreen_c__AnonStorey.type);
		_goToScreen_c__AnonStorey.block = this.inputBlocker.Request();
		this.loadingScreenType = _goToScreen_c__AnonStorey.type;
		this.incrementLoadingAttemptId();
		_goToScreen_c__AnonStorey.thisAttemptId = this.loadingAttemptId;
		UILoadBindings.SoundBundleDef[] soundBindings = this.loadBindings.GetSoundBindings(_goToScreen_c__AnonStorey.prefab.GetType());
		for (int i = 0; i < soundBindings.Length; i++)
		{
			UILoadBindings.SoundBundleDef soundBundleDef = soundBindings[i];
			this.soundFileManager.PreloadBundle(soundBundleDef.key, soundBundleDef.preloadIndividualSounds);
		}
		this.dataLoader.Load(this.loadBindings.GetBindings(_goToScreen_c__AnonStorey.prefab.GetType()), new Action<LoadSequenceResults>(_goToScreen_c__AnonStorey.__m__0));
	}

	private void incrementLoadingAttemptId()
	{
		this.loadingAttemptId++;
		if (this.loadingAttemptId > 2147483647)
		{
			this.loadingAttemptId = 0;
		}
	}

	private void onDataLoadComplete(GameScreen prefab, LoadSequenceResults data, ScreenType type, Payload payload)
	{
		UIScreenAdapter._onDataLoadComplete_c__AnonStorey2 _onDataLoadComplete_c__AnonStorey = new UIScreenAdapter._onDataLoadComplete_c__AnonStorey2();
		_onDataLoadComplete_c__AnonStorey.prefab = prefab;
		_onDataLoadComplete_c__AnonStorey.data = data;
		_onDataLoadComplete_c__AnonStorey.type = type;
		_onDataLoadComplete_c__AnonStorey.payload = payload;
		_onDataLoadComplete_c__AnonStorey._this = this;
		bool ignoreDataDependencies = this.gameDataManager.ConfigData.uiConfig.ignoreDataDependencies;
		if (_onDataLoadComplete_c__AnonStorey.data.status != DataLoadStatus.SUCCESS && !ignoreDataDependencies)
		{
			this.loadingScreenType = ScreenType.None;
			this.dialogController.ShowOneButtonDialog("TEMP MESSAGE", this.localization.GetText("ui.screen.loadFailed"), "OK", WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
		else
		{
			UIScreenAdapter._onDataLoadComplete_c__AnonStorey1 _onDataLoadComplete_c__AnonStorey2 = new UIScreenAdapter._onDataLoadComplete_c__AnonStorey1();
			_onDataLoadComplete_c__AnonStorey2.__f__ref_2 = _onDataLoadComplete_c__AnonStorey;
			this.incrementLoadingAttemptId();
			_onDataLoadComplete_c__AnonStorey2.thisAttemptId = this.loadingAttemptId;
			_onDataLoadComplete_c__AnonStorey2.block = this.inputBlocker.Request();
			this.sceneController.PreloadUIScene(_onDataLoadComplete_c__AnonStorey.type, new Action(_onDataLoadComplete_c__AnonStorey2.__m__0));
		}
	}

	private void postLoadCreateScreen(GameScreen prefab, LoadSequenceResults data, ScreenType type, Payload payload)
	{
		this.createScreen(prefab, type, payload);
		this.playerInput.Input.ConsumeButtonPresses();
		Payload payload2 = null;
		if (payload != null)
		{
			payload2 = (payload.Clone() as Payload);
		}
		this.display.ShowScreen(prefab, payload2, data, false);
	}

	private void createScreen(GameScreen prefab, ScreenType type, Payload payload)
	{
		foreach (Type current in this.controllerSubscribedTypes)
		{
			this.events.Unsubscribe(current, new Events.EventHandler(this.onLocalEvent));
		}
		this.controllerSubscribedTypes.Clear();
		if (this.controller != null)
		{
			this.controller.Unregister();
		}
		this.previousScreenType = this.currentScreenType;
		this.currentScreenType = type;
		if (this.gameController.currentGame == null)
		{
			this.previousCrashDetector.UpdateStatus(PreviousCrashDetector.GameStatus.Menu);
		}
		if (this.previousScreenType == ScreenType.None)
		{
			this.syncSceneDisplay();
		}
		this.controller = this.injector.GetInstance<UIScreenController>(type);
		if (this.controller != null)
		{
			this.controller.Register(this);
			this.controller.payload = payload;
			this.controller.Initialize();
		}
		this.payload = payload;
		this.signalBus.Dispatch(UIScreenAdapter.SCREEN_CHANGED);
	}

	private void syncSceneDisplay()
	{
		this.sceneController.ActivateUIScene(this.currentScreenType);
	}

	public void OnScreenCreated(GameScreen screen)
	{
		this.currentScreen = screen;
		Payload payload = null;
		if (this.payload != null)
		{
			payload = (this.payload.Clone() as Payload);
		}
		this.currentScreen.LoadPayload(payload);
	}

	public void Subscribe(Type type)
	{
		this.controllerSubscribedTypes.Add(type);
		this.events.Subscribe(type, new Events.EventHandler(this.onLocalEvent));
	}

	public void SendUpdate(GameEvent message)
	{
		this.handleUIUpdate(message);
	}

	private void handleUIUpdate(GameEvent update)
	{
		if (update is UIPayloadUpdate && this.currentScreen != null)
		{
			Payload payload = null;
			if (this.payload != null)
			{
				payload = (this.payload.Clone() as Payload);
			}
			this.currentScreen.UpdatePayload(payload);
		}
	}

	protected void onLocalEvent(GameEvent message)
	{
		Type type = message.GetType();
		this.controller.HandleRequest(message);
	}

	public T GetUIScene<T>() where T : UIScene
	{
		return this.sceneController.GetUIScene<T>();
	}

	private void _onDisconnect_m__0()
	{
		if (this.dialog != null)
		{
			this.dialog.Close();
		}
		this.dialog = null;
		this.netErrorCode = NetErrorCode.None;
	}
}
