// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	public class DummyRollbackLayerDebugger : IRollbackLayerDebugger
	{
		private RollbackMismatchReport mismatchReport = new RollbackMismatchReport();

		RollbackMismatchReport IRollbackLayerDebugger.MismatchReport
		{
			get
			{
				return this.mismatchReport;
			}
		}

		IDebugRollbackLayer IRollbackLayerDebugger.RollbackLayer
		{
			get
			{
				return null;
			}
		}

		RollbackStateContainer IRollbackLayerDebugger.ActiveState
		{
			get
			{
				return null;
			}
		}

		bool IRollbackLayerDebugger.ContainsMismatch
		{
			get
			{
				return false;
			}
		}

		List<RollbackState> IRollbackLayerDebugger.MismatchedStates
		{
			get
			{
				return null;
			}
		}

		void IRollbackLayerDebugger.Initialize(IRollbackClient client, RollbackDebugSettings settings)
		{
		}

		void IRollbackLayerDebugger.LoadReplaySystem(IDebugReplaySystem replaySystem)
		{
		}

		void IRollbackLayerDebugger.LoadRollbackLayer(IDebugRollbackLayer rollbackLayer)
		{
		}

		bool IRollbackLayerDebugger.TestStates(int frame, out RollbackMismatchReport report)
		{
			report = null;
			return true;
		}

		bool IRollbackLayerDebugger.TestStates(RollbackStateContainer activeState, int frame, out RollbackMismatchReport report)
		{
			report = null;
			return true;
		}
	}
}
