// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCursor : ClientBehavior, IPlayerCursor
{
	private static Vector2 OFFSCREEN_DISABLE = new Vector2(-99999f, -99999f);

	public Image cursorSprite;

	public TextMeshProUGUI Text;

	public ColorSpriteContainer CursorSprites;

	private global::CursorMode currentMode = global::CursorMode.Controller;

	private bool isHidden;

	private bool isPaused;

	private float screenScaleFactor = 1f;

	private Transform theTransform;

	public float BaseSpeed = 10f;

	public float RightScreenBoundary = 40f;

	public float BottomScreenBoundary = 40f;

	private float speed;

	private bool suppressKeyboard;

	private bool initialized;

	private List<ActionBinding> keyboardBindings = new List<ActionBinding>();

	private List<ActionBinding> controllerBindings = new List<ActionBinding>();

	private bool isKeyboardBindingsActive;

	private bool isControllerBindingsActive;

	private Vector3 lastPosition;

	private IInputDevice device;

	[Inject]
	public ILocalization localization
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

	public PlayerNum Player
	{
		get;
		private set;
	}

	public PlayerCursorActions Actions
	{
		get;
		private set;
	}

	public RaycastResult[] RaycastCache
	{
		get;
		set;
	}

	public GameObject LastSelectedObject
	{
		get;
		set;
	}

	public Vector2 Position
	{
		get
		{
			return this.theTransform.position;
		}
	}

	public Vector3 PositionDelta
	{
		get
		{
			return this.theTransform.position - this.lastPosition;
		}
	}

	public int PointerId
	{
		get
		{
			return PlayerUtil.GetIntFromPlayerNum(this.Player, false);
		}
	}

	public bool SubmitPressed
	{
		get
		{
			return this.Actions.Submit.WasPressed;
		}
	}

	public bool SubmitHeld
	{
		get
		{
			return this.Actions.Submit.IsPressed;
		}
	}

	public bool SubmitReleased
	{
		get
		{
			return this.Actions.Submit.WasReleased;
		}
	}

	public bool CancelPressed
	{
		get
		{
			return this.Actions.Cancel.WasPressed;
		}
	}

	public bool AltSubmitPressed
	{
		get
		{
			return this.Actions.AltSubmit.WasPressed || (this.Actions.AltModifier.IsPressed && this.Actions.Submit.WasPressed);
		}
	}

	public bool AltCancelPressed
	{
		get
		{
			return this.Actions.AltCancel.WasPressed || (this.Actions.AltModifier.IsPressed && this.Actions.Cancel.WasPressed);
		}
	}

	public bool StartPressed
	{
		get
		{
			return this.Actions.Start.WasPressed;
		}
	}

	public bool FaceButton3Pressed
	{
		get
		{
			return this.Actions.FaceButton3.WasPressed;
		}
	}

	public bool Advance1Pressed
	{
		get
		{
			return this.Actions.Advance1.WasPressed;
		}
	}

	public bool Previous1Pressed
	{
		get
		{
			return this.Actions.Previous1.WasPressed;
		}
	}

	public bool Advance2Pressed
	{
		get
		{
			return this.Actions.Advance2.WasPressed;
		}
	}

	public bool Previous2Pressed
	{
		get
		{
			return this.Actions.Previous2.WasPressed;
		}
	}

	public bool RightStickUpPressed
	{
		get
		{
			return this.Actions.RightStickUp.WasPressed;
		}
	}

	public bool RightStickDownPressed
	{
		get
		{
			return this.Actions.RightStickDown.WasPressed;
		}
	}

	public bool AdvanceSelectedPressed
	{
		get
		{
			return this.Actions.Submit.IsPressed && this.Actions.Up.WasPressed;
		}
	}

	public bool PreviousSelectedPressed
	{
		get
		{
			return this.Actions.Submit.IsPressed && this.Actions.Down.WasPressed;
		}
	}

	public bool AnythingPressed
	{
		get
		{
			return this.keyboardPressed() || this.controllerPressed();
		}
	}

	public bool IsHidden
	{
		get
		{
			return this.isHidden;
		}
		set
		{
			if (this.isHidden != value)
			{
				this.isHidden = value;
				this.updateCurrentMode();
				base.signalBus.GetSignal<PlayerCursorStatusSignal>().Dispatch(this);
			}
		}
	}

	public bool IsPaused
	{
		get
		{
			return this.isPaused;
		}
		set
		{
			this.isPaused = value;
		}
	}

	public bool IsInteracting
	{
		get
		{
			return !this.isHidden;
		}
	}

	public global::CursorMode CurrentMode
	{
		get
		{
			return this.currentMode;
		}
	}

	public override void Awake()
	{
		this.theTransform = base.transform;
		base.Awake();
	}

	public void SuppressKeyboard(bool suppress)
	{
		if (this.suppressKeyboard != suppress)
		{
			this.suppressKeyboard = suppress;
			this.updateKeyboardBindings();
		}
	}

	private void updateControllerBindings()
	{
		bool flag = this.device is InputDevice && this.device != InputDevice.Null;
		if (this.device != null && !flag)
		{
			this.disableControllerBindings();
		}
		else
		{
			this.enableControllerBindings();
		}
	}

	private void updateKeyboardBindings()
	{
		if (this.shouldDisableKeyboard())
		{
			this.disableKeyboardBindings();
		}
		else
		{
			this.enableKeyboardBindings();
		}
	}

	private bool shouldDisableKeyboard()
	{
		if (this.suppressKeyboard)
		{
			return true;
		}
		bool flag = this.device is KeyboardInputDevice;
		return this.device != null && !flag;
	}

	public void ResetPosition(Vector2 vect)
	{
		this.theTransform.position = vect;
	}

	public void Init(PlayerNum player)
	{
		this.Player = player;
		this.Actions = new PlayerCursorActions();
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Submit, Key.Space));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Submit, Key.Return));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Cancel, Key.Escape));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.FaceButton3, Key.X));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.FaceButton3, Key.Backspace));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.FaceButton3, Key.Delete));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Up, Key.UpArrow));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Up, Key.W));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Down, Key.DownArrow));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Down, Key.S));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Left, Key.LeftArrow));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Left, Key.A));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Right, Key.RightArrow));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Right, Key.D));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Advance1, Key.Q));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Previous1, Key.E));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Advance2, Key.RightBracket));
		this.keyboardBindings.Add(new ActionBinding(this.Actions.Previous2, Key.LeftBracket));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Submit, InputControlType.Action1));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Cancel, InputControlType.Action2));
		this.controllerBindings.Add(new ActionBinding(this.Actions.FaceButton3, InputControlType.Action3));
		this.controllerBindings.Add(new ActionBinding(this.Actions.AltCancel, InputControlType.Action4));
		this.controllerBindings.Add(new ActionBinding(this.Actions.AltModifier, InputControlType.LeftTrigger));
		this.controllerBindings.Add(new ActionBinding(this.Actions.AltModifier, InputControlType.RightTrigger));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Start, InputControlType.Start));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Start, InputControlType.Menu));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Start, InputControlType.Options));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Up, InputControlType.LeftStickUp));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Down, InputControlType.LeftStickDown));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Left, InputControlType.LeftStickLeft));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Right, InputControlType.LeftStickRight));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Advance1, InputControlType.RightTrigger));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Previous1, InputControlType.LeftTrigger));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Advance2, InputControlType.RightBumper));
		this.controllerBindings.Add(new ActionBinding(this.Actions.Previous2, InputControlType.LeftBumper));
		this.controllerBindings.Add(new ActionBinding(this.Actions.RightStickUp, InputControlType.RightStickUp));
		this.controllerBindings.Add(new ActionBinding(this.Actions.RightStickDown, InputControlType.RightStickDown));
		this.Actions.RightStickUp.Raw = false;
		this.Actions.RightStickUp.LowerDeadZone = 0.7f;
		this.Actions.RightStickDown.Raw = false;
		this.Actions.RightStickDown.LowerDeadZone = 0.7f;
		this.updateDevice();
		this.updateKeyboardBindings();
		this.updateControllerBindings();
		this.initialized = true;
		if (this.Text != null)
		{
			if (player == PlayerNum.All)
			{
				this.Text.text = string.Empty;
			}
			else
			{
				this.Text.text = this.localization.GetText("ui.playerCursor", PlayerUtil.GetIntFromPlayerNum(player, false).ToString());
				this.Text.color = PlayerUtil.GetColorFromUIColor(PlayerUtil.GetUIColorFromPlayerNum(player));
			}
		}
		this.cursorSprite.sprite = this.getCursorSprite(player);
		CanvasScaler componentInParent = base.GetComponentInParent<CanvasScaler>();
		float num = componentInParent.scaleFactor;
		num = ((num >= 1f) ? num : 1f);
		float num2 = (float)Screen.width / base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x;
		this.screenScaleFactor = num * num2;
		this.speed = this.BaseSpeed * this.screenScaleFactor;
		this.updateCurrentMode();
		base.signalBus.GetSignal<SetPlayerDeviceSignal>().AddListener(new Action<int, IInputDevice>(this.onPlayerDeviceSet));
		base.signalBus.GetSignal<PlayerAssignedSignal>().AddListener(new Action<PlayerNum>(this.onPlayerAssigned));
	}

	public void InitMode(global::CursorMode mode)
	{
		this.currentMode = mode;
	}

	private void enableControllerBindings()
	{
		if (!this.isControllerBindingsActive)
		{
			this.isControllerBindingsActive = true;
			foreach (ActionBinding current in this.controllerBindings)
			{
				current.Action.AddBinding(current.Button);
			}
		}
	}

	private void disableControllerBindings()
	{
		if (this.isControllerBindingsActive)
		{
			this.isControllerBindingsActive = false;
			foreach (ActionBinding current in this.controllerBindings)
			{
				current.Action.RemoveBinding(current.Button);
			}
		}
	}

	private bool controllerPressed()
	{
		if (this.isControllerBindingsActive)
		{
			foreach (ActionBinding current in this.controllerBindings)
			{
				if (current.Action.LastDeviceClass != InputDeviceClass.Keyboard && current.Action.IsPressed)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	private void enableKeyboardBindings()
	{
		if (!this.isKeyboardBindingsActive)
		{
			this.isKeyboardBindingsActive = true;
			foreach (ActionBinding current in this.keyboardBindings)
			{
				current.Action.AddBinding(current.Key);
			}
		}
	}

	private void disableKeyboardBindings()
	{
		if (this.isKeyboardBindingsActive)
		{
			this.isKeyboardBindingsActive = false;
			foreach (ActionBinding current in this.keyboardBindings)
			{
				current.Action.RemoveBinding(current.Key);
			}
		}
	}

	private bool keyboardPressed()
	{
		if (this.isKeyboardBindingsActive)
		{
			foreach (ActionBinding current in this.keyboardBindings)
			{
				if (current.Action.LastDeviceClass == InputDeviceClass.Keyboard && current.Action.IsPressed)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	private Sprite getCursorSprite(PlayerNum player)
	{
		UIColor uIColorFromPlayerNum = PlayerUtil.GetUIColorFromPlayerNum(player);
		return this.CursorSprites.GetSprite(uIColorFromPlayerNum);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		this.Actions.Destroy();
		base.signalBus.GetSignal<SetPlayerDeviceSignal>().RemoveListener(new Action<int, IInputDevice>(this.onPlayerDeviceSet));
		base.signalBus.GetSignal<PlayerAssignedSignal>().RemoveListener(new Action<PlayerNum>(this.onPlayerAssigned));
	}

	private void onPlayerAssigned(PlayerNum player)
	{
		if (player == this.Player)
		{
			this.updateDevice();
		}
	}

	private void onPlayerDeviceSet(int portId, IInputDevice device)
	{
		PlayerInputPort portWithPlayerNum = this.userInputManager.GetPortWithPlayerNum(this.Player);
		if (portWithPlayerNum != null && portId == portWithPlayerNum.Id)
		{
			this.updateDevice();
		}
	}

	private void updateDevice()
	{
		if (base.battleServerAPI.IsOnlineMatchReady && base.battleServerAPI.IsSinglePlayerNetworkGame)
		{
			this.Actions.Device = null;
			this.device = null;
			this.updateKeyboardBindings();
			this.updateControllerBindings();
		}
		else
		{
			IInputDevice deviceWithPlayerNum = this.userInputManager.GetDeviceWithPlayerNum(this.Player);
			if (deviceWithPlayerNum != this.device)
			{
				this.Actions.Device = (deviceWithPlayerNum as InputDevice);
				this.device = deviceWithPlayerNum;
				this.updateKeyboardBindings();
				this.updateControllerBindings();
			}
		}
	}

	private void Update()
	{
		if (!this.initialized || this.cursorSprite == null)
		{
			return;
		}
		this.lastPosition = this.theTransform.position;
		this.updateCurrentMode();
		this.updateVisibility();
		this.updatePosition();
		if (this.isAnyInput(this.lastPosition))
		{
			this.notIdle();
		}
	}

	private bool isAnyInput(Vector3 prevPosition)
	{
		if (this.IsInteracting)
		{
			if (prevPosition != this.theTransform.position)
			{
				return true;
			}
			if (this.controllerPressed())
			{
				return true;
			}
			if (this.Actions.Submit.IsPressed)
			{
				return true;
			}
			if (this.Actions.AltSubmit.IsPressed)
			{
				return true;
			}
			if (this.SubmitPressed)
			{
				return true;
			}
		}
		return false;
	}

	private void notIdle()
	{
		base.signalBus.GetSignal<PlayerCursorNotIdleSignal>().Dispatch(this.Player);
	}

	private void updatePosition()
	{
		if (this.currentMode == global::CursorMode.Disabled)
		{
			this.theTransform.position = PlayerCursor.OFFSCREEN_DISABLE;
		}
		else if (!this.isPaused)
		{
			Vector2 value = this.Actions.Move.Value;
			float d = this.speed * (Time.deltaTime / WTime.frameTime);
			Vector3 position = this.theTransform.position + value * d;
			position.x = Mathf.Clamp(position.x, 0f, (float)Screen.width - this.RightScreenBoundary * this.screenScaleFactor);
			position.y = Mathf.Clamp(position.y, this.BottomScreenBoundary * this.screenScaleFactor, (float)Screen.height);
			this.theTransform.position = position;
		}
	}

	private void updateCurrentMode()
	{
		if (this.isHidden)
		{
			this.currentMode = global::CursorMode.Disabled;
		}
		else if (this.keyboardPressed())
		{
			this.currentMode = global::CursorMode.Keyboard;
		}
		else if (this.controllerPressed())
		{
			this.currentMode = global::CursorMode.Controller;
		}
	}

	private void updateVisibility()
	{
		if (this.currentMode == global::CursorMode.Controller || this.currentMode == global::CursorMode.Keyboard)
		{
			this.cursorSprite.enabled = true;
			this.Text.enabled = true;
		}
		else
		{
			this.cursorSprite.enabled = false;
			this.Text.enabled = false;
		}
	}
}
