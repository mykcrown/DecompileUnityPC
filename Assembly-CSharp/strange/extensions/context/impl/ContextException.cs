using System;
using strange.extensions.context.api;

namespace strange.extensions.context.impl
{
	// Token: 0x02000226 RID: 550
	public class ContextException : Exception
	{
		// Token: 0x06000AB1 RID: 2737 RVA: 0x00052C23 File Offset: 0x00051023
		public ContextException()
		{
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00052C2B File Offset: 0x0005102B
		public ContextException(string message, ContextExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00052C3B File Offset: 0x0005103B
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x00052C43 File Offset: 0x00051043
		public ContextExceptionType type { get; set; }
	}
}
