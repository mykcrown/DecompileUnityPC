using System;

namespace strange.extensions.reflector.api
{
	// Token: 0x0200026D RID: 621
	public interface IReflectionBinder
	{
		// Token: 0x06000C9B RID: 3227
		IReflectedClass Get(Type type);

		// Token: 0x06000C9C RID: 3228
		IReflectedClass Get<T>();
	}
}
