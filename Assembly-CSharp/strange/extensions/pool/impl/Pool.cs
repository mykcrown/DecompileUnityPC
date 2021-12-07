using System;
using strange.extensions.pool.api;
using strange.framework.api;

namespace strange.extensions.pool.impl
{
	// Token: 0x02000269 RID: 617
	public class Pool<T> : Pool, IPool<T>, IPool, IManagedList
	{
		// Token: 0x06000C5B RID: 3163 RVA: 0x00055E05 File Offset: 0x00054205
		public Pool()
		{
			base.poolType = typeof(T);
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00055E1D File Offset: 0x0005421D
		public new T GetInstance()
		{
			return (T)((object)base.GetInstance());
		}
	}
}
