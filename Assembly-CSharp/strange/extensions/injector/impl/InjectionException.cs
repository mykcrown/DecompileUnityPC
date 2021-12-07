using System;
using strange.extensions.injector.api;

namespace strange.extensions.injector.impl
{
	// Token: 0x0200024E RID: 590
	public class InjectionException : Exception
	{
		// Token: 0x06000BD6 RID: 3030 RVA: 0x000549A3 File Offset: 0x00052DA3
		public InjectionException()
		{
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000549AB File Offset: 0x00052DAB
		public InjectionException(string message, InjectionExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x000549BB File Offset: 0x00052DBB
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x000549C3 File Offset: 0x00052DC3
		public InjectionExceptionType type { get; set; }
	}
}
