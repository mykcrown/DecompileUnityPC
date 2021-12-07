// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using System;

public class ContextConfig
{
	protected IInjectionBinder injectionBinder;

	protected ConfigData config;

	public ContextConfig(MasterContext.InitContext initContext)
	{
		this.injectionBinder = initContext.injectionBinder;
		this.config = initContext.config;
	}
}
