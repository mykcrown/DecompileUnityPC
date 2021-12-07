using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020006B5 RID: 1717
public class UIInputModule : StandaloneInputModule, ICustomInputModule
{
	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x000E1774 File Offset: 0x000DFB74
	// (set) Token: 0x06002AD9 RID: 10969 RVA: 0x000E177C File Offset: 0x000DFB7C
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x06002ADA RID: 10970 RVA: 0x000E1785 File Offset: 0x000DFB85
	// (set) Token: 0x06002ADB RID: 10971 RVA: 0x000E178D File Offset: 0x000DFB8D
	[Inject]
	public PlayerInputLocator playerInput { get; set; }

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x06002ADC RID: 10972 RVA: 0x000E1796 File Offset: 0x000DFB96
	// (set) Token: 0x06002ADD RID: 10973 RVA: 0x000E179E File Offset: 0x000DFB9E
	[Inject]
	public ISelectionManager selectionManager { get; set; }

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x06002ADE RID: 10974 RVA: 0x000E17A7 File Offset: 0x000DFBA7
	// (set) Token: 0x06002ADF RID: 10975 RVA: 0x000E17AF File Offset: 0x000DFBAF
	public UIManager uiManager { private get; set; }

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x06002AE0 RID: 10976 RVA: 0x000E17B8 File Offset: 0x000DFBB8
	// (set) Token: 0x06002AE1 RID: 10977 RVA: 0x000E17C0 File Offset: 0x000DFBC0
	public PlayerAction SubmitAction { get; set; }

	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x000E17C9 File Offset: 0x000DFBC9
	// (set) Token: 0x06002AE3 RID: 10979 RVA: 0x000E17D1 File Offset: 0x000DFBD1
	public PlayerAction CancelAction { get; set; }

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x000E17DA File Offset: 0x000DFBDA
	// (set) Token: 0x06002AE5 RID: 10981 RVA: 0x000E17E2 File Offset: 0x000DFBE2
	public PlayerAction LeftAction { get; set; }

	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x000E17EB File Offset: 0x000DFBEB
	// (set) Token: 0x06002AE7 RID: 10983 RVA: 0x000E17F3 File Offset: 0x000DFBF3
	public PlayerAction RightAction { get; set; }

	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x000E17FC File Offset: 0x000DFBFC
	// (set) Token: 0x06002AE9 RID: 10985 RVA: 0x000E1804 File Offset: 0x000DFC04
	public PlayerAction UpAction { get; set; }

	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x06002AEA RID: 10986 RVA: 0x000E180D File Offset: 0x000DFC0D
	// (set) Token: 0x06002AEB RID: 10987 RVA: 0x000E1815 File Offset: 0x000DFC15
	public PlayerAction DownAction { get; set; }

	// Token: 0x17000A7E RID: 2686
	// (get) Token: 0x06002AEC RID: 10988 RVA: 0x000E181E File Offset: 0x000DFC1E
	// (set) Token: 0x06002AED RID: 10989 RVA: 0x000E1826 File Offset: 0x000DFC26
	public PlayerAction YButtonAction { get; set; }

	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x06002AEE RID: 10990 RVA: 0x000E182F File Offset: 0x000DFC2F
	// (set) Token: 0x06002AEF RID: 10991 RVA: 0x000E1837 File Offset: 0x000DFC37
	public PlayerAction XButtonAction { get; set; }

	// Token: 0x17000A80 RID: 2688
	// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000E1840 File Offset: 0x000DFC40
	// (set) Token: 0x06002AF1 RID: 10993 RVA: 0x000E1848 File Offset: 0x000DFC48
	public PlayerAction RightTriggerAction { get; set; }

	// Token: 0x17000A81 RID: 2689
	// (get) Token: 0x06002AF2 RID: 10994 RVA: 0x000E1851 File Offset: 0x000DFC51
	// (set) Token: 0x06002AF3 RID: 10995 RVA: 0x000E1859 File Offset: 0x000DFC59
	public PlayerAction LeftTriggerAction { get; set; }

	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x000E1862 File Offset: 0x000DFC62
	// (set) Token: 0x06002AF5 RID: 10997 RVA: 0x000E186A File Offset: 0x000DFC6A
	public PlayerAction LeftBumperAction { get; set; }

	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x06002AF6 RID: 10998 RVA: 0x000E1873 File Offset: 0x000DFC73
	// (set) Token: 0x06002AF7 RID: 10999 RVA: 0x000E187B File Offset: 0x000DFC7B
	public PlayerAction ZButtonAction { get; set; }

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000E1884 File Offset: 0x000DFC84
	// (set) Token: 0x06002AF9 RID: 11001 RVA: 0x000E188C File Offset: 0x000DFC8C
	public PlayerAction DPadLeftAction { get; set; }

	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x06002AFA RID: 11002 RVA: 0x000E1895 File Offset: 0x000DFC95
	// (set) Token: 0x06002AFB RID: 11003 RVA: 0x000E189D File Offset: 0x000DFC9D
	public PlayerAction DPadRightAction { get; set; }

	// Token: 0x17000A86 RID: 2694
	// (get) Token: 0x06002AFC RID: 11004 RVA: 0x000E18A6 File Offset: 0x000DFCA6
	// (set) Token: 0x06002AFD RID: 11005 RVA: 0x000E18AE File Offset: 0x000DFCAE
	public PlayerAction DPadUpAction { get; set; }

	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x06002AFE RID: 11006 RVA: 0x000E18B7 File Offset: 0x000DFCB7
	// (set) Token: 0x06002AFF RID: 11007 RVA: 0x000E18BF File Offset: 0x000DFCBF
	public PlayerAction DPadDownAction { get; set; }

	// Token: 0x17000A88 RID: 2696
	// (get) Token: 0x06002B00 RID: 11008 RVA: 0x000E18C8 File Offset: 0x000DFCC8
	// (set) Token: 0x06002B01 RID: 11009 RVA: 0x000E18D0 File Offset: 0x000DFCD0
	public PlayerTwoAxisAction LeftStickAction { get; set; }

	// Token: 0x17000A89 RID: 2697
	// (get) Token: 0x06002B02 RID: 11010 RVA: 0x000E18D9 File Offset: 0x000DFCD9
	// (set) Token: 0x06002B03 RID: 11011 RVA: 0x000E18E1 File Offset: 0x000DFCE1
	public PlayerTwoAxisAction RightStickAction { get; set; }

	// Token: 0x17000A8A RID: 2698
	// (get) Token: 0x06002B04 RID: 11012 RVA: 0x000E18EA File Offset: 0x000DFCEA
	public ControlMode CurrentMode
	{
		get
		{
			return this.currentMode;
		}
	}

	// Token: 0x17000A8B RID: 2699
	// (get) Token: 0x06002B05 RID: 11013 RVA: 0x000E18F2 File Offset: 0x000DFCF2
	public bool IsMouseMode
	{
		get
		{
			return this.currentMode == ControlMode.MouseMode;
		}
	}

	// Token: 0x17000A8C RID: 2700
	// (get) Token: 0x06002B06 RID: 11014 RVA: 0x000E18FD File Offset: 0x000DFCFD
	public bool IsLocked
	{
		get
		{
			return this.lockingPlayer != null;
		}
	}

	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x06002B07 RID: 11015 RVA: 0x000E190B File Offset: 0x000DFD0B
	public WavedashTMProInput CurrentInputField
	{
		get
		{
			return this.currentInputField;
		}
	}

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x06002B08 RID: 11016 RVA: 0x000E1913 File Offset: 0x000DFD13
	public static bool DisableMenuRightStick
	{
		get
		{
			return UIInputModule.disableMenuRightStick;
		}
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x000E191A File Offset: 0x000DFD1A
	public void InitControlMode(ControlMode controlMode)
	{
		this.currentMode = controlMode;
	}

	// Token: 0x06002B0A RID: 11018 RVA: 0x000E1923 File Offset: 0x000DFD23
	public void SetPauseMode(bool isPause)
	{
		this.Actions.SetPauseMode(isPause);
	}

	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x06002B0B RID: 11019 RVA: 0x000E1931 File Offset: 0x000DFD31
	// (set) Token: 0x06002B0C RID: 11020 RVA: 0x000E1939 File Offset: 0x000DFD39
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

	// Token: 0x17000A90 RID: 2704
	// (set) Token: 0x06002B0D RID: 11021 RVA: 0x000E1970 File Offset: 0x000DFD70
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

	// Token: 0x06002B0E RID: 11022 RVA: 0x000E1A00 File Offset: 0x000DFE00
	private void enableKeyboardBindings()
	{
		this.Actions.EnableKeyboard();
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x000E1A0D File Offset: 0x000DFE0D
	private void disableKeyboardBindings()
	{
		this.Actions.DisableKeyboard();
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x000E1A1A File Offset: 0x000DFE1A
	private void enableControllerBindings()
	{
		this.Actions.EnableController();
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x000E1A27 File Offset: 0x000DFE27
	private void disableControllerBindings()
	{
		this.Actions.DisableController();
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x000E1A34 File Offset: 0x000DFE34
	private bool keyboardPressed()
	{
		return this.suppressActionsForFrames <= 0 && this.Actions.KeyboardPressed();
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x000E1A4F File Offset: 0x000DFE4F
	private bool isPressed(PlayerAction action)
	{
		return this.suppressActionsForFrames <= 0 && action.IsPressed;
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x000E1A65 File Offset: 0x000DFE65
	private bool wasPressed(PlayerAction action)
	{
		return this.suppressActionsForFrames <= 0 && action.WasPressed;
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x000E1A7C File Offset: 0x000DFE7C
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

	// Token: 0x06002B16 RID: 11030 RVA: 0x000E1AC9 File Offset: 0x000DFEC9
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

	// Token: 0x06002B17 RID: 11031 RVA: 0x000E1AFF File Offset: 0x000DFEFF
	private bool isRepeat(PlayerAction button)
	{
		return this.isPressed(button) && Time.realtimeSinceStartup > this.getButtonRepeatTime(button);
	}

	// Token: 0x17000A91 RID: 2705
	// (get) Token: 0x06002B18 RID: 11032 RVA: 0x000E1B1E File Offset: 0x000DFF1E
	private bool submitWasPressed
	{
		get
		{
			return this.wasPressed(this.SubmitAction);
		}
	}

	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x06002B19 RID: 11033 RVA: 0x000E1B2C File Offset: 0x000DFF2C
	private bool cancelWasPressed
	{
		get
		{
			return this.wasPressed(this.CancelAction);
		}
	}

	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x06002B1A RID: 11034 RVA: 0x000E1B3A File Offset: 0x000DFF3A
	private bool yButtonWasPressed
	{
		get
		{
			return this.wasPressed(this.YButtonAction);
		}
	}

	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x06002B1B RID: 11035 RVA: 0x000E1B48 File Offset: 0x000DFF48
	private bool xButtonWasPressed
	{
		get
		{
			return this.wasPressed(this.XButtonAction);
		}
	}

	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x06002B1C RID: 11036 RVA: 0x000E1B56 File Offset: 0x000DFF56
	private bool rightTriggerWasPressed
	{
		get
		{
			return this.wasPressed(this.RightTriggerAction);
		}
	}

	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x06002B1D RID: 11037 RVA: 0x000E1B64 File Offset: 0x000DFF64
	private bool leftTriggerWasPressed
	{
		get
		{
			return this.wasPressed(this.LeftTriggerAction);
		}
	}

	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x06002B1E RID: 11038 RVA: 0x000E1B72 File Offset: 0x000DFF72
	private bool leftBumperWasPressed
	{
		get
		{
			return this.wasPressed(this.LeftBumperAction);
		}
	}

	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x06002B1F RID: 11039 RVA: 0x000E1B80 File Offset: 0x000DFF80
	private bool zWasPressed
	{
		get
		{
			return this.wasPressed(this.ZButtonAction);
		}
	}

	// Token: 0x17000A99 RID: 2713
	// (get) Token: 0x06002B20 RID: 11040 RVA: 0x000E1B8E File Offset: 0x000DFF8E
	private bool dPadLeftWasPressed
	{
		get
		{
			return this.wasPressed(this.DPadLeftAction);
		}
	}

	// Token: 0x17000A9A RID: 2714
	// (get) Token: 0x06002B21 RID: 11041 RVA: 0x000E1B9C File Offset: 0x000DFF9C
	private bool dPadRightWasPressed
	{
		get
		{
			return this.wasPressed(this.DPadRightAction);
		}
	}

	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x06002B22 RID: 11042 RVA: 0x000E1BAA File Offset: 0x000DFFAA
	private bool dPadUpWasPressed
	{
		get
		{
			return this.wasPressed(this.DPadUpAction);
		}
	}

	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x06002B23 RID: 11043 RVA: 0x000E1BB8 File Offset: 0x000DFFB8
	private bool dPadDownWasPressed
	{
		get
		{
			return this.wasPressed(this.DPadDownAction);
		}
	}

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x06002B24 RID: 11044 RVA: 0x000E1BC6 File Offset: 0x000DFFC6
	private bool leftWasPressed
	{
		get
		{
			return this.wasPressed(this.LeftAction);
		}
	}

	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x06002B25 RID: 11045 RVA: 0x000E1BD4 File Offset: 0x000DFFD4
	private bool rightWasPressed
	{
		get
		{
			return this.wasPressed(this.RightAction);
		}
	}

	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x06002B26 RID: 11046 RVA: 0x000E1BE2 File Offset: 0x000DFFE2
	private bool upWasPressed
	{
		get
		{
			return this.wasPressed(this.UpAction);
		}
	}

	// Token: 0x17000AA0 RID: 2720
	// (get) Token: 0x06002B27 RID: 11047 RVA: 0x000E1BF0 File Offset: 0x000DFFF0
	private bool downWasPressed
	{
		get
		{
			return this.wasPressed(this.DownAction);
		}
	}

	// Token: 0x17000AA1 RID: 2721
	// (get) Token: 0x06002B28 RID: 11048 RVA: 0x000E1BFE File Offset: 0x000DFFFE
	private bool leftIsRepeat
	{
		get
		{
			return this.isRepeat(this.LeftAction);
		}
	}

	// Token: 0x17000AA2 RID: 2722
	// (get) Token: 0x06002B29 RID: 11049 RVA: 0x000E1C0C File Offset: 0x000E000C
	private bool rightIsRepeat
	{
		get
		{
			return this.isRepeat(this.RightAction);
		}
	}

	// Token: 0x17000AA3 RID: 2723
	// (get) Token: 0x06002B2A RID: 11050 RVA: 0x000E1C1A File Offset: 0x000E001A
	private bool upIsRepeat
	{
		get
		{
			return this.isRepeat(this.UpAction);
		}
	}

	// Token: 0x17000AA4 RID: 2724
	// (get) Token: 0x06002B2B RID: 11051 RVA: 0x000E1C28 File Offset: 0x000E0028
	private bool downIsRepeat
	{
		get
		{
			return this.isRepeat(this.DownAction);
		}
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000E1C36 File Offset: 0x000E0036
	private float getButtonRepeatTime(IInputControl button)
	{
		if (this.nextButtonRepeatTime.ContainsKey(button))
		{
			return this.nextButtonRepeatTime[button];
		}
		return 0f;
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x000E1C5C File Offset: 0x000E005C
	private float getAnalogRepeatTime(IInputControl button, MoveDirection direction)
	{
		if (this.nextAnalogRepeatTime.ContainsKey(button) && this.nextAnalogRepeatTime[button].ContainsKey(direction))
		{
			return this.nextAnalogRepeatTime[button][direction];
		}
		return 0f;
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x000E1CA9 File Offset: 0x000E00A9
	private void setNextAnalogRepeatTime(IInputControl button, MoveDirection direction, float value)
	{
		if (!this.nextAnalogRepeatTime.ContainsKey(button))
		{
			this.nextAnalogRepeatTime[button] = new Dictionary<MoveDirection, float>();
		}
		this.nextAnalogRepeatTime[button][direction] = value;
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x000E1CE0 File Offset: 0x000E00E0
	public void SupressButtonsPressedThisFrame()
	{
		this.suppressActionsForFrames = 1;
		this.ShouldActivateModule();
		this.UpdateModule();
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x000E1CF8 File Offset: 0x000E00F8
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

	// Token: 0x06002B31 RID: 11057 RVA: 0x000E1D44 File Offset: 0x000E0144
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

	// Token: 0x17000AA5 RID: 2725
	// (get) Token: 0x06002B32 RID: 11058 RVA: 0x000E1DA4 File Offset: 0x000E01A4
	private bool anythingWasPressed
	{
		get
		{
			return this.cancelWasPressed || this.submitWasPressed || this.leftWasPressed || this.rightWasPressed || this.upWasPressed || this.downWasPressed || this.leftStickHorizontalWasPressed || this.leftStickVerticalWasPressed || this.rightTriggerWasPressed || this.leftTriggerWasPressed || this.dPadLeftWasPressed || this.dPadRightWasPressed || this.dPadUpWasPressed || this.dPadDownWasPressed || this.leftBumperWasPressed;
		}
	}

	// Token: 0x17000AA6 RID: 2726
	// (get) Token: 0x06002B33 RID: 11059 RVA: 0x000E1E54 File Offset: 0x000E0254
	private bool rightStickHorizontalIsPressed
	{
		get
		{
			return this.thisRightStickHorizontal != MoveDirection.None;
		}
	}

	// Token: 0x17000AA7 RID: 2727
	// (get) Token: 0x06002B34 RID: 11060 RVA: 0x000E1E62 File Offset: 0x000E0262
	private bool rightStickVerticalIsPressed
	{
		get
		{
			return this.thisRightStickVertical != MoveDirection.None;
		}
	}

	// Token: 0x17000AA8 RID: 2728
	// (get) Token: 0x06002B35 RID: 11061 RVA: 0x000E1E70 File Offset: 0x000E0270
	private bool rightStickHorizontalWasPressed
	{
		get
		{
			return this.rightStickHorizontalIsPressed && this.lastRightStickHorizontal == this.thisRightStickHorizontal;
		}
	}

	// Token: 0x17000AA9 RID: 2729
	// (get) Token: 0x06002B36 RID: 11062 RVA: 0x000E1E8E File Offset: 0x000E028E
	private bool rightStickVerticalWasPressed
	{
		get
		{
			return this.rightStickVerticalIsPressed && this.lastRightStickVertical == this.thisRightStickVertical;
		}
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x000E1EAC File Offset: 0x000E02AC
	private bool rightStickDirectionIsRepeat(MoveDirection direction)
	{
		if (direction == MoveDirection.None)
		{
			return false;
		}
		bool flag = direction == this.thisRightStickHorizontal || direction == this.thisRightStickVertical;
		return flag && Time.realtimeSinceStartup > this.getAnalogRepeatTime(this.RightStickAction, direction);
	}

	// Token: 0x17000AAA RID: 2730
	// (get) Token: 0x06002B38 RID: 11064 RVA: 0x000E1EF8 File Offset: 0x000E02F8
	private bool leftStickHorizontalIsPressed
	{
		get
		{
			return this.thisLeftStickHorizontal != MoveDirection.None;
		}
	}

	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x06002B39 RID: 11065 RVA: 0x000E1F06 File Offset: 0x000E0306
	private bool leftStickVerticalIsPressed
	{
		get
		{
			return this.thisLeftStickVertical != MoveDirection.None;
		}
	}

	// Token: 0x17000AAC RID: 2732
	// (get) Token: 0x06002B3A RID: 11066 RVA: 0x000E1F14 File Offset: 0x000E0314
	private bool leftStickHorizontalWasPressed
	{
		get
		{
			return this.leftStickHorizontalIsPressed && this.lastLeftStickHorizontal != this.thisLeftStickHorizontal;
		}
	}

	// Token: 0x17000AAD RID: 2733
	// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000E1F35 File Offset: 0x000E0335
	private bool leftStickVerticalWasPressed
	{
		get
		{
			return this.leftStickVerticalIsPressed && this.lastLeftStickVertical != this.thisLeftStickVertical;
		}
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x000E1F58 File Offset: 0x000E0358
	private bool leftStickDirectionIsRepeat(MoveDirection direction)
	{
		if (direction == MoveDirection.None)
		{
			return false;
		}
		bool flag = direction == this.thisLeftStickHorizontal || direction == this.thisLeftStickVertical;
		return flag && Time.realtimeSinceStartup > this.getAnalogRepeatTime(this.LeftStickAction, direction);
	}

	// Token: 0x17000AAE RID: 2734
	// (get) Token: 0x06002B3D RID: 11069 RVA: 0x000E1FA4 File Offset: 0x000E03A4
	private bool anyMouseEvent
	{
		get
		{
			return this.mouseHasMoved || this.mouseButtonIsPressed;
		}
	}

	// Token: 0x17000AAF RID: 2735
	// (get) Token: 0x06002B3E RID: 11070 RVA: 0x000E1FBA File Offset: 0x000E03BA
	private bool mouseHasMoved
	{
		get
		{
			return this.thisMousePosition != this.lastMousePosition;
		}
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x000E1FCD File Offset: 0x000E03CD
	public override void UpdateModule()
	{
		this.lastMousePosition = this.thisMousePosition;
		this.thisMousePosition = Input.mousePosition;
	}

	// Token: 0x17000AB0 RID: 2736
	// (get) Token: 0x06002B40 RID: 11072 RVA: 0x000E1FE6 File Offset: 0x000E03E6
	private bool mouseButtonIsPressed
	{
		get
		{
			return Input.GetMouseButtonDown(0);
		}
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x000E1FF0 File Offset: 0x000E03F0
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

	// Token: 0x06002B42 RID: 11074 RVA: 0x000E2064 File Offset: 0x000E0464
	public void DisableMouseControls()
	{
		this.focusOnMouseHover = false;
		this.allowMouseInput = false;
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x000E2074 File Offset: 0x000E0474
	public void EnableMouseControls()
	{
		this.focusOnMouseHover = true;
		this.allowMouseInput = true;
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x000E2084 File Offset: 0x000E0484
	public override void Process()
	{
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000E2088 File Offset: 0x000E0488
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

	// Token: 0x06002B46 RID: 11078 RVA: 0x000E2158 File Offset: 0x000E0558
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
		foreach (IGlobalInputDelegate globalDelegate in this.GlobalDelegates)
		{
			this.sendInputsToGlobalDelegate(globalDelegate);
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

	// Token: 0x06002B47 RID: 11079 RVA: 0x000E2564 File Offset: 0x000E0964
	private void sendInputsToGlobalDelegate(IGlobalInputDelegate globalDelegate)
	{
		if (this.xButtonWasPressed)
		{
			globalDelegate.OnXButtonPressed();
		}
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x000E2577 File Offset: 0x000E0977
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

	// Token: 0x06002B49 RID: 11081 RVA: 0x000E259C File Offset: 0x000E099C
	private void setCurrentlyClicked(GameObject obj)
	{
		if (this.currentInputField != obj)
		{
			if (this.currentInputField != null)
			{
				WavedashTMProInput wavedashTMProInput = this.currentInputField;
				wavedashTMProInput.EndEditCallback = (Action)Delegate.Remove(wavedashTMProInput.EndEditCallback, new Action(this.releaseCurrentlyClicked));
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
				WavedashTMProInput wavedashTMProInput2 = this.currentInputField;
				wavedashTMProInput2.EndEditCallback = (Action)Delegate.Combine(wavedashTMProInput2.EndEditCallback, new Action(this.releaseCurrentlyClicked));
			}
			bool value = this.currentInputField != null;
			this.Actions.SuppressKeyboard(value);
		}
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x000E267F File Offset: 0x000E0A7F
	private void releaseCurrentlyClicked()
	{
		this.setCurrentlyClicked(null);
		this.suppressActionsForFrames = 2;
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x000E2690 File Offset: 0x000E0A90
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

	// Token: 0x06002B4C RID: 11084 RVA: 0x000E26F8 File Offset: 0x000E0AF8
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

	// Token: 0x06002B4D RID: 11085 RVA: 0x000E277F File Offset: 0x000E0B7F
	private void postProcessButtonRepeatState(PlayerAction button)
	{
		if (this.isRepeat(button))
		{
			this.nextButtonRepeatTime[button] = Time.realtimeSinceStartup + this.moveRepeatDelayDuration;
		}
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x000E27A8 File Offset: 0x000E0BA8
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

	// Token: 0x06002B4F RID: 11087 RVA: 0x000E282E File Offset: 0x000E0C2E
	public void SetHorizontalRepeatDelay(float repeatDelay)
	{
		this.overrideHorizontalRepeatDelay = repeatDelay;
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x000E2837 File Offset: 0x000E0C37
	public void ResetHorizontalRepeatDelay()
	{
		this.overrideHorizontalRepeatDelay = -1f;
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x000E2844 File Offset: 0x000E0C44
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

	// Token: 0x06002B52 RID: 11090 RVA: 0x000E297C File Offset: 0x000E0D7C
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

	// Token: 0x06002B53 RID: 11091 RVA: 0x000E29D0 File Offset: 0x000E0DD0
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

	// Token: 0x06002B54 RID: 11092 RVA: 0x000E2AA4 File Offset: 0x000E0EA4
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

	// Token: 0x06002B55 RID: 11093 RVA: 0x000E2BFC File Offset: 0x000E0FFC
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

	// Token: 0x06002B57 RID: 11095 RVA: 0x000E2C9C File Offset: 0x000E109C
	bool ICustomInputModule.get_enabled()
	{
		return base.enabled;
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x000E2CA4 File Offset: 0x000E10A4
	void ICustomInputModule.set_enabled(bool value)
	{
		base.enabled = value;
	}

	// Token: 0x04001EC2 RID: 7874
	public IScreenInputDelegate ScreenDelegate;

	// Token: 0x04001EC3 RID: 7875
	public IButtonInputDelegate ButtonDelegate;

	// Token: 0x04001EC4 RID: 7876
	public List<IGlobalInputDelegate> GlobalDelegates = new List<IGlobalInputDelegate>();

	// Token: 0x04001EC6 RID: 7878
	private bool allowMouseInput;

	// Token: 0x04001EC7 RID: 7879
	private bool focusOnMouseHover;

	// Token: 0x04001EC8 RID: 7880
	private int suppressActionsForFrames;

	// Token: 0x04001EC9 RID: 7881
	private Vector3 thisMousePosition;

	// Token: 0x04001ECA RID: 7882
	private Vector3 lastMousePosition;

	// Token: 0x04001EDD RID: 7901
	private MoveDirection thisRightStickHorizontal = MoveDirection.None;

	// Token: 0x04001EDE RID: 7902
	private MoveDirection lastRightStickHorizontal = MoveDirection.None;

	// Token: 0x04001EDF RID: 7903
	private MoveDirection thisRightStickVertical = MoveDirection.None;

	// Token: 0x04001EE0 RID: 7904
	private MoveDirection lastRightStickVertical = MoveDirection.None;

	// Token: 0x04001EE1 RID: 7905
	private MoveDirection thisLeftStickHorizontal = MoveDirection.None;

	// Token: 0x04001EE2 RID: 7906
	private MoveDirection lastLeftStickHorizontal = MoveDirection.None;

	// Token: 0x04001EE3 RID: 7907
	private MoveDirection thisLeftStickVertical = MoveDirection.None;

	// Token: 0x04001EE4 RID: 7908
	private MoveDirection lastLeftStickVertical = MoveDirection.None;

	// Token: 0x04001EE5 RID: 7909
	private float leftStickX;

	// Token: 0x04001EE6 RID: 7910
	private float leftStickY;

	// Token: 0x04001EE7 RID: 7911
	private float rightStickX;

	// Token: 0x04001EE8 RID: 7912
	private float rightStickY;

	// Token: 0x04001EE9 RID: 7913
	private Dictionary<IInputControl, float> nextButtonRepeatTime = new Dictionary<IInputControl, float>();

	// Token: 0x04001EEA RID: 7914
	private Dictionary<IInputControl, Dictionary<MoveDirection, float>> nextAnalogRepeatTime = new Dictionary<IInputControl, Dictionary<MoveDirection, float>>();

	// Token: 0x04001EEB RID: 7915
	private ControlMode currentMode;

	// Token: 0x04001EEC RID: 7916
	private IInputDevice _lockingDevice;

	// Token: 0x04001EED RID: 7917
	private PlayerInputPort lockingPlayer;

	// Token: 0x04001EEE RID: 7918
	public MenuActions Actions;

	// Token: 0x04001EEF RID: 7919
	public float analogMoveThreshold = 0.5f;

	// Token: 0x04001EF0 RID: 7920
	public float moveRepeatFirstDuration = 0.3f;

	// Token: 0x04001EF1 RID: 7921
	[SerializeField]
	private float moveRepeatDelayDuration = 0.07f;

	// Token: 0x04001EF2 RID: 7922
	public float overrideHorizontalRepeatDelay = -1f;

	// Token: 0x04001EF3 RID: 7923
	private GameObject hoverObject;

	// Token: 0x04001EF4 RID: 7924
	private WavedashTMProInput currentInputField;

	// Token: 0x04001EF5 RID: 7925
	private static bool disableMenuRightStick;

	// Token: 0x04001EF6 RID: 7926
	private float rightStickStuckSpinTime;
}
