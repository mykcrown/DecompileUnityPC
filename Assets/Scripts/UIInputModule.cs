// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputModule : StandaloneInputModule, ICustomInputModule
{
	public IScreenInputDelegate ScreenDelegate;

	public IButtonInputDelegate ButtonDelegate;

	public List<IGlobalInputDelegate> GlobalDelegates = new List<IGlobalInputDelegate>();

	private bool allowMouseInput;

	private bool focusOnMouseHover;

	private int suppressActionsForFrames;

	private Vector3 thisMousePosition;

	private Vector3 lastMousePosition;

	private MoveDirection thisRightStickHorizontal = MoveDirection.None;

	private MoveDirection lastRightStickHorizontal = MoveDirection.None;

	private MoveDirection thisRightStickVertical = MoveDirection.None;

	private MoveDirection lastRightStickVertical = MoveDirection.None;

	private MoveDirection thisLeftStickHorizontal = MoveDirection.None;

	private MoveDirection lastLeftStickHorizontal = MoveDirection.None;

	private MoveDirection thisLeftStickVertical = MoveDirection.None;

	private MoveDirection lastLeftStickVertical = MoveDirection.None;

	private float leftStickX;

	private float leftStickY;

	private float rightStickX;

	private float rightStickY;

	private Dictionary<IInputControl, float> nextButtonRepeatTime = new Dictionary<IInputControl, float>();

	private Dictionary<IInputControl, Dictionary<MoveDirection, float>> nextAnalogRepeatTime = new Dictionary<IInputControl, Dictionary<MoveDirection, float>>();

	private ControlMode currentMode;

	private IInputDevice _lockingDevice;

	private PlayerInputPort lockingPlayer;

	public MenuActions Actions;

	public float analogMoveThreshold = 0.5f;

	public float moveRepeatFirstDuration = 0.3f;

	[SerializeField]
	private float moveRepeatDelayDuration = 0.07f;

	public float overrideHorizontalRepeatDelay = -1f;

	private GameObject hoverObject;

	private WavedashTMProInput currentInputField;

	private static bool disableMenuRightStick;

	private float rightStickStuckSpinTime;

	[Inject]
	public IUserInputManager userInputManager
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

	public UIManager uiManager
	{
		private get;
		set;
	}

	public PlayerAction SubmitAction
	{
		get;
		set;
	}

	public PlayerAction CancelAction
	{
		get;
		set;
	}

	public PlayerAction LeftAction
	{
		get;
		set;
	}

	public PlayerAction RightAction
	{
		get;
		set;
	}

	public PlayerAction UpAction
	{
		get;
		set;
	}

	public PlayerAction DownAction
	{
		get;
		set;
	}

	public PlayerAction YButtonAction
	{
		get;
		set;
	}

	public PlayerAction XButtonAction
	{
		get;
		set;
	}

	public PlayerAction RightTriggerAction
	{
		get;
		set;
	}

	public PlayerAction LeftTriggerAction
	{
		get;
		set;
	}

	public PlayerAction LeftBumperAction
	{
		get;
		set;
	}

	public PlayerAction ZButtonAction
	{
		get;
		set;
	}

	public PlayerAction DPadLeftAction
	{
		get;
		set;
	}

	public PlayerAction DPadRightAction
	{
		get;
		set;
	}

	public PlayerAction DPadUpAction
	{
		get;
		set;
	}

	public PlayerAction DPadDownAction
	{
		get;
		set;
	}

	public PlayerTwoAxisAction LeftStickAction
	{
		get;
		set;
	}

	public PlayerTwoAxisAction RightStickAction
	{
		get;
		set;
	}

	public ControlMode CurrentMode
	{
		get
		{
			return this.currentMode;
		}
	}

	public bool IsMouseMode
	{
		get
		{
			return this.currentMode == ControlMode.MouseMode;
		}
	}

	public bool IsLocked
	{
		get
		{
			return this.lockingPlayer != null;
		}
	}

	public WavedashTMProInput CurrentInputField
	{
		get
		{
			return this.currentInputField;
		}
	}

	public static bool DisableMenuRightStick
	{
		get
		{
			return UIInputModule.disableMenuRightStick;
		}
	}

	public PlayerInputPort LockingPort
	{
		get
		{
			return this.lockingPlayer;
		}
		set
		{
			if (value == null)
			{
				this.lockingPlayer = null;
				this.lockingDevice = null;
			}
			else
			{
				this.lockingPlayer = value;
				this.lockingDevice = value.Device;
			}
		}
	}

	private IInputDevice lockingDevice
	{
		set
		{
			this._lockingDevice = value;
			if (this._lockingDevice is InputDevice)
			{
				this.Actions.Device = (this._lockingDevice as InputDevice);
				this.disableKeyboardBindings();
				this.enableControllerBindings();
			}
			else if (this._lockingDevice is KeyboardInputDevice)
			{
				this.Actions.Device = null;
				this.enableKeyboardBindings();
				this.disableControllerBindings();
			}
			else
			{
				this.Actions.Device = null;
				this.enableKeyboardBindings();
				this.enableControllerBindings();
			}
		}
	}

	private bool submitWasPressed
	{
		get
		{
			return this.wasPressed(this.SubmitAction);
		}
	}

	private bool cancelWasPressed
	{
		get
		{
			return this.wasPressed(this.CancelAction);
		}
	}

	private bool yButtonWasPressed
	{
		get
		{
			return this.wasPressed(this.YButtonAction);
		}
	}

	private bool xButtonWasPressed
	{
		get
		{
			return this.wasPressed(this.XButtonAction);
		}
	}

	private bool rightTriggerWasPressed
	{
		get
		{
			return this.wasPressed(this.RightTriggerAction);
		}
	}

	private bool leftTriggerWasPressed
	{
		get
		{
			return this.wasPressed(this.LeftTriggerAction);
		}
	}

	private bool leftBumperWasPressed
	{
		get
		{
			return this.wasPressed(this.LeftBumperAction);
		}
	}

	private bool zWasPressed
	{
		get
		{
			return this.wasPressed(this.ZButtonAction);
		}
	}

	private bool dPadLeftWasPressed
	{
		get
		{
			return this.wasPressed(this.DPadLeftAction);
		}
	}

	private bool dPadRightWasPressed
	{
		get
		{
			return this.wasPressed(this.DPadRightAction);
		}
	}

	private bool dPadUpWasPressed
	{
		get
		{
			return this.wasPressed(this.DPadUpAction);
		}
	}

	private bool dPadDownWasPressed
	{
		get
		{
			return this.wasPressed(this.DPadDownAction);
		}
	}

	private bool leftWasPressed
	{
		get
		{
			return this.wasPressed(this.LeftAction);
		}
	}

	private bool rightWasPressed
	{
		get
		{
			return this.wasPressed(this.RightAction);
		}
	}

	private bool upWasPressed
	{
		get
		{
			return this.wasPressed(this.UpAction);
		}
	}

	private bool downWasPressed
	{
		get
		{
			return this.wasPressed(this.DownAction);
		}
	}

	private bool leftIsRepeat
	{
		get
		{
			return this.isRepeat(this.LeftAction);
		}
	}

	private bool rightIsRepeat
	{
		get
		{
			return this.isRepeat(this.RightAction);
		}
	}

	private bool upIsRepeat
	{
		get
		{
			return this.isRepeat(this.UpAction);
		}
	}

	private bool downIsRepeat
	{
		get
		{
			return this.isRepeat(this.DownAction);
		}
	}

	private bool anythingWasPressed
	{
		get
		{
			return this.cancelWasPressed || this.submitWasPressed || this.leftWasPressed || this.rightWasPressed || this.upWasPressed || this.downWasPressed || this.leftStickHorizontalWasPressed || this.leftStickVerticalWasPressed || this.rightTriggerWasPressed || this.leftTriggerWasPressed || this.dPadLeftWasPressed || this.dPadRightWasPressed || this.dPadUpWasPressed || this.dPadDownWasPressed || this.leftBumperWasPressed;
		}
	}

	private bool rightStickHorizontalIsPressed
	{
		get
		{
			return this.thisRightStickHorizontal != MoveDirection.None;
		}
	}

	private bool rightStickVerticalIsPressed
	{
		get
		{
			return this.thisRightStickVertical != MoveDirection.None;
		}
	}

	private bool rightStickHorizontalWasPressed
	{
		get
		{
			return this.rightStickHorizontalIsPressed && this.lastRightStickHorizontal == this.thisRightStickHorizontal;
		}
	}

	private bool rightStickVerticalWasPressed
	{
		get
		{
			return this.rightStickVerticalIsPressed && this.lastRightStickVertical == this.thisRightStickVertical;
		}
	}

	private bool leftStickHorizontalIsPressed
	{
		get
		{
			return this.thisLeftStickHorizontal != MoveDirection.None;
		}
	}

	private bool leftStickVerticalIsPressed
	{
		get
		{
			return this.thisLeftStickVertical != MoveDirection.None;
		}
	}

	private bool leftStickHorizontalWasPressed
	{
		get
		{
			return this.leftStickHorizontalIsPressed && this.lastLeftStickHorizontal != this.thisLeftStickHorizontal;
		}
	}

	private bool leftStickVerticalWasPressed
	{
		get
		{
			return this.leftStickVerticalIsPressed && this.lastLeftStickVertical != this.thisLeftStickVertical;
		}
	}

	private bool anyMouseEvent
	{
		get
		{
			return this.mouseHasMoved || this.mouseButtonIsPressed;
		}
	}

	private bool mouseHasMoved
	{
		get
		{
			return this.thisMousePosition != this.lastMousePosition;
		}
	}

	private bool mouseButtonIsPressed
	{
		get
		{
			return Input.GetMouseButtonDown(0);
		}
	}

	public void InitControlMode(ControlMode controlMode)
	{
		this.currentMode = controlMode;
	}

	public void SetPauseMode(bool isPause)
	{
		this.Actions.SetPauseMode(isPause);
	}

	private void enableKeyboardBindings()
	{
		this.Actions.EnableKeyboard();
	}

	private void disableKeyboardBindings()
	{
		this.Actions.DisableKeyboard();
	}

	private void enableControllerBindings()
	{
		this.Actions.EnableController();
	}

	private void disableControllerBindings()
	{
		this.Actions.DisableController();
	}

	private bool keyboardPressed()
	{
		return this.suppressActionsForFrames <= 0 && this.Actions.KeyboardPressed();
	}

	private bool isPressed(PlayerAction action)
	{
		return this.suppressActionsForFrames <= 0 && action.IsPressed;
	}

	private bool wasPressed(PlayerAction action)
	{
		return this.suppressActionsForFrames <= 0 && action.WasPressed;
	}

	private InputEventData getEventData(PlayerAction action, bool isMouseEvent)
	{
		InputEventData result = default(InputEventData);
		result.isMouseEvent = isMouseEvent;
		result.port = null;
		if (!isMouseEvent)
		{
			IInputDevice eventDevice = this.getEventDevice(action);
			if (eventDevice != null)
			{
				result.port = this.userInputManager.GetPortWithDevice(eventDevice);
			}
		}
		return result;
	}

	private IInputDevice getEventDevice(PlayerAction action)
	{
		if (action.ActiveDevice == null)
		{
			return null;
		}
		if (action.ActiveDevice == InputDevice.Null)
		{
			return this.playerInput.Input.GetKeyboard();
		}
		return action.ActiveDevice;
	}

	private bool isRepeat(PlayerAction button)
	{
		return this.isPressed(button) && Time.realtimeSinceStartup > this.getButtonRepeatTime(button);
	}

	private float getButtonRepeatTime(IInputControl button)
	{
		if (this.nextButtonRepeatTime.ContainsKey(button))
		{
			return this.nextButtonRepeatTime[button];
		}
		return 0f;
	}

	private float getAnalogRepeatTime(IInputControl button, MoveDirection direction)
	{
		if (this.nextAnalogRepeatTime.ContainsKey(button) && this.nextAnalogRepeatTime[button].ContainsKey(direction))
		{
			return this.nextAnalogRepeatTime[button][direction];
		}
		return 0f;
	}

	private void setNextAnalogRepeatTime(IInputControl button, MoveDirection direction, float value)
	{
		if (!this.nextAnalogRepeatTime.ContainsKey(button))
		{
			this.nextAnalogRepeatTime[button] = new Dictionary<MoveDirection, float>();
		}
		this.nextAnalogRepeatTime[button][direction] = value;
	}

	public void SupressButtonsPressedThisFrame()
	{
		this.suppressActionsForFrames = 1;
		this.ShouldActivateModule();
		this.UpdateModule();
	}

	public override bool ShouldActivateModule()
	{
		if (!base.enabled || !base.gameObject.activeInHierarchy)
		{
			return false;
		}
		this.updateInputState();
		bool flag = this.anythingWasPressed;
		if (this.allowMouseInput)
		{
			flag |= this.anyMouseEvent;
		}
		return flag;
	}

	public override void ActivateModule()
	{
		base.ActivateModule();
		this.thisMousePosition = Input.mousePosition;
		this.lastMousePosition = Input.mousePosition;
		GameObject gameObject = base.eventSystem.currentSelectedGameObject;
		if (gameObject == null)
		{
			gameObject = base.eventSystem.firstSelectedGameObject;
		}
		this.selectionManager.Select(gameObject, this.GetBaseEventData());
	}

	private bool rightStickDirectionIsRepeat(MoveDirection direction)
	{
		if (direction == MoveDirection.None)
		{
			return false;
		}
		bool flag = direction == this.thisRightStickHorizontal || direction == this.thisRightStickVertical;
		return flag && Time.realtimeSinceStartup > this.getAnalogRepeatTime(this.RightStickAction, direction);
	}

	private bool leftStickDirectionIsRepeat(MoveDirection direction)
	{
		if (direction == MoveDirection.None)
		{
			return false;
		}
		bool flag = direction == this.thisLeftStickHorizontal || direction == this.thisLeftStickVertical;
		return flag && Time.realtimeSinceStartup > this.getAnalogRepeatTime(this.LeftStickAction, direction);
	}

	public override void UpdateModule()
	{
		this.lastMousePosition = this.thisMousePosition;
		this.thisMousePosition = Input.mousePosition;
	}

	protected override void ProcessMove(PointerEventData pointerEvent)
	{
		if (this.currentMode != ControlMode.MouseMode)
		{
			return;
		}
		GameObject pointerEnter = pointerEvent.pointerEnter;
		GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
		base.HandlePointerExitAndEnter(pointerEvent, gameObject);
		if (this.focusOnMouseHover && pointerEnter != pointerEvent.pointerEnter)
		{
			GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(pointerEvent.pointerEnter);
			this.selectionManager.Select(eventHandler, pointerEvent);
			this.hoverObject = eventHandler;
		}
	}

	public void DisableMouseControls()
	{
		this.focusOnMouseHover = false;
		this.allowMouseInput = false;
	}

	public void EnableMouseControls()
	{
		this.focusOnMouseHover = true;
		this.allowMouseInput = true;
	}

	public override void Process()
	{
	}

	private void checkModeUpdates()
	{
		bool anythingWasPressed = this.anythingWasPressed;
		bool anyMouseEvent = this.anyMouseEvent;
		ControlMode controlMode = this.currentMode;
		if (anyMouseEvent)
		{
			this.currentMode = ControlMode.MouseMode;
		}
		else if (anythingWasPressed)
		{
			if (this.keyboardPressed())
			{
				this.currentMode = ControlMode.KeyboardMode;
			}
			else
			{
				this.currentMode = ControlMode.ControllerMode;
			}
		}
		if (controlMode != this.currentMode)
		{
			controlMode = this.currentMode;
			this.uiManager.OnUpdateMouseMode();
		}
		if (this.ScreenDelegate != null)
		{
			if (anythingWasPressed)
			{
				this.ScreenDelegate.OnAnythingPressed();
			}
			if (anyMouseEvent)
			{
				this.ScreenDelegate.OnAnyMouseEvent();
			}
		}
		if (this.ButtonDelegate != null)
		{
			if (anythingWasPressed)
			{
				this.ButtonDelegate.OnAnythingPressed();
			}
			if (anyMouseEvent)
			{
				this.ButtonDelegate.OnAnyMouseEvent();
			}
		}
	}

	private void Update()
	{
		WavedashTMProInput inputFieldThisFrame = this.currentInputField;
		if (this.allowMouseInput)
		{
			base.ProcessMouseEvent();
			this.selectionManager.Validate();
			if (Input.GetMouseButtonDown(0))
			{
				this.setCurrentlyClicked(this.hoverObject);
			}
		}
		this.updateInputState();
		this.checkModeUpdates();
		this.sendVectorEventToSelectedObject();
		this.sendButtonEventToSelectedObject();
		foreach (IGlobalInputDelegate current in this.GlobalDelegates)
		{
			this.sendInputsToGlobalDelegate(current);
		}
		if (this.ButtonDelegate != null)
		{
			if (!UIInputModule.DisableMenuRightStick)
			{
				this.ButtonDelegate.UpdateRightStick(this.rightStickX, this.rightStickY);
			}
			bool flag = false;
			if (this.cancelWasPressed)
			{
				this.ButtonDelegate.OnCancelPressed();
			}
			if (this.submitWasPressed)
			{
				flag = true;
				this.ButtonDelegate.OnSubmitPressed();
			}
			if (this.yButtonWasPressed)
			{
				this.ButtonDelegate.OnYButtonPressed();
			}
			if (this.xButtonWasPressed)
			{
				this.ButtonDelegate.OnXButtonPressed();
			}
			if (this.rightTriggerWasPressed)
			{
				this.ButtonDelegate.OnRightTriggerPressed();
			}
			if (this.leftTriggerWasPressed)
			{
				this.ButtonDelegate.OnLeftTriggerPressed();
			}
			if (this.zWasPressed)
			{
				this.ButtonDelegate.OnZPressed();
			}
			if (this.leftBumperWasPressed)
			{
				this.ButtonDelegate.OnLeftBumperPressed();
			}
			if (this.dPadLeftWasPressed)
			{
				flag = true;
				this.ButtonDelegate.OnDPadLeft();
			}
			if (this.dPadRightWasPressed)
			{
				flag = true;
				this.ButtonDelegate.OnDPadRight();
			}
			if (this.dPadUpWasPressed)
			{
				flag = true;
				this.ButtonDelegate.OnDPadUp();
			}
			if (this.dPadDownWasPressed)
			{
				flag = true;
				this.ButtonDelegate.OnDPadDown();
			}
			if (this.leftWasPressed || this.leftIsRepeat)
			{
				flag = true;
				this.ButtonDelegate.OnLeft();
			}
			if (this.rightWasPressed || this.rightIsRepeat)
			{
				flag = true;
				this.ButtonDelegate.OnRight();
			}
			if (this.upWasPressed || this.upIsRepeat)
			{
				flag = true;
				this.ButtonDelegate.OnUp();
			}
			if (this.downWasPressed || this.downIsRepeat)
			{
				flag = true;
				this.ButtonDelegate.OnDown();
			}
			if (this.leftStickHorizontalWasPressed || this.leftStickDirectionIsRepeat(this.thisLeftStickHorizontal))
			{
				MoveDirection moveDirection = this.thisLeftStickHorizontal;
				if (moveDirection != MoveDirection.Left)
				{
					if (moveDirection == MoveDirection.Right)
					{
						flag = true;
						this.ButtonDelegate.OnRight();
					}
				}
				else
				{
					flag = true;
					this.ButtonDelegate.OnLeft();
				}
			}
			if (this.leftStickVerticalWasPressed || this.leftStickDirectionIsRepeat(this.thisLeftStickVertical))
			{
				MoveDirection moveDirection2 = this.thisLeftStickVertical;
				if (moveDirection2 != MoveDirection.Up)
				{
					if (moveDirection2 == MoveDirection.Down)
					{
						flag = true;
						this.ButtonDelegate.OnDown();
					}
				}
				else
				{
					flag = true;
					this.ButtonDelegate.OnUp();
				}
			}
			if (!UIInputModule.DisableMenuRightStick)
			{
				if (this.rightStickHorizontalWasPressed || this.rightStickDirectionIsRepeat(this.thisRightStickHorizontal))
				{
					MoveDirection moveDirection3 = this.thisRightStickHorizontal;
					if (moveDirection3 != MoveDirection.Right)
					{
						if (moveDirection3 == MoveDirection.Left)
						{
							this.ButtonDelegate.OnRightStickLeft();
						}
					}
					else
					{
						this.ButtonDelegate.OnRightStickRight();
					}
				}
				if (this.rightStickVerticalWasPressed || this.rightStickDirectionIsRepeat(this.thisRightStickVertical))
				{
					MoveDirection moveDirection4 = this.thisRightStickVertical;
					if (moveDirection4 != MoveDirection.Up)
					{
						if (moveDirection4 == MoveDirection.Down)
						{
							this.ButtonDelegate.OnRightStickDown();
						}
					}
					else
					{
						this.ButtonDelegate.OnRightStickUp();
					}
				}
			}
			if (flag)
			{
				this.ButtonDelegate.OnAnyNavigationButtonPressed();
			}
		}
		this.checkInputField(inputFieldThisFrame);
		this.postProcess();
	}

	private void sendInputsToGlobalDelegate(IGlobalInputDelegate globalDelegate)
	{
		if (this.xButtonWasPressed)
		{
			globalDelegate.OnXButtonPressed();
		}
	}

	public void SetSelectedInputField(WavedashTMProInput inputField)
	{
		if (inputField == null)
		{
			this.releaseCurrentlyClicked();
		}
		else
		{
			this.setCurrentlyClicked(inputField.gameObject);
		}
	}

	private void setCurrentlyClicked(GameObject obj)
	{
		if (this.currentInputField != obj)
		{
			if (this.currentInputField != null)
			{
				WavedashTMProInput expr_28 = this.currentInputField;
				expr_28.EndEditCallback = (Action)Delegate.Remove(expr_28.EndEditCallback, new Action(this.releaseCurrentlyClicked));
				this.currentInputField.SetInputActive(false);
			}
			if (obj == null)
			{
				this.currentInputField = null;
			}
			else
			{
				this.currentInputField = obj.GetComponent<WavedashTMProInput>();
			}
			if (this.currentInputField != null)
			{
				this.currentInputField.SetInputActive(true);
				WavedashTMProInput expr_9C = this.currentInputField;
				expr_9C.EndEditCallback = (Action)Delegate.Combine(expr_9C.EndEditCallback, new Action(this.releaseCurrentlyClicked));
			}
			bool value = this.currentInputField != null;
			this.Actions.SuppressKeyboard(value);
		}
	}

	private void releaseCurrentlyClicked()
	{
		this.setCurrentlyClicked(null);
		this.suppressActionsForFrames = 2;
	}

	private void checkInputField(WavedashTMProInput inputFieldThisFrame)
	{
		if (inputFieldThisFrame != null)
		{
			BaseEventData baseEventData = this.GetBaseEventData();
			if (Input.GetKeyDown(KeyCode.Return))
			{
				inputFieldThisFrame.OnSubmitPressed();
			}
			else if (Input.GetKeyDown(KeyCode.Tab))
			{
				inputFieldThisFrame.OnTabPressed();
			}
			if (inputFieldThisFrame != null)
			{
				ExecuteEvents.Execute<IUpdateSelectedHandler>(inputFieldThisFrame.gameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			}
		}
	}

	private void postProcess()
	{
		this.postProcessAnalogRepeatState(this.LeftStickAction, ref this.thisLeftStickHorizontal, ref this.thisLeftStickVertical);
		this.postProcessAnalogRepeatState(this.RightStickAction, ref this.thisRightStickHorizontal, ref this.thisRightStickVertical);
		this.postProcessButtonRepeatState(this.LeftAction);
		this.postProcessButtonRepeatState(this.RightAction);
		this.postProcessButtonRepeatState(this.UpAction);
		this.postProcessButtonRepeatState(this.DownAction);
		if (this.suppressActionsForFrames > 0)
		{
			this.suppressActionsForFrames--;
		}
	}

	private void postProcessButtonRepeatState(PlayerAction button)
	{
		if (this.isRepeat(button))
		{
			this.nextButtonRepeatTime[button] = Time.realtimeSinceStartup + this.moveRepeatDelayDuration;
		}
	}

	private void postProcessAnalogRepeatState(PlayerTwoAxisAction button, ref MoveDirection thisHorizontal, ref MoveDirection thisVertical)
	{
		if (thisHorizontal != MoveDirection.None && Time.realtimeSinceStartup > this.getAnalogRepeatTime(button, thisHorizontal))
		{
			float num = this.moveRepeatDelayDuration;
			if (this.overrideHorizontalRepeatDelay != -1f)
			{
				num = this.overrideHorizontalRepeatDelay;
			}
			this.setNextAnalogRepeatTime(button, thisHorizontal, Time.realtimeSinceStartup + num);
		}
		if (thisVertical != MoveDirection.None && Time.realtimeSinceStartup > this.getAnalogRepeatTime(button, thisVertical))
		{
			this.setNextAnalogRepeatTime(button, thisVertical, Time.realtimeSinceStartup + this.moveRepeatDelayDuration);
		}
	}

	public void SetHorizontalRepeatDelay(float repeatDelay)
	{
		this.overrideHorizontalRepeatDelay = repeatDelay;
	}

	public void ResetHorizontalRepeatDelay()
	{
		this.overrideHorizontalRepeatDelay = -1f;
	}

	private void updateInputState()
	{
		float num = this.rightStickX;
		float num2 = this.rightStickY;
		this.updateAnalogState(this.LeftStickAction, ref this.thisLeftStickHorizontal, ref this.lastLeftStickHorizontal, ref this.thisLeftStickVertical, ref this.lastLeftStickVertical, ref this.leftStickX, ref this.leftStickY);
		this.updateAnalogState(this.RightStickAction, ref this.thisRightStickHorizontal, ref this.lastRightStickHorizontal, ref this.thisRightStickVertical, ref this.lastRightStickVertical, ref this.rightStickX, ref this.rightStickY);
		if (this.rightStickStuckSpinTime < 5f)
		{
			if (Mathf.Abs(this.rightStickX) == 1f && Mathf.Abs(this.rightStickY) == 1f && num == this.rightStickX && num2 == this.rightStickY)
			{
				this.rightStickStuckSpinTime += Time.deltaTime;
				if (this.rightStickStuckSpinTime >= 5f)
				{
					UIInputModule.disableMenuRightStick = true;
				}
			}
			else
			{
				this.rightStickStuckSpinTime = 0f;
			}
		}
		this.updateButtonRepeatState(this.LeftAction);
		this.updateButtonRepeatState(this.RightAction);
		this.updateButtonRepeatState(this.UpAction);
		this.updateButtonRepeatState(this.DownAction);
	}

	private void updateButtonRepeatState(PlayerAction button)
	{
		if (!this.isPressed(button))
		{
			this.nextButtonRepeatTime[button] = 0f;
		}
		else if (this.wasPressed(button))
		{
			this.nextButtonRepeatTime[button] = Time.realtimeSinceStartup + this.moveRepeatFirstDuration;
		}
	}

	private void updateAnalogState(PlayerTwoAxisAction action, ref MoveDirection thisHorizontal, ref MoveDirection lastHorizontal, ref MoveDirection thisVertical, ref MoveDirection lastVertical, ref float rawX, ref float rawY)
	{
		lastHorizontal = thisHorizontal;
		lastVertical = thisVertical;
		thisVertical = MoveDirection.None;
		thisHorizontal = MoveDirection.None;
		if (action != null)
		{
			if (Utility.AbsoluteIsOverThreshold(action.X, this.analogMoveThreshold))
			{
				thisHorizontal = ((action.X <= 0f) ? MoveDirection.Left : MoveDirection.Right);
			}
			if (Utility.AbsoluteIsOverThreshold(action.Y, this.analogMoveThreshold))
			{
				thisVertical = ((action.Y <= 0f) ? MoveDirection.Down : MoveDirection.Up);
			}
			rawX = action.X;
			rawY = action.Y;
			if (lastHorizontal != thisHorizontal)
			{
				this.setNextAnalogRepeatTime(action, thisHorizontal, Time.realtimeSinceStartup + this.moveRepeatFirstDuration);
			}
			if (lastVertical != thisVertical)
			{
				this.setNextAnalogRepeatTime(action, thisVertical, Time.realtimeSinceStartup + this.moveRepeatFirstDuration);
			}
		}
	}

	private void sendVectorEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null)
		{
			return;
		}
		MoveDirection moveDirection = MoveDirection.None;
		MoveDirection moveDirection2 = MoveDirection.None;
		if (this.leftWasPressed)
		{
			moveDirection = MoveDirection.Left;
		}
		else if (this.rightWasPressed)
		{
			moveDirection = MoveDirection.Right;
		}
		else if (this.leftIsRepeat)
		{
			moveDirection = MoveDirection.Left;
		}
		else if (this.rightIsRepeat)
		{
			moveDirection = MoveDirection.Right;
		}
		if (this.upWasPressed)
		{
			moveDirection2 = MoveDirection.Up;
		}
		else if (this.downWasPressed)
		{
			moveDirection2 = MoveDirection.Down;
		}
		else if (this.upIsRepeat)
		{
			moveDirection2 = MoveDirection.Up;
		}
		else if (this.downIsRepeat)
		{
			moveDirection2 = MoveDirection.Down;
		}
		if (this.leftStickVerticalWasPressed || this.leftStickDirectionIsRepeat(this.thisLeftStickVertical))
		{
			moveDirection2 = this.thisLeftStickVertical;
		}
		if (this.leftStickHorizontalWasPressed || this.leftStickDirectionIsRepeat(this.thisLeftStickHorizontal))
		{
			moveDirection = this.thisLeftStickHorizontal;
		}
		if (moveDirection2 != MoveDirection.None)
		{
			AxisEventData axisEventData = new AxisEventData(base.eventSystem);
			axisEventData.moveDir = moveDirection2;
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
		}
		if (moveDirection != MoveDirection.None)
		{
			AxisEventData axisEventData2 = new AxisEventData(base.eventSystem);
			axisEventData2.moveDir = moveDirection;
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData2, ExecuteEvents.moveHandler);
		}
	}

	private void sendButtonEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null)
		{
			return;
		}
		BaseEventData baseEventData = this.GetBaseEventData();
		if (this.submitWasPressed)
		{
			IDeviceSubmitHandler component = base.eventSystem.currentSelectedGameObject.GetComponent<IDeviceSubmitHandler>();
			if (component != null)
			{
				component.OnSubmit(this.getEventData(this.SubmitAction, false));
			}
			else
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
		}
		if (this.cancelWasPressed)
		{
			ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
		}
	}

	bool ICustomInputModule.get_enabled()
	{
		return base.enabled;
	}

	void ICustomInputModule.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
