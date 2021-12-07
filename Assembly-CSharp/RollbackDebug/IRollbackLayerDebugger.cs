using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	// Token: 0x0200085C RID: 2140
	public interface IRollbackLayerDebugger
	{
		// Token: 0x06003568 RID: 13672
		void Initialize(IRollbackClient client, RollbackDebugSettings settings);

		// Token: 0x06003569 RID: 13673
		void LoadReplaySystem(IDebugReplaySystem replaySystem);

		// Token: 0x0600356A RID: 13674
		void LoadRollbackLayer(IDebugRollbackLayer rollbackLayer);

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x0600356B RID: 13675
		RollbackMismatchReport MismatchReport { get; }

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x0600356C RID: 13676
		IDebugRollbackLayer RollbackLayer { get; }

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x0600356D RID: 13677
		RollbackStateContainer ActiveState { get; }

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x0600356E RID: 13678
		bool ContainsMismatch { get; }

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x0600356F RID: 13679
		List<RollbackState> MismatchedStates { get; }

		// Token: 0x06003570 RID: 13680
		bool TestStates(int frame, out RollbackMismatchReport report);

		// Token: 0x06003571 RID: 13681
		bool TestStates(RollbackStateContainer activeState, int frame, out RollbackMismatchReport report);
	}
}
