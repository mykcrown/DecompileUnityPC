using System;

namespace strange.extensions.injector.api
{
	// Token: 0x02000247 RID: 583
	public interface IInjectorFactory
	{
		// Token: 0x06000BA6 RID: 2982
		object Get(IInjectionBinding binding);

		// Token: 0x06000BA7 RID: 2983
		object Get(IInjectionBinding binding, object[] args);
	}
}
