using System;
using System.Collections.Generic;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009BF RID: 2495
public class PlayerCursor : ClientBehavior, IPlayerCursor
{
	// Token: 0x170010A9 RID: 4265
	// (get) Token: 0x060045C2 RID: 17858 RVA: 0x0013124C File Offset: 0x0012F64C
	// (set) Token: 0x060045C3 RID: 17859 RVA: 0x00131254 File Offset: 0x0012F654
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170010AA RID: 4266
	// (get) Token: 0x060045C4 RID: 17860 RVA: 0x0013125D File Offset: 0x0012F65D
	// (set) Token: 0x060045C5 RID: 17861 RVA: 0x00131265 File Offset: 0x0012F665
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x170010AB RID: 4267
	// (get) Token: 0x060045C6 RID: 17862 RVA: 0x0013126E File Offset: 0x0012F66E
	// (set) Token: 0x060045C7 RID: 17863 RVA: 0x00131276 File Offset: 0x0012F676
	public PlayerNum Player { get; private set; }

	// Token: 0x170010AC RID: 4268
	// (get) Token: 0x060045C8 RID: 17864 RVA: 0x0013127F File Offset: 0x0012F67F
	// (set) Token: 0x060045C9 RID: 17865 RVA: 0x00131287 File Offset: 0x0012F687
	public PlayerCursorActions Actions { get; private set; }

	// Token: 0x170010AD RID: 4269
	// (get) Token: 0x060045CA RID: 17866 RVA: 0x00131290 File Offset: 0x0012F690
	// (set) Token: 0x060045CB RID: 17867 RVA: 0x00131298 File Offset: 0x0012F698
	public RaycastResult[] RaycastCache { get; set; }

	// Token: 0x170010AE RID: 4270
	// (get) Token: 0x060045CC RID: 17868 RVA: 0x001312A1 File Offset: 0x0012F6A1
	// (set) Token: 0x060045CD RID: 17869 RVA: 0x001312A9 File Offset: 0x0012F6A9
	public GameObject LastSelectedObject { get; set; }

	// Token: 0x060045CE RID: 17870 RVA: 0x001312B2 File Offset: 0x0012F6B2
	public override void Awake()
	{
		this.theTransform = base.transform;
		base.Awake();
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x001312C6 File Offset: 0x0012F6C6
	public void SuppressKeyboard(bool suppress)
	{
		if (this.suppressKeyboard != suppress)
		{
			this.suppressKeyboard = suppress;
			this.updateKeyboardBindings();
		}
	}

	// Token: 0x060045D0 RID: 17872 RVA: 0x001312E4 File Offset: 0x0012F6E4
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

	// Token: 0x060045D1 RID: 17873 RVA: 0x00131337 File Offset: 0x0012F737
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

	// Token: 0x060045D2 RID: 17874 RVA: 0x00131358 File Offset: 0x0012F758
	private bool shouldDisableKeyboard()
	{
		if (this.suppressKeyboard)
		{
			return true;
		}
		bool flag = this.device is KeyboardInputDevice;
		return this.device != null && !flag;
	}

	// Token: 0x060045D3 RID: 17875 RVA: 0x00131395 File Offset: 0x0012F795
	public void ResetPosition(Vector2 vect)
	{
		this.theTransform.position = vect;
	}

	// Token: 0x170010AF RID: 4271
	// (get) Token: 0x060045D4 RID: 17876 RVA: 0x001313A8 File Offset: 0x0012F7A8
	public Vector2 Position
	{
		get
		{
			return this.theTransform.position;
		}
	}

	// Token: 0x170010B0 RID: 4272
	// (get) Token: 0x060045D5 RID: 17877 RVA: 0x001313BA File Offset: 0x0012F7BA
	public Vector3 PositionDelta
	{
		get
		{
			return this.theTransform.position - this.lastPosition;
		}
	}

	// Token: 0x170010B1 RID: 4273
	// (get) Token: 0x060045D6 RID: 17878 RVA: 0x001313D2 File Offset: 0x0012F7D2
	public int PointerId
	{
		get
		{
			return PlayerUtil.GetIntFromPlayerNum(this.Player, false);
		}
	}

	// Token: 0x170010B2 RID: 4274
	// (get) Token: 0x060045D7 RID: 17879 RVA: 0x001313E0 File Offset: 0x0012F7E0
	public bool SubmitPressed
	{
		get
		{
			return this.Actions.Submit.WasPressed;
		}
	}

	// Token: 0x170010B3 RID: 4275
	// (get) Token: 0x060045D8 RID: 17880 RVA: 0x001313F2 File Offset: 0x0012F7F2
	public bool SubmitHeld
	{
		get
		{
			return this.Actions.Submit.IsPressed;
		}
	}

	// Token: 0x170010B4 RID: 4276
	// (get) Token: 0x060045D9 RID: 17881 RVA: 0x00131404 File Offset: 0x0012F804
	public bool SubmitReleased
	{
		get
		{
			return this.Actions.Submit.WasReleased;
		}
	}

	// Token: 0x170010B5 RID: 4277
	// (get) Token: 0x060045DA RID: 17882 RVA: 0x00131416 File Offset: 0x0012F816
	public bool CancelPressed
	{
		get
		{
			return this.Actions.Cancel.WasPressed;
		}
	}

	// Token: 0x170010B6 RID: 4278
	// (get) Token: 0x060045DB RID: 17883 RVA: 0x00131428 File Offset: 0x0012F828
	public bool AltSubmitPressed
	{
		get
		{
			return this.Actions.AltSubmit.WasPressed || (this.Actions.AltModifier.IsPressed && this.Actions.Submit.WasPressed);
		}
	}

	// Token: 0x170010B7 RID: 4279
	// (get) Token: 0x060045DC RID: 17884 RVA: 0x00131478 File Offset: 0x0012F878
	public bool AltCancelPressed
	{
		get
		{
			return this.Actions.AltCancel.WasPressed || (this.Actions.AltModifier.IsPressed && this.Actions.Cancel.WasPressed);
		}
	}

	// Token: 0x170010B8 RID: 4280
	// (get) Token: 0x060045DD RID: 17885 RVA: 0x001314C5 File Offset: 0x0012F8C5
	public bool StartPressed
	{
		get
		{
			return this.Actions.Start.WasPressed;
		}
	}

	// Token: 0x170010B9 RID: 4281
	// (get) Token: 0x060045DE RID: 17886 RVA: 0x001314D7 File Offset: 0x0012F8D7
	public bool FaceButton3Pressed
	{
		get
		{
			return this.Actions.FaceButton3.WasPressed;
		}
	}

	// Token: 0x170010BA RID: 4282
	// (get) Token: 0x060045DF RID: 17887 RVA: 0x001314E9 File Offset: 0x0012F8E9
	public bool Advance1Pressed
	{
		get
		{
			return this.Actions.Advance1.WasPressed;
		}
	}

	// Token: 0x170010BB RID: 4283
	// (get) Token: 0x060045E0 RID: 17888 RVA: 0x001314FB File Offset: 0x0012F8FB
	public bool Previous1Pressed
	{
		get
		{
			return this.Actions.Previous1.WasPressed;
		}
	}

	// Token: 0x170010BC RID: 4284
	// (get) Token: 0x060045E1 RID: 17889 RVA: 0x0013150D File Offset: 0x0012F90D
	public bool Advance2Pressed
	{
		get
		{
			return this.Actions.Advance2.WasPressed;
		}
	}

	// Token: 0x170010BD RID: 4285
	// (get) Token: 0x060045E2 RID: 17890 RVA: 0x0013151F File Offset: 0x0012F91F
	public bool Previous2Pressed
	{
		get
		{
			return this.Actions.Previous2.WasPressed;
		}
	}

	// Token: 0x170010BE RID: 4286
	// (get) Token: 0x060045E3 RID: 17891 RVA: 0x00131531 File Offset: 0x0012F931
	public bool RightStickUpPressed
	{
		get
		{
			return this.Actions.RightStickUp.WasPressed;
		}
	}

	// Token: 0x170010BF RID: 4287
	// (get) Token: 0x060045E4 RID: 17892 RVA: 0x00131543 File Offset: 0x0012F943
	public bool RightStickDownPressed
	{
		get
		{
			return this.Actions.RightStickDown.WasPressed;
		}
	}

	// Token: 0x170010C0 RID: 4288
	// (get) Token: 0x060045E5 RID: 17893 RVA: 0x00131555 File Offset: 0x0012F955
	public bool AdvanceSelectedPressed
	{
		get
		{
			return this.Actions.Submit.IsPressed && this.Actions.Up.WasPressed;
		}
	}

	// Token: 0x170010C1 RID: 4289
	// (get) Token: 0x060045E6 RID: 17894 RVA: 0x0013157F File Offset: 0x0012F97F
	public bool PreviousSelectedPressed
	{
		get
		{
			return this.Actions.Submit.IsPressed && this.Actions.Down.WasPressed;
		}
	}

	// Token: 0x170010C2 RID: 4290
	// (get) Token: 0x060045E7 RID: 17895 RVA: 0x001315A9 File Offset: 0x0012F9A9
	public bool AnythingPressed
	{
		get
		{
			return this.keyboardPressed() || this.controllerPressed();
		}
	}

	// Token: 0x060045E8 RID: 17896 RVA: 0x001315C0 File Offset: 0x0012F9C0
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

	// Token: 0x060045E9 RID: 17897 RVA: 0x00131B9A File Offset: 0x0012FF9A
	public void InitMode(global::CursorMode mode)
	{
		this.currentMode = mode;
	}

	// Token: 0x060045EA RID: 17898 RVA: 0x00131BA4 File Offset: 0x0012FFA4
	private void enableControllerBindings()
	{
		if (!this.isControllerBindingsActive)
		{
			this.isControllerBindingsActive = true;
			foreach (ActionBinding actionBinding in this.controllerBindings)
			{
				actionBinding.Action.AddBinding(actionBinding.Button);
			}
		}
	}

	// Token: 0x060045EB RID: 17899 RVA: 0x00131C20 File Offset: 0x00130020
	private void disableControllerBindings()
	{
		if (this.isControllerBindingsActive)
		{
			this.isControllerBindingsActive = false;
			foreach (ActionBinding actionBinding in this.controllerBindings)
			{
				actionBinding.Action.RemoveBinding(actionBinding.Button);
			}
		}
	}

	// Token: 0x060045EC RID: 17900 RVA: 0x00131C98 File Offset: 0x00130098
	private bool controllerPressed()
	{
		if (this.isControllerBindingsActive)
		{
			foreach (ActionBinding actionBinding in this.controllerBindings)
			{
				if (actionBinding.Action.LastDeviceClass != InputDeviceClass.Keyboard && actionBinding.Action.IsPressed)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x060045ED RID: 17901 RVA: 0x00131D24 File Offset: 0x00130124
	private void enableKeyboardBindings()
	{
		if (!this.isKeyboardBindingsActive)
		{
			this.isKeyboardBindingsActive = true;
			foreach (ActionBinding actionBinding in this.keyboardBindings)
			{
				actionBinding.Action.AddBinding(actionBinding.Key);
			}
		}
	}

	// Token: 0x060045EE RID: 17902 RVA: 0x00131DA0 File Offset: 0x001301A0
	private void disableKeyboardBindings()
	{
		if (this.isKeyboardBindingsActive)
		{
			this.isKeyboardBindingsActive = false;
			foreach (ActionBinding actionBinding in this.keyboardBindings)
			{
				actionBinding.Action.RemoveBinding(actionBinding.Key);
			}
		}
	}

	// Token: 0x060045EF RID: 17903 RVA: 0x00131E18 File Offset: 0x00130218
	private bool keyboardPressed()
	{
		if (this.isKeyboardBindingsActive)
		{
			foreach (ActionBinding actionBinding in this.keyboardBindings)
			{
				if (actionBinding.Action.LastDeviceClass == InputDeviceClass.Keyboard && actionBinding.Action.IsPressed)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x170010C3 RID: 4291
	// (get) Token: 0x060045F0 RID: 17904 RVA: 0x00131EA4 File Offset: 0x001302A4
	// (set) Token: 0x060045F1 RID: 17905 RVA: 0x00131EAC File Offset: 0x001302AC
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

	// Token: 0x170010C4 RID: 4292
	// (get) Token: 0x060045F2 RID: 17906 RVA: 0x00131ED8 File Offset: 0x001302D8
	// (set) Token: 0x060045F3 RID: 17907 RVA: 0x00131EE0 File Offset: 0x001302E0
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

	// Token: 0x060045F4 RID: 17908 RVA: 0x00131EEC File Offset: 0x001302EC
	private Sprite getCursorSprite(PlayerNum player)
	{
		UIColor uicolorFromPlayerNum = PlayerUtil.GetUIColorFromPlayerNum(player);
		return this.CursorSprites.GetSprite(uicolorFromPlayerNum);
	}

	// Token: 0x060045F5 RID: 17909 RVA: 0x00131F0C File Offset: 0x0013030C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.Actions.Destroy();
		base.signalBus.GetSignal<SetPlayerDeviceSignal>().RemoveListener(new Action<int, IInputDevice>(this.onPlayerDeviceSet));
		base.signalBus.GetSignal<PlayerAssignedSignal>().RemoveListener(new Action<PlayerNum>(this.onPlayerAssigned));
	}

	// Token: 0x060045F6 RID: 17910 RVA: 0x00131F62 File Offset: 0x00130362
	private void onPlayerAssigned(PlayerNum player)
	{
		if (player == this.Player)
		{
			this.updateDevice();
		}
	}

	// Token: 0x060045F7 RID: 17911 RVA: 0x00131F78 File Offset: 0x00130378
	private void onPlayerDeviceSet(int portId, IInputDevice device)
	{
		PlayerInputPort portWithPlayerNum = this.userInputManager.GetPortWithPlayerNum(this.Player);
		if (portWithPlayerNum != null && portId == portWithPlayerNum.Id)
		{
			this.updateDevice();
		}
	}

	// Token: 0x060045F8 RID: 17912 RVA: 0x00131FB8 File Offset: 0x001303B8
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

	// Token: 0x060045F9 RID: 17913 RVA: 0x0013204C File Offset: 0x0013044C
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

	// Token: 0x060045FA RID: 17914 RVA: 0x001320B0 File Offset: 0x001304B0
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

	// Token: 0x060045FB RID: 17915 RVA: 0x00132129 File Offset: 0x00130529
	private void notIdle()
	{
		base.signalBus.GetSignal<PlayerCursorNotIdleSignal>().Dispatch(this.Player);
	}

	// Token: 0x060045FC RID: 17916 RVA: 0x00132144 File Offset: 0x00130544
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

	// Token: 0x060045FD RID: 17917 RVA: 0x00132224 File Offset: 0x00130624
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

	// Token: 0x060045FE RID: 17918 RVA: 0x00132274 File Offset: 0x00130674
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

	// Token: 0x170010C5 RID: 4293
	// (get) Token: 0x060045FF RID: 17919 RVA: 0x001322CE File Offset: 0x001306CE
	public bool IsInteracting
	{
		get
		{
			return !this.isHidden;
		}
	}

	// Token: 0x170010C6 RID: 4294
	// (get) Token: 0x06004600 RID: 17920 RVA: 0x001322D9 File Offset: 0x001306D9
	public global::CursorMode CurrentMode
	{
		get
		{
			return this.currentMode;
		}
	}

	// Token: 0x04002E39 RID: 11833
	private static Vector2 OFFSCREEN_DISABLE = new Vector2(-99999f, -99999f);

	// Token: 0x04002E3A RID: 11834
	public Image cursorSprite;

	// Token: 0x04002E3B RID: 11835
	public TextMeshProUGUI Text;

	// Token: 0x04002E3C RID: 11836
	public ColorSpriteContainer CursorSprites;

	// Token: 0x04002E3D RID: 11837
	private global::CursorMode currentMode = global::CursorMode.Controller;

	// Token: 0x04002E3E RID: 11838
	private bool isHidden;

	// Token: 0x04002E3F RID: 11839
	private bool isPaused;

	// Token: 0x04002E40 RID: 11840
	private float screenScaleFactor = 1f;

	// Token: 0x04002E41 RID: 11841
	private Transform theTransform;

	// Token: 0x04002E44 RID: 11844
	public float BaseSpeed = 10f;

	// Token: 0x04002E45 RID: 11845
	public float RightScreenBoundary = 40f;

	// Token: 0x04002E46 RID: 11846
	public float BottomScreenBoundary = 40f;

	// Token: 0x04002E49 RID: 11849
	private float speed;

	// Token: 0x04002E4A RID: 11850
	private bool suppressKeyboard;

	// Token: 0x04002E4B RID: 11851
	private bool initialized;

	// Token: 0x04002E4C RID: 11852
	private List<ActionBinding> keyboardBindings = new List<ActionBinding>();

	// Token: 0x04002E4D RID: 11853
	private List<ActionBinding> controllerBindings = new List<ActionBinding>();

	// Token: 0x04002E4E RID: 11854
	private bool isKeyboardBindingsActive;

	// Token: 0x04002E4F RID: 11855
	private bool isControllerBindingsActive;

	// Token: 0x04002E50 RID: 11856
	private Vector3 lastPosition;

	// Token: 0x04002E51 RID: 11857
	private IInputDevice device;
}
