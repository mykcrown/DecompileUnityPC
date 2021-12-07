using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200006B RID: 107
	public class InputControlMapping
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00018804 File Offset: 0x00016C04
		public float MapValue(float value)
		{
			if (this.Raw)
			{
				value *= this.Scale;
				value = ((!this.SourceRange.Excludes(value)) ? value : 0f);
			}
			else
			{
				value = Mathf.Clamp(value * this.Scale, -1f, 1f);
				value = InputRange.Remap(value, this.SourceRange, this.TargetRange);
			}
			if (this.Invert)
			{
				value = -value;
			}
			return value;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00018886 File Offset: 0x00016C86
		// (set) Token: 0x060003CB RID: 971 RVA: 0x000188B4 File Offset: 0x00016CB4
		public string Handle
		{
			get
			{
				return (!string.IsNullOrEmpty(this.handle)) ? this.handle : this.Target.ToString();
			}
			set
			{
				this.handle = value;
			}
		}

		// Token: 0x040002F6 RID: 758
		public InputControlSource Source;

		// Token: 0x040002F7 RID: 759
		public InputControlType Target;

		// Token: 0x040002F8 RID: 760
		public bool Invert;

		// Token: 0x040002F9 RID: 761
		public float Scale = 1f;

		// Token: 0x040002FA RID: 762
		public bool Raw;

		// Token: 0x040002FB RID: 763
		public bool Passive;

		// Token: 0x040002FC RID: 764
		public bool IgnoreInitialZeroValue;

		// Token: 0x040002FD RID: 765
		public float Sensitivity = 1f;

		// Token: 0x040002FE RID: 766
		public float LowerDeadZone;

		// Token: 0x040002FF RID: 767
		public float UpperDeadZone = 1f;

		// Token: 0x04000300 RID: 768
		public InputRange SourceRange = InputRange.MinusOneToOne;

		// Token: 0x04000301 RID: 769
		public InputRange TargetRange = InputRange.MinusOneToOne;

		// Token: 0x04000302 RID: 770
		private string handle;
	}
}
