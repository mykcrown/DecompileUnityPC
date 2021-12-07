using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000667 RID: 1639
[RequireComponent(typeof(EventSystem))]
public class UIManager : ITickable, IScreenDisplay, IWindowDisplay, IGamewideOverlayDisplay
{
	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x0600282B RID: 10283 RVA: 0x000C339C File Offset: 0x000C179C
	// (set) Token: 0x0600282C RID: 10284 RVA: 0x000C33A4 File Offset: 0x000C17A4
	[Inject]
	public IUIAdapter adapter { get; set; }

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x0600282D RID: 10285 RVA: 0x000C33AD File Offset: 0x000C17AD
	// (set) Token: 0x0600282E RID: 10286 RVA: 0x000C33B5 File Offset: 0x000C17B5
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x0600282F RID: 10287 RVA: 0x000C33BE File Offset: 0x000C17BE
	// (set) Token: 0x06002830 RID: 10288 RVA: 0x000C33C6 File Offset: 0x000C17C6
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x06002831 RID: 10289 RVA: 0x000C33CF File Offset: 0x000C17CF
	// (set) Token: 0x06002832 RID: 10290 RVA: 0x000C33D7 File Offset: 0x000C17D7
	[Inject]
	public IInputBlocker inputBlocker { get; set; }

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x06002833 RID: 10291 RVA: 0x000C33E0 File Offset: 0x000C17E0
	// (set) Token: 0x06002834 RID: 10292 RVA: 0x000C33E8 File Offset: 0x000C17E8
	[Inject]
	public WindowTransitionManager windowTransitionManager { get; set; }

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x06002835 RID: 10293 RVA: 0x000C33F1 File Offset: 0x000C17F1
	// (set) Token: 0x06002836 RID: 10294 RVA: 0x000C33F9 File Offset: 0x000C17F9
	[Inject]
	public GamewideOverlayTransitionManager gamewideOverlayTransitionManager { get; set; }

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x06002837 RID: 10295 RVA: 0x000C3402 File Offset: 0x000C1802
	// (set) Token: 0x06002838 RID: 10296 RVA: 0x000C340A File Offset: 0x000C180A
	[Inject]
	public IScreenTransitionMap screenTransitions { get; set; }

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x06002839 RID: 10297 RVA: 0x000C3413 File Offset: 0x000C1813
	// (set) Token: 0x0600283A RID: 10298 RVA: 0x000C341B File Offset: 0x000C181B
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x0600283B RID: 10299 RVA: 0x000C3424 File Offset: 0x000C1824
	// (set) Token: 0x0600283C RID: 10300 RVA: 0x000C342C File Offset: 0x000C182C
	[Inject]
	public ICursorManager cursorManager { get; set; }

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x0600283D RID: 10301 RVA: 0x000C3435 File Offset: 0x000C1835
	// (set) Token: 0x0600283E RID: 10302 RVA: 0x000C343D File Offset: 0x000C183D
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x0600283F RID: 10303 RVA: 0x000C3446 File Offset: 0x000C1846
	// (set) Token: 0x06002840 RID: 10304 RVA: 0x000C344E File Offset: 0x000C184E
	[Inject]
	public PlayerInputLocator playerInput { get; set; }

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x06002841 RID: 10305 RVA: 0x000C3457 File Offset: 0x000C1857
	// (set) Token: 0x06002842 RID: 10306 RVA: 0x000C345F File Offset: 0x000C185F
	[Inject]
	public IKeyBindingManager keyBindingManager { get; set; }

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x06002843 RID: 10307 RVA: 0x000C3468 File Offset: 0x000C1868
	// (set) Token: 0x06002844 RID: 10308 RVA: 0x000C3470 File Offset: 0x000C1870
	[Inject]
	public ISelectionManager selectionManager { get; set; }

	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x06002845 RID: 10309 RVA: 0x000C3479 File Offset: 0x000C1879
	// (set) Token: 0x06002846 RID: 10310 RVA: 0x000C3481 File Offset: 0x000C1881
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x06002847 RID: 10311 RVA: 0x000C348A File Offset: 0x000C188A
	// (set) Token: 0x06002848 RID: 10312 RVA: 0x000C3492 File Offset: 0x000C1892
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x06002849 RID: 10313 RVA: 0x000C349B File Offset: 0x000C189B
	// (set) Token: 0x0600284A RID: 10314 RVA: 0x000C34A3 File Offset: 0x000C18A3
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x0600284B RID: 10315 RVA: 0x000C34AC File Offset: 0x000C18AC
	// (set) Token: 0x0600284C RID: 10316 RVA: 0x000C34B4 File Offset: 0x000C18B4
	private CanvasGroup canvasGroup { get; set; }

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x0600284D RID: 10317 RVA: 0x000C34BD File Offset: 0x000C18BD
	// (set) Token: 0x0600284E RID: 10318 RVA: 0x000C34C5 File Offset: 0x000C18C5
	private Canvas canvas { get; set; }

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x0600284F RID: 10319 RVA: 0x000C34CE File Offset: 0x000C18CE
	// (set) Token: 0x06002850 RID: 10320 RVA: 0x000C34D6 File Offset: 0x000C18D6
	public Canvas CameraCanvas { get; set; }

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06002851 RID: 10321 RVA: 0x000C34DF File Offset: 0x000C18DF
	// (set) Token: 0x06002852 RID: 10322 RVA: 0x000C34E7 File Offset: 0x000C18E7
	public EventSystem EventSystem { get; private set; }

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06002853 RID: 10323 RVA: 0x000C34F0 File Offset: 0x000C18F0
	// (set) Token: 0x06002854 RID: 10324 RVA: 0x000C34F8 File Offset: 0x000C18F8
	public GraphicRaycaster GraphicRaycaster { get; private set; }

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06002855 RID: 10325 RVA: 0x000C3501 File Offset: 0x000C1901
	// (set) Token: 0x06002856 RID: 10326 RVA: 0x000C3509 File Offset: 0x000C1909
	public GUIText GlobalDebugText { get; private set; }

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06002857 RID: 10327 RVA: 0x000C3512 File Offset: 0x000C1912
	// (set) Token: 0x06002858 RID: 10328 RVA: 0x000C351A File Offset: 0x000C191A
	public GUIText DebugTextEvents { get; private set; }

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x06002859 RID: 10329 RVA: 0x000C3523 File Offset: 0x000C1923
	// (set) Token: 0x0600285A RID: 10330 RVA: 0x000C352C File Offset: 0x000C192C
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

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x0600285B RID: 10331 RVA: 0x000C3595 File Offset: 0x000C1995
	// (set) Token: 0x0600285C RID: 10332 RVA: 0x000C359D File Offset: 0x000C199D
	public GameScreen CurrentScreen { get; private set; }

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x0600285D RID: 10333 RVA: 0x000C35A6 File Offset: 0x000C19A6
	// (set) Token: 0x0600285E RID: 10334 RVA: 0x000C35CE File Offset: 0x000C19CE
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

	// Token: 0x0600285F RID: 10335 RVA: 0x000C35F4 File Offset: 0x000C19F4
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
			GUIText guitext = gameObject3.AddComponent<GUIText>();
			guitext.pixelOffset = new Vector2((float)((i != 0) ? 1880 : 55) * ((float)Screen.width / this.guiConfig.canvasScaler.referenceResolution.x), 1070f * ((float)Screen.height / this.guiConfig.canvasScaler.referenceResolution.y));
			if (i == 0)
			{
				guitext.text = "Debug Mode";
			}
			guitext.anchor = ((i != 0) ? TextAnchor.UpperRight : TextAnchor.UpperLeft);
			guitext.alignment = ((i != 0) ? TextAlignment.Right : TextAlignment.Left);
			guitext.color = Color.white;
			guitext.richText = true;
			guitext.transform.SetParent(gameObject2.transform, false);
			guitext.enabled = Debug.isDebugBuild;
			this.PlayerDebugText.Add(guitext);
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
		foreach (WindowRequest request in this.queuedWindows)
		{
			this.addToWindowContainer(request);
		}
		this.queuedWindows.Clear();
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x000C3C34 File Offset: 0x000C2034
	private void onRaycastUpdate()
	{
		if (this.CurrentInputModule is CursorInputModule)
		{
			(this.CurrentInputModule as CursorInputModule).ReprocessHoversImmediate();
		}
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x000C3C58 File Offset: 0x000C2058
	private void onToggleUIVisibility(GameEvent message)
	{
		ToggleUIVisibilityCommand toggleUIVisibilityCommand = message as ToggleUIVisibilityCommand;
		this.canvasGroup.alpha = (float)((!toggleUIVisibilityCommand.visibility) ? 0 : 1);
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06002862 RID: 10338 RVA: 0x000C3C8A File Offset: 0x000C208A
	public bool IsTextEntryMode
	{
		get
		{
			return this.CurrentInputModule != null && this.CurrentInputModule.CurrentInputField != null;
		}
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x000C3CB0 File Offset: 0x000C20B0
	public void SetPauseMode(bool isPause)
	{
		foreach (ICustomInputModule customInputModule in this.inputModulesByType.Values)
		{
			customInputModule.SetPauseMode(isPause);
		}
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x000C3D14 File Offset: 0x000C2114
	private ICustomInputModule setupMenuInputModule(GameObject canvasObj)
	{
		MenuActions menuActions = new MenuActions();
		UIInputModule uiinputModule = canvasObj.AddComponent<UIInputModule>();
		this.injector.Inject(uiinputModule);
		menuActions.Device = null;
		uiinputModule.Actions = menuActions;
		uiinputModule.enabled = false;
		uiinputModule.SubmitAction = menuActions.Submit;
		uiinputModule.CancelAction = menuActions.Cancel;
		uiinputModule.LeftStickAction = menuActions.LeftStick;
		uiinputModule.LeftAction = menuActions.Left;
		uiinputModule.RightAction = menuActions.Right;
		uiinputModule.UpAction = menuActions.Up;
		uiinputModule.DownAction = menuActions.Down;
		uiinputModule.YButtonAction = menuActions.YButtonAction;
		uiinputModule.XButtonAction = menuActions.XButtonAction;
		uiinputModule.RightTriggerAction = menuActions.RightTrigger;
		uiinputModule.LeftBumperAction = menuActions.LeftBumper;
		uiinputModule.ZButtonAction = menuActions.ZButtonAction;
		uiinputModule.LeftTriggerAction = menuActions.LeftTrigger;
		uiinputModule.RightStickAction = menuActions.RightStick;
		uiinputModule.DPadLeftAction = menuActions.DPadLeft;
		uiinputModule.DPadRightAction = menuActions.DPadRight;
		uiinputModule.DPadDownAction = menuActions.DPadDown;
		uiinputModule.DPadUpAction = menuActions.DPadUp;
		return uiinputModule;
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x000C3E28 File Offset: 0x000C2228
	private ICustomInputModule setupCursorInputModule(GameObject canvasObj)
	{
		CursorInputModule cursorInputModule = canvasObj.AddComponent<CursorInputModule>();
		cursorInputModule.enabled = false;
		return cursorInputModule;
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x000C3E44 File Offset: 0x000C2244
	private void OnDestroy()
	{
		this.events.Unsubscribe(typeof(SetUIInputModuleCommand), new Events.EventHandler(this.onActivateUIInputModuleCommand));
		this.events.Unsubscribe(typeof(DeactivateUIInputModuleCommand), new Events.EventHandler(this.onDeactivateUIInputModuleCommand));
		this.events.Unsubscribe(typeof(ActivateCursorModuleCommand), new Events.EventHandler(this.onActivateCursorModuleCommand));
		this.events.Unsubscribe(typeof(DeactivateCursorModuleCommand), new Events.EventHandler(this.onDeactivateCursorModuleCommand));
		this.events.Unsubscribe(typeof(ClearInputStateCommand), new Events.EventHandler(this.clearInputState));
		this.events.Unsubscribe(typeof(ToggleUIVisibilityCommand), new Events.EventHandler(this.onToggleUIVisibility));
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x000C3F18 File Offset: 0x000C2318
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
			Debug.LogError("Failed to load screen, since it was null!");
		}
	}

	// Token: 0x06002868 RID: 10344 RVA: 0x000C4060 File Offset: 0x000C2460
	private void onDrawComplete()
	{
		this.drawComplete = true;
		this.checkScreenReady();
	}

	// Token: 0x06002869 RID: 10345 RVA: 0x000C406F File Offset: 0x000C246F
	private void onSetupComplete()
	{
		this.setupComplete = true;
		this.checkScreenReady();
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x000C4080 File Offset: 0x000C2480
	private void checkScreenReady()
	{
		if (this.isScreenReadyToDisplay())
		{
			this.resetScreenLoadStatus();
			ScreenTransition transition = this.screenTransitions.Get(this.prevScreen, this.CurrentScreen);
			this.doTransition(transition);
		}
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x000C40BD File Offset: 0x000C24BD
	private void resetScreenLoadStatus()
	{
		this.drawComplete = false;
		this.setupComplete = false;
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x000C40CD File Offset: 0x000C24CD
	private bool isScreenReadyToDisplay()
	{
		return this.drawComplete && this.setupComplete;
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x000C40EC File Offset: 0x000C24EC
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

	// Token: 0x0600286E RID: 10350 RVA: 0x000C4157 File Offset: 0x000C2557
	private void noTransition()
	{
		this.onTransitionBegin();
		this.onTransitionComplete();
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x000C4168 File Offset: 0x000C2568
	private void crossfadeTransition(ScreenTransition transition)
	{
		this.CurrentScreen.Alpha = 0f;
		this.onTransitionBegin();
		float time = transition.time;
		DOTween.To(() => this.prevScreen.Alpha, delegate(float x)
		{
			this.prevScreen.Alpha = x;
		}, 0f, time).SetEase(Ease.OutSine);
		DOTween.To(() => this.CurrentScreen.Alpha, delegate(float x)
		{
			this.CurrentScreen.Alpha = x;
		}, 1f, time).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.onTransitionComplete));
	}

	// Token: 0x06002870 RID: 10352 RVA: 0x000C41F8 File Offset: 0x000C25F8
	private void gameCompleteTransition(ScreenTransition transition)
	{
		this.CurrentScreen.Alpha = 0f;
		this.prevScreen.Alpha = 1f;
		float flipTime = transition.time;
		float delay = transition.delay;
		this.timer.SetTimeout((int)(delay * 1000f), delegate
		{
			this.onTransitionBegin();
			DOTween.To(() => this.prevScreen.Alpha, delegate(float x)
			{
				this.prevScreen.Alpha = x;
			}, 0f, flipTime).SetEase(Ease.OutSine);
			DOTween.To(() => this.CurrentScreen.Alpha, delegate(float x)
			{
				this.CurrentScreen.Alpha = x;
			}, 1f, flipTime).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.onTransitionComplete));
		});
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x000C4264 File Offset: 0x000C2664
	private void onTransitionBegin()
	{
		this.CurrentScreen.CheckForTransitionBegin();
	}

	// Token: 0x06002872 RID: 10354 RVA: 0x000C4274 File Offset: 0x000C2674
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

	// Token: 0x06002873 RID: 10355 RVA: 0x000C42E0 File Offset: 0x000C26E0
	public void ClearPreviousScreen()
	{
		this.clearPreviousScreen();
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x000C42E8 File Offset: 0x000C26E8
	private void clearPreviousScreen()
	{
		if (this.prevScreen != null)
		{
			this.signalBus.Dispatch(UIManager.SCREEN_CLOSED);
			UnityEngine.Object.Destroy(this.prevScreen.gameObject);
		}
		List<BaseWindow> list = new List<BaseWindow>(this.activeWindows);
		foreach (BaseWindow baseWindow in list)
		{
			if (baseWindow.Request.closeOnScreenChange && baseWindow.SourceScreen == this.adapter.PreviousScreen)
			{
				baseWindow.Close();
			}
		}
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x000C43A4 File Offset: 0x000C27A4
	private bool shouldSelectFirstElement()
	{
		return this.isFirstElementInputActive() && !(this.CurrentScreen == null) && !(this.CurrentInputModule is CursorInputModule) && !(this.EventSystem.currentSelectedGameObject != null);
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x000C43FC File Offset: 0x000C27FC
	private bool isFirstElementInputActive()
	{
		return InputManager.ActiveDevice.AnyButton.IsPressed || InputManager.ActiveDevice.LeftStick.IsPressed || InputManager.AnyKeyIsPressed;
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x000C4430 File Offset: 0x000C2830
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
			foreach (string str in this.textEventQueue)
			{
				GUIText debugTextEvents = this.DebugTextEvents;
				debugTextEvents.text = debugTextEvents.text + str + "\n";
			}
		}
	}

	// Token: 0x06002878 RID: 10360 RVA: 0x000C4580 File Offset: 0x000C2980
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

	// Token: 0x06002879 RID: 10361 RVA: 0x000C45B9 File Offset: 0x000C29B9
	public void ReportRollbackHealth(NetworkHealthReport health)
	{
		this.healthReporter.ReportHealth(health);
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x000C45C8 File Offset: 0x000C29C8
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

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x0600287B RID: 10363 RVA: 0x000C461A File Offset: 0x000C2A1A
	// (set) Token: 0x0600287C RID: 10364 RVA: 0x000C4624 File Offset: 0x000C2A24
	public bool DebugTextEnabled
	{
		get
		{
			return this.debugTextEnabled;
		}
		set
		{
			foreach (GUIText guitext in this.PlayerDebugText)
			{
				guitext.enabled = value;
			}
			this.GlobalDebugText.enabled = value;
			this.DebugTextEvents.enabled = value;
			this.debugTextEnabled = value;
		}
	}

	// Token: 0x0600287D RID: 10365 RVA: 0x000C46A0 File Offset: 0x000C2AA0
	private void clearInputState(GameEvent message = null)
	{
		if (this.CurrentInputModule != null)
		{
			this.CurrentInputModule.ShouldActivateModule();
			this.CurrentInputModule.UpdateModule();
		}
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x000C46C4 File Offset: 0x000C2AC4
	GameScreen IScreenDisplay.GetScreenByType(ScreenType type)
	{
		if (!this.guiConfig.screens.ContainsKey(type))
		{
			Debug.LogError("Failed to find UI prefab for " + type.ToString() + " screen");
			return null;
		}
		return this.guiConfig.screens[type];
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x000C471C File Offset: 0x000C2B1C
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
		foreach (ICustomInputModule customInputModule in this.inputModulesByType.Values)
		{
			BaseInputModule baseInputModule = (BaseInputModule)customInputModule;
			baseInputModule.enabled = false;
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

	// Token: 0x06002880 RID: 10368 RVA: 0x000C486C File Offset: 0x000C2C6C
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

	// Token: 0x06002881 RID: 10369 RVA: 0x000C48BC File Offset: 0x000C2CBC
	private IScreenInputDelegate getScreenDelegate()
	{
		if (this.CurrentScreen != null)
		{
			return this.CurrentScreen;
		}
		return null;
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x000C48D1 File Offset: 0x000C2CD1
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

	// Token: 0x06002883 RID: 10371 RVA: 0x000C4910 File Offset: 0x000C2D10
	private List<IGlobalInputDelegate> getGlobalDelegates()
	{
		List<IGlobalInputDelegate> list = new List<IGlobalInputDelegate>();
		foreach (BaseGamewideOverlay item in this.activeGamewideOverlays)
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000C4974 File Offset: 0x000C2D74
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

	// Token: 0x06002885 RID: 10373 RVA: 0x000C49D0 File Offset: 0x000C2DD0
	private void onActivateUIInputModuleCommand(GameEvent message)
	{
		SetUIInputModuleCommand setUIInputModuleCommand = message as SetUIInputModuleCommand;
		this.switchInputModule(setUIInputModuleCommand.type, null);
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x000C49F1 File Offset: 0x000C2DF1
	private void onDeactivateUIInputModuleCommand(GameEvent message)
	{
		this.switchInputModule(UIInputModuleType.None, null);
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000C49FC File Offset: 0x000C2DFC
	private void onActivateCursorModuleCommand(GameEvent message)
	{
		ActivateCursorModuleCommand cursorActivate = message as ActivateCursorModuleCommand;
		this.switchInputModule(UIInputModuleType.Cursor, cursorActivate);
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000C4A18 File Offset: 0x000C2E18
	public void AddCursor(PlayerCursor cursor)
	{
		cursor.transform.SetParent(this.cursorContainer, false);
		cursor.InitMode((!this.shouldDisplayMouse()) ? global::CursorMode.Controller : global::CursorMode.Mouse);
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x000C4A44 File Offset: 0x000C2E44
	private void onDeactivateCursorModuleCommand(GameEvent message)
	{
		this.switchInputModule(UIInputModuleType.None, null);
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x000C4A50 File Offset: 0x000C2E50
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

	// Token: 0x0600288B RID: 10379 RVA: 0x000C4AA4 File Offset: 0x000C2EA4
	private bool allowMouseInteraction()
	{
		return this.CurrentInputModule == null || !(this.CurrentInputModule is UIInputModule) || this.CurrentInputModule.IsMouseMode;
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x000C4AD0 File Offset: 0x000C2ED0
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

	// Token: 0x0600288D RID: 10381 RVA: 0x000C4B30 File Offset: 0x000C2F30
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

	// Token: 0x0600288E RID: 10382 RVA: 0x000C4BF8 File Offset: 0x000C2FF8
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

	// Token: 0x0600288F RID: 10383 RVA: 0x000C4D20 File Offset: 0x000C3120
	private void updateSelectionOnWindowOpen(BaseWindow window)
	{
		window.PreviousSelection = EventSystem.current.currentSelectedGameObject;
		if (this.CurrentInputModule is CursorInputModule)
		{
			(this.CurrentInputModule as CursorInputModule).SuppressButtonsPressedThisFrame();
		}
		GameObject obj = window.FirstSelected;
		UIInputModule uiinputModule = this.CurrentInputModule as UIInputModule;
		if (uiinputModule == null)
		{
			obj = null;
		}
		else if (uiinputModule.IsMouseMode)
		{
			obj = null;
		}
		this.selectionManager.Select(obj);
		window.ReadyForSelections();
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x000C4DA4 File Offset: 0x000C31A4
	public void Remove(BaseWindow window)
	{
		if (!window.IsRemoving)
		{
			window.IsRemoving = true;
			this.activeWindows.Remove(window);
			if (this.activeWindows.Count == 0 && this.CurrentScreen != null)
			{
				this.CurrentScreen.OnWindowsClosed();
			}
			GameObject obj = window.gameObject;
			this.updateInputDelegate();
			this.windowTransitionManager.ShowOutTransition(window, window.Request.outTransition, delegate
			{
				if (this.isInWindow(window, EventSystem.current.currentSelectedGameObject))
				{
					this.selectionManager.Select(window.PreviousSelection);
				}
				foreach (BaseWindow baseWindow in this.activeWindows)
				{
					if (this.isInWindow(window, baseWindow.PreviousSelection))
					{
						baseWindow.PreviousSelection = window.PreviousSelection;
					}
				}
				UnityEngine.Object.Destroy(obj);
				this.signalBus.Dispatch(UIManager.WINDOW_CLOSED);
			});
		}
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x000C4E70 File Offset: 0x000C3270
	private bool isInWindow(BaseWindow window, GameObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		BaseWindow componentInParent = obj.GetComponentInParent<BaseWindow>();
		return window == componentInParent;
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x000C4E99 File Offset: 0x000C3299
	private void windowRequestsClose(BaseWindow window)
	{
		this.Remove(window);
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x000C4EA2 File Offset: 0x000C32A2
	public int GetWindowCount()
	{
		return this.activeWindows.Count;
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x000C4EAF File Offset: 0x000C32AF
	public BaseWindow[] GetActiveWindows()
	{
		return this.activeWindows.ToArray();
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x000C4EBC File Offset: 0x000C32BC
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

	// Token: 0x06002896 RID: 10390 RVA: 0x000C4F14 File Offset: 0x000C3314
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

	// Token: 0x06002897 RID: 10391 RVA: 0x000C4FBC File Offset: 0x000C33BC
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

	// Token: 0x06002898 RID: 10392 RVA: 0x000C5034 File Offset: 0x000C3434
	private void updateSelectionOnGamewideOverlayOpen(BaseGamewideOverlay overlay)
	{
		if (this.CurrentInputModule is CursorInputModule)
		{
			(this.CurrentInputModule as CursorInputModule).SuppressButtonsPressedThisFrame();
		}
		GameObject obj = overlay.FirstSelected;
		UIInputModule uiinputModule = this.CurrentInputModule as UIInputModule;
		if (uiinputModule == null)
		{
			obj = null;
		}
		else if (uiinputModule.IsMouseMode)
		{
			obj = null;
		}
		this.selectionManager.Select(obj);
		overlay.ReadyForSelections();
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x000C50A8 File Offset: 0x000C34A8
	public void Remove(BaseGamewideOverlay gamewideOverlay)
	{
		if (!gamewideOverlay.IsRemoving)
		{
			gamewideOverlay.IsRemoving = true;
			this.activeGamewideOverlays.Remove(gamewideOverlay);
			GameObject obj = gamewideOverlay.gameObject;
			this.updateInputDelegate();
			this.gamewideOverlayTransitionManager.ShowOutTransition(gamewideOverlay, gamewideOverlay.Request.outTransition, delegate
			{
				UnityEngine.Object.Destroy(obj);
				if (this.isInGamewideOverlayContainer(EventSystem.current.currentSelectedGameObject))
				{
					this.selectionManager.Select(null);
				}
				this.signalBus.Dispatch(UIManager.GAMEWIDE_OVERLAY_CLOSED);
			});
		}
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x000C5116 File Offset: 0x000C3516
	private bool isInGamewideOverlayContainer(GameObject obj)
	{
		return !(obj == null) && obj.GetComponentInParent<GamewideOverlayContainer>() != null;
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x000C5132 File Offset: 0x000C3532
	private void gamewideOverlayRequestsClose(BaseGamewideOverlay gamewideOverlay)
	{
		this.Remove(gamewideOverlay);
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x000C513B File Offset: 0x000C353B
	public int GetGamewideOverlayCount()
	{
		return this.activeGamewideOverlays.Count;
	}

	// Token: 0x0600289D RID: 10397 RVA: 0x000C5148 File Offset: 0x000C3548
	private void updateInputDelegate()
	{
		if (this.CurrentInputModule != null)
		{
			CursorInputModule cursorInputModule = this.CurrentInputModule as CursorInputModule;
			if (cursorInputModule != null)
			{
				cursorInputModule.cursorDelegate = this.getCursorDelegate();
			}
			UIInputModule uiinputModule = this.CurrentInputModule as UIInputModule;
			if (uiinputModule != null)
			{
				uiinputModule.ScreenDelegate = this.getScreenDelegate();
				uiinputModule.ButtonDelegate = this.getButtonDelegate();
				uiinputModule.GlobalDelegates = this.getGlobalDelegates();
			}
		}
	}

	// Token: 0x0600289E RID: 10398 RVA: 0x000C51C0 File Offset: 0x000C35C0
	public void ConsumeButtonPresses()
	{
		this.playerInput.Input.ConsumeButtonPresses();
	}

	// Token: 0x04001D42 RID: 7490
	public static string SCREEN_CLOSED = "UIManager.SCREEN_CLOSED";

	// Token: 0x04001D43 RID: 7491
	public static string SCREEN_OPENED = "UIManager.SCREEN_OPENED";

	// Token: 0x04001D44 RID: 7492
	public static string WINDOW_CLOSED = "UIManager.WINDOW_CLOSED";

	// Token: 0x04001D45 RID: 7493
	public static string GAMEWIDE_OVERLAY_CLOSED = "UIManager.GAMEWIDE_OVERLAY_CLOSED";

	// Token: 0x04001D46 RID: 7494
	public static string RAYCAST_UPDATE = "UIManager.RAYCAST_UPDATE";

	// Token: 0x04001D59 RID: 7513
	private Transform screenContainer;

	// Token: 0x04001D5A RID: 7514
	private Transform windowContainer;

	// Token: 0x04001D5B RID: 7515
	private Transform gamewideOverlayContainer;

	// Token: 0x04001D5C RID: 7516
	private CanvasGroup windowCanvasGroup;

	// Token: 0x04001D5D RID: 7517
	private CanvasGroup screenCanvasGroup;

	// Token: 0x04001D5E RID: 7518
	private CanvasGroup gamewideOverlayCanvasGroup;

	// Token: 0x04001D5F RID: 7519
	private Transform cursorContainer;

	// Token: 0x04001D63 RID: 7523
	private bool debugTextEnabled;

	// Token: 0x04001D66 RID: 7526
	private static readonly int globalTextEventCount = 12;

	// Token: 0x04001D67 RID: 7527
	private Queue<string> textEventQueue = new Queue<string>();

	// Token: 0x04001D68 RID: 7528
	public List<GUIText> PlayerDebugText = new List<GUIText>();

	// Token: 0x04001D69 RID: 7529
	private Color debugTextColor = Color.white;

	// Token: 0x04001D6A RID: 7530
	private bool drawComplete;

	// Token: 0x04001D6B RID: 7531
	private bool setupComplete;

	// Token: 0x04001D6C RID: 7532
	private bool debugTextEventsDirty;

	// Token: 0x04001D6D RID: 7533
	private bool waitToClearPrevious;

	// Token: 0x04001D6E RID: 7534
	private GameClient client;

	// Token: 0x04001D6F RID: 7535
	private ICustomInputModule currentInputModule;

	// Token: 0x04001D70 RID: 7536
	private ICustomInputModule lastActiveInputModule;

	// Token: 0x04001D71 RID: 7537
	private Dictionary<UIInputModuleType, ICustomInputModule> inputModulesByType;

	// Token: 0x04001D72 RID: 7538
	private GameScreen prevScreen;

	// Token: 0x04001D74 RID: 7540
	private bool isCurrentScreenDrawing;

	// Token: 0x04001D75 RID: 7541
	private NetworkHealthReporter healthReporter;

	// Token: 0x04001D76 RID: 7542
	private List<WindowRequest> queuedWindows = new List<WindowRequest>();

	// Token: 0x04001D77 RID: 7543
	private List<BaseWindow> activeWindows = new List<BaseWindow>();

	// Token: 0x04001D78 RID: 7544
	private List<GamewideOverlayRequest> queuedOverlays = new List<GamewideOverlayRequest>();

	// Token: 0x04001D79 RID: 7545
	private List<BaseGamewideOverlay> activeGamewideOverlays = new List<BaseGamewideOverlay>();

	// Token: 0x04001D7A RID: 7546
	private UIConfig guiConfig;
}
