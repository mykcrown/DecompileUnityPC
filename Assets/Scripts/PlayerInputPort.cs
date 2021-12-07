// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerInputPort : InputController
{
	private sealed class _vibrate_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal InputDevice _incontrolDevice___0;

		internal float leftMotor;

		internal float rightMotor;

		internal float duration;

		internal PlayerInputPort _this;

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

		public _vibrate_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._incontrolDevice___0 = (this._this.Device as InputDevice);
				if (this._incontrolDevice___0 != null)
				{
					this._incontrolDevice___0.Vibrate(this.leftMotor, this.rightMotor);
				}
				this._current = new WaitForSeconds(this.duration);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._incontrolDevice___0 != null)
				{
					this._incontrolDevice___0.StopVibration();
				}
				this._PC = -1;
				break;
			}
			return false;
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
	}

	public bool AllowDebug;

	private IInputDevice device;

	private bool shouldpersistSettings;

	private bool isP2Debug;

	[Inject]
	public DebugKeys debugKeys
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

	public InputSettingsData Settings
	{
		get;
		private set;
	}

	public override bool allowTapJumping
	{
		get
		{
			return this.Settings.tapToJumpEnabled;
		}
	}

	public override bool allowRecoveryJumping
	{
		get
		{
			return this.Settings.recoveryJumpingEnabled;
		}
	}

	public override bool allowTapStrike
	{
		get
		{
			return this.Settings.tapToStrikeEnabled;
		}
	}

	public override bool requireDoubleTapToRun
	{
		get
		{
			return this.Settings.doubleTapToRun;
		}
	}

	public int Id
	{
		get;
		set;
	}

	public IInputDevice Device
	{
		get
		{
			return this.device;
		}
	}

	public bool ShouldPersistSettings
	{
		get
		{
			return this.shouldpersistSettings;
		}
	}

	public bool IsP2Debug
	{
		get
		{
			return this.isP2Debug;
		}
	}

	public PlayerNum Player
	{
		get
		{
			return this.userInputManager.GetPlayerNum(this);
		}
	}

	private IUserInputManager userInputManager
	{
		get;
		set;
	}

	public override void Init()
	{
		base.Init();
		this.AllowDebug = true;
		this.device = InputDevice.Null;
		this.LoadSettings(new InputSettingsData());
	}

	public void Initialize(InputConfigData options, UserInputManager userInputManager)
	{
		base.Initialize(options);
		this.userInputManager = userInputManager;
		this.isP2Debug = false;
	}

	public void OnAssignedToPlayer()
	{
	}

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

	private DefaultInputBinding findBinding(IEnumerable<DefaultInputBinding> bindings, ButtonPress button)
	{
		foreach (DefaultInputBinding current in bindings)
		{
			if (current.button == button)
			{
				return current;
			}
		}
		return null;
	}

	public void SetDevice(IInputDevice device, bool shouldPersistSettings)
	{
		if (device == null)
		{
			UnityEngine.Debug.LogError("It is invalid to set an inputController to use a null device! Use InputDevice.Null instead");
			return;
		}
		this.device = device;
		this.shouldpersistSettings = shouldPersistSettings;
	}

	public void ClearBinding(BindingSource binding)
	{
		this.Settings.inputActions.RemoveBinding(binding);
	}

	public void ResetBindings()
	{
		this.Settings.inputActions.Reset();
	}

	private bool isTaunt(ButtonPress button)
	{
		return button == ButtonPress.TauntDown || button == ButtonPress.TauntUp || button == ButtonPress.TauntLeft || button == ButtonPress.TauntRight;
	}

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

	private bool checkSnapAngle(float targetAngle, float actualAngle, float snapThreshold, ref float horizontalValue, ref float verticalValue)
	{
		if (Mathf.Abs(Mathf.DeltaAngle(targetAngle, actualAngle)) <= snapThreshold)
		{
			float num = Mathf.Clamp(Mathf.Sqrt(horizontalValue * horizontalValue + verticalValue * verticalValue), 0f, 1f);
			horizontalValue = Mathf.Cos(targetAngle * 0.0174532924f) * num;
			verticalValue = Mathf.Sin(targetAngle * 0.0174532924f) * num;
			return true;
		}
		return false;
	}

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

	public override void Vibrate(float leftMotor, float rightMotor, float duration)
	{
		if (!this.inputConfig.enableVibrate)
		{
			return;
		}
		base.StartCoroutine(this.vibrate(leftMotor, rightMotor, duration));
	}

	public void Vibrate(float intensity, float duration)
	{
		if (!this.inputConfig.enableVibrate)
		{
			return;
		}
		base.StartCoroutine(this.vibrate(intensity, intensity, duration));
	}

	private IEnumerator vibrate(float leftMotor, float rightMotor, float duration)
	{
		PlayerInputPort._vibrate_c__Iterator0 _vibrate_c__Iterator = new PlayerInputPort._vibrate_c__Iterator0();
		_vibrate_c__Iterator.leftMotor = leftMotor;
		_vibrate_c__Iterator.rightMotor = rightMotor;
		_vibrate_c__Iterator.duration = duration;
		_vibrate_c__Iterator._this = this;
		return _vibrate_c__Iterator;
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		this.handleMetagameInput(base.allInputData);
	}

	private void handleMetagameInput(ICollection<InputData> inputs)
	{
		if (inputs != null && this.gameManager != null)
		{
			foreach (KeyValuePair<int, PlayerAction> current in this.Settings.inputActions.ButtonMappings)
			{
				int key = current.Key;
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
							foreach (PlayerReference current2 in this.gameManager.PlayerReferences)
							{
								if (!current2.Controller.IsEliminated && !current2.IsBenched && current2.PlayerInfo.type == PlayerType.Human)
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
								PlayerReference playerReference = this.gameManager.GetPlayerReference(this.Player);
								if (playerReference.Controller == null)
								{
									continue;
								}
								if (playerReference.Controller.IsEliminated || playerReference.IsBenched)
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

	private bool handleAsMetagameInput(ButtonPress button)
	{
		return InputController.IsMetagameInput(button, true) || this.debugKeys.HasControllerShortcut(button);
	}

	private bool wasMetagameButtonPressed(ButtonPress button)
	{
		PlayerAction buttonAction = this.Settings.inputActions.GetButtonAction(button);
		return buttonAction != null && buttonAction.WasPressed;
	}
}
