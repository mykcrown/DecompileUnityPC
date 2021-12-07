using System;
using System.IO;

namespace InControl
{
	// Token: 0x02000052 RID: 82
	public abstract class BindingSource : InputControlSource, IEquatable<BindingSource>
	{
		// Token: 0x060002A1 RID: 673
		public abstract float GetValue(InputDevice inputDevice);

		// Token: 0x060002A2 RID: 674
		public abstract bool GetState(InputDevice inputDevice);

		// Token: 0x060002A3 RID: 675
		public abstract bool Equals(BindingSource other);

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060002A4 RID: 676
		public abstract string Name { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060002A5 RID: 677
		public abstract string DeviceName { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060002A6 RID: 678
		public abstract InputDeviceClass DeviceClass { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060002A7 RID: 679
		public abstract InputDeviceStyle DeviceStyle { get; }

		// Token: 0x060002A8 RID: 680 RVA: 0x00013A8C File Offset: 0x00011E8C
		public static bool operator ==(BindingSource a, BindingSource b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.BindingSourceType == b.BindingSourceType && a.Equals(b));
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00013AC4 File Offset: 0x00011EC4
		public static bool operator !=(BindingSource a, BindingSource b)
		{
			return !(a == b);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00013AD0 File Offset: 0x00011ED0
		public override bool Equals(object obj)
		{
			return this.Equals((BindingSource)obj);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00013ADE File Offset: 0x00011EDE
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060002AC RID: 684
		public abstract BindingSourceType BindingSourceType { get; }

		// Token: 0x060002AD RID: 685
		internal abstract void Save(BinaryWriter writer);

		// Token: 0x060002AE RID: 686
		internal abstract void Load(BinaryReader reader, ushort dataFormatVersion);

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00013AE6 File Offset: 0x00011EE6
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x00013AEE File Offset: 0x00011EEE
		internal PlayerAction BoundTo { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00013AF7 File Offset: 0x00011EF7
		internal virtual bool IsValid
		{
			get
			{
				return true;
			}
		}
	}
}
