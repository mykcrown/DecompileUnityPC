// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	public class InputDevice : IControllerInputDevice, IInputDevice
	{
		protected struct AnalogSnapshotEntry
		{
			public float value;

			public float maxValue;

			public float minValue;

			public void TrackMinMaxValue(float currentValue)
			{
				this.maxValue = Mathf.Max(this.maxValue, currentValue);
				this.minValue = Mathf.Min(this.minValue, currentValue);
			}
		}

		public static readonly InputDevice Null = new InputDevice("None");

		private List<InputControl> controls;

		public bool Passive;

		private float leftLowerDeadZone;

		private float rightLowerDeadZone;

		private float leftTriggerLowerDeadZone;

		private float rightTriggerLowerDeadZone;

		private InputControl cachedLeftStickUp;

		private InputControl cachedLeftStickDown;

		private InputControl cachedLeftStickLeft;

		private InputControl cachedLeftStickRight;

		private InputControl cachedRightStickUp;

		private InputControl cachedRightStickDown;

		private InputControl cachedRightStickLeft;

		private InputControl cachedRightStickRight;

		private InputControl cachedDPadUp;

		private InputControl cachedDPadDown;

		private InputControl cachedDPadLeft;

		private InputControl cachedDPadRight;

		private InputControl cachedAction1;

		private InputControl cachedAction2;

		private InputControl cachedAction3;

		private InputControl cachedAction4;

		private InputControl cachedLeftTrigger;

		private InputControl cachedRightTrigger;

		private InputControl cachedLeftBumper;

		private InputControl cachedRightBumper;

		private InputControl cachedLeftStickButton;

		private InputControl cachedRightStickButton;

		private InputControl cachedLeftStickX;

		private InputControl cachedLeftStickY;

		private InputControl cachedRightStickX;

		private InputControl cachedRightStickY;

		private InputControl cachedDPadX;

		private InputControl cachedDPadY;

		private InputControl cachedCommand;

		public string Name
		{
			get;
			protected set;
		}

		public string Meta
		{
			get;
			protected set;
		}

		public int SortOrder
		{
			get;
			protected set;
		}

		public InputDeviceClass DeviceClass
		{
			get;
			protected set;
		}

		public InputDeviceStyle DeviceStyle
		{
			get;
			protected set;
		}

		public Guid GUID
		{
			get;
			private set;
		}

		public ulong LastChangeTick
		{
			get;
			private set;
		}

		public bool IsAttached
		{
			get;
			private set;
		}

		protected bool RawSticks
		{
			get;
			private set;
		}

		public ReadOnlyCollection<InputControl> Controls
		{
			get;
			protected set;
		}

		protected InputControl[] ControlsByTarget
		{
			get;
			private set;
		}

		public TwoAxisInputControl LeftStick
		{
			get;
			private set;
		}

		public TwoAxisInputControl RightStick
		{
			get;
			private set;
		}

		public TwoAxisInputControl DPad
		{
			get;
			private set;
		}

		protected InputDevice.AnalogSnapshotEntry[] AnalogSnapshot
		{
			get;
			set;
		}

		protected virtual float MaxLeftStickUpperDeadZone
		{
			get
			{
				return 1f;
			}
		}

		protected virtual float MaxRightStickUpperDeadZone
		{
			get
			{
				return 1f;
			}
		}

		protected virtual float MaxTriggerUpperDeadZone
		{
			get
			{
				return 1f;
			}
		}

		public float LeftStickLowerDeadZone
		{
			get
			{
				return this.leftLowerDeadZone / this.MaxLeftStickUpperDeadZone;
			}
			set
			{
				this.leftLowerDeadZone = value * this.MaxLeftStickUpperDeadZone;
				this.GetControl(InputControlType.LeftStickX).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickY).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickLeft).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickRight).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickUp).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickDown).LowerDeadZone = this.leftLowerDeadZone;
			}
		}

		public float RightStickLowerDeadZone
		{
			get
			{
				return this.rightLowerDeadZone / this.MaxRightStickUpperDeadZone;
			}
			set
			{
				this.rightLowerDeadZone = value * this.MaxRightStickUpperDeadZone;
				this.GetControl(InputControlType.RightStickX).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickY).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickLeft).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickRight).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickUp).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickDown).LowerDeadZone = this.rightLowerDeadZone;
			}
		}

		public float LeftTriggerDeadZone
		{
			get
			{
				return this.leftTriggerLowerDeadZone / this.MaxTriggerUpperDeadZone;
			}
			set
			{
				this.leftTriggerLowerDeadZone = value * this.MaxTriggerUpperDeadZone;
				this.GetControl(InputControlType.LeftTrigger).LowerDeadZone = this.leftTriggerLowerDeadZone;
			}
		}

		public float RightTriggerDeadZone
		{
			get
			{
				return this.rightTriggerLowerDeadZone / this.MaxTriggerUpperDeadZone;
			}
			set
			{
				this.rightTriggerLowerDeadZone = value * this.MaxTriggerUpperDeadZone;
				this.GetControl(InputControlType.RightTrigger).LowerDeadZone = this.rightTriggerLowerDeadZone;
			}
		}

		public virtual float DefaultLeftStickLowerDeadZone
		{
			get
			{
				return 0f;
			}
		}

		public virtual float DefaultRightStickLowerDeadZone
		{
			get
			{
				return 0f;
			}
		}

		public virtual float DefaultLeftTriggerDeadZone
		{
			get
			{
				return 0f;
			}
		}

		public virtual float DefaultRightTriggerDeadZone
		{
			get
			{
				return 0f;
			}
		}

		public InputControl this[InputControlType controlType]
		{
			get
			{
				return this.GetControl(controlType);
			}
		}

		public virtual bool IsSupportedOnThisPlatform
		{
			get
			{
				return true;
			}
		}

		public virtual bool IsKnown
		{
			get
			{
				return true;
			}
		}

		public bool IsUnknown
		{
			get
			{
				return !this.IsKnown;
			}
		}

		[Obsolete("Use InputDevice.CommandIsPressed instead.", false)]
		public bool MenuIsPressed
		{
			get
			{
				return this.IsKnown && this.Command.IsPressed;
			}
		}

		[Obsolete("Use InputDevice.CommandWasPressed instead.", false)]
		public bool MenuWasPressed
		{
			get
			{
				return this.IsKnown && this.Command.WasPressed;
			}
		}

		[Obsolete("Use InputDevice.CommandWasReleased instead.", false)]
		public bool MenuWasReleased
		{
			get
			{
				return this.IsKnown && this.Command.WasReleased;
			}
		}

		public bool CommandIsPressed
		{
			get
			{
				return this.IsKnown && this.Command.IsPressed;
			}
		}

		public bool CommandWasPressed
		{
			get
			{
				return this.IsKnown && this.Command.WasPressed;
			}
		}

		public bool CommandWasReleased
		{
			get
			{
				return this.IsKnown && this.Command.WasReleased;
			}
		}

		public InputControl AnyButton
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return inputControl;
					}
				}
				return InputControl.Null;
			}
		}

		public bool AnyButtonIsPressed
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool AnyButtonWasPressed
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.WasPressed)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool AnyButtonWasReleased
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.WasReleased)
					{
						return true;
					}
				}
				return false;
			}
		}

		public TwoAxisInputControl Direction
		{
			get
			{
				return (this.DPad.UpdateTick <= this.LeftStick.UpdateTick) ? this.LeftStick : this.DPad;
			}
		}

		public InputControl LeftStickUp
		{
			get
			{
				InputControl arg_1D_0;
				if ((arg_1D_0 = this.cachedLeftStickUp) == null)
				{
					arg_1D_0 = (this.cachedLeftStickUp = this.GetControl(InputControlType.LeftStickUp));
				}
				return arg_1D_0;
			}
		}

		public InputControl LeftStickDown
		{
			get
			{
				InputControl arg_1D_0;
				if ((arg_1D_0 = this.cachedLeftStickDown) == null)
				{
					arg_1D_0 = (this.cachedLeftStickDown = this.GetControl(InputControlType.LeftStickDown));
				}
				return arg_1D_0;
			}
		}

		public InputControl LeftStickLeft
		{
			get
			{
				InputControl arg_1D_0;
				if ((arg_1D_0 = this.cachedLeftStickLeft) == null)
				{
					arg_1D_0 = (this.cachedLeftStickLeft = this.GetControl(InputControlType.LeftStickLeft));
				}
				return arg_1D_0;
			}
		}

		public InputControl LeftStickRight
		{
			get
			{
				InputControl arg_1D_0;
				if ((arg_1D_0 = this.cachedLeftStickRight) == null)
				{
					arg_1D_0 = (this.cachedLeftStickRight = this.GetControl(InputControlType.LeftStickRight));
				}
				return arg_1D_0;
			}
		}

		public InputControl RightStickUp
		{
			get
			{
				InputControl arg_1D_0;
				if ((arg_1D_0 = this.cachedRightStickUp) == null)
				{
					arg_1D_0 = (this.cachedRightStickUp = this.GetControl(InputControlType.RightStickUp));
				}
				return arg_1D_0;
			}
		}

		public InputControl RightStickDown
		{
			get
			{
				InputControl arg_1D_0;
				if ((arg_1D_0 = this.cachedRightStickDown) == null)
				{
					arg_1D_0 = (this.cachedRightStickDown = this.GetControl(InputControlType.RightStickDown));
				}
				return arg_1D_0;
			}
		}

		public InputControl RightStickLeft
		{
			get
			{
				InputControl arg_1D_0;
				if ((arg_1D_0 = this.cachedRightStickLeft) == null)
				{
					arg_1D_0 = (this.cachedRightStickLeft = this.GetControl(InputControlType.RightStickLeft));
				}
				return arg_1D_0;
			}
		}

		public InputControl RightStickRight
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedRightStickRight) == null)
				{
					arg_1E_0 = (this.cachedRightStickRight = this.GetControl(InputControlType.RightStickRight));
				}
				return arg_1E_0;
			}
		}

		public InputControl DPadUp
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedDPadUp) == null)
				{
					arg_1E_0 = (this.cachedDPadUp = this.GetControl(InputControlType.DPadUp));
				}
				return arg_1E_0;
			}
		}

		public InputControl DPadDown
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedDPadDown) == null)
				{
					arg_1E_0 = (this.cachedDPadDown = this.GetControl(InputControlType.DPadDown));
				}
				return arg_1E_0;
			}
		}

		public InputControl DPadLeft
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedDPadLeft) == null)
				{
					arg_1E_0 = (this.cachedDPadLeft = this.GetControl(InputControlType.DPadLeft));
				}
				return arg_1E_0;
			}
		}

		public InputControl DPadRight
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedDPadRight) == null)
				{
					arg_1E_0 = (this.cachedDPadRight = this.GetControl(InputControlType.DPadRight));
				}
				return arg_1E_0;
			}
		}

		public InputControl Action1
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedAction1) == null)
				{
					arg_1E_0 = (this.cachedAction1 = this.GetControl(InputControlType.Action1));
				}
				return arg_1E_0;
			}
		}

		public InputControl Action2
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedAction2) == null)
				{
					arg_1E_0 = (this.cachedAction2 = this.GetControl(InputControlType.Action2));
				}
				return arg_1E_0;
			}
		}

		public InputControl Action3
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedAction3) == null)
				{
					arg_1E_0 = (this.cachedAction3 = this.GetControl(InputControlType.Action3));
				}
				return arg_1E_0;
			}
		}

		public InputControl Action4
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedAction4) == null)
				{
					arg_1E_0 = (this.cachedAction4 = this.GetControl(InputControlType.Action4));
				}
				return arg_1E_0;
			}
		}

		public InputControl LeftTrigger
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedLeftTrigger) == null)
				{
					arg_1E_0 = (this.cachedLeftTrigger = this.GetControl(InputControlType.LeftTrigger));
				}
				return arg_1E_0;
			}
		}

		public InputControl RightTrigger
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedRightTrigger) == null)
				{
					arg_1E_0 = (this.cachedRightTrigger = this.GetControl(InputControlType.RightTrigger));
				}
				return arg_1E_0;
			}
		}

		public InputControl LeftBumper
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedLeftBumper) == null)
				{
					arg_1E_0 = (this.cachedLeftBumper = this.GetControl(InputControlType.LeftBumper));
				}
				return arg_1E_0;
			}
		}

		public InputControl RightBumper
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedRightBumper) == null)
				{
					arg_1E_0 = (this.cachedRightBumper = this.GetControl(InputControlType.RightBumper));
				}
				return arg_1E_0;
			}
		}

		public InputControl LeftStickButton
		{
			get
			{
				InputControl arg_1D_0;
				if ((arg_1D_0 = this.cachedLeftStickButton) == null)
				{
					arg_1D_0 = (this.cachedLeftStickButton = this.GetControl(InputControlType.LeftStickButton));
				}
				return arg_1D_0;
			}
		}

		public InputControl RightStickButton
		{
			get
			{
				InputControl arg_1E_0;
				if ((arg_1E_0 = this.cachedRightStickButton) == null)
				{
					arg_1E_0 = (this.cachedRightStickButton = this.GetControl(InputControlType.RightStickButton));
				}
				return arg_1E_0;
			}
		}

		public InputControl LeftStickX
		{
			get
			{
				InputControl arg_21_0;
				if ((arg_21_0 = this.cachedLeftStickX) == null)
				{
					arg_21_0 = (this.cachedLeftStickX = this.GetControl(InputControlType.LeftStickX));
				}
				return arg_21_0;
			}
		}

		public InputControl LeftStickY
		{
			get
			{
				InputControl arg_21_0;
				if ((arg_21_0 = this.cachedLeftStickY) == null)
				{
					arg_21_0 = (this.cachedLeftStickY = this.GetControl(InputControlType.LeftStickY));
				}
				return arg_21_0;
			}
		}

		public InputControl RightStickX
		{
			get
			{
				InputControl arg_21_0;
				if ((arg_21_0 = this.cachedRightStickX) == null)
				{
					arg_21_0 = (this.cachedRightStickX = this.GetControl(InputControlType.RightStickX));
				}
				return arg_21_0;
			}
		}

		public InputControl RightStickY
		{
			get
			{
				InputControl arg_21_0;
				if ((arg_21_0 = this.cachedRightStickY) == null)
				{
					arg_21_0 = (this.cachedRightStickY = this.GetControl(InputControlType.RightStickY));
				}
				return arg_21_0;
			}
		}

		public InputControl DPadX
		{
			get
			{
				InputControl arg_21_0;
				if ((arg_21_0 = this.cachedDPadX) == null)
				{
					arg_21_0 = (this.cachedDPadX = this.GetControl(InputControlType.DPadX));
				}
				return arg_21_0;
			}
		}

		public InputControl DPadY
		{
			get
			{
				InputControl arg_21_0;
				if ((arg_21_0 = this.cachedDPadY) == null)
				{
					arg_21_0 = (this.cachedDPadY = this.GetControl(InputControlType.DPadY));
				}
				return arg_21_0;
			}
		}

		public InputControl Command
		{
			get
			{
				InputControl arg_21_0;
				if ((arg_21_0 = this.cachedCommand) == null)
				{
					arg_21_0 = (this.cachedCommand = this.GetControl(InputControlType.Command));
				}
				return arg_21_0;
			}
		}

		internal virtual int NumUnknownAnalogs
		{
			get
			{
				return 0;
			}
		}

		internal virtual int NumUnknownButtons
		{
			get
			{
				return 0;
			}
		}

		public InputDevice() : this(string.Empty)
		{
		}

		public InputDevice(string name) : this(name, false)
		{
		}

		public InputDevice(string name, bool rawSticks)
		{
			this.Name = name;
			this.RawSticks = rawSticks;
			this.Meta = string.Empty;
			this.GUID = Guid.NewGuid();
			this.LastChangeTick = 0uL;
			this.SortOrder = 2147483647;
			this.DeviceClass = InputDeviceClass.Unknown;
			this.DeviceStyle = InputDeviceStyle.Unknown;
			this.Passive = false;
			this.ControlsByTarget = new InputControl[521];
			this.controls = new List<InputControl>(32);
			this.Controls = new ReadOnlyCollection<InputControl>(this.controls);
			this.RemoveAliasControls();
		}

		public void MangleDefaultSettings(InputSettingsData settings)
		{
		}

		internal void OnAttached()
		{
			this.IsAttached = true;
			this.AddAliasControls();
		}

		internal void OnDetached()
		{
			this.IsAttached = false;
			this.StopVibration();
			this.RemoveAliasControls();
		}

		private void AddAliasControls()
		{
			this.RemoveAliasControls();
			if (this.IsKnown)
			{
				this.LeftStick = new TwoAxisInputControl();
				this.RightStick = new TwoAxisInputControl();
				this.DPad = new TwoAxisInputControl();
				this.AddControl(InputControlType.LeftStickX, "Left Stick X");
				this.AddControl(InputControlType.LeftStickY, "Left Stick Y");
				this.AddControl(InputControlType.RightStickX, "Right Stick X");
				this.AddControl(InputControlType.RightStickY, "Right Stick Y");
				this.AddControl(InputControlType.DPadX, "DPad X");
				this.AddControl(InputControlType.DPadY, "DPad Y");
				this.AddControl(InputControlType.Command, "Command");
				this.ExpireControlCache();
			}
		}

		private void RemoveAliasControls()
		{
			this.LeftStick = TwoAxisInputControl.Null;
			this.RightStick = TwoAxisInputControl.Null;
			this.DPad = TwoAxisInputControl.Null;
			this.RemoveControl(InputControlType.LeftStickX);
			this.RemoveControl(InputControlType.LeftStickY);
			this.RemoveControl(InputControlType.RightStickX);
			this.RemoveControl(InputControlType.RightStickY);
			this.RemoveControl(InputControlType.DPadX);
			this.RemoveControl(InputControlType.DPadY);
			this.RemoveControl(InputControlType.Command);
			this.ExpireControlCache();
		}

		protected void ClearControls()
		{
			Array.Clear(this.ControlsByTarget, 0, this.ControlsByTarget.Length);
			this.controls.Clear();
			this.ExpireControlCache();
		}

		public bool HasControl(InputControlType controlType)
		{
			return this.ControlsByTarget[(int)controlType] != null;
		}

		public InputControl GetControl(InputControlType controlType)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			return inputControl ?? InputControl.Null;
		}

		public static InputControlType GetInputControlTypeByName(string inputControlName)
		{
			return (InputControlType)Enum.Parse(typeof(InputControlType), inputControlName);
		}

		public InputControl GetControlByName(string controlName)
		{
			InputControlType inputControlTypeByName = InputDevice.GetInputControlTypeByName(controlName);
			return this.GetControl(inputControlTypeByName);
		}

		public InputControl AddControl(InputControlType controlType, string handle)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			if (inputControl == null)
			{
				inputControl = new InputControl(handle, controlType);
				this.ControlsByTarget[(int)controlType] = inputControl;
				this.controls.Add(inputControl);
				this.ExpireControlCache();
			}
			return inputControl;
		}

		public InputControl AddControl(InputControlType controlType, string handle, float lowerDeadZone, float upperDeadZone)
		{
			InputControl inputControl = this.AddControl(controlType, handle);
			inputControl.LowerDeadZone = lowerDeadZone;
			inputControl.UpperDeadZone = upperDeadZone;
			return inputControl;
		}

		private void RemoveControl(InputControlType controlType)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			if (inputControl != null)
			{
				this.ControlsByTarget[(int)controlType] = null;
				this.controls.Remove(inputControl);
				this.ExpireControlCache();
			}
		}

		public void ClearInputState()
		{
			this.LeftStick.ClearInputState();
			this.RightStick.ClearInputState();
			this.DPad.ClearInputState();
			int count = this.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.ClearInputState();
				}
			}
		}

		protected void UpdateWithState(InputControlType controlType, bool state, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithState(state, updateTick, deltaTime);
		}

		protected void UpdateWithValue(InputControlType controlType, float value, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithValue(value, updateTick, deltaTime);
		}

		internal void UpdateLeftStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		internal void UpdateLeftStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		internal void CommitLeftStick()
		{
			this.LeftStickUp.Commit();
			this.LeftStickDown.Commit();
			this.LeftStickLeft.Commit();
			this.LeftStickRight.Commit();
		}

		internal void UpdateRightStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		internal void UpdateRightStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		internal void CommitRightStick()
		{
			this.RightStickUp.Commit();
			this.RightStickDown.Commit();
			this.RightStickLeft.Commit();
			this.RightStickRight.Commit();
		}

		public virtual void Update(ulong updateTick, float deltaTime)
		{
		}

		private bool AnyCommandControlIsPressed()
		{
			for (int i = 100; i <= 113; i++)
			{
				InputControl inputControl = this.ControlsByTarget[i];
				if (inputControl != null && inputControl.IsPressed)
				{
					return true;
				}
			}
			return false;
		}

		private void ProcessLeftStick(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.LeftStickLeft.NextRawValue, this.LeftStickRight.NextRawValue);
			float y = Utility.ValueFromSides(this.LeftStickDown.NextRawValue, this.LeftStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.LeftStickLeft.Raw || this.LeftStickRight.Raw || this.LeftStickUp.Raw || this.LeftStickDown.Raw)
			{
				vector = new Vector2(x, y);
			}
			else
			{
				float lowerDeadZone = Utility.Max(this.LeftStickLeft.LowerDeadZone, this.LeftStickRight.LowerDeadZone, this.LeftStickUp.LowerDeadZone, this.LeftStickDown.LowerDeadZone);
				float upperDeadZone = Utility.Min(this.LeftStickLeft.UpperDeadZone, this.LeftStickRight.UpperDeadZone, this.LeftStickUp.UpperDeadZone, this.LeftStickDown.UpperDeadZone);
				vector = Utility.ApplySeparateDeadZone(x, y, lowerDeadZone, upperDeadZone);
			}
			this.LeftStick.Raw = true;
			this.LeftStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.LeftStickX.Raw = true;
			this.LeftStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.LeftStickY.Raw = true;
			this.LeftStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.LeftStickLeft.SetValue(this.LeftStick.Left.Value, updateTick);
			this.LeftStickRight.SetValue(this.LeftStick.Right.Value, updateTick);
			this.LeftStickUp.SetValue(this.LeftStick.Up.Value, updateTick);
			this.LeftStickDown.SetValue(this.LeftStick.Down.Value, updateTick);
		}

		private void ProcessRightStick(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.RightStickLeft.NextRawValue, this.RightStickRight.NextRawValue);
			float y = Utility.ValueFromSides(this.RightStickDown.NextRawValue, this.RightStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.RightStickLeft.Raw || this.RightStickRight.Raw || this.RightStickUp.Raw || this.RightStickDown.Raw)
			{
				vector = new Vector2(x, y);
			}
			else
			{
				float lowerDeadZone = Utility.Max(this.RightStickLeft.LowerDeadZone, this.RightStickRight.LowerDeadZone, this.RightStickUp.LowerDeadZone, this.RightStickDown.LowerDeadZone);
				float upperDeadZone = Utility.Min(this.RightStickLeft.UpperDeadZone, this.RightStickRight.UpperDeadZone, this.RightStickUp.UpperDeadZone, this.RightStickDown.UpperDeadZone);
				vector = Utility.ApplySeparateDeadZone(x, y, lowerDeadZone, upperDeadZone);
			}
			this.RightStick.Raw = true;
			this.RightStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.RightStickX.Raw = true;
			this.RightStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.RightStickY.Raw = true;
			this.RightStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.RightStickLeft.SetValue(this.RightStick.Left.Value, updateTick);
			this.RightStickRight.SetValue(this.RightStick.Right.Value, updateTick);
			this.RightStickUp.SetValue(this.RightStick.Up.Value, updateTick);
			this.RightStickDown.SetValue(this.RightStick.Down.Value, updateTick);
		}

		private void ProcessDPad(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.DPadLeft.NextRawValue, this.DPadRight.NextRawValue);
			float y = Utility.ValueFromSides(this.DPadDown.NextRawValue, this.DPadUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.DPadLeft.Raw || this.DPadRight.Raw || this.DPadUp.Raw || this.DPadDown.Raw)
			{
				vector = new Vector2(x, y);
			}
			else
			{
				float lowerDeadZone = Utility.Max(this.DPadLeft.LowerDeadZone, this.DPadRight.LowerDeadZone, this.DPadUp.LowerDeadZone, this.DPadDown.LowerDeadZone);
				float upperDeadZone = Utility.Min(this.DPadLeft.UpperDeadZone, this.DPadRight.UpperDeadZone, this.DPadUp.UpperDeadZone, this.DPadDown.UpperDeadZone);
				vector = Utility.ApplySeparateDeadZone(x, y, lowerDeadZone, upperDeadZone);
			}
			this.DPad.Raw = true;
			this.DPad.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.DPadX.Raw = true;
			this.DPadX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.DPadY.Raw = true;
			this.DPadY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.DPadLeft.SetValue(this.DPad.Left.Value, updateTick);
			this.DPadRight.SetValue(this.DPad.Right.Value, updateTick);
			this.DPadUp.SetValue(this.DPad.Up.Value, updateTick);
			this.DPadDown.SetValue(this.DPad.Down.Value, updateTick);
		}

		public void Commit(ulong updateTick, float deltaTime)
		{
			if (this.IsKnown)
			{
				this.ProcessLeftStick(updateTick, deltaTime);
				this.ProcessRightStick(updateTick, deltaTime);
				this.ProcessDPad(updateTick, deltaTime);
			}
			int count = this.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.Commit();
					if (inputControl.HasChanged && !inputControl.Passive)
					{
						this.LastChangeTick = updateTick;
					}
				}
			}
			if (this.IsKnown)
			{
				this.Command.CommitWithState(this.AnyCommandControlIsPressed(), updateTick, deltaTime);
			}
		}

		public bool LastChangedAfter(InputDevice device)
		{
			return device == null || this.LastChangeTick > device.LastChangeTick;
		}

		internal void RequestActivation()
		{
			this.LastChangeTick = InputManager.CurrentTick;
		}

		public virtual void Vibrate(float leftMotor, float rightMotor)
		{
		}

		public void Vibrate(float intensity)
		{
			this.Vibrate(intensity, intensity);
		}

		public void StopVibration()
		{
			this.Vibrate(0f);
		}

		public virtual void SetLightColor(float red, float green, float blue)
		{
		}

		public void SetLightColor(Color color)
		{
			this.SetLightColor(color.r * color.a, color.g * color.a, color.b * color.a);
		}

		public virtual void SetLightFlash(float flashOnDuration, float flashOffDuration)
		{
		}

		public void StopLightFlash()
		{
			this.SetLightFlash(1f, 0f);
		}

		private void ExpireControlCache()
		{
			this.cachedLeftStickUp = null;
			this.cachedLeftStickDown = null;
			this.cachedLeftStickLeft = null;
			this.cachedLeftStickRight = null;
			this.cachedRightStickUp = null;
			this.cachedRightStickDown = null;
			this.cachedRightStickLeft = null;
			this.cachedRightStickRight = null;
			this.cachedDPadUp = null;
			this.cachedDPadDown = null;
			this.cachedDPadLeft = null;
			this.cachedDPadRight = null;
			this.cachedAction1 = null;
			this.cachedAction2 = null;
			this.cachedAction3 = null;
			this.cachedAction4 = null;
			this.cachedLeftTrigger = null;
			this.cachedRightTrigger = null;
			this.cachedLeftBumper = null;
			this.cachedRightBumper = null;
			this.cachedLeftStickButton = null;
			this.cachedRightStickButton = null;
			this.cachedLeftStickX = null;
			this.cachedLeftStickY = null;
			this.cachedRightStickX = null;
			this.cachedRightStickY = null;
			this.cachedDPadX = null;
			this.cachedDPadY = null;
			this.cachedCommand = null;
		}

		internal virtual bool ReadRawButtonState(int index)
		{
			return false;
		}

		internal virtual float ReadRawAnalogValue(int index)
		{
			return 0f;
		}

		internal void TakeSnapshot()
		{
			if (this.AnalogSnapshot == null)
			{
				this.AnalogSnapshot = new InputDevice.AnalogSnapshotEntry[this.NumUnknownAnalogs];
			}
			for (int i = 0; i < this.NumUnknownAnalogs; i++)
			{
				float value = Utility.ApplySnapping(this.ReadRawAnalogValue(i), 0.5f);
				this.AnalogSnapshot[i].value = value;
			}
		}

		internal UnknownDeviceControl GetFirstPressedAnalog()
		{
			if (this.AnalogSnapshot != null)
			{
				for (int i = 0; i < this.NumUnknownAnalogs; i++)
				{
					InputControlType control = InputControlType.Analog0 + i;
					float num = Utility.ApplySnapping(this.ReadRawAnalogValue(i), 0.5f);
					float num2 = num - this.AnalogSnapshot[i].value;
					this.AnalogSnapshot[i].TrackMinMaxValue(num);
					if (num2 > 0.1f)
					{
						num2 = this.AnalogSnapshot[i].maxValue - this.AnalogSnapshot[i].value;
					}
					if (num2 < -0.1f)
					{
						num2 = this.AnalogSnapshot[i].minValue - this.AnalogSnapshot[i].value;
					}
					if (num2 > 1.9f)
					{
						return new UnknownDeviceControl(control, InputRangeType.MinusOneToOne);
					}
					if (num2 < -0.9f)
					{
						return new UnknownDeviceControl(control, InputRangeType.ZeroToMinusOne);
					}
					if (num2 > 0.9f)
					{
						return new UnknownDeviceControl(control, InputRangeType.ZeroToOne);
					}
				}
			}
			return UnknownDeviceControl.None;
		}

		internal UnknownDeviceControl GetFirstPressedButton()
		{
			for (int i = 0; i < this.NumUnknownButtons; i++)
			{
				if (this.ReadRawButtonState(i))
				{
					return new UnknownDeviceControl(InputControlType.Button0 + i, InputRangeType.ZeroToOne);
				}
			}
			return UnknownDeviceControl.None;
		}
	}
}
