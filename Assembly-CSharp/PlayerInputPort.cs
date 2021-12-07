using System;
using System.Collections;
using System.Collections.Generic;
using FixedPoint;
using InControl;
using UnityEngine;

// Token: 0x020006B0 RID: 1712
public class PlayerInputPort : InputController
{
	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06002A92 RID: 10898 RVA: 0x000E0B8C File Offset: 0x000DEF8C
	// (set) Token: 0x06002A93 RID: 10899 RVA: 0x000E0B94 File Offset: 0x000DEF94
	[Inject]
	public DebugKeys debugKeys { get; set; }

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06002A94 RID: 10900 RVA: 0x000E0B9D File Offset: 0x000DEF9D
	// (set) Token: 0x06002A95 RID: 10901 RVA: 0x000E0BA5 File Offset: 0x000DEFA5
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x06002A96 RID: 10902 RVA: 0x000E0BAE File Offset: 0x000DEFAE
	// (set) Token: 0x06002A97 RID: 10903 RVA: 0x000E0BB6 File Offset: 0x000DEFB6
	public InputSettingsData Settings { get; private set; }

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x06002A98 RID: 10904 RVA: 0x000E0BBF File Offset: 0x000DEFBF
	public override bool allowTapJumping
	{
		get
		{
			return this.Settings.tapToJumpEnabled;
		}
	}

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x06002A99 RID: 10905 RVA: 0x000E0BCC File Offset: 0x000DEFCC
	public override bool allowRecoveryJumping
	{
		get
		{
			return this.Settings.recoveryJumpingEnabled;
		}
	}

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x06002A9A RID: 10906 RVA: 0x000E0BD9 File Offset: 0x000DEFD9
	public override bool allowTapStrike
	{
		get
		{
			return this.Settings.tapToStrikeEnabled;
		}
	}

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000E0BE6 File Offset: 0x000DEFE6
	public override bool requireDoubleTapToRun
	{
		get
		{
			return this.Settings.doubleTapToRun;
		}
	}

	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x06002A9C RID: 10908 RVA: 0x000E0BF3 File Offset: 0x000DEFF3
	// (set) Token: 0x06002A9D RID: 10909 RVA: 0x000E0BFB File Offset: 0x000DEFFB
	public int Id { get; set; }

	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x06002A9E RID: 10910 RVA: 0x000E0C04 File Offset: 0x000DF004
	public IInputDevice Device
	{
		get
		{
			return this.device;
		}
	}

	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x06002A9F RID: 10911 RVA: 0x000E0C0C File Offset: 0x000DF00C
	public bool ShouldPersistSettings
	{
		get
		{
			return this.shouldpersistSettings;
		}
	}

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x000E0C14 File Offset: 0x000DF014
	public bool IsP2Debug
	{
		get
		{
			return this.isP2Debug;
		}
	}

	// Token: 0x17000A72 RID: 2674
	// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000E0C1C File Offset: 0x000DF01C
	public PlayerNum Player
	{
		get
		{
			return this.userInputManager.GetPlayerNum(this);
		}
	}

	// Token: 0x17000A73 RID: 2675
	// (get) Token: 0x06002AA2 RID: 10914 RVA: 0x000E0C2A File Offset: 0x000DF02A
	// (set) Token: 0x06002AA3 RID: 10915 RVA: 0x000E0C32 File Offset: 0x000DF032
	private IUserInputManager userInputManager { get; set; }

	// Token: 0x06002AA4 RID: 10916 RVA: 0x000E0C3B File Offset: 0x000DF03B
	public override void Init()
	{
		base.Init();
		this.AllowDebug = true;
		this.device = InputDevice.Null;
		this.LoadSettings(new InputSettingsData());
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x000E0C60 File Offset: 0x000DF060
	public void Initialize(InputConfigData options, UserInputManager userInputManager)
	{
		base.Initialize(options);
		this.userInputManager = userInputManager;
		this.isP2Debug = false;
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x000E0C77 File Offset: 0x000DF077
	public void OnAssignedToPlayer()
	{
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x000E0C7C File Offset: 0x000DF07C
	public void LoadSettings(InputSettingsData settings)
	{
		if (this.Settings != null)
		{
			this.Settings.inputActions.Destroy();
		}
		this.Settings = settings;
		if (this.Settings != null)
		{
			this.Settings.inputActions.Device = ((!(this.device is InputDevice)) ? InputDevice.Null : (this.device as InputDevice));
			if (this.device is IControllerInputDevice)
			{
				(this.device as IControllerInputDevice).LeftStickLowerDeadZone = settings.leftStickDeadZone;
				(this.device as IControllerInputDevice).RightStickLowerDeadZone = settings.rightStickDeadZone;
				(this.device as IControllerInputDevice).LeftTriggerDeadZone = settings.leftTriggerDeadZone;
				(this.device as IControllerInputDevice).RightTriggerDeadZone = settings.rightTriggerDeadZone;
			}
		}
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x000E0D54 File Offset: 0x000DF154
	private DefaultInputBinding findBinding(IEnumerable<DefaultInputBinding> bindings, ButtonPress button)
	{
		foreach (DefaultInputBinding defaultInputBinding in bindings)
		{
			if (defaultInputBinding.button == button)
			{
				return defaultInputBinding;
			}
		}
		return null;
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x000E0DB8 File Offset: 0x000DF1B8
	public void SetDevice(IInputDevice device, bool shouldPersistSettings)
	{
		if (device == null)
		{
			Debug.LogError("It is invalid to set an inputController to use a null device! Use InputDevice.Null instead");
			return;
		}
		this.device = device;
		this.shouldpersistSettings = shouldPersistSettings;
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x000E0DD9 File Offset: 0x000DF1D9
	public void ClearBinding(BindingSource binding)
	{
		this.Settings.inputActions.RemoveBinding(binding);
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x000E0DEC File Offset: 0x000DF1EC
	public void ResetBindings()
	{
		this.Settings.inputActions.Reset();
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x000E0DFE File Offset: 0x000DF1FE
	private bool isTaunt(ButtonPress button)
	{
		return button == ButtonPress.TauntDown || button == ButtonPress.TauntUp || button == ButtonPress.TauntLeft || button == ButtonPress.TauntRight;
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x000E0E20 File Offset: 0x000DF220
	public override void ReadAllInputs(ref InputValuesSnapshot values, InputValue valueBuffer, bool tauntsOnly)
	{
		if (!tauntsOnly)
		{
			this.readLeftAxes(ref values, valueBuffer);
		}
		for (int i = 0; i < base.allInputData.Count; i++)
		{
			InputData inputData = base.allInputData[i];
			if (!InputController.IsMetagameInput(inputData.button, false) && inputData.inputType != InputType.HorizontalAxis && inputData.inputType != InputType.VerticalAxis && (!tauntsOnly || this.isTaunt(inputData.button)))
			{
				valueBuffer.Clear();
				this.ReadInputValue(inputData, valueBuffer);
				values.SetValue(inputData, valueBuffer);
			}
		}
		if (values.GetButton(ButtonPress.Tilt))
		{
			valueBuffer.Clear();
			values.GetValue(base.horizontalAxis, valueBuffer);
			Fixed fixedValue = FixedMath.Clamp(valueBuffer.axis.Value, -this.config.inputConfig.tiltModifierInputMagnitude, this.config.inputConfig.tiltModifierInputMagnitude);
			valueBuffer.axis.Set(fixedValue);
			values.SetValue(base.horizontalAxis, valueBuffer);
			valueBuffer.Clear();
			values.GetValue(base.verticalAxis, valueBuffer);
			Fixed fixedValue2 = FixedMath.Clamp(valueBuffer.axis.Value, -this.config.inputConfig.tiltModifierInputMagnitude, this.config.inputConfig.tiltModifierInputMagnitude);
			valueBuffer.axis.Set(fixedValue2);
			values.SetValue(base.verticalAxis, valueBuffer);
		}
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x000E0F90 File Offset: 0x000DF390
	private void readLeftAxes(ref InputValuesSnapshot values, InputValue valueBuffer)
	{
		float axis = 0f;
		float axis2 = 0f;
		InputData data = null;
		InputData data2 = null;
		OneAxisInputControl oneAxisInputControl = null;
		OneAxisInputControl oneAxisInputControl2 = null;
		for (int i = 0; i < base.allInputData.Count; i++)
		{
			InputData inputData = base.allInputData[i];
			if (inputData.inputType == InputType.HorizontalAxis)
			{
				data = inputData;
				oneAxisInputControl = this.Settings.inputActions.Horizontal;
				axis = oneAxisInputControl.Value;
			}
			else if (inputData.inputType == InputType.VerticalAxis)
			{
				data2 = inputData;
				oneAxisInputControl2 = this.Settings.inputActions.Vertical;
				axis2 = oneAxisInputControl2.Value;
			}
		}
		this.checkAllSnapAngles(ref axis, ref axis2);
		valueBuffer.Clear();
		valueBuffer.Set(axis, oneAxisInputControl.IsPressed);
		values.SetValue(data, valueBuffer);
		valueBuffer.Clear();
		valueBuffer.Set(axis2, oneAxisInputControl2.IsPressed);
		values.SetValue(data2, valueBuffer);
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x000E1080 File Offset: 0x000DF480
	private void checkAllSnapAngles(ref float horizontalValue, ref float verticalValue)
	{
		float actualAngle = 57.29578f * Mathf.Atan2(verticalValue, horizontalValue);
		if (this.config.inputConfig.cardinalSnapAngle != 0)
		{
			int cardinalSnapAngle = this.config.inputConfig.cardinalSnapAngle;
			for (int i = 0; i < 4; i++)
			{
				bool flag = this.checkSnapAngle((float)(i * 90), actualAngle, (float)cardinalSnapAngle, ref horizontalValue, ref verticalValue);
				if (flag)
				{
					return;
				}
			}
		}
		if (this.config.inputConfig.diagonalSnapAngle != 0)
		{
			int diagonalSnapAngle = this.config.inputConfig.diagonalSnapAngle;
			for (int j = 0; j < 4; j++)
			{
				bool flag2 = this.checkSnapAngle((float)(45 + j * 90), actualAngle, (float)diagonalSnapAngle, ref horizontalValue, ref verticalValue);
				if (flag2)
				{
					return;
				}
			}
		}
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x000E114C File Offset: 0x000DF54C
	private bool checkSnapAngle(float targetAngle, float actualAngle, float snapThreshold, ref float horizontalValue, ref float verticalValue)
	{
		if (Mathf.Abs(Mathf.DeltaAngle(targetAngle, actualAngle)) <= snapThreshold)
		{
			float num = Mathf.Clamp(Mathf.Sqrt(horizontalValue * horizontalValue + verticalValue * verticalValue), 0f, 1f);
			horizontalValue = Mathf.Cos(targetAngle * 0.017453292f) * num;
			verticalValue = Mathf.Sin(targetAngle * 0.017453292f) * num;
			return true;
		}
		return false;
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x000E11B4 File Offset: 0x000DF5B4
	public override void ReadInputValue(InputData inputData, InputValue valueBuffer)
	{
		if (inputData != null)
		{
			OneAxisInputControl oneAxisInputControl = null;
			switch (inputData.inputType)
			{
			case InputType.HorizontalAxis:
				oneAxisInputControl = this.Settings.inputActions.Horizontal;
				break;
			case InputType.VerticalAxis:
				oneAxisInputControl = this.Settings.inputActions.Vertical;
				break;
			case InputType.Button:
				oneAxisInputControl = this.Settings.inputActions.GetButtonAction(inputData.button);
				break;
			case InputType.TriggerAxis:
				oneAxisInputControl = this.Settings.inputActions.GetButtonAction(inputData.button);
				break;
			}
			if (oneAxisInputControl != null)
			{
				float value = oneAxisInputControl.Value;
				valueBuffer.Set(value, oneAxisInputControl.IsPressed);
			}
		}
		else
		{
			valueBuffer.Set(0f, false);
		}
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x000E127A File Offset: 0x000DF67A
	public override void Vibrate(float leftMotor, float rightMotor, float duration)
	{
		if (!this.inputConfig.enableVibrate)
		{
			return;
		}
		base.StartCoroutine(this.vibrate(leftMotor, rightMotor, duration));
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x000E129D File Offset: 0x000DF69D
	public void Vibrate(float intensity, float duration)
	{
		if (!this.inputConfig.enableVibrate)
		{
			return;
		}
		base.StartCoroutine(this.vibrate(intensity, intensity, duration));
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000E12C0 File Offset: 0x000DF6C0
	private IEnumerator vibrate(float leftMotor, float rightMotor, float duration)
	{
		InputDevice incontrolDevice = this.Device as InputDevice;
		if (incontrolDevice != null)
		{
			incontrolDevice.Vibrate(leftMotor, rightMotor);
		}
		yield return new WaitForSeconds(duration);
		if (incontrolDevice != null)
		{
			incontrolDevice.StopVibration();
		}
		yield break;
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x000E12F0 File Offset: 0x000DF6F0
	public override void DoUpdate()
	{
		base.DoUpdate();
		this.handleMetagameInput(base.allInputData);
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000E1304 File Offset: 0x000DF704
	private void handleMetagameInput(ICollection<InputData> inputs)
	{
		if (inputs != null && this.gameManager != null)
		{
			foreach (KeyValuePair<int, PlayerAction> keyValuePair in this.Settings.inputActions.ButtonMappings)
			{
				int key = keyValuePair.Key;
				ButtonPress buttonPress = (ButtonPress)key;
				if (this.handleAsMetagameInput(buttonPress) && this.wasMetagameButtonPressed(buttonPress))
				{
					if (buttonPress == ButtonPress.Start)
					{
						if (base.battleServerAPI.IsConnected)
						{
							base.signalBus.GetSignal<UpdatePauseScreenOnline>().Dispatch(true);
						}
						else
						{
							bool flag = true;
							foreach (PlayerReference playerReference in this.gameManager.PlayerReferences)
							{
								if (!playerReference.Controller.IsEliminated && !playerReference.IsBenched && playerReference.PlayerInfo.type == PlayerType.Human)
								{
									flag = false;
								}
							}
							if (!flag)
							{
								if (this.Player == PlayerNum.None)
								{
									continue;
								}
								PlayerReference playerReference2 = this.gameManager.GetPlayerReference(this.Player);
								if (playerReference2.Controller == null)
								{
									continue;
								}
								if (playerReference2.Controller.IsEliminated || playerReference2.IsBenched)
								{
									continue;
								}
							}
							base.gameController.currentGame.TogglePaused(this);
						}
					}
					else if (this.AllowDebug && this.debugKeys.DebugControlsEnabled)
					{
						Action action = this.debugKeys.FindControllerShortcut(buttonPress);
						if (action != null)
						{
							action();
						}
					}
				}
			}
		}
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x000E1514 File Offset: 0x000DF914
	private bool handleAsMetagameInput(ButtonPress button)
	{
		return InputController.IsMetagameInput(button, true) || this.debugKeys.HasControllerShortcut(button);
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000E1534 File Offset: 0x000DF934
	private bool wasMetagameButtonPressed(ButtonPress button)
	{
		PlayerAction buttonAction = this.Settings.inputActions.GetButtonAction(button);
		return buttonAction != null && buttonAction.WasPressed;
	}

	// Token: 0x04001EB8 RID: 7864
	public bool AllowDebug;

	// Token: 0x04001EB9 RID: 7865
	private IInputDevice device;

	// Token: 0x04001EBA RID: 7866
	private bool shouldpersistSettings;

	// Token: 0x04001EBB RID: 7867
	private bool isP2Debug;
}
