using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	// Token: 0x02000851 RID: 2129
	public class DummyReplaySystem : IDebugReplaySystem
	{
		// Token: 0x06003544 RID: 13636 RVA: 0x000FC080 File Offset: 0x000FA480
		public DummyReplaySystem(DummyDebugRollbackLayer source)
		{
			foreach (int num in source.Buffer.Keys)
			{
				this.states.Add(num - 1, new DummyRollbackStateContainer(source.Buffer[num].MyMode));
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003545 RID: 13637 RVA: 0x000FC110 File Offset: 0x000FA510
		bool IDebugReplaySystem.RecordStates
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003546 RID: 13638 RVA: 0x000FC113 File Offset: 0x000FA513
		bool IDebugReplaySystem.RecordHashes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000FC116 File Offset: 0x000FA516
		RollbackStateContainer IDebugReplaySystem.GetStateAtFrameEnd(int frame)
		{
			if (this.states.ContainsKey(frame))
			{
				return this.states[frame];
			}
			return null;
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000FC137 File Offset: 0x000FA537
		short IDebugReplaySystem.GetHashAtFrameEnd(int frame)
		{
			return 0;
		}

		// Token: 0x0400249D RID: 9373
		private Dictionary<int, RollbackStateContainer> states = new Dictionary<int, RollbackStateContainer>();
	}
}
