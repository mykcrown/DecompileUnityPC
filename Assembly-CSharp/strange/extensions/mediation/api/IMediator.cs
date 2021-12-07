using System;
using UnityEngine;

namespace strange.extensions.mediation.api
{
	// Token: 0x02000258 RID: 600
	public interface IMediator
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000C0E RID: 3086
		// (set) Token: 0x06000C0F RID: 3087
		GameObject contextView { get; set; }

		// Token: 0x06000C10 RID: 3088
		void PreRegister();

		// Token: 0x06000C11 RID: 3089
		void OnRegister();

		// Token: 0x06000C12 RID: 3090
		void OnRemove();
	}
}
