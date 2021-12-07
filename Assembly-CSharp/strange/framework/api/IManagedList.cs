using System;

namespace strange.framework.api
{
	// Token: 0x0200028C RID: 652
	public interface IManagedList
	{
		// Token: 0x06000D85 RID: 3461
		IManagedList Add(object value);

		// Token: 0x06000D86 RID: 3462
		IManagedList Add(object[] list);

		// Token: 0x06000D87 RID: 3463
		IManagedList Remove(object value);

		// Token: 0x06000D88 RID: 3464
		IManagedList Remove(object[] list);

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000D89 RID: 3465
		object value { get; }
	}
}
