using System;
using strange.framework.api;

namespace strange.extensions.injector.api
{
	// Token: 0x02000243 RID: 579
	public interface ICrossContextInjectionBinder : IInjectionBinder, IInstanceProvider
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000B6F RID: 2927
		// (set) Token: 0x06000B70 RID: 2928
		IInjectionBinder CrossContextBinder { get; set; }
	}
}
