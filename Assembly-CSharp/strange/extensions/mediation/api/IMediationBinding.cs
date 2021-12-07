using System;
using strange.framework.api;

namespace strange.extensions.mediation.api
{
	// Token: 0x02000257 RID: 599
	public interface IMediationBinding : IBinding
	{
		// Token: 0x06000C07 RID: 3079
		IMediationBinding ToMediator<T>();

		// Token: 0x06000C08 RID: 3080
		IMediationBinding ToAbstraction<T>();

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000C09 RID: 3081
		object abstraction { get; }

		// Token: 0x06000C0A RID: 3082
		IMediationBinding Bind<T>();

		// Token: 0x06000C0B RID: 3083
		IMediationBinding Bind(object key);

		// Token: 0x06000C0C RID: 3084
		IMediationBinding To<T>();

		// Token: 0x06000C0D RID: 3085
		IMediationBinding To(object o);
	}
}
