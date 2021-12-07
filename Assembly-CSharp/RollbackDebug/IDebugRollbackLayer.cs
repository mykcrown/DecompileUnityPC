using System;

namespace RollbackDebug
{
	// Token: 0x0200084F RID: 2127
	public interface IDebugRollbackLayer
	{
		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06003533 RID: 13619
		int RollbackBufferSize { get; }

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06003534 RID: 13620
		bool IsRollingBack { get; }

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06003535 RID: 13621
		int CurrentGameFrame { get; }

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06003536 RID: 13622
		int ActiveRollbackFrame { get; }

		// Token: 0x06003537 RID: 13623
		RollbackStateContainer GetBufferedState(int frame);

		// Token: 0x06003538 RID: 13624
		void AdvanceFrame();

		// Token: 0x06003539 RID: 13625
		bool Rollback(int amount);
	}
}
