using System;
using strange.extensions.signal.api;

namespace strange.extensions.signal.impl
{
	// Token: 0x02000285 RID: 645
	public class SignalException : Exception
	{
		// Token: 0x06000D5A RID: 3418 RVA: 0x0005752B File Offset: 0x0005592B
		public SignalException()
		{
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00057533 File Offset: 0x00055933
		public SignalException(string message, SignalExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00057543 File Offset: 0x00055943
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x0005754B File Offset: 0x0005594B
		public SignalExceptionType type { get; set; }
	}
}
