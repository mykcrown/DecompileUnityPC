using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000056 RID: 86
	public class DeviceBindingSource : BindingSource
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x00013AFA File Offset: 0x00011EFA
		internal DeviceBindingSource()
		{
			this.Control = InputControlType.None;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00013B09 File Offset: 0x00011F09
		public DeviceBindingSource(InputControlType control)
		{
			this.Control = control;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00013B18 File Offset: 0x00011F18
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x00013B20 File Offset: 0x00011F20
		public InputControlType Control { get; protected set; }

		// Token: 0x060002B8 RID: 696 RVA: 0x00013B29 File Offset: 0x00011F29
		public override float GetValue(InputDevice inputDevice)
		{
			if (inputDevice == null)
			{
				return 0f;
			}
			return inputDevice.GetControl(this.Control).Value;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00013B48 File Offset: 0x00011F48
		public override bool GetState(InputDevice inputDevice)
		{
			return inputDevice != null && inputDevice.GetControl(this.Control).State;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00013B64 File Offset: 0x00011F64
		public override string Name
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				InputDevice device = base.BoundTo.Device;
				InputControl control = device.GetControl(this.Control);
				if (control == InputControl.Null)
				{
					return this.Control.ToString();
				}
				return device.GetControl(this.Control).Handle;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00013BCC File Offset: 0x00011FCC
		public override string DeviceName
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				InputDevice device = base.BoundTo.Device;
				if (device == InputDevice.Null)
				{
					return "Controller";
				}
				return device.Name;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00013C0D File Offset: 0x0001200D
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return (base.BoundTo != null) ? base.BoundTo.Device.DeviceClass : InputDeviceClass.Unknown;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00013C30 File Offset: 0x00012030
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return (base.BoundTo != null) ? base.BoundTo.Device.DeviceStyle : InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00013C54 File Offset: 0x00012054
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
			return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00013C94 File Offset: 0x00012094
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
			return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00013CCC File Offset: 0x000120CC
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00013CED File Offset: 0x000120ED
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.DeviceBindingSource;
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00013CF0 File Offset: 0x000120F0
		internal override void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00013CFE File Offset: 0x000120FE
		internal override void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			this.Control = (InputControlType)reader.ReadInt32();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00013D0C File Offset: 0x0001210C
		internal override bool IsValid
		{
			get
			{
				if (base.BoundTo == null)
				{
					Debug.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
					return false;
				}
				return base.BoundTo.Device.HasControl(this.Control) || Utility.TargetIsStandard(this.Control);
			}
		}
	}
}
