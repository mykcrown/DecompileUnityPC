using System;

namespace AI
{
	// Token: 0x020002FE RID: 766
	public interface IAIManager
	{
		// Token: 0x060010C9 RID: 4297
		bool IsAnyAIActive();

		// Token: 0x060010CA RID: 4298
		bool IsAnyPassiveAIActive();

		// Token: 0x060010CB RID: 4299
		CompositeNodeData GetPassiveRootNode();

		// Token: 0x060010CC RID: 4300
		CompositeNodeData GetDefaultRootNode();
	}
}
