using System;
using System.Collections.Generic;
using MemberwiseEquality;
using UnityEngine;

namespace RollbackDebug
{
	// Token: 0x02000859 RID: 2137
	public class RollbackLayerDebugger : IRollbackLayerDebugger
	{
		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x0600355A RID: 13658 RVA: 0x000FD0B5 File Offset: 0x000FB4B5
		// (set) Token: 0x0600355B RID: 13659 RVA: 0x000FD0BD File Offset: 0x000FB4BD
		public IDebugRollbackLayer RollbackLayer { get; private set; }

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x0600355C RID: 13660 RVA: 0x000FD0C6 File Offset: 0x000FB4C6
		public RollbackMismatchReport MismatchReport
		{
			get
			{
				return this.mismatchReport;
			}
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000FD0CE File Offset: 0x000FB4CE
		public void Initialize(IRollbackClient client, RollbackDebugSettings settings)
		{
			this.client = client;
			this.settings = settings;
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000FD0DE File Offset: 0x000FB4DE
		public void LoadReplaySystem(IDebugReplaySystem replaySystem)
		{
			this.replaySystem = replaySystem;
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000FD0E7 File Offset: 0x000FB4E7
		public void LoadRollbackLayer(IDebugRollbackLayer rollbackLayer)
		{
			this.RollbackLayer = rollbackLayer;
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06003560 RID: 13664 RVA: 0x000FD0F0 File Offset: 0x000FB4F0
		public RollbackStateContainer ActiveState
		{
			get
			{
				return this.RollbackLayer.GetBufferedState(this.RollbackLayer.ActiveRollbackFrame);
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06003561 RID: 13665 RVA: 0x000FD108 File Offset: 0x000FB508
		public bool ContainsMismatch
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06003562 RID: 13666 RVA: 0x000FD10B File Offset: 0x000FB50B
		public List<RollbackState> MismatchedStates
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000FD110 File Offset: 0x000FB510
		public bool TestStates(int frame, out RollbackMismatchReport mismatchReport)
		{
			RollbackStateContainer bufferedState = this.RollbackLayer.GetBufferedState(this.RollbackLayer.ActiveRollbackFrame);
			return this.TestStates(bufferedState, frame, out mismatchReport);
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000FD140 File Offset: 0x000FB540
		public bool TestStates(RollbackStateContainer activeState, int frame, out RollbackMismatchReport outReport)
		{
			outReport = this.mismatchReport;
			this.mismatchReport.Clear();
			this.mismatchReport.frame = frame;
			string empty = string.Empty;
			List<string> errors = this.mismatchReport.errors;
			this.mismatchReport.activeState = activeState;
			bool flag = true;
			if (this.replaySystem.RecordStates)
			{
				RollbackStateContainer stateAtFrameEnd = this.replaySystem.GetStateAtFrameEnd(frame - 1);
				this.mismatchReport.replayState = stateAtFrameEnd;
				bool flag2 = this.compareContainers(activeState, stateAtFrameEnd, this.mismatchReport);
				flag = (flag && flag2);
				if (!flag2)
				{
					errors.Add(empty);
					empty = string.Empty;
					this.mismatchReport.stateTest = RollbackMismatchReport.ReportStatus.Mismatch;
				}
				else
				{
					this.mismatchReport.stateTest = RollbackMismatchReport.ReportStatus.Match;
				}
			}
			if (this.replaySystem.RecordHashes)
			{
				short hashAtFrameEnd = this.replaySystem.GetHashAtFrameEnd(frame - 1);
				short memberwiseHashCode = activeState.GetMemberwiseHashCode();
				bool flag3 = hashAtFrameEnd == memberwiseHashCode;
				flag = (flag && flag3);
				if (!flag3)
				{
					if (this.replaySystem.RecordStates)
					{
						activeState.TestHash(this.replaySystem.GetStateAtFrameEnd(frame - 1), ref empty, true);
					}
					errors.Add(string.Concat(new object[]
					{
						"Hash mismatch (debug) on frame ",
						frame,
						": ",
						hashAtFrameEnd,
						" != ",
						memberwiseHashCode,
						"\n",
						empty
					}));
					this.mismatchReport.hashTest = RollbackMismatchReport.ReportStatus.Mismatch;
				}
				else
				{
					this.mismatchReport.hashTest = RollbackMismatchReport.ReportStatus.Match;
				}
				if (flag3 && !flag)
				{
					errors.Add("Detected an equality error but no hash error. This can occur when the same mismatched value is in the hash twice, or if a struct doesn't override ==");
				}
			}
			if (!flag)
			{
				Debug.LogError("Detected rollback error on frame " + frame);
				foreach (string message in errors)
				{
					Debug.LogError(message);
				}
				if (this.settings.debugPauseOnMismatch)
				{
					this.client.Halt();
				}
			}
			return flag;
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000FD36C File Offset: 0x000FB76C
		private bool compareContainers(RollbackStateContainer activeContainer, RollbackStateContainer replayContainer, RollbackMismatchReport report)
		{
			if (replayContainer == null)
			{
				return true;
			}
			List<string> errors = this.mismatchReport.errors;
			if (activeContainer.Count != replayContainer.Count)
			{
				errors.Add(string.Concat(new object[]
				{
					"Length mismatch: ",
					activeContainer.Count,
					"(active) != ",
					replayContainer.Count,
					"(replay)"
				}));
				return false;
			}
			bool flag = false;
			for (int i = 0; i < activeContainer.Count; i++)
			{
				RollbackState state = activeContainer.GetState(i);
				RollbackState state2 = replayContainer.GetState(i);
				if (!state.MemberwiseEquals(state2))
				{
					string item = this.ToString() + " mismatch:\n";
					if (!ReflectionEqualityProcessor.CheckReflectionEquality(state, state2, ref item, this.mismatchReport.mismatchedFieldHashCodes))
					{
						errors.Add(item);
					}
					else
					{
						errors.Add("Failed to detect issues in CheckReflectionEquality in " + state.GetType());
					}
					flag = true;
					this.mismatchReport.mismatchedStateIndices.Add(i);
				}
			}
			return !flag;
		}

		// Token: 0x040024B3 RID: 9395
		private IRollbackClient client;

		// Token: 0x040024B4 RID: 9396
		private IDebugReplaySystem replaySystem;

		// Token: 0x040024B6 RID: 9398
		private RollbackDebugSettings settings;

		// Token: 0x040024B7 RID: 9399
		private RollbackMismatchReport mismatchReport = new RollbackMismatchReport();
	}
}
