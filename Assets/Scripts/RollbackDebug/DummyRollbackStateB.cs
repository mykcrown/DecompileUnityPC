// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	[Serializable]
	internal class DummyRollbackStateB : RollbackState
	{
		[IsClonedManually]
		public List<int> intList;

		[IsClonedManually]
		public string[] stringArray;

		[IsClonedManually]
		public DummyRollbackStateA nestedState;

		public DummyRollbackStateB()
		{
		}

		public DummyRollbackStateB(List<int> a, string[] b, DummyRollbackStateA c)
		{
			this.intList = a;
			this.stringArray = b;
			this.nestedState = c;
		}

		public override object Clone()
		{
			DummyRollbackStateB dummyRollbackStateB = base.Clone() as DummyRollbackStateB;
			dummyRollbackStateB.intList = new List<int>(this.intList);
			dummyRollbackStateB.stringArray = (this.stringArray.Clone() as string[]);
			dummyRollbackStateB.nestedState = (this.nestedState.Clone() as DummyRollbackStateA);
			return dummyRollbackStateB;
		}
	}
}
