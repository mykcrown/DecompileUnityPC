// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	public class XboxOneInputDevice : InputDevice
	{
		private const uint AnalogLeftStickX = 0u;

		private const uint AnalogLeftStickY = 1u;

		private const uint AnalogRightStickX = 3u;

		private const uint AnalogRightStickY = 4u;

		private const uint AnalogLeftTrigger = 8u;

		private const uint AnalogRightTrigger = 9u;

		private const float LowerDeadZone = 0.6f;

		private const float UpperDeadZone = 0.9f;

		private string[] analogAxisNameForId;

		internal uint JoystickId
		{
			get;
			private set;
		}

		public ulong ControllerId
		{
			get;
			private set;
		}

		public override float DefaultLeftStickLowerDeadZone
		{
			get
			{
				return 0.6f;
			}
		}

		public override float DefaultRightStickLowerDeadZone
		{
			get
			{
				return 0.6f;
			}
		}

		public override float DefaultLeftTriggerDeadZone
		{
			get
			{
				return 0.6f;
			}
		}

		public override float DefaultRightTriggerDeadZone
		{
			get
			{
				return 0.6f;
			}
		}

		protected override float MaxLeftStickUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		protected override float MaxRightStickUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		protected override float MaxTriggerUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		public bool IsConnected
		{
			get
			{
				return false;
			}
		}

		public XboxOneInputDevice(uint joystickId) : base("Xbox One Controller")
		{
			this.JoystickId = joystickId;
			base.SortOrder = (int)joystickId;
			base.Meta = "Xbox One Device #" + joystickId;
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.XboxOne;
			this.CacheAnalogAxisNames();
			base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left", 0.6f, 0.9f);
			base.AddControl(InputControlType.LeftStickRight, "Left Stick Right", 0.6f, 0.9f);
			base.AddControl(InputControlType.LeftStickUp, "Left Stick Up", 0.6f, 0.9f);
			base.AddControl(InputControlType.LeftStickDown, "Left Stick Down", 0.6f, 0.9f);
			base.AddControl(InputControlType.RightStickLeft, "Right Stick Left", 0.6f, 0.9f);
			base.AddControl(InputControlType.RightStickRight, "Right Stick Right", 0.6f, 0.9f);
			base.AddControl(InputControlType.RightStickUp, "Right Stick Up", 0.6f, 0.9f);
			base.AddControl(InputControlType.RightStickDown, "Right Stick Down", 0.6f, 0.9f);
			base.AddControl(InputControlType.LeftTrigger, "Left Trigger", 0.6f, 0.9f);
			base.AddControl(InputControlType.RightTrigger, "Right Trigger", 0.6f, 0.9f);
			base.AddControl(InputControlType.DPadUp, "DPad Up", 0.6f, 0.9f);
			base.AddControl(InputControlType.DPadDown, "DPad Down", 0.6f, 0.9f);
			base.AddControl(InputControlType.DPadLeft, "DPad Left", 0.6f, 0.9f);
			base.AddControl(InputControlType.DPadRight, "DPad Right", 0.6f, 0.9f);
			base.AddControl(InputControlType.Action1, "A");
			base.AddControl(InputControlType.Action2, "B");
			base.AddControl(InputControlType.Action3, "X");
			base.AddControl(InputControlType.Action4, "Y");
			base.AddControl(InputControlType.LeftBumper, "Left Bumper");
			base.AddControl(InputControlType.RightBumper, "Right Bumper");
			base.AddControl(InputControlType.LeftStickButton, "Left Stick Button");
			base.AddControl(InputControlType.RightStickButton, "Right Stick Button");
			base.AddControl(InputControlType.View, "View");
			base.AddControl(InputControlType.Menu, "Menu");
		}

		public override void Update(ulong updateTick, float deltaTime)
		{
		}

		public override void Vibrate(float leftMotor, float rightMotor)
		{
		}

		public void Vibrate(float leftMotor, float rightMotor, float leftTrigger, float rightTrigger)
		{
		}

		private string AnalogAxisNameForId(uint analogId)
		{
			return this.analogAxisNameForId[(int)((UIntPtr)analogId)];
		}

		private void CacheAnalogAxisNameForId(uint analogId)
		{
			this.analogAxisNameForId[(int)((UIntPtr)analogId)] = string.Concat(new object[]
			{
				"joystick ",
				this.JoystickId,
				" analog ",
				analogId
			});
		}

		private void CacheAnalogAxisNames()
		{
			this.analogAxisNameForId = new string[16];
			this.CacheAnalogAxisNameForId(0u);
			this.CacheAnalogAxisNameForId(1u);
			this.CacheAnalogAxisNameForId(3u);
			this.CacheAnalogAxisNameForId(4u);
			this.CacheAnalogAxisNameForId(8u);
			this.CacheAnalogAxisNameForId(9u);
		}
	}
}
