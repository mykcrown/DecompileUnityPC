using System;
using strange.framework.api;

namespace strange.framework.impl
{
	// Token: 0x02000290 RID: 656
	public class BinderException : Exception
	{
		// Token: 0x06000DAA RID: 3498 RVA: 0x00057554 File Offset: 0x00055954
		public BinderException()
		{
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0005755C File Offset: 0x0005595C
		public BinderException(string message, BinderExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0005756C File Offset: 0x0005596C
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00057574 File Offset: 0x00055974
		public BinderExceptionType type { get; set; }
	}
}
