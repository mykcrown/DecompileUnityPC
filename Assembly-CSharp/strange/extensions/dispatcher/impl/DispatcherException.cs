using System;
using strange.extensions.dispatcher.api;

namespace strange.extensions.dispatcher.impl
{
	// Token: 0x0200023B RID: 571
	public class DispatcherException : Exception
	{
		// Token: 0x06000B4A RID: 2890 RVA: 0x00053C40 File Offset: 0x00052040
		public DispatcherException()
		{
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00053C48 File Offset: 0x00052048
		public DispatcherException(string message, DispatcherExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00053C58 File Offset: 0x00052058
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x00053C60 File Offset: 0x00052060
		public DispatcherExceptionType type { get; set; }
	}
}
