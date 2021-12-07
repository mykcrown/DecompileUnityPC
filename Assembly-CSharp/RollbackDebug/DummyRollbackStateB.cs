using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	// Token: 0x02000855 RID: 2133
	[Serializable]
	internal class DummyRollbackStateB : RollbackState
	{
		// Token: 0x0600354E RID: 13646 RVA: 0x000FCE1D File Offset: 0x000FB21D
		public DummyRollbackStateB()
		{
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x000FCE25 File Offset: 0x000FB225
		public DummyRollbackStateB(List<int> a, string[] b, DummyRollbackStateA c)
		{
			this.intList = a;
			this.stringArray = b;
			this.nestedState = c;
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x000FCE44 File Offset: 0x000FB244
		public override object Clone()
		{
			DummyRollbackStateB dummyRollbackStateB = base.Clone() as DummyRollbackStateB;
			dummyRollbackStateB.intList = new List<int>(this.intList);
			dummyRollbackStateB.stringArray = (this.stringArray.Clone() as string[]);
			dummyRollbackStateB.nestedState = (this.nestedState.Clone() as DummyRollbackStateA);
			return dummyRollbackStateB;
		}

		// Token: 0x040024A9 RID: 9385
		[IsClonedManually]
		public List<int> intList;

		// Token: 0x040024AA RID: 9386
		[IsClonedManually]
		public string[] stringArray;

		// Token: 0x040024AB RID: 9387
		[IsClonedManually]
		public DummyRollbackStateA nestedState;
	}
}
