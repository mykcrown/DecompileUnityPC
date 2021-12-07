using System;
using strange.framework.api;

namespace strange.extensions.pool.api
{
	// Token: 0x02000263 RID: 611
	public interface IPool<T> : IPool, IManagedList
	{
		// Token: 0x06000C47 RID: 3143
		T GetInstance();
	}
}
