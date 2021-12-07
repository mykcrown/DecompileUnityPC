// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	public class RollbackMismatchReport
	{
		public enum ReportStatus
		{
			None,
			Match,
			Mismatch
		}

		public int frame;

		public RollbackMismatchReport.ReportStatus hashTest;

		public RollbackMismatchReport.ReportStatus stateTest;

		public List<string> errors = new List<string>();

		public RollbackStateContainer activeState;

		public RollbackStateContainer replayState;

		public HashSet<int> mismatchedStateIndices = new HashSet<int>();

		public HashSet<int> mismatchedFieldHashCodes = new HashSet<int>();

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
	}
}
