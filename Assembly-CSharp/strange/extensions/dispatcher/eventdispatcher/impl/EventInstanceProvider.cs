using System;
using strange.framework.api;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	// Token: 0x02000238 RID: 568
	internal class EventInstanceProvider : IInstanceProvider
	{
		// Token: 0x06000B38 RID: 2872 RVA: 0x00053B44 File Offset: 0x00051F44
		public T GetInstance<T>()
		{
			object obj = new TmEvent();
			return (T)((object)obj);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00053B5F File Offset: 0x00051F5F
		public object GetInstance(Type key)
		{
			return new TmEvent();
		}
	}
}
