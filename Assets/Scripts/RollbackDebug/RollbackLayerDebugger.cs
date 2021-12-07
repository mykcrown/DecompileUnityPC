// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RollbackDebug
{
	public class RollbackLayerDebugger : IRollbackLayerDebugger
	{
		private IRollbackClient client;

		private IDebugReplaySystem replaySystem;

		private RollbackDebugSettings settings;

		private RollbackMismatchReport mismatchReport = new RollbackMismatchReport();

		public IDebugRollbackLayer RollbackLayer
		{
			get;
			private set;
		}

		public RollbackMismatchReport MismatchReport
		{
			get
			{
				return this.mismatchReport;
			}
		}

		public RollbackStateContainer ActiveState
		{
			get
			{
				return this.RollbackLayer.GetBufferedState(this.RollbackLayer.ActiveRollbackFrame);
			}
		}

		public bool ContainsMismatch
		{
			get
			{
				return false;
			}
		}

		public List<RollbackState> MismatchedStates
		{
			get
			{
				return null;
			}
		}

		public void Initialize(IRollbackClient client, RollbackDebugSettings settings)
		{
			this.client = client;
			this.settings = settings;
		}

		public void LoadReplaySystem(IDebugReplaySystem replaySystem)
		{
			this.replaySystem = replaySystem;
		}

		public void LoadRollbackLayer(IDebugRollbackLayer rollbackLayer)
		{
			this.RollbackLayer = rollbackLayer;
		}

		public bool TestStates(int frame, out RollbackMismatchReport mismatchReport)
		{
			RollbackStateContainer bufferedState = this.RollbackLayer.GetBufferedState(this.RollbackLayer.ActiveRollbackFrame);
			return this.TestStates(bufferedState, frame, out mismatchReport);
		}

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
				flag &= flag2;
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
				flag &= flag3;
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
				UnityEngine.Debug.LogError("Detected rollback error on frame " + frame);
				foreach (string current in errors)
				{
					UnityEngine.Debug.LogError(current);
				}
				if (this.settings.debugPauseOnMismatch)
				{
					this.client.Halt();
				}
			}
			return flag;
		}

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
	}
}
