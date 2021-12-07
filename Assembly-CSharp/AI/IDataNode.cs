using System;

namespace AI
{
	// Token: 0x02000331 RID: 817
	public interface IDataNode
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001181 RID: 4481
		FloatDataDictionary FloatData { get; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06001182 RID: 4482
		IntDataDictionary IntData { get; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001183 RID: 4483
		FixedDataDictionary FixedData { get; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001184 RID: 4484
		BoolDataDictionary BoolData { get; }
	}
}
