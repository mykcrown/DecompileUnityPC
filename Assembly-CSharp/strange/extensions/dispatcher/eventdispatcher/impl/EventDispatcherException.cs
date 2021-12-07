using System;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	// Token: 0x02000239 RID: 569
	public class EventDispatcherException : Exception
	{
		// Token: 0x06000B3A RID: 2874 RVA: 0x00053B66 File Offset: 0x00051F66
		public EventDispatcherException()
		{
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00053B6E File Offset: 0x00051F6E
		public EventDispatcherException(string message, EventDispatcherExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00053B7E File Offset: 0x00051F7E
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x00053B86 File Offset: 0x00051F86
		public EventDispatcherExceptionType type { get; set; }
	}
}
