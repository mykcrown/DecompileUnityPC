// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameScreen : ClientBehavior, ICursorInputDelegate, IScreenInputDelegate, IButtonInputDelegate
{
	private sealed class _tweenInItem_c__AnonStorey0
	{
		internal GameObject theList;

		internal Action callback;

		internal Vector3 __m__0()
		{
			return this.theList.transform.localPosition;
		}

		internal void __m__1(Vector3 valueIn)
		{
			this.theList.transform.localPosition = valueIn;
		}

		internal void __m__2()
		{
			if (this.callback != null)
			{
				this.callback();
			}
		}
	}

	public GameObject FirstSelected;

	public GameObject CursorPrefab;

	public UIInputModuleType UIInput = UIInputModuleType.Menu;

	public bool listenForDevices;

	protected bool isShown;

	private float _screenWidth;

	private float _alpha = 1f;

	private CanvasGroup _canvasGroup;

	protected MousePlayerCursor mouseCursor;

	protected List<IPlayerCursor> cursors = new List<IPlayerCursor>();

	private List<MenuItemList> menuControllers = new List<MenuItemList>();

	private List<InputInstructions> inputInstructions = new List<InputInstructions>();

	protected Payload payload;

	private bool didLoadPayload;

	protected LoadSequenceResults data;

	protected bool isDestroyed;

	private bool isStarted;

	private bool didPostStartAndPayload;

	private bool didTransistionBegin;

	private bool isSelectionInitalized;

	private InputBlock block;

	public Action OnDrawCompleteCallback;

	[Inject]
	public UIManager uiManager
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
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

	[Inject]
	public IGamewideOverlayController gamewideOverlayController
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
	public PlayerInputLocator playerInput
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
	public UserAudioSettings userAudioSettings
	{
		get;
		set;
	}

	protected virtual bool SupportsAutoJoin
	{
		get
		{
			return false;
		}
	}

	public virtual bool WaitForDrawCallback
	{
		get
		{
			return false;
		}
	}

	protected float screenWidth
	{
		get
		{
			return base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x;
		}
	}

	protected float screenHeight
	{
		get
		{
			return base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.y;
		}
	}

	public virtual bool AlwaysHideMouse
	{
		get
		{
			return false;
		}
	}

	public float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			this._canvasGroup.alpha = this._alpha;
		}
	}

	public virtual void TickFrame()
	{
	}

	public virtual void LoadData(LoadSequenceResults data)
	{
		this.data = data;
	}

	public override void Awake()
	{
		base.Awake();
		this._canvasGroup = base.GetComponent<CanvasGroup>();
	}

	public virtual void Start()
	{
		this.isStarted = true;
		this._screenWidth = (float)Screen.width;
		this.checkForPostStartAndPayload();
	}

	protected void onDrawComplete()
	{
		if (this.OnDrawCompleteCallback != null)
		{
			this.OnDrawCompleteCallback();
		}
	}

	public virtual void LoadPayload(Payload payload)
	{
		this.payload = payload;
		this.didLoadPayload = true;
		this.checkForPostStartAndPayload();
	}

	private void checkForPostStartAndPayload()
	{
		if (!this.didLoadPayload)
		{
			return;
		}
		if (!this.isStarted)
		{
			return;
		}
		if (this.didPostStartAndPayload)
		{
			return;
		}
		this.didPostStartAndPayload = true;
		this.postStartAndPayload();
		this.CheckForTransitionBegin();
	}

	protected void lockInput()
	{
		if (this.block == null)
		{
			this.block = this.inputBlocker.Request();
		}
	}

	protected void unlockInput()
	{
		if (this.block != null)
		{
			this.inputBlocker.Release(this.block);
			this.block = null;
		}
	}

	protected virtual void postStartAndPayload()
	{
		this.AddScreenListeners();
	}

	protected virtual void AddScreenListeners()
	{
		if (this.SupportsAutoJoin)
		{
			base.listen(AutoJoin.AUTOJOIN, new Action(this.onAutoJoinRequest));
		}
	}

	protected virtual void onAutoJoinRequest()
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Next));
	}

	public virtual void OnTransitionComplete()
	{
		this.initMenuSelectionSystem();
	}

	public void CheckForTransitionBegin()
	{
		if (!this.didPostStartAndPayload)
		{
			return;
		}
		if (this.didTransistionBegin)
		{
			return;
		}
		this.didTransistionBegin = true;
		this.onTransitionBegin();
	}

	protected virtual Vector2 getCursorDefaultPosition(PlayerNum playerNum)
	{
		return new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
	}

	protected virtual void onTransitionBegin()
	{
	}

	protected void tweenInItem(GameObject theList, int directionX, int directionY, float time, float delay = 0f, Action callback = null)
	{
		GameScreen._tweenInItem_c__AnonStorey0 _tweenInItem_c__AnonStorey = new GameScreen._tweenInItem_c__AnonStorey0();
		_tweenInItem_c__AnonStorey.theList = theList;
		_tweenInItem_c__AnonStorey.callback = callback;
		Vector3 localPosition = _tweenInItem_c__AnonStorey.theList.transform.localPosition;
		Vector3 localPosition2 = _tweenInItem_c__AnonStorey.theList.transform.localPosition;
		localPosition2.x += base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x * (float)directionX;
		localPosition2.y += base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.y * (float)directionY;
		_tweenInItem_c__AnonStorey.theList.transform.localPosition = localPosition2;
		DOTween.To(new DOGetter<Vector3>(_tweenInItem_c__AnonStorey.__m__0), new DOSetter<Vector3>(_tweenInItem_c__AnonStorey.__m__1), localPosition, time).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(new TweenCallback(_tweenInItem_c__AnonStorey.__m__2));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		this.isDestroyed = true;
		this.destroyAllCursors();
		this.unlockInput();
	}

	public virtual void UpdatePayload(Payload payload)
	{
	}

	protected virtual void Update()
	{
		if (this._screenWidth != (float)Screen.width)
		{
			this.onScreenSizeUpdate();
			this._screenWidth = (float)Screen.width;
		}
	}

	protected virtual void onScreenSizeUpdate()
	{
	}

	public virtual void OnAddedToHeirarchy()
	{
		this.isShown = true;
		if (this.UIInput == UIInputModuleType.Cursor)
		{
			if (this.CursorPrefab == null)
			{
				throw new Exception("GameScreen with CursorInputModule must have CursorPrefab set.");
			}
			this.mouseCursor = new GameObject("MouseCursor").AddComponent<MousePlayerCursor>();
			this.mouseCursor.transform.SetParent(base.transform, false);
			this.mouseCursor.ResetPosition(Vector2.zero);
			this.cursors.Add(this.mouseCursor);
			this.createScreenCursors();
			base.events.Broadcast(new ActivateCursorModuleCommand(this.cursors));
		}
		else
		{
			base.events.Broadcast(new SetUIInputModuleCommand(this.UIInput));
		}
	}

	public void OnWindowsOpened()
	{
		this.pauseAllCursors();
	}

	public void OnWindowsClosed()
	{
		if (this.UIInput == UIInputModuleType.Cursor)
		{
			base.events.Broadcast(new ActivateCursorModuleCommand(this.cursors));
		}
		else
		{
			base.events.Broadcast(new SetUIInputModuleCommand(this.UIInput));
		}
		this.unpauseAllCursors();
	}

	protected virtual void createScreenCursors()
	{
	}

	public virtual void Deactivate()
	{
		if (this.isSelectionInitalized && this.uiManager.CurrentInputModule is UIInputModule)
		{
			((UIInputModule)this.uiManager.CurrentInputModule).EnableMouseControls();
		}
		if (this.UIInput == UIInputModuleType.Cursor)
		{
			this.mouseCursor = null;
			base.events.Broadcast(new DeactivateCursorModuleCommand());
		}
		else
		{
			base.events.Broadcast(new DeactivateUIInputModuleCommand(this.UIInput));
		}
	}

	public virtual void OnCancelPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnStartPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnSubmitPressed(PointerEventData eventData)
	{
	}

	public virtual void OnAltCancelPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnAltSubmitPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnMouseMode()
	{
		this.onMouseModeUpdate();
	}

	public virtual void OnControllerMode()
	{
		this.onMouseModeUpdate();
	}

	public virtual void OnAdvance1Pressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnPrevious1Pressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnAdvance2Pressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnPrevious2Pressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnRightStickUpPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnRightStickDownPressed(IPlayerCursor cursor)
	{
	}

	public virtual void GoToNextScreen()
	{
	}

	public virtual void SelectOption(int option, int player)
	{
	}

	protected virtual void initVisualCursorDevice(PlayerCursor cursor, PlayerNum playerNum)
	{
		cursor.Init(playerNum);
	}

	protected virtual PlayerCursor createCursor(PlayerNum playerNum)
	{
		PlayerCursor component = UnityEngine.Object.Instantiate<GameObject>(this.CursorPrefab).GetComponent<PlayerCursor>();
		base.injector.Inject(component);
		this.uiManager.AddCursor(component);
		this.initVisualCursorDevice(component, playerNum);
		component.ResetPosition(this.getCursorDefaultPosition(playerNum));
		this.cursors.Add(component);
		return component;
	}

	protected void destroyAllCursors()
	{
		for (int i = this.cursors.Count - 1; i >= 0; i--)
		{
			this.destroyCursor(this.cursors[i]);
		}
	}

	protected virtual void destroyCursor(IPlayerCursor cursor)
	{
		this.cursors.Remove(cursor);
		UnityEngine.Object.Destroy((cursor as MonoBehaviour).gameObject);
	}

	protected void hideCursor(IPlayerCursor cursor)
	{
		cursor.IsHidden = true;
	}

	protected void showCursor(IPlayerCursor cursor)
	{
		cursor.IsHidden = false;
	}

	protected bool isCursorShown(IPlayerCursor cursor)
	{
		return !cursor.IsHidden;
	}

	protected void pauseAllCursors()
	{
		for (int i = this.cursors.Count - 1; i >= 0; i--)
		{
			this.cursors[i].IsPaused = true;
		}
	}

	protected void unpauseAllCursors()
	{
		for (int i = this.cursors.Count - 1; i >= 0; i--)
		{
			this.cursors[i].IsPaused = false;
		}
	}

	protected IPlayerCursor findPlayerCursor(PlayerNum playerNum)
	{
		for (int i = this.cursors.Count - 1; i >= 0; i--)
		{
			if (this.cursors[i] is MousePlayerCursor)
			{
				if (playerNum == PlayerNum.All)
				{
					return this.cursors[i];
				}
			}
			else
			{
				PlayerCursor playerCursor = this.cursors[i] as PlayerCursor;
				if (playerCursor != null && playerCursor.Player == playerNum)
				{
					return playerCursor;
				}
			}
		}
		return null;
	}

	public virtual void OnAnythingPressed()
	{
		if (this.isSelectionInitalized && this.uiManager.CurrentInputModule is UIInputModule)
		{
			((UIInputModule)this.uiManager.CurrentInputModule).DisableMouseControls();
			this.handleMenuSelectionOnButtonMode();
		}
		this.onMouseModeUpdate();
	}

	public virtual void OnAnyNavigationButtonPressed()
	{
	}

	public virtual void OnAnyMouseEvent()
	{
		if (this.isSelectionInitalized && this.uiManager.CurrentInputModule is UIInputModule)
		{
			((UIInputModule)this.uiManager.CurrentInputModule).EnableMouseControls();
		}
		this.onMouseModeUpdate();
	}

	protected virtual void onMouseModeUpdate()
	{
		this.uiManager.OnUpdateMouseMode();
		foreach (InputInstructions current in this.inputInstructions)
		{
			current.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
	}

	protected MenuItemList createMenuController()
	{
		MenuItemList instance = base.injector.GetInstance<MenuItemList>();
		this.registerMenuController(instance);
		return instance;
	}

	private void registerMenuController(MenuItemList menu)
	{
		this.menuControllers.Add(menu);
	}

	protected bool isAnyMenuItemSelected()
	{
		foreach (MenuItemList current in this.menuControllers)
		{
			if (current.CurrentSelection != null)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual bool isAnythingFocused()
	{
		return this.isAnyMenuItemSelected() || this.uiManager.GetWindowCount() > 0;
	}

	protected virtual void handleMenuSelectionOnButtonMode()
	{
		if (!this.isAnythingFocused())
		{
			FirstSelectionInfo initialSelection = this.getInitialSelection();
			if (initialSelection != null)
			{
				if (initialSelection.button && initialSelection.button.InteractableButton && initialSelection.menu != null)
				{
					this.FirstSelected = initialSelection.button.InteractableButton.gameObject;
					initialSelection.menu.AutoSelect(initialSelection.button);
				}
				else
				{
					UnityEngine.Debug.LogWarningFormat("Null Button, Null interactableButton, or Null Menu in WillSelect. {0}, {1}", new object[]
					{
						(initialSelection.button).ToString(),
						(initialSelection.button.InteractableButton).ToString(),
						(initialSelection.menu != null).ToString()
					});
				}
			}
		}
	}

	protected virtual FirstSelectionInfo getInitialSelection()
	{
		if (this.menuControllers.Count > 0)
		{
			MenuItemList menuItemList = this.menuControllers[0];
			MenuItemButton[] buttons = menuItemList.GetButtons();
			if (buttons.Length > 0)
			{
				return new FirstSelectionInfo(menuItemList, buttons[0]);
			}
		}
		return null;
	}

	protected void selectTextField(WavedashTMProInput field)
	{
		this.uiManager.CurrentInputModule.SetSelectedInputField(field);
	}

	protected void initMenuSelectionSystem()
	{
		this.isSelectionInitalized = true;
		if (this.uiManager.CurrentInputModule != null)
		{
			if (this.uiManager.CurrentInputModule.CurrentMode != ControlMode.MouseMode)
			{
				this.OnAnythingPressed();
			}
			else
			{
				this.OnAnyMouseEvent();
			}
		}
	}

	protected GameObject addBackButtonForCursorScreen(Transform buttonAnchor, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		while (buttonAnchor.childCount > 0)
		{
			Transform child = buttonAnchor.GetChild(0);
			child.SetParent(null);
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
		gameObject.transform.SetParent(buttonAnchor, false);
		CursorTargetButton componentInChildren = gameObject.GetComponentInChildren<CursorTargetButton>();
		componentInChildren.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onBackButtonSubmit);
		return gameObject;
	}

	private void onBackButtonSubmit(CursorTargetButton theButton, PointerEventData pointerEvent)
	{
		this.GoToPreviousScreen();
	}

	protected InputInstructions addBackButtonForMenuScreen(Transform buttonAnchor, GameObject prefab)
	{
		InputInstructions inputInstructions = this.addInputInstuctionsForMenuScreen(buttonAnchor, prefab);
		WavedashUIButton expr_0F = inputInstructions.MouseInstuctionsButton;
		expr_0F.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(expr_0F.OnPointerClickEvent, new Action<InputEventData>(this.onBackButtonClicked));
		return inputInstructions;
	}

	protected InputInstructions addInputInstuctionsForMenuScreen(Transform instructionsAnchor, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		while (instructionsAnchor.childCount > 0)
		{
			Transform child = instructionsAnchor.GetChild(0);
			child.SetParent(null);
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
		InputInstructions component = gameObject.GetComponent<InputInstructions>();
		if (component == null)
		{
			UnityEngine.Debug.LogError("Could not find an Input Instructions component on this object. One must be attached.");
		}
		gameObject.transform.SetParent(instructionsAnchor, false);
		this.inputInstructions.Add(component);
		ControlMode controlMode = (this.uiManager.CurrentInputModule == null) ? ControlMode.MouseMode : this.uiManager.CurrentInputModule.CurrentMode;
		component.SetControlMode(controlMode);
		return component;
	}

	protected virtual void onBackButtonClicked(InputEventData inputEventData)
	{
		this.GoToPreviousScreen();
	}

	public virtual void GoToPreviousScreen()
	{
		base.events.Broadcast(new PreviousScreenRequest());
	}

	public virtual void OnCancelPressed()
	{
	}

	public virtual void OnSubmitPressed()
	{
	}

	public virtual void OnRightTriggerPressed()
	{
	}

	public virtual void OnLeftTriggerPressed()
	{
	}

	public virtual void OnLeftBumperPressed()
	{
	}

	public virtual void OnZPressed()
	{
	}

	public virtual void OnRightStickRight()
	{
	}

	public virtual void OnRightStickLeft()
	{
	}

	public virtual void OnRightStickUp()
	{
	}

	public virtual void OnRightStickDown()
	{
	}

	public virtual void UpdateRightStick(float x, float y)
	{
	}

	public virtual void OnLeft()
	{
	}

	public virtual void OnRight()
	{
	}

	public virtual void OnUp()
	{
	}

	public virtual void OnDown()
	{
	}

	public virtual void OnDPadLeft()
	{
	}

	public virtual void OnDPadRight()
	{
	}

	public virtual void OnDPadUp()
	{
	}

	public virtual void OnDPadDown()
	{
	}

	public virtual void OnYButtonPressed()
	{
	}

	public virtual void OnXButtonPressed()
	{
	}
}
