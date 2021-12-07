// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	public class DummyReplaySystem : IDebugReplaySystem
	{
		private Dictionary<int, RollbackStateContainer> states = new Dictionary<int, RollbackStateContainer>();

		bool IDebugReplaySystem.RecordStates
		{
			get
			{
				return true;
			}
		}

		bool IDebugReplaySystem.RecordHashes
		{
			get
			{
				return false;
			}
		}

		public DummyReplaySystem(DummyDebugRollbackLayer source)
		{
			foreach (int current in source.Buffer.Keys)
			{
				this.states.Add(current - 1, new DummyRollbackStateContainer(source.Buffer[current].MyMode));
			}
		}

		RollbackStateContainer IDebugReplaySystem.GetStateAtFrameEnd(int frame)
		{
			if (this.states.ContainsKey(frame))
			{
				return this.states[frame];
			}
			return null;
		}

		short IDebugReplaySystem.GetHashAtFrameEnd(int frame)
		{
			return 0;
		}
	}
}
