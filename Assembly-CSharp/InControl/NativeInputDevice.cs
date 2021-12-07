using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200011C RID: 284
	public class NativeInputDevice : InputDevice
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x0002A3F5 File Offset: 0x000287F5
		internal NativeInputDevice()
		{
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0002A3FD File Offset: 0x000287FD
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x0002A405 File Offset: 0x00028805
		internal uint Handle { get; private set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0002A40E File Offset: 0x0002880E
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x0002A416 File Offset: 0x00028816
		internal NativeDeviceInfo Info { get; private set; }

		// Token: 0x06000605 RID: 1541 RVA: 0x0002A420 File Offset: 0x00028820
		internal void Initialize(uint deviceHandle, NativeDeviceInfo deviceInfo, NativeInputDeviceProfile deviceProfile)
		{
			this.Handle = deviceHandle;
			this.Info = deviceInfo;
			this.profile = deviceProfile;
			base.SortOrder = (int)(1000U + this.Handle);
			this.numUnknownButtons = Math.Min((int)this.Info.numButtons, 20);
			this.numUnknownAnalogs = Math.Min((int)this.Info.numAnalogs, 20);
			this.buttons = new short[this.Info.numButtons];
			this.analogs = new short[this.Info.numAnalogs];
			base.AnalogSnapshot = null;
			base.ClearInputState();
			base.ClearControls();
			if (this.IsKnown)
			{
				base.Name = (this.profile.Name ?? this.Info.name);
				base.Meta = (this.profile.Meta ?? this.Info.name);
				base.DeviceClass = this.profile.DeviceClass;
				base.DeviceStyle = this.profile.DeviceStyle;
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					InputControl inputControl = base.AddControl(inputControlMapping.Target, inputControlMapping.Handle);
					inputControl.Sensitivity = Mathf.Min(this.profile.Sensitivity, inputControlMapping.Sensitivity);
					inputControl.LowerDeadZone = Mathf.Max(this.profile.LowerDeadZone, inputControlMapping.LowerDeadZone);
					inputControl.UpperDeadZone = Mathf.Min(this.profile.UpperDeadZone, inputControlMapping.UpperDeadZone);
					inputControl.Raw = inputControlMapping.Raw;
					inputControl.Passive = inputControlMapping.Passive;
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					InputControl inputControl2 = base.AddControl(inputControlMapping2.Target, inputControlMapping2.Handle);
					inputControl2.Passive = inputControlMapping2.Passive;
				}
			}
			else
			{
				base.Name = "Unknown Device";
				base.Meta = this.Info.name;
				for (int k = 0; k < this.NumUnknownButtons; k++)
				{
					base.AddControl(InputControlType.Button0 + k, "Button " + k);
				}
				for (int l = 0; l < this.NumUnknownAnalogs; l++)
				{
					base.AddControl(InputControlType.Analog0 + l, "Analog " + l, 0.2f, 0.9f);
				}
			}
			this.skipUpdateFrames = 1;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0002A710 File Offset: 0x00028B10
		internal void Initialize(uint deviceHandle, NativeDeviceInfo deviceInfo)
		{
			this.Initialize(deviceHandle, deviceInfo, this.profile);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0002A720 File Offset: 0x00028B20
		public override void Update(ulong updateTick, float deltaTime)
		{
			if (this.skipUpdateFrames > 0)
			{
				this.skipUpdateFrames--;
				return;
			}
			IntPtr source;
			if (Native.GetDeviceState(this.Handle, out source))
			{
				Marshal.Copy(source, this.buttons, 0, this.buttons.Length);
				source = new IntPtr(source.ToInt64() + (long)(this.buttons.Length * 2));
				Marshal.Copy(source, this.analogs, 0, this.analogs.Length);
			}
			if (this.IsKnown)
			{
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					float value = inputControlMapping.Source.GetValue(this);
					InputControl control = base.GetControl(inputControlMapping.Target);
					if (!inputControlMapping.IgnoreInitialZeroValue || !control.IsOnZeroTick || !Utility.IsZero(value))
					{
						float value2 = inputControlMapping.MapValue(value);
						control.UpdateWithValue(value2, updateTick, deltaTime);
					}
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					bool state = inputControlMapping2.Source.GetState(this);
					base.UpdateWithState(inputControlMapping2.Target, state, updateTick, deltaTime);
				}
			}
			else
			{
				for (int k = 0; k < this.NumUnknownButtons; k++)
				{
					base.UpdateWithState(InputControlType.Button0 + k, this.ReadRawButtonState(k), updateTick, deltaTime);
				}
				for (int l = 0; l < this.NumUnknownAnalogs; l++)
				{
					base.UpdateWithValue(InputControlType.Analog0 + l, this.ReadRawAnalogValue(l), updateTick, deltaTime);
				}
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0002A8E9 File Offset: 0x00028CE9
		internal override bool ReadRawButtonState(int index)
		{
			return index < this.buttons.Length && this.buttons[index] > -32767;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0002A90A File Offset: 0x00028D0A
		internal override float ReadRawAnalogValue(int index)
		{
			if (index < this.analogs.Length)
			{
				return (float)this.analogs[index] / 32767f;
			}
			return 0f;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0002A92F File Offset: 0x00028D2F
		private byte FloatToByte(float value)
		{
			return (byte)(Mathf.Clamp01(value) * 255f);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0002A93E File Offset: 0x00028D3E
		public override void Vibrate(float leftMotor, float rightMotor)
		{
			Native.SetHapticState(this.Handle, this.FloatToByte(leftMotor), this.FloatToByte(rightMotor));
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0002A959 File Offset: 0x00028D59
		public override void SetLightColor(float red, float green, float blue)
		{
			Native.SetLightColor(this.Handle, this.FloatToByte(red), this.FloatToByte(green), this.FloatToByte(blue));
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0002A97B File Offset: 0x00028D7B
		public override void SetLightFlash(float flashOnDuration, float flashOffDuration)
		{
			Native.SetLightFlash(this.Handle, this.FloatToByte(flashOnDuration), this.FloatToByte(flashOffDuration));
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0002A998 File Offset: 0x00028D98
		public bool HasSameVendorID(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameVendorID(deviceInfo);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0002A9B4 File Offset: 0x00028DB4
		public bool HasSameProductID(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameProductID(deviceInfo);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0002A9D0 File Offset: 0x00028DD0
		public bool HasSameVersionNumber(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameVersionNumber(deviceInfo);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0002A9EC File Offset: 0x00028DEC
		public bool HasSameLocation(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameLocation(deviceInfo);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0002AA08 File Offset: 0x00028E08
		public bool HasSameSerialNumber(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameSerialNumber(deviceInfo);
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0002AA24 File Offset: 0x00028E24
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.profile == null || this.profile.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0002AA3F File Offset: 0x00028E3F
		public override bool IsKnown
		{
			get
			{
				return this.profile != null;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0002AA4D File Offset: 0x00028E4D
		internal override int NumUnknownButtons
		{
			get
			{
				return this.numUnknownButtons;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0002AA55 File Offset: 0x00028E55
		internal override int NumUnknownAnalogs
		{
			get
			{
				return this.numUnknownAnalogs;
			}
		}

		// Token: 0x0400047C RID: 1148
		private const int maxUnknownButtons = 20;

		// Token: 0x0400047D RID: 1149
		private const int maxUnknownAnalogs = 20;

		// Token: 0x04000480 RID: 1152
		private short[] buttons;

		// Token: 0x04000481 RID: 1153
		private short[] analogs;

		// Token: 0x04000482 RID: 1154
		private NativeInputDeviceProfile profile;

		// Token: 0x04000483 RID: 1155
		private int skipUpdateFrames;

		// Token: 0x04000484 RID: 1156
		private int numUnknownButtons;

		// Token: 0x04000485 RID: 1157
		private int numUnknownAnalogs;
	}
}
