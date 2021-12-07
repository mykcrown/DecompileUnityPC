// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	public interface IRollbackLayerDebugger
	{
		RollbackMismatchReport MismatchReport
		{
			get;
		}

		IDebugRollbackLayer RollbackLayer
		{
			get;
		}

		RollbackStateContainer ActiveState
		{
			get;
		}

		bool ContainsMismatch
		{
			get;
		}

		List<RollbackState> MismatchedStates
		{
			get;
		}

		void Initialize(IRollbackClient client, RollbackDebugSettings settings);

		void LoadReplaySystem(IDebugReplaySystem replaySystem);

		void LoadRollbackLayer(IDebugRollbackLayer rollbackLayer);

		bool TestStates(int frame, out RollbackMismatchReport report);

		bool TestStates(RollbackStateContainer activeState, int frame, out RollbackMismatchReport report);
	}
}
