using System;
using strange.extensions.pool.api;

namespace strange.extensions.pool.impl
{
	// Token: 0x0200026B RID: 619
	public class PoolException : Exception
	{
		// Token: 0x06000C7D RID: 3197 RVA: 0x00055E2A File Offset: 0x0005422A
		public PoolException()
		{
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00055E32 File Offset: 0x00054232
		public PoolException(string message, PoolExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00055E42 File Offset: 0x00054242
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x00055E4A File Offset: 0x0005424A
		public PoolExceptionType type { get; set; }
	}
}
