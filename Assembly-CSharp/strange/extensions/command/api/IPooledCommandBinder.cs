using System;
using strange.extensions.pool.impl;

namespace strange.extensions.command.api
{
	// Token: 0x02000216 RID: 534
	public interface IPooledCommandBinder
	{
		// Token: 0x06000A2F RID: 2607
		Pool<T> GetPool<T>();

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000A30 RID: 2608
		// (set) Token: 0x06000A31 RID: 2609
		bool usePooling { get; set; }
	}
}
