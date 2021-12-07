using System;

namespace SharedCore
{
	// Token: 0x02000B81 RID: 2945
	[Serializable]
	public class WeightedValue<T>
	{
		// Token: 0x060054F4 RID: 21748 RVA: 0x001B421A File Offset: 0x001B261A
		public WeightedValue(T self, int weight = 1)
		{
			this.Value = self;
			this.Weight = weight;
		}

		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x060054F5 RID: 21749 RVA: 0x001B4230 File Offset: 0x001B2630
		// (set) Token: 0x060054F6 RID: 21750 RVA: 0x001B4238 File Offset: 0x001B2638
		public T Value { get; set; }

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x060054F7 RID: 21751 RVA: 0x001B4241 File Offset: 0x001B2641
		// (set) Token: 0x060054F8 RID: 21752 RVA: 0x001B4249 File Offset: 0x001B2649
		public int Weight { get; set; }
	}
}
