using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000064 RID: 100
	public class UnknownDeviceBindingSource : BindingSource
	{
		// Token: 0x06000386 RID: 902 RVA: 0x00017A0E File Offset: 0x00015E0E
		internal UnknownDeviceBindingSource()
		{
			this.Control = UnknownDeviceControl.None;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00017A21 File Offset: 0x00015E21
		public UnknownDeviceBindingSource(UnknownDeviceControl control)
		{
			this.Control = control;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00017A30 File Offset: 0x00015E30
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00017A38 File Offset: 0x00015E38
		public UnknownDeviceControl Control { get; protected set; }

		// Token: 0x0600038A RID: 906 RVA: 0x00017A44 File Offset: 0x00015E44
		public override float GetValue(InputDevice device)
		{
			return this.Control.GetValue(device);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00017A60 File Offset: 0x00015E60
		public override bool GetState(InputDevice device)
		{
			return device != null && Utility.IsNotZero(this.GetValue(device));
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00017A78 File Offset: 0x00015E78
		public override string Name
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				string str = string.Empty;
				if (this.Control.SourceRange == InputRangeType.ZeroToMinusOne)
				{
					str = "Negative ";
				}
				else if (this.Control.SourceRange == InputRangeType.ZeroToOne)
				{
					str = "Positive ";
				}
				InputDevice device = base.BoundTo.Device;
				if (device == InputDevice.Null)
				{
					return str + this.Control.Control.ToString();
				}
				InputControl control = device.GetControl(this.Control.Control);
				if (control == InputControl.Null)
				{
					return str + this.Control.Control.ToString();
				}
				return str + control.Handle;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00017B60 File Offset: 0x00015F60
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
					return "Unknown Controller";
				}
				return device.Name;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00017BA1 File Offset: 0x00015FA1
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Controller;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00017BA4 File Offset: 0x00015FA4
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00017BA8 File Offset: 0x00015FA8
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
			return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00017BEC File Offset: 0x00015FEC
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
			return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00017C28 File Offset: 0x00016028
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00017C49 File Offset: 0x00016049
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.UnknownDeviceBindingSource;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00017C4C File Offset: 0x0001604C
		internal override bool IsValid
		{
			get
			{
				if (base.BoundTo == null)
				{
					Debug.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
					return false;
				}
				InputDevice device = base.BoundTo.Device;
				return device == InputDevice.Null || device.HasControl(this.Control.Control);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00017CA0 File Offset: 0x000160A0
		internal override void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			UnknownDeviceControl control = default(UnknownDeviceControl);
			control.Load(reader);
			this.Control = control;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00017CC4 File Offset: 0x000160C4
		internal override void Save(BinaryWriter writer)
		{
			this.Control.Save(writer);
		}
	}
}
