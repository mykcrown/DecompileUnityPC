// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	public class OuyaEverywhereDevice : InputDevice
	{
		private const float LowerDeadZone = 0.2f;

		private const float UpperDeadZone = 0.9f;

		public int DeviceIndex
		{
			get;
			private set;
		}

		public override float DefaultLeftStickLowerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		public override float DefaultRightStickLowerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		public override float DefaultLeftTriggerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		public override float DefaultRightTriggerDeadZone
		{
			get
			{
				return 0.2f;
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

		public OuyaEverywhereDevice(int deviceIndex) : base("OUYA Controller")
		{
			this.DeviceIndex = deviceIndex;
			base.SortOrder = deviceIndex;
			base.Meta = "OUYA Everywhere Device #" + deviceIndex;
			base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left");
			base.AddControl(InputControlType.LeftStickRight, "Left Stick Right");
			base.AddControl(InputControlType.LeftStickUp, "Left Stick Up");
			base.AddControl(InputControlType.LeftStickDown, "Left Stick Down");
			base.AddControl(InputControlType.RightStickLeft, "Right Stick Left");
			base.AddControl(InputControlType.RightStickRight, "Right Stick Right");
			base.AddControl(InputControlType.RightStickUp, "Right Stick Up");
			base.AddControl(InputControlType.RightStickDown, "Right Stick Down");
			base.AddControl(InputControlType.LeftTrigger, "Left Trigger");
			base.AddControl(InputControlType.RightTrigger, "Right Trigger");
			base.AddControl(InputControlType.DPadUp, "DPad Up");
			base.AddControl(InputControlType.DPadDown, "DPad Down");
			base.AddControl(InputControlType.DPadLeft, "DPad Left");
			base.AddControl(InputControlType.DPadRight, "DPad Right");
			base.AddControl(InputControlType.Action1, "O");
			base.AddControl(InputControlType.Action2, "A");
			base.AddControl(InputControlType.Action3, "Y");
			base.AddControl(InputControlType.Action4, "U");
			base.AddControl(InputControlType.LeftBumper, "Left Bumper");
			base.AddControl(InputControlType.RightBumper, "Right Bumper");
			base.AddControl(InputControlType.LeftStickButton, "Left Stick Button");
			base.AddControl(InputControlType.RightStickButton, "Right Stick Button");
			base.AddControl(InputControlType.Menu, "Menu");
		}

		public void BeforeAttach()
		{
		}

		public override void Update(ulong updateTick, float deltaTime)
		{
		}
	}
}
