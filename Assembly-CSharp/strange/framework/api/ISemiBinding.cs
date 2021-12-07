using System;

namespace strange.framework.api
{
	// Token: 0x0200028D RID: 653
	public interface ISemiBinding : IManagedList
	{
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000D8A RID: 3466
		// (set) Token: 0x06000D8B RID: 3467
		Enum constraint { get; set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000D8C RID: 3468
		// (set) Token: 0x06000D8D RID: 3469
		bool uniqueValues { get; set; }
	}
}
