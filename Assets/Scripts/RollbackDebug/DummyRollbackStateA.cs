// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace RollbackDebug
{
	[Serializable]
	internal class DummyRollbackStateA : RollbackState
	{
		public int intVal;

		public bool boolVal;

		public string stringVal;

		public DummyRollbackStateA()
		{
		}

		public DummyRollbackStateA(int a, bool b, string c)
		{
			this.intVal = a;
			this.boolVal = b;
			this.stringVal = c;
		}
	}
}
