using System;
using strange.extensions.mediation.api;

namespace strange.extensions.context.api
{
	// Token: 0x02000223 RID: 547
	public interface IContextView : IView
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000A93 RID: 2707
		// (set) Token: 0x06000A94 RID: 2708
		IContext context { get; set; }
	}
}
