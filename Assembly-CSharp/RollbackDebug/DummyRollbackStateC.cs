using System;
using UnityEngine;

namespace RollbackDebug
{
	// Token: 0x02000856 RID: 2134
	[Serializable]
	internal class DummyRollbackStateC : RollbackState
	{
		// Token: 0x06003551 RID: 13649 RVA: 0x000FCE9B File Offset: 0x000FB29B
		public DummyRollbackStateC()
		{
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x000FCEA3 File Offset: 0x000FB2A3
		public DummyRollbackStateC(Vector3 v)
		{
			this.vector = v;
		}

		// Token: 0x040024AC RID: 9388
		public Vector3 vector;
	}
}
