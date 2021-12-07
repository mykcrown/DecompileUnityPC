using System;

namespace InControl
{
	// Token: 0x0200004E RID: 78
	public class OuyaEverywhereDevice : InputDevice
	{
		// Token: 0x0600028B RID: 651 RVA: 0x00013780 File Offset: 0x00011B80
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600028C RID: 652 RVA: 0x000138F6 File Offset: 0x00011CF6
		// (set) Token: 0x0600028D RID: 653 RVA: 0x000138FE File Offset: 0x00011CFE
		public int DeviceIndex { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00013907 File Offset: 0x00011D07
		public override float DefaultLeftStickLowerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0001390E File Offset: 0x00011D0E
		public override float DefaultRightStickLowerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00013915 File Offset: 0x00011D15
		public override float DefaultLeftTriggerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0001391C File Offset: 0x00011D1C
		public override float DefaultRightTriggerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00013923 File Offset: 0x00011D23
		protected override float MaxLeftStickUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0001392A File Offset: 0x00011D2A
		protected override float MaxRightStickUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00013931 File Offset: 0x00011D31
		protected override float MaxTriggerUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00013938 File Offset: 0x00011D38
		public void BeforeAttach()
		{
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0001393A File Offset: 0x00011D3A
		public override void Update(ulong updateTick, float deltaTime)
		{
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0001393C File Offset: 0x00011D3C
		public bool IsConnected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040001E1 RID: 481
		private const float LowerDeadZone = 0.2f;

		// Token: 0x040001E2 RID: 482
		private const float UpperDeadZone = 0.9f;
	}
}
