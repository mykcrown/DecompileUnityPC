using System;

namespace RollbackDebug
{
	// Token: 0x02000854 RID: 2132
	[Serializable]
	internal class DummyRollbackStateA : RollbackState
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x000FCDF8 File Offset: 0x000FB1F8
		public DummyRollbackStateA()
		{
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x000FCE00 File Offset: 0x000FB200
		public DummyRollbackStateA(int a, bool b, string c)
		{
			this.intVal = a;
			this.boolVal = b;
			this.stringVal = c;
		}

		// Token: 0x040024A6 RID: 9382
		public int intVal;

		// Token: 0x040024A7 RID: 9383
		public bool boolVal;

		// Token: 0x040024A8 RID: 9384
		public string stringVal;
	}
}
