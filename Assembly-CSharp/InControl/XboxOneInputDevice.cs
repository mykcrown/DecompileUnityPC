using System;

namespace InControl
{
	// Token: 0x020001DB RID: 475
	public class XboxOneInputDevice : InputDevice
	{
		// Token: 0x06000871 RID: 2161 RVA: 0x0004BDDC File Offset: 0x0004A1DC
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

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0004C000 File Offset: 0x0004A400
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x0004C008 File Offset: 0x0004A408
		internal uint JoystickId { get; private set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0004C011 File Offset: 0x0004A411
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x0004C019 File Offset: 0x0004A419
		public ulong ControllerId { get; private set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0004C022 File Offset: 0x0004A422
		public override float DefaultLeftStickLowerDeadZone
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0004C029 File Offset: 0x0004A429
		public override float DefaultRightStickLowerDeadZone
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0004C030 File Offset: 0x0004A430
		public override float DefaultLeftTriggerDeadZone
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0004C037 File Offset: 0x0004A437
		public override float DefaultRightTriggerDeadZone
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0004C03E File Offset: 0x0004A43E
		protected override float MaxLeftStickUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0004C045 File Offset: 0x0004A445
		protected override float MaxRightStickUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0004C04C File Offset: 0x0004A44C
		protected override float MaxTriggerUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0004C053 File Offset: 0x0004A453
		public override void Update(ulong updateTick, float deltaTime)
		{
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0004C055 File Offset: 0x0004A455
		public bool IsConnected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0004C058 File Offset: 0x0004A458
		public override void Vibrate(float leftMotor, float rightMotor)
		{
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0004C05A File Offset: 0x0004A45A
		public void Vibrate(float leftMotor, float rightMotor, float leftTrigger, float rightTrigger)
		{
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0004C05C File Offset: 0x0004A45C
		private string AnalogAxisNameForId(uint analogId)
		{
			return this.analogAxisNameForId[(int)((UIntPtr)analogId)];
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0004C067 File Offset: 0x0004A467
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

		// Token: 0x06000883 RID: 2179 RVA: 0x0004C0A4 File Offset: 0x0004A4A4
		private void CacheAnalogAxisNames()
		{
			this.analogAxisNameForId = new string[16];
			this.CacheAnalogAxisNameForId(0U);
			this.CacheAnalogAxisNameForId(1U);
			this.CacheAnalogAxisNameForId(3U);
			this.CacheAnalogAxisNameForId(4U);
			this.CacheAnalogAxisNameForId(8U);
			this.CacheAnalogAxisNameForId(9U);
		}

		// Token: 0x040005EE RID: 1518
		private const uint AnalogLeftStickX = 0U;

		// Token: 0x040005EF RID: 1519
		private const uint AnalogLeftStickY = 1U;

		// Token: 0x040005F0 RID: 1520
		private const uint AnalogRightStickX = 3U;

		// Token: 0x040005F1 RID: 1521
		private const uint AnalogRightStickY = 4U;

		// Token: 0x040005F2 RID: 1522
		private const uint AnalogLeftTrigger = 8U;

		// Token: 0x040005F3 RID: 1523
		private const uint AnalogRightTrigger = 9U;

		// Token: 0x040005F4 RID: 1524
		private const float LowerDeadZone = 0.6f;

		// Token: 0x040005F5 RID: 1525
		private const float UpperDeadZone = 0.9f;

		// Token: 0x040005F8 RID: 1528
		private string[] analogAxisNameForId;
	}
}
