using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	// Token: 0x0200085A RID: 2138
	public class RollbackMismatchReport
	{
		// Token: 0x06003567 RID: 13671 RVA: 0x000FD4B0 File Offset: 0x000FB8B0
		public void Clear()
		{
			this.hashTest = RollbackMismatchReport.ReportStatus.None;
			this.stateTest = RollbackMismatchReport.ReportStatus.None;
			this.errors.Clear();
			this.mismatchedFieldHashCodes.Clear();
			this.mismatchedStateIndices.Clear();
			this.activeState = null;
			this.replayState = null;
			this.frame = 0;
		}

		// Token: 0x040024B8 RID: 9400
		public int frame;

		// Token: 0x040024B9 RID: 9401
		public RollbackMismatchReport.ReportStatus hashTest;

		// Token: 0x040024BA RID: 9402
		public RollbackMismatchReport.ReportStatus stateTest;

		// Token: 0x040024BB RID: 9403
		public List<string> errors = new List<string>();

		// Token: 0x040024BC RID: 9404
		public RollbackStateContainer activeState;

		// Token: 0x040024BD RID: 9405
		public RollbackStateContainer replayState;

		// Token: 0x040024BE RID: 9406
		public HashSet<int> mismatchedStateIndices = new HashSet<int>();

		// Token: 0x040024BF RID: 9407
		public HashSet<int> mismatchedFieldHashCodes = new HashSet<int>();

		// Token: 0x0200085B RID: 2139
		public enum ReportStatus
		{
			// Token: 0x040024C1 RID: 9409
			None,
			// Token: 0x040024C2 RID: 9410
			Match,
			// Token: 0x040024C3 RID: 9411
			Mismatch
		}
	}
}
