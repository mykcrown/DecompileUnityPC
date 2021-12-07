using System;
using strange.extensions.injector.api;

// Token: 0x02000894 RID: 2196
public class ContextConfig
{
	// Token: 0x0600373B RID: 14139 RVA: 0x00100DDE File Offset: 0x000FF1DE
	public ContextConfig(MasterContext.InitContext initContext)
	{
		this.injectionBinder = initContext.injectionBinder;
		this.config = initContext.config;
	}

	// Token: 0x04002573 RID: 9587
	protected IInjectionBinder injectionBinder;

	// Token: 0x04002574 RID: 9588
	protected ConfigData config;
}
