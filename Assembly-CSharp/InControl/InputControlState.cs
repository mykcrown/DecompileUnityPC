using System;

namespace InControl
{
	// Token: 0x0200006D RID: 109
	public struct InputControlState
	{
		// Token: 0x060003CE RID: 974 RVA: 0x000188BD File Offset: 0x00016CBD
		public void Reset()
		{
			this.State = false;
			this.Value = 0f;
			this.RawValue = 0f;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000188DC File Offset: 0x00016CDC
		public void Set(float value)
		{
			this.Value = value;
			this.State = Utility.IsNotZero(value);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000188F1 File Offset: 0x00016CF1
		public void Set(float value, float threshold)
		{
			this.Value = value;
			this.State = Utility.AbsoluteIsOverThreshold(value, threshold);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00018907 File Offset: 0x00016D07
		public void Set(bool state)
		{
			this.State = state;
			this.Value = ((!state) ? 0f : 1f);
			this.RawValue = this.Value;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00018937 File Offset: 0x00016D37
		public static implicit operator bool(InputControlState state)
		{
			return state.State;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00018940 File Offset: 0x00016D40
		public static implicit operator float(InputControlState state)
		{
			return state.Value;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00018949 File Offset: 0x00016D49
		public static bool operator ==(InputControlState a, InputControlState b)
		{
			return a.State == b.State && Utility.Approximately(a.Value, b.Value);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00018973 File Offset: 0x00016D73
		public static bool operator !=(InputControlState a, InputControlState b)
		{
			return a.State != b.State || !Utility.Approximately(a.Value, b.Value);
		}

		// Token: 0x04000303 RID: 771
		public bool State;

		// Token: 0x04000304 RID: 772
		public float Value;

		// Token: 0x04000305 RID: 773
		public float RawValue;
	}
}
