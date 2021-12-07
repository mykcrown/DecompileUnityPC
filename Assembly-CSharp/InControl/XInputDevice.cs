using System;
using XInputDotNetPure;

namespace InControl
{
	// Token: 0x020001DD RID: 477
	public class XInputDevice : InputDevice
	{
		// Token: 0x0600088A RID: 2186 RVA: 0x0004C22C File Offset: 0x0004A62C
		public XInputDevice(int deviceIndex, XInputDeviceManager owner) : base("XInput Controller")
		{
			this.owner = owner;
			this.DeviceIndex = deviceIndex;
			base.SortOrder = deviceIndex;
			base.Meta = "XInput Device #" + deviceIndex;
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.XboxOne;
			base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left", 0.2f, 0.9f);
			base.AddControl(InputControlType.LeftStickRight, "Left Stick Right", 0.2f, 0.9f);
			base.AddControl(InputControlType.LeftStickUp, "Left Stick Up", 0.2f, 0.9f);
			base.AddControl(InputControlType.LeftStickDown, "Left Stick Down", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightStickLeft, "Right Stick Left", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightStickRight, "Right Stick Right", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightStickUp, "Right Stick Up", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightStickDown, "Right Stick Down", 0.2f, 0.9f);
			base.AddControl(InputControlType.LeftTrigger, "Left Trigger", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightTrigger, "Right Trigger", 0.2f, 0.9f);
			base.AddControl(InputControlType.DPadUp, "DPad Up", 0.2f, 0.9f);
			base.AddControl(InputControlType.DPadDown, "DPad Down", 0.2f, 0.9f);
			base.AddControl(InputControlType.DPadLeft, "DPad Left", 0.2f, 0.9f);
			base.AddControl(InputControlType.DPadRight, "DPad Right", 0.2f, 0.9f);
			base.AddControl(InputControlType.Action1, "A");
			base.AddControl(InputControlType.Action2, "B");
			base.AddControl(InputControlType.Action3, "X");
			base.AddControl(InputControlType.Action4, "Y");
			base.AddControl(InputControlType.LeftBumper, "Left Bumper");
			base.AddControl(InputControlType.RightBumper, "Right Bumper");
			base.AddControl(InputControlType.LeftStickButton, "Left Stick Button");
			base.AddControl(InputControlType.RightStickButton, "Right Stick Button");
			base.AddControl(InputControlType.Start, "Start");
			base.AddControl(InputControlType.Back, "Back");
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0004C451 File Offset: 0x0004A851
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x0004C459 File Offset: 0x0004A859
		public int DeviceIndex { get; private set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0004C462 File Offset: 0x0004A862
		public override float DefaultLeftStickLowerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0004C469 File Offset: 0x0004A869
		public override float DefaultRightStickLowerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x0004C470 File Offset: 0x0004A870
		public override float DefaultLeftTriggerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0004C477 File Offset: 0x0004A877
		public override float DefaultRightTriggerDeadZone
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x0004C47E File Offset: 0x0004A87E
		protected override float MaxLeftStickUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x0004C485 File Offset: 0x0004A885
		protected override float MaxRightStickUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0004C48C File Offset: 0x0004A88C
		protected override float MaxTriggerUpperDeadZone
		{
			get
			{
				return 0.9f;
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0004C494 File Offset: 0x0004A894
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.GetState();
			base.UpdateLeftStickWithValue(this.state.ThumbSticks.Left.Vector, updateTick, deltaTime);
			base.UpdateRightStickWithValue(this.state.ThumbSticks.Right.Vector, updateTick, deltaTime);
			base.UpdateWithValue(InputControlType.LeftTrigger, this.state.Triggers.Left, updateTick, deltaTime);
			base.UpdateWithValue(InputControlType.RightTrigger, this.state.Triggers.Right, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadUp, this.state.DPad.Up == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadDown, this.state.DPad.Down == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadLeft, this.state.DPad.Left == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadRight, this.state.DPad.Right == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action1, this.state.Buttons.A == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action2, this.state.Buttons.B == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action3, this.state.Buttons.X == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action4, this.state.Buttons.Y == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.LeftBumper, this.state.Buttons.LeftShoulder == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.RightBumper, this.state.Buttons.RightShoulder == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.LeftStickButton, this.state.Buttons.LeftStick == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.RightStickButton, this.state.Buttons.RightStick == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Start, this.state.Buttons.Start == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Back, this.state.Buttons.Back == ButtonState.Pressed, updateTick, deltaTime);
			base.Commit(updateTick, deltaTime);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0004C6FE File Offset: 0x0004AAFE
		public override void Vibrate(float leftMotor, float rightMotor)
		{
			GamePad.SetVibration((PlayerIndex)this.DeviceIndex, leftMotor, rightMotor);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0004C70D File Offset: 0x0004AB0D
		internal void GetState()
		{
			this.state = this.owner.GetState(this.DeviceIndex);
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x0004C726 File Offset: 0x0004AB26
		public bool IsConnected
		{
			get
			{
				return this.state.IsConnected;
			}
		}

		// Token: 0x040005FB RID: 1531
		private const float LowerDeadZone_RightStick = 0.2f;

		// Token: 0x040005FC RID: 1532
		private const float UpperDeadZone_RightStick = 0.9f;

		// Token: 0x040005FD RID: 1533
		private const float LowerDeadZone_LeftStick = 0.2f;

		// Token: 0x040005FE RID: 1534
		private const float UpperDeadZone_LeftStick = 0.9f;

		// Token: 0x040005FF RID: 1535
		private const float LowerDeadZone_Triggers = 0.2f;

		// Token: 0x04000600 RID: 1536
		private const float UpperDeadZone_Triggers = 0.9f;

		// Token: 0x04000601 RID: 1537
		private const float LowerDeadZone_Dpad = 0.2f;

		// Token: 0x04000602 RID: 1538
		private const float UpperDeadZone_Dpad = 0.9f;

		// Token: 0x04000603 RID: 1539
		private XInputDeviceManager owner;

		// Token: 0x04000604 RID: 1540
		private GamePadState state;
	}
}
