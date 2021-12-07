using System;
using strange.extensions.reflector.api;

namespace strange.extensions.reflector.impl
{
	// Token: 0x02000272 RID: 626
	public class ReflectionException : Exception
	{
		// Token: 0x06000CC5 RID: 3269 RVA: 0x000563F8 File Offset: 0x000547F8
		public ReflectionException()
		{
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00056400 File Offset: 0x00054800
		public ReflectionException(string message, ReflectionExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00056410 File Offset: 0x00054810
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x00056418 File Offset: 0x00054818
		public ReflectionExceptionType type { get; set; }
	}
}
