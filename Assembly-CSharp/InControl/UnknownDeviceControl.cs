using System;
using System.IO;

namespace InControl
{
	// Token: 0x02000067 RID: 103
	public struct UnknownDeviceControl : IEquatable<UnknownDeviceControl>
	{
		// Token: 0x0600039D RID: 925 RVA: 0x00017E65 File Offset: 0x00016265
		public UnknownDeviceControl(InputControlType control, InputRangeType sourceRange)
		{
			this.Control = control;
			this.SourceRange = sourceRange;
			this.IsButton = Utility.TargetIsButton(control);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00017E90 File Offset: 0x00016290
		internal float GetValue(InputDevice device)
		{
			if (device == null)
			{
				return 0f;
			}
			float value = device.GetControl(this.Control).Value;
			return InputRange.Remap(value, this.SourceRange, InputRangeType.ZeroToOne);
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00017EC8 File Offset: 0x000162C8
		public int Index
		{
			get
			{
				return this.Control - ((!this.IsButton) ? InputControlType.Analog0 : InputControlType.Button0);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00017EEB File Offset: 0x000162EB
		public static bool operator ==(UnknownDeviceControl a, UnknownDeviceControl b)
		{
			if (object.ReferenceEquals(null, a))
			{
				return object.ReferenceEquals(null, b);
			}
			return a.Equals(b);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00017F13 File Offset: 0x00016313
		public static bool operator !=(UnknownDeviceControl a, UnknownDeviceControl b)
		{
			return !(a == b);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00017F1F File Offset: 0x0001631F
		public bool Equals(UnknownDeviceControl other)
		{
			return this.Control == other.Control && this.SourceRange == other.SourceRange;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00017F45 File Offset: 0x00016345
		public override bool Equals(object other)
		{
			return this.Equals((UnknownDeviceControl)other);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00017F53 File Offset: 0x00016353
		public override int GetHashCode()
		{
			return this.Control.GetHashCode() ^ this.SourceRange.GetHashCode();
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00017F78 File Offset: 0x00016378
		public static implicit operator bool(UnknownDeviceControl control)
		{
			return control.Control != InputControlType.None;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00017F87 File Offset: 0x00016387
		public override string ToString()
		{
			return string.Format("UnknownDeviceControl( {0}, {1} )", this.Control.ToString(), this.SourceRange.ToString());
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00017FB5 File Offset: 0x000163B5
		internal void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
			writer.Write((int)this.SourceRange);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00017FCF File Offset: 0x000163CF
		internal void Load(BinaryReader reader)
		{
			this.Control = (InputControlType)reader.ReadInt32();
			this.SourceRange = (InputRangeType)reader.ReadInt32();
			this.IsButton = Utility.TargetIsButton(this.Control);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x040002D7 RID: 727
		public static readonly UnknownDeviceControl None = new UnknownDeviceControl(InputControlType.None, InputRangeType.None);

		// Token: 0x040002D8 RID: 728
		public InputControlType Control;

		// Token: 0x040002D9 RID: 729
		public InputRangeType SourceRange;

		// Token: 0x040002DA RID: 730
		public bool IsButton;

		// Token: 0x040002DB RID: 731
		public bool IsAnalog;
	}
}
