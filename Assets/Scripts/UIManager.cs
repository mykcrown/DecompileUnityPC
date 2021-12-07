// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using InControl;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
public class UIManager : ITickable, IScreenDisplay, IWindowDisplay, IGamewideOverlayDisplay
{
	private sealed class _gameCompleteTransition_c__AnonStorey0
	{
		internal float flipTime;

		internal UIManager _this;

		internal void __m__0()
		{
			this._this.onTransitionBegin();
			DOTween.To(new DOGetter<float>(this.__m__1), new DOSetter<float>(this.__m__2), 0f, this.flipTime).SetEase(Ease.OutSine);
			DOTween.To(new DOGetter<float>(this.__m__3), new DOSetter<float>(this.__m__4), 1f, this.flipTime).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this._this.onTransitionComplete));
		}

		internal float __m__1()
		{
			return this._this.prevScreen.Alpha;
		}

		internal void __m__2(float x)
		{
			this._this.prevScreen.Alpha = x;
		}

		internal float __m__3()
		{
			return this._this.CurrentScreen.Alpha;
		}

		internal void __m__4(float x)
		{
			this._this.CurrentScreen.Alpha = x;
		}
	}

	private sealed class _Remove_c__AnonStorey1
	{
		internal BaseWindow window;

		internal UIManager _this;
	}

	private sealed class _Remove_c__AnonStorey2
	{
		internal GameObject obj;

		internal UIManager._Remove_c__AnonStorey1 __f__ref_1;

		internal void __m__0()
		{
			if (this.__f__ref_1._this.isInWindow(this.__f__ref_1.window, EventSystem.current.currentSelectedGameObject))
			{
				this.__f__ref_1._this.selectionManager.Select(this.__f__ref_1.window.PreviousSelection);
			}
			foreach (BaseWindow current in this.__f__ref_1._this.activeWindows)
			{
				if (this.__f__ref_1._this.isInWindow(this.__f__ref_1.window, current.PreviousSelection))
				{
					current.PreviousSelection = this.__f__ref_1.window.PreviousSelection;
				}
			}
			UnityEngine.Object.Destroy(this.obj);
			this.__f__ref_1._this.signalBus.Dispatch(UIManager.WINDOW_CLOSED);
		}
	}

	private sealed class _Remove_c__AnonStorey3
	{
		internal GameObject obj;

		internal UIManager _this;

		internal void __m__0()
		{
			UnityEngine.Object.Destroy(this.obj);
			if (this._this.isInGamewideOverlayContainer(EventSystem.current.currentSelectedGameObject))
			{
				this._this.selectionManager.Select(null);
			}
			this._this.signalBus.Dispatch(UIManager.GAMEWIDE_OVERLAY_CLOSED);
		}
	}

	public static string SCREEN_CLOSED = "UIManager.SCREEN_CLOSED";

	public static string SCREEN_OPENED = "UIManager.SCREEN_OPENED";

	public static string WINDOW_CLOSED = "UIManager.WINDOW_CLOSED";

	public static string GAMEWIDE_OVERLAY_CLOSED = "UIManager.GAMEWIDE_OVERLAY_CLOSED";

	public static string RAYCAST_UPDATE = "UIManager.RAYCAST_UPDATE";

	private Transform screenContainer;

	private Transform windowContainer;

	private Transform gamewideOverlayContainer;

	private CanvasGroup windowCanvasGroup;

	private CanvasGroup screenCanvasGroup;

	private CanvasGroup gamewideOverlayCanvasGroup;

	private Transform cursorContainer;

	private bool debugTextEnabled;

	private static readonly int globalTextEventCount = 12;

	private Queue<string> textEventQueue = new Queue<string>();

	public List<GUIText> PlayerDebugText = new List<GUIText>();

	private Color debugTextColor = Color.white;

	private bool drawComplete;

	private bool setupComplete;

	private bool debugTextEventsDirty;

	private bool waitToClearPrevious;

	private GameClient client;

	private ICustomInputModule currentInputModule;

	private ICustomInputModule lastActiveInputModule;

	private Dictionary<UIInputModuleType, ICustomInputModule> inputModulesByType;

	private GameScreen prevScreen;

	private bool isCurrentScreenDrawing;

	private NetworkHealthReporter healthReporter;

	private List<WindowRequest> queuedWindows = new List<WindowRequest>();

	private List<BaseWindow> activeWindows = new List<BaseWindow>();

	private List<GamewideOverlayRequest> queuedOverlays = new List<GamewideOverlayRequest>();

	private List<BaseGamewideOverlay> activeGamewideOverlays = new List<BaseGamewideOverlay>();

	private UIConfig guiConfig;

	[Inject]
	public IUIAdapter adapter
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
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
	public IInputBlocker inputBlocker
	{
		get;
		set;
	}

	[Inject]
	public WindowTransitionManager windowTransitionManager
	{
		get;
		set;
	}

	[Inject]
	public GamewideOverlayTransitionManager gamewideOverlayTransitionManager
	{
		get;
		set;
	}

	[Inject]
	public IScreenTransitionMap screenTransitions
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
	public ICursorManager cursorManager
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
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
	public IKeyBindingManager keyBindingManager
	{
		get;
		set;
	}

	[Inject]
	public ISelectionManager selectionManager
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
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
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		get;
		set;
	}

	private CanvasGroup canvasGroup
	{
		get;
		set;
	}

	private Canvas canvas
	{
		get;
		set;
	}

	public Canvas CameraCanvas
	{
		get;
		set;
	}

	public EventSystem EventSystem
	{
		get;
		private set;
	}

	public GraphicRaycaster GraphicRaycaster
	{
		get;
		private set;
	}

	public GUIText GlobalDebugText
	{
		get;
		private set;
	}

	public GUIText DebugTextEvents
	{
		get;
		private set;
	}

	public ICustomInputModule CurrentInputModule
	{
		get
		{
			return this.currentInputModule;
		}
		private set
		{
			if (this.currentInputModule != null)
			{
				this.lastActiveInputModule = this.currentInputModule;
			}
			this.currentInputModule = value;
			if (this.currentInputModule != null)
			{
				this.currentInputModule.uiManager = this;
				if (this.lastActiveInputModule != null)
				{
					this.currentInputModule.InitControlMode(this.lastActiveInputModule.CurrentMode);
				}
			}
			this.OnUpdateMouseMode();
		}
	}

	public GameScreen CurrentScreen
	{
		get;
		private set;
	}

	public PlayerInputPort LockingPort
	{
		get
		{
			return (!(this.CurrentInputModule is UIInputModule)) ? null : ((UIInputModule)this.CurrentInputModule).LockingPort;
		}
		set
		{
			if (this.CurrentInputModule is UIInputModule)
			{
				((UIInputModule)this.CurrentInputModule).LockingPort = value;
			}
		}
	}

	public bool IsTextEntryMode
	{
		get
		{
			return this.CurrentInputModule != null && this.CurrentInputModule.CurrentInputField != null;
		}
	}

	public bool DebugTextEnabled
	{
		get
		{
			return this.debugTextEnabled;
		}
		set
		{
			foreach (GUIText current in this.PlayerDebugText)
			{
				current.enabled = value;
			}
			this.GlobalDebugText.enabled = value;
			this.DebugTextEvents.enabled = value;
			this.debugTextEnabled = value;
		}
	}

	public void Init(UIConfig gui, GameClient client)
	{
		this.guiConfig = gui;
		this.client = client;
		this.adapter.Initialize(this.events, this);
		this.events.Subscribe(typeof(ToggleUIVisibilityCommand), new Events.EventHandler(this.onToggleUIVisibility));
		this.signalBus.GetSignal<InputLockUpdateSignal>().AddListener(new Action(this.updateModuleEnabledState));
		this.signalBus.AddListener(KeyBindingManager.UPDATED, new Action(this.updateModuleEnabledState));
		this.signalBus.AddListener(UIManager.RAYCAST_UPDATE, new Action(this.onRaycastUpdate));
		this.canvasGroup = client.gameObject.GetComponentInChildren<CanvasGroup>();
		this.canvas = client.gameObject.GetComponentInChildren<Canvas>();
		CanvasContainer component = this.canvas.gameObject.GetComponent<CanvasContainer>();
		this.screenContainer = component.ScreenContainer;
		this.windowContainer = component.WindowContainer;
		this.gamewideOverlayContainer = component.GamewideOverlayContainer;
		this.cursorContainer = component.CursorContainer;
		this.windowCanvasGroup = this.windowContainer.GetComponent<CanvasGroup>();
		this.screenCanvasGroup = this.screenContainer.GetComponent<CanvasGroup>();
		this.gamewideOverlayCanvasGroup = this.gamewideOverlayContainer.GetComponent<CanvasGroup>();
		GameObject gameObject = this.canvas.gameObject;
		this.CameraCanvas = new GameObject("CameraCanvas").AddComponent<Canvas>();
		this.CameraCanvas.transform.SetParent(client.transform);
		this.CameraCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		this.CameraCanvas.planeDistance = 10f;
		this.CameraCanvas.worldCamera = Camera.main;
		this.EventSystem = gameObject.AddComponent<EventSystem>();
		this.GraphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
		if (this.guiConfig.useCanvasScaler)
		{
			CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
			canvasScaler.defaultSpriteDPI = this.guiConfig.canvasScaler.defaultSpriteDPI;
			canvasScaler.fallbackScreenDPI = this.guiConfig.canvasScaler.fallbackScreenDPI;
			canvasScaler.matchWidthOrHeight = this.guiConfig.canvasScaler.matchWidthOrHeight;
			canvasScaler.physicalUnit = this.guiConfig.canvasScaler.physicalUnit;
			canvasScaler.referencePixelsPerUnit = this.guiConfig.canvasScaler.referencePixelsPerUnit;
			canvasScaler.referenceResolution = this.guiConfig.canvasScaler.referenceResolution;
			canvasScaler.scaleFactor = this.guiConfig.canvasScaler.scaleFactor;
			canvasScaler.screenMatchMode = this.guiConfig.canvasScaler.screenMatchMode;
			canvasScaler.uiScaleMode = this.guiConfig.canvasScaler.scaleMode;
		}
		this.inputModulesByType = new Dictionary<UIInputModuleType, ICustomInputModule>
		{
			{
				UIInputModuleType.Menu,
				this.setupMenuInputModule(gameObject)
			},
			{
				UIInputModuleType.Cursor,
				this.setupCursorInputModule(gameObject)
			}
		};
		GameObject gameObject2 = new GameObject("DebugContainer");
		gameObject2.transform.SetParent(this.canvasGroup.transform);
		this.healthReporter = new GameObject("NetHealth").AddComponent<NetworkHealthReporter>();
		this.healthReporter.transform.SetParent(component.transform);
		for (int i = 0; i < 2; i++)
		{
			GameObject gameObject3 = new GameObject("Debug" + i);
			GUIText gUIText = gameObject3.AddComponent<GUIText>();
			gUIText.pixelOffset = new Vector2((float)((i != 0) ? 1880 : 55) * ((float)Screen.width / this.guiConfig.canvasScaler.referenceResolution.x), 1070f * ((float)Screen.height / this.guiConfig.canvasScaler.referenceResolution.y));
			if (i == 0)
			{
				gUIText.text = "Debug Mode";
			}
			gUIText.anchor = ((i != 0) ? TextAnchor.UpperRight : TextAnchor.UpperLeft);
			gUIText.alignment = ((i != 0) ? TextAlignment.Right : TextAlignment.Left);
			gUIText.color = Color.white;
			gUIText.richText = true;
			gUIText.transform.SetParent(gameObject2.transform, false);
			gUIText.enabled = Debug.isDebugBuild;
			this.PlayerDebugText.Add(gUIText);
		}
		this.GlobalDebugText = new GameObject("GlobalDebug").AddComponent<GUIText>();
		this.GlobalDebugText.transform.SetParent(gameObject2.transform, false);
		this.GlobalDebugText.pixelOffset = new Vector2((float)(Screen.width / 2), (float)Screen.height - 20f);
		this.GlobalDebugText.anchor = TextAnchor.UpperCenter;
		this.GlobalDebugText.color = Color.white;
		this.GlobalDebugText.richText = true;
		this.GlobalDebugText.alignment = TextAlignment.Left;
		this.GlobalDebugText.enabled = false;
		this.DebugTextEvents = new GameObject("TextEvents").AddComponent<GUIText>();
		this.DebugTextEvents.transform.SetParent(gameObject2.transform, false);
		this.DebugTextEvents.pixelOffset = new Vector2((float)(Screen.width / 2), (float)Screen.height - 100f);
		this.DebugTextEvents.anchor = TextAnchor.UpperCenter;
		this.DebugTextEvents.color = Color.white;
		this.DebugTextEvents.richText = true;
		this.DebugTextEvents.alignment = TextAlignment.Left;
		this.DebugTextEvents.enabled = false;
		this.events.Subscribe(typeof(SetUIInputModuleCommand), new Events.EventHandler(this.onActivateUIInputModuleCommand));
		this.events.Subscribe(typeof(DeactivateUIInputModuleCommand), new Events.EventHandler(this.onDeactivateUIInputModuleCommand));
		this.events.Subscribe(typeof(ActivateCursorModuleCommand), new Events.EventHandler(this.onActivateCursorModuleCommand));
		this.events.Subscribe(typeof(DeactivateCursorModuleCommand), new Events.EventHandler(this.onDeactivateCursorModuleCommand));
		this.events.Subscribe(typeof(ClearInputStateCommand), new Events.EventHandler(this.clearInputState));
		foreach (WindowRequest current in this.queuedWindows)
		{
			this.addToWindowContainer(current);
		}
		this.queuedWindows.Clear();
	}

	private void onRaycastUpdate()
	{
		if (this.CurrentInputModule is CursorInputModule)
		{
			(this.CurrentInputModule as CursorInputModule).ReprocessHoversImmediate();
		}
	}

	private void onToggleUIVisibility(GameEvent message)
	{
		ToggleUIVisibilityCommand toggleUIVisibilityCommand = message as ToggleUIVisibilityCommand;
		this.canvasGroup.alpha = (float)((!toggleUIVisibilityCommand.visibility) ? 0 : 1);
	}

	public void SetPauseMode(bool isPause)
	{
		foreach (ICustomInputModule current in this.inputModulesByType.Values)
		{
			current.SetPauseMode(isPause);
		}
	}

	private ICustomInputModule setupMenuInputModule(GameObject canvasObj)
	{
		MenuActions menuActions = new MenuActions();
		UIInputModule uIInputModule = canvasObj.AddComponent<UIInputModule>();
		this.injector.Inject(uIInputModule);
		menuActions.Device = null;
		uIInputModule.Actions = menuActions;
		uIInputModule.enabled = false;
		uIInputModule.SubmitAction = menuActions.Submit;
		uIInputModule.CancelAction = menuActions.Cancel;
		uIInputModule.LeftStickAction = menuActions.LeftStick;
		uIInputModule.LeftAction = menuActions.Left;
		uIInputModule.RightAction = menuActions.Right;
		uIInputModule.UpAction = menuActions.Up;
		uIInputModule.DownAction = menuActions.Down;
		uIInputModule.YButtonAction = menuActions.YButtonAction;
		uIInputModule.XButtonAction = menuActions.XButtonAction;
		uIInputModule.RightTriggerAction = menuActions.RightTrigger;
		uIInputModule.LeftBumperAction = menuActions.LeftBumper;
		uIInputModule.ZButtonAction = menuActions.ZButtonAction;
		uIInputModule.LeftTriggerAction = menuActions.LeftTrigger;
		uIInputModule.RightStickAction = menuActions.RightStick;
		uIInputModule.DPadLeftAction = menuActions.DPadLeft;
		uIInputModule.DPadRightAction = menuActions.DPadRight;
		uIInputModule.DPadDownAction = menuActions.DPadDown;
		uIInputModule.DPadUpAction = menuActions.DPadUp;
		return uIInputModule;
	}

	private ICustomInputModule setupCursorInputModule(GameObject canvasObj)
	{
		CursorInputModule cursorInputModule = canvasObj.AddComponent<CursorInputModule>();
		cursorInputModule.enabled = false;
		return cursorInputModule;
	}

	private void OnDestroy()
	{
		this.events.Unsubscribe(typeof(SetUIInputModuleCommand), new Events.EventHandler(this.onActivateUIInputModuleCommand));
		this.events.Unsubscribe(typeof(DeactivateUIInputModuleCommand), new Events.EventHandler(this.onDeactivateUIInputModuleCommand));
		this.events.Unsubscribe(typeof(ActivateCursorModuleCommand), new Events.EventHandler(this.onActivateCursorModuleCommand));
		this.events.Unsubscribe(typeof(DeactivateCursorModuleCommand), new Events.EventHandler(this.onDeactivateCursorModuleCommand));
		this.events.Unsubscribe(typeof(ClearInputStateCommand), new Events.EventHandler(this.clearInputState));
		this.events.Unsubscribe(typeof(ToggleUIVisibilityCommand), new Events.EventHandler(this.onToggleUIVisibility));
	}

	void IScreenDisplay.ShowScreen(GameScreen prefab, Payload payload, LoadSequenceResults data, bool waitToClearPrevious)
	{
		if (prefab != null)
		{
			if (this.isCurrentScreenDrawing || this.waitToClearPrevious)
			{
				this.clearPreviousScreen();
			}
			this.waitToClearPrevious = waitToClearPrevious;
			this.isCurrentScreenDrawing = true;
			this.resetScreenLoadStatus();
			this.prevScreen = this.CurrentScreen;
			GameScreen gameScreen = UnityEngine.Object.Instantiate<GameScreen>(prefab);
			this.clearInputState(null);
			if (this.CurrentScreen != null)
			{
				this.CurrentScreen.OnDrawCompleteCallback = null;
				this.CurrentScreen.Deactivate();
			}
			this.CurrentScreen = gameScreen;
			this.CurrentScreen.OnDrawCompleteCallback = new Action(this.onDrawComplete);
			this.drawComplete = !this.CurrentScreen.WaitForDrawCallback;
			this.CurrentScreen.Alpha = 0f;
			this.CurrentScreen.transform.SetParent(this.screenContainer, false);
			this.CurrentScreen.transform.SetAsFirstSibling();
			this.adapter.OnScreenCreated(gameScreen);
			this.CurrentScreen.LoadData(data);
			this.CurrentScreen.OnAddedToHeirarchy();
			this.client.Input.ListenForDevices = this.CurrentScreen.listenForDevices;
			this.onSetupComplete();
		}
		else
		{
			UnityEngine.Debug.LogError("Failed to load screen, since it was null!");
		}
	}

	private void onDrawComplete()
	{
		this.drawComplete = true;
		this.checkScreenReady();
	}

	private void onSetupComplete()
	{
		this.setupComplete = true;
		this.checkScreenReady();
	}

	private void checkScreenReady()
	{
		if (this.isScreenReadyToDisplay())
		{
			this.resetScreenLoadStatus();
			ScreenTransition transition = this.screenTransitions.Get(this.prevScreen, this.CurrentScreen);
			this.doTransition(transition);
		}
	}

	private void resetScreenLoadStatus()
	{
		this.drawComplete = false;
		this.setupComplete = false;
	}

	private bool isScreenReadyToDisplay()
	{
		return this.drawComplete && this.setupComplete;
	}

	private void doTransition(ScreenTransition transition)
	{
		this.CurrentScreen.Alpha = 1f;
		if (transition == null)
		{
			this.noTransition();
		}
		else
		{
			ScreenTransitionType type = transition.type;
			if (type != ScreenTransitionType.CROSSFADE)
			{
				if (type != ScreenTransitionType.GAME_COMPLETE)
				{
					this.noTransition();
				}
				else
				{
					this.gameCompleteTransition(transition);
				}
			}
			else
			{
				this.crossfadeTransition(transition);
			}
		}
	}

	private void noTransition()
	{
		this.onTransitionBegin();
		this.onTransitionComplete();
	}

	private void crossfadeTransition(ScreenTransition transition)
	{
		this.CurrentScreen.Alpha = 0f;
		this.onTransitionBegin();
		float time = transition.time;
		DOTween.To(new DOGetter<float>(this._crossfadeTransition_m__0), new DOSetter<float>(this._crossfadeTransition_m__1), 0f, time).SetEase(Ease.OutSine);
		DOTween.To(new DOGetter<float>(this._crossfadeTransition_m__2), new DOSetter<float>(this._crossfadeTransition_m__3), 1f, time).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.onTransitionComplete));
	}

	private void gameCompleteTransition(ScreenTransition transition)
	{
		UIManager._gameCompleteTransition_c__AnonStorey0 _gameCompleteTransition_c__AnonStorey = new UIManager._gameCompleteTransition_c__AnonStorey0();
		_gameCompleteTransition_c__AnonStorey._this = this;
		this.CurrentScreen.Alpha = 0f;
		this.prevScreen.Alpha = 1f;
		_gameCompleteTransition_c__AnonStorey.flipTime = transition.time;
		float delay = transition.delay;
		this.timer.SetTimeout((int)(delay * 1000f), new Action(_gameCompleteTransition_c__AnonStorey.__m__0));
	}

	private void onTransitionBegin()
	{
		this.CurrentScreen.CheckForTransitionBegin();
	}

	private void onTransitionComplete()
	{
		if (this.CurrentScreen.FirstSelected != null)
		{
			this.selectionManager.Select(this.CurrentScreen.FirstSelected);
		}
		if (!this.waitToClearPrevious)
		{
			this.clearPreviousScreen();
		}
		this.CurrentScreen.OnTransitionComplete();
		this.isCurrentScreenDrawing = false;
		this.signalBus.Dispatch(UIManager.SCREEN_OPENED);
	}

	public void ClearPreviousScreen()
	{
		this.clearPreviousScreen();
	}

	private void clearPreviousScreen()
	{
		if (this.prevScreen != null)
		{
			this.signalBus.Dispatch(UIManager.SCREEN_CLOSED);
			UnityEngine.Object.Destroy(this.prevScreen.gameObject);
		}
		List<BaseWindow> list = new List<BaseWindow>(this.activeWindows);
		foreach (BaseWindow current in list)
		{
			if (current.Request.closeOnScreenChange && current.SourceScreen == this.adapter.PreviousScreen)
			{
				current.Close();
			}
		}
	}

	private bool shouldSelectFirstElement()
	{
		return this.isFirstElementInputActive() && !(this.CurrentScreen == null) && !(this.CurrentInputModule is CursorInputModule) && !(this.EventSystem.currentSelectedGameObject != null);
	}

	private bool isFirstElementInputActive()
	{
		return InputManager.ActiveDevice.AnyButton.IsPressed || InputManager.ActiveDevice.LeftStick.IsPressed || InputManager.AnyKeyIsPressed;
	}

	public void TickFrame()
	{
		if (this.CurrentScreen != null)
		{
			this.CurrentScreen.TickFrame();
		}
		if (this.shouldSelectFirstElement())
		{
			GameObject firstSelected = this.getFirstSelected();
			if (firstSelected != null)
			{
				this.selectionManager.Select(firstSelected);
			}
		}
		if (this.config.DebugConfig.debugTextColor != this.debugTextColor)
		{
			this.debugTextColor = this.config.DebugConfig.debugTextColor;
			for (int i = 0; i < this.PlayerDebugText.Count; i++)
			{
				this.PlayerDebugText[i].color = this.debugTextColor;
			}
		}
		if (this.debugTextEventsDirty && this.debugTextEnabled)
		{
			this.debugTextEventsDirty = false;
			this.DebugTextEvents.text = string.Empty;
			foreach (string current in this.textEventQueue)
			{
				GUIText expr_FB = this.DebugTextEvents;
				expr_FB.text = expr_FB.text + current + "\n";
			}
		}
	}

	public GUIText GetDebugText(PlayerNum player)
	{
		if (player == PlayerNum.Player1)
		{
			return this.PlayerDebugText[0];
		}
		if (player == this.config.DebugConfig.debugText2Player)
		{
			return this.PlayerDebugText[1];
		}
		return null;
	}

	public void ReportRollbackHealth(NetworkHealthReport health)
	{
		this.healthReporter.ReportHealth(health);
	}

	public void AddDebugTextEvent(string value)
	{
		if (!this.debugTextEnabled)
		{
			return;
		}
		this.textEventQueue.Enqueue(value);
		while (this.textEventQueue.Count > UIManager.globalTextEventCount)
		{
			this.textEventQueue.Dequeue();
		}
		this.debugTextEventsDirty = true;
	}

	private void clearInputState(GameEvent message = null)
	{
		if (this.CurrentInputModule != null)
		{
			this.CurrentInputModule.ShouldActivateModule();
			this.CurrentInputModule.UpdateModule();
		}
	}

	GameScreen IScreenDisplay.GetScreenByType(ScreenType type)
	{
		if (!this.guiConfig.screens.ContainsKey(type))
		{
			UnityEngine.Debug.LogError("Failed to find UI prefab for " + type.ToString() + " screen");
			return null;
		}
		return this.guiConfig.screens[type];
	}

	private void switchInputModule(UIInputModuleType type, ActivateCursorModuleCommand cursorActivate = null)
	{
		if (this.CurrentInputModule != null && this.CurrentInputModule is UIInputModule)
		{
			(this.CurrentInputModule as UIInputModule).ScreenDelegate = null;
			(this.CurrentInputModule as UIInputModule).ButtonDelegate = null;
		}
		if (this.CurrentInputModule == this.inputModulesByType[UIInputModuleType.Cursor])
		{
			CursorInputModule cursorInputModule = (CursorInputModule)this.CurrentInputModule;
			cursorInputModule.cursorObjects = null;
			cursorInputModule.cursorDelegate = null;
		}
		using (Dictionary<UIInputModuleType, ICustomInputModule>.ValueCollection.Enumerator enumerator = this.inputModulesByType.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BaseInputModule baseInputModule = (BaseInputModule)enumerator.Current;
				baseInputModule.enabled = false;
			}
		}
		if (type != UIInputModuleType.None)
		{
			this.CurrentInputModule = this.inputModulesByType[type];
			this.updateModuleEnabledState();
			if (this.CurrentInputModule as CursorInputModule)
			{
				(this.CurrentInputModule as CursorInputModule).SuppressButtonsPressedThisFrame();
			}
			this.clearInputState(null);
			if (type == UIInputModuleType.Cursor)
			{
				CursorInputModule cursorInputModule2 = (CursorInputModule)this.CurrentInputModule;
				cursorInputModule2.cursorObjects = cursorActivate.cursors;
			}
		}
		else
		{
			this.CurrentInputModule = null;
		}
		this.updateInputDelegate();
	}

	private ICursorInputDelegate getCursorDelegate()
	{
		if (this.activeWindows.Count > 0)
		{
			return this.activeWindows[this.activeWindows.Count - 1];
		}
		if (this.CurrentScreen != null)
		{
			return this.CurrentScreen;
		}
		return null;
	}

	private IScreenInputDelegate getScreenDelegate()
	{
		if (this.CurrentScreen != null)
		{
			return this.CurrentScreen;
		}
		return null;
	}

	private IButtonInputDelegate getButtonDelegate()
	{
		if (this.activeWindows.Count > 0)
		{
			return this.activeWindows[this.activeWindows.Count - 1];
		}
		if (this.CurrentScreen != null)
		{
			return this.CurrentScreen;
		}
		return null;
	}

	private List<IGlobalInputDelegate> getGlobalDelegates()
	{
		List<IGlobalInputDelegate> list = new List<IGlobalInputDelegate>();
		foreach (BaseGamewideOverlay current in this.activeGamewideOverlays)
		{
			list.Add(current);
		}
		return list;
	}

	private GameObject getFirstSelected()
	{
		if (this.activeWindows.Count > 0)
		{
			return this.activeWindows[this.activeWindows.Count - 1].FirstSelected;
		}
		if (this.CurrentScreen != null)
		{
			return this.CurrentScreen.FirstSelected;
		}
		return null;
	}

	private void onActivateUIInputModuleCommand(GameEvent message)
	{
		SetUIInputModuleCommand setUIInputModuleCommand = message as SetUIInputModuleCommand;
		this.switchInputModule(setUIInputModuleCommand.type, null);
	}

	private void onDeactivateUIInputModuleCommand(GameEvent message)
	{
		this.switchInputModule(UIInputModuleType.None, null);
	}

	private void onActivateCursorModuleCommand(GameEvent message)
	{
		ActivateCursorModuleCommand cursorActivate = message as ActivateCursorModuleCommand;
		this.switchInputModule(UIInputModuleType.Cursor, cursorActivate);
	}

	public void AddCursor(PlayerCursor cursor)
	{
		cursor.transform.SetParent(this.cursorContainer, false);
		cursor.InitMode((!this.shouldDisplayMouse()) ? global::CursorMode.Controller : global::CursorMode.Mouse);
	}

	private void onDeactivateCursorModuleCommand(GameEvent message)
	{
		this.switchInputModule(UIInputModuleType.None, null);
	}

	public void OnUpdateMouseMode()
	{
		bool blocksRaycasts = this.allowMouseInteraction();
		this.windowCanvasGroup.blocksRaycasts = blocksRaycasts;
		this.screenCanvasGroup.blocksRaycasts = blocksRaycasts;
		this.gamewideOverlayCanvasGroup.blocksRaycasts = blocksRaycasts;
		if (this.CurrentInputModule != null)
		{
			this.cursorManager.SetDisplay(this.shouldDisplayMouse());
		}
	}

	private bool allowMouseInteraction()
	{
		return this.CurrentInputModule == null || !(this.CurrentInputModule is UIInputModule) || this.CurrentInputModule.IsMouseMode;
	}

	private bool shouldDisplayMouse()
	{
		if (this.CurrentScreen != null && this.CurrentScreen.AlwaysHideMouse)
		{
			return false;
		}
		if (this.CurrentInputModule != null)
		{
			return this.CurrentInputModule.IsMouseMode;
		}
		return this.lastActiveInputModule == null || this.lastActiveInputModule.IsMouseMode;
	}

	public T Add<T>(GameObject prefab, WindowTransition transition, bool pushToBack = false, bool closeOnScreenChange = false, bool useOverrideOpenSound = false, AudioData overrideOpenSound = default(AudioData)) where T : BaseWindow
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		T component = gameObject.GetComponent<T>();
		this.injector.Inject(component);
		WindowRequest windowRequest = new WindowRequest();
		windowRequest.window = component;
		windowRequest.inTransition = transition;
		windowRequest.outTransition = transition;
		windowRequest.pushToBack = pushToBack;
		windowRequest.closeOnScreenChange = closeOnScreenChange;
		component.Request = windowRequest;
		component.IsRemoving = false;
		if (useOverrideOpenSound)
		{
			component.UseOverrideOpenSound = useOverrideOpenSound;
			component.OverrideOpenSound = overrideOpenSound;
		}
		if (this.windowContainer == null)
		{
			this.queuedWindows.Add(windowRequest);
		}
		else
		{
			this.addToWindowContainer(windowRequest);
		}
		return component;
	}

	private void addToWindowContainer(WindowRequest request)
	{
		BaseWindow window = request.window;
		window.SourceScreen = this.adapter.CurrentScreen;
		window.IsRemoving = false;
		window.gameObject.transform.SetParent(this.windowContainer, false);
		Vector3 position = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2));
		window.gameObject.transform.position = position;
		this.windowTransitionManager.ShowInTransition(window, request.inTransition);
		window.CloseRequest = new Action<BaseWindow>(this.windowRequestsClose);
		if (this.activeWindows.Count == 0)
		{
			if (this.CurrentScreen != null)
			{
				this.CurrentScreen.OnWindowsOpened();
			}
			this.switchInputModule(UIInputModuleType.Menu, null);
		}
		if (request.pushToBack)
		{
			this.activeWindows.Insert(0, window);
			window.gameObject.transform.SetAsFirstSibling();
		}
		else
		{
			this.activeWindows.Add(window);
		}
		window.Open();
		BaseWindow window2 = this.activeWindows[this.activeWindows.Count - 1];
		this.updateSelectionOnWindowOpen(window2);
		this.updateInputDelegate();
	}

	private void updateSelectionOnWindowOpen(BaseWindow window)
	{
		window.PreviousSelection = EventSystem.current.currentSelectedGameObject;
		if (this.CurrentInputModule is CursorInputModule)
		{
			(this.CurrentInputModule as CursorInputModule).SuppressButtonsPressedThisFrame();
		}
		GameObject obj = window.FirstSelected;
		UIInputModule uIInputModule = this.CurrentInputModule as UIInputModule;
		if (uIInputModule == null)
		{
			obj = null;
		}
		else if (uIInputModule.IsMouseMode)
		{
			obj = null;
		}
		this.selectionManager.Select(obj);
		window.ReadyForSelections();
	}

	public void Remove(BaseWindow window)
	{
		UIManager._Remove_c__AnonStorey1 _Remove_c__AnonStorey = new UIManager._Remove_c__AnonStorey1();
		_Remove_c__AnonStorey.window = window;
		_Remove_c__AnonStorey._this = this;
		if (!_Remove_c__AnonStorey.window.IsRemoving)
		{
			UIManager._Remove_c__AnonStorey2 _Remove_c__AnonStorey2 = new UIManager._Remove_c__AnonStorey2();
			_Remove_c__AnonStorey2.__f__ref_1 = _Remove_c__AnonStorey;
			_Remove_c__AnonStorey.window.IsRemoving = true;
			this.activeWindows.Remove(_Remove_c__AnonStorey.window);
			if (this.activeWindows.Count == 0 && this.CurrentScreen != null)
			{
				this.CurrentScreen.OnWindowsClosed();
			}
			_Remove_c__AnonStorey2.obj = _Remove_c__AnonStorey.window.gameObject;
			this.updateInputDelegate();
			this.windowTransitionManager.ShowOutTransition(_Remove_c__AnonStorey.window, _Remove_c__AnonStorey.window.Request.outTransition, new Action(_Remove_c__AnonStorey2.__m__0));
		}
	}

	private bool isInWindow(BaseWindow window, GameObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		BaseWindow componentInParent = obj.GetComponentInParent<BaseWindow>();
		return window == componentInParent;
	}

	private void windowRequestsClose(BaseWindow window)
	{
		this.Remove(window);
	}

	public int GetWindowCount()
	{
		return this.activeWindows.Count;
	}

	public BaseWindow[] GetActiveWindows()
	{
		return this.activeWindows.ToArray();
	}

	private void updateModuleEnabledState()
	{
		if (this.CurrentInputModule != null)
		{
			if (this.inputBlocker.IsLocked() || this.keyBindingManager.IsBindingKey)
			{
				this.CurrentInputModule.enabled = false;
			}
			else
			{
				this.CurrentInputModule.enabled = true;
			}
		}
	}

	public T AddGamewideOverlay<T>(GameObject prefab, WindowTransition transition) where T : BaseGamewideOverlay
	{
		if (prefab == null)
		{
			return (T)((object)null);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		T component = gameObject.GetComponent<T>();
		this.injector.Inject(component);
		GamewideOverlayRequest gamewideOverlayRequest = new GamewideOverlayRequest();
		gamewideOverlayRequest.overlay = component;
		gamewideOverlayRequest.inTransition = transition;
		gamewideOverlayRequest.outTransition = transition;
		component.Request = gamewideOverlayRequest;
		component.IsRemoving = false;
		if (this.gamewideOverlayContainer == null)
		{
			this.queuedOverlays.Add(gamewideOverlayRequest);
		}
		else
		{
			this.addToGamewideOverlayContainer(gamewideOverlayRequest);
		}
		return component;
	}

	private void addToGamewideOverlayContainer(GamewideOverlayRequest request)
	{
		BaseGamewideOverlay overlay = request.overlay;
		overlay.IsRemoving = false;
		overlay.gameObject.transform.SetParent(this.gamewideOverlayContainer, false);
		this.gamewideOverlayTransitionManager.ShowInTransition(overlay, request.inTransition);
		overlay.OnOpen();
		overlay.CloseRequest = new Action<BaseGamewideOverlay>(this.gamewideOverlayRequestsClose);
		this.activeGamewideOverlays.Add(overlay);
		this.updateInputDelegate();
		this.updateSelectionOnGamewideOverlayOpen(overlay);
	}

	private void updateSelectionOnGamewideOverlayOpen(BaseGamewideOverlay overlay)
	{
		if (this.CurrentInputModule is CursorInputModule)
		{
			(this.CurrentInputModule as CursorInputModule).SuppressButtonsPressedThisFrame();
		}
		GameObject obj = overlay.FirstSelected;
		UIInputModule uIInputModule = this.CurrentInputModule as UIInputModule;
		if (uIInputModule == null)
		{
			obj = null;
		}
		else if (uIInputModule.IsMouseMode)
		{
			obj = null;
		}
		this.selectionManager.Select(obj);
		overlay.ReadyForSelections();
	}

	public void Remove(BaseGamewideOverlay gamewideOverlay)
	{
		if (!gamewideOverlay.IsRemoving)
		{
			UIManager._Remove_c__AnonStorey3 _Remove_c__AnonStorey = new UIManager._Remove_c__AnonStorey3();
			_Remove_c__AnonStorey._this = this;
			gamewideOverlay.IsRemoving = true;
			this.activeGamewideOverlays.Remove(gamewideOverlay);
			_Remove_c__AnonStorey.obj = gamewideOverlay.gameObject;
			this.updateInputDelegate();
			this.gamewideOverlayTransitionManager.ShowOutTransition(gamewideOverlay, gamewideOverlay.Request.outTransition, new Action(_Remove_c__AnonStorey.__m__0));
		}
	}

	private bool isInGamewideOverlayContainer(GameObject obj)
	{
		return !(obj == null) && obj.GetComponentInParent<GamewideOverlayContainer>() != null;
	}

	private void gamewideOverlayRequestsClose(BaseGamewideOverlay gamewideOverlay)
	{
		this.Remove(gamewideOverlay);
	}

	public int GetGamewideOverlayCount()
	{
		return this.activeGamewideOverlays.Count;
	}

	private void updateInputDelegate()
	{
		if (this.CurrentInputModule != null)
		{
			CursorInputModule cursorInputModule = this.CurrentInputModule as CursorInputModule;
			if (cursorInputModule != null)
			{
				cursorInputModule.cursorDelegate = this.getCursorDelegate();
			}
			UIInputModule uIInputModule = this.CurrentInputModule as UIInputModule;
			if (uIInputModule != null)
			{
				uIInputModule.ScreenDelegate = this.getScreenDelegate();
				uIInputModule.ButtonDelegate = this.getButtonDelegate();
				uIInputModule.GlobalDelegates = this.getGlobalDelegates();
			}
		}
	}

	public void ConsumeButtonPresses()
	{
		this.playerInput.Input.ConsumeButtonPresses();
	}

	private float _crossfadeTransition_m__0()
	{
		return this.prevScreen.Alpha;
	}

	private void _crossfadeTransition_m__1(float x)
	{
		this.prevScreen.Alpha = x;
	}

	private float _crossfadeTransition_m__2()
	{
		return this.CurrentScreen.Alpha;
	}

	private void _crossfadeTransition_m__3(float x)
	{
		this.CurrentScreen.Alpha = x;
	}
}
