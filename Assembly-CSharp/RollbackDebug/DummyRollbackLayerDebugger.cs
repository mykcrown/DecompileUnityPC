using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	// Token: 0x0200085D RID: 2141
	public class DummyRollbackLayerDebugger : IRollbackLayerDebugger
	{
		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003573 RID: 13683 RVA: 0x000FD514 File Offset: 0x000FB914
		RollbackMismatchReport IRollbackLayerDebugger.MismatchReport
		{
			get
			{
				return this.mismatchReport;
			}
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000FD51C File Offset: 0x000FB91C
		void IRollbackLayerDebugger.Initialize(IRollbackClient client, RollbackDebugSettings settings)
		{
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000FD51E File Offset: 0x000FB91E
		void IRollbackLayerDebugger.LoadReplaySystem(IDebugReplaySystem replaySystem)
		{
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000FD520 File Offset: 0x000FB920
		void IRollbackLayerDebugger.LoadRollbackLayer(IDebugRollbackLayer rollbackLayer)
		{
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x000FD522 File Offset: 0x000FB922
		IDebugRollbackLayer IRollbackLayerDebugger.RollbackLayer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x000FD525 File Offset: 0x000FB925
		RollbackStateContainer IRollbackLayerDebugger.ActiveState
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003579 RID: 13689 RVA: 0x000FD528 File Offset: 0x000FB928
		bool IRollbackLayerDebugger.ContainsMismatch
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x0600357A RID: 13690 RVA: 0x000FD52B File Offset: 0x000FB92B
		List<RollbackState> IRollbackLayerDebugger.MismatchedStates
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000FD52E File Offset: 0x000FB92E
		bool IRollbackLayerDebugger.TestStates(int frame, out RollbackMismatchReport report)
		{
			report = null;
			return true;
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000FD534 File Offset: 0x000FB934
		bool IRollbackLayerDebugger.TestStates(RollbackStateContainer activeState, int frame, out RollbackMismatchReport report)
		{
			report = null;
			return true;
		}

		// Token: 0x040024C4 RID: 9412
		private RollbackMismatchReport mismatchReport = new RollbackMismatchReport();
	}
}
