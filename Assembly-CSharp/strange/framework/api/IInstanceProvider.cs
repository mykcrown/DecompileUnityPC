using System;

namespace strange.framework.api
{
	// Token: 0x0200028B RID: 651
	public interface IInstanceProvider
	{
		// Token: 0x06000D83 RID: 3459
		T GetInstance<T>();

		// Token: 0x06000D84 RID: 3460
		object GetInstance(Type key);
	}
}
