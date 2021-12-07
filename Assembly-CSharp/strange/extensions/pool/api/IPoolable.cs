using System;

namespace strange.extensions.pool.api
{
	// Token: 0x02000265 RID: 613
	public interface IPoolable
	{
		// Token: 0x06000C57 RID: 3159
		void Restore();

		// Token: 0x06000C58 RID: 3160
		void Retain();

		// Token: 0x06000C59 RID: 3161
		void Release();

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000C5A RID: 3162
		bool retain { get; }
	}
}
