using System;
using System.Collections.Generic;
using UnityEngine;

namespace RollbackDebug
{
	// Token: 0x02000852 RID: 2130
	public class DummyRollbackStateContainer : RollbackStateContainer
	{
		// Token: 0x06003549 RID: 13641 RVA: 0x000FCBD8 File Offset: 0x000FAFD8
		public DummyRollbackStateContainer(DummyRollbackStateContainer.Mode mode) : base(true)
		{
			this.MyMode = mode;
			switch (mode)
			{
			default:
				base.WriteState(new DummyRollbackStateA(100, false, "C"));
				base.WriteState(new DummyRollbackStateB(new List<int>
				{
					1,
					2
				}, new string[]
				{
					"a",
					"b"
				}, null));
				base.WriteState(new DummyRollbackStateC(new Vector3(1f, 2f, 3f)));
				break;
			case DummyRollbackStateContainer.Mode.StateCountMismatch:
				base.WriteState(new DummyRollbackStateA(100, false, "C"));
				break;
			case DummyRollbackStateContainer.Mode.ValueTypeMismatch:
				base.WriteState(new DummyRollbackStateA(1, true, "C"));
				base.WriteState(new DummyRollbackStateB(new List<int>
				{
					1,
					2
				}, new string[]
				{
					"a",
					"b"
				}, null));
				base.WriteState(new DummyRollbackStateC(new Vector3(2f, 2f, 3f)));
				break;
			case DummyRollbackStateContainer.Mode.StructMismatch:
				base.WriteState(new DummyRollbackStateA(100, false, "C"));
				base.WriteState(new DummyRollbackStateB(new List<int>
				{
					1,
					2
				}, new string[]
				{
					"a",
					"b"
				}, null));
				base.WriteState(new DummyRollbackStateC(new Vector3(2f, 2f, 3f)));
				break;
			case DummyRollbackStateContainer.Mode.EnumerableMismatch:
				base.WriteState(new DummyRollbackStateA(100, false, "C"));
				base.WriteState(new DummyRollbackStateB(new List<int>
				{
					1
				}, new string[]
				{
					"c",
					"b"
				}, null));
				base.WriteState(new DummyRollbackStateC(new Vector3(1f, 2f, 3f)));
				break;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x0600354A RID: 13642 RVA: 0x000FCDE7 File Offset: 0x000FB1E7
		// (set) Token: 0x0600354B RID: 13643 RVA: 0x000FCDEF File Offset: 0x000FB1EF
		public DummyRollbackStateContainer.Mode MyMode { get; private set; }

		// Token: 0x02000853 RID: 2131
		public enum Mode
		{
			// Token: 0x040024A0 RID: 9376
			Base,
			// Token: 0x040024A1 RID: 9377
			StateCountMismatch,
			// Token: 0x040024A2 RID: 9378
			ValueTypeMismatch,
			// Token: 0x040024A3 RID: 9379
			StructMismatch,
			// Token: 0x040024A4 RID: 9380
			EnumerableMismatch,
			// Token: 0x040024A5 RID: 9381
			NestedObjectMismatch
		}
	}
}
