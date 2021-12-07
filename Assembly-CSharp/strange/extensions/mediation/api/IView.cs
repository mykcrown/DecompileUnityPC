using System;

namespace strange.extensions.mediation.api
{
	// Token: 0x02000259 RID: 601
	public interface IView
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000C13 RID: 3091
		// (set) Token: 0x06000C14 RID: 3092
		bool requiresContext { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000C15 RID: 3093
		// (set) Token: 0x06000C16 RID: 3094
		bool registeredWithContext { get; set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000C17 RID: 3095
		bool autoRegisterWithContext { get; }
	}
}
