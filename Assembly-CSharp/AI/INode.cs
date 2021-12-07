using System;
using System.Collections.Generic;

namespace AI
{
	// Token: 0x0200032A RID: 810
	public interface INode
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600116B RID: 4459
		// (set) Token: 0x0600116C RID: 4460
		List<INode> children { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600116D RID: 4461
		// (set) Token: 0x0600116E RID: 4462
		int shuffleWeight { get; set; }

		// Token: 0x0600116F RID: 4463
		void Init(BehaviorTree context);

		// Token: 0x06001170 RID: 4464
		NodeResult TickFrame();
	}
}
