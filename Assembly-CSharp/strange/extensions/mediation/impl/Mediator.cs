using System;
using strange.extensions.context.api;
using strange.extensions.mediation.api;
using UnityEngine;

namespace strange.extensions.mediation.impl
{
	// Token: 0x02000261 RID: 609
	public class Mediator : MonoBehaviour, IMediator
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00055334 File Offset: 0x00053734
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x0005533C File Offset: 0x0005373C
		[Inject(ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView { get; set; }

		// Token: 0x06000C39 RID: 3129 RVA: 0x00055345 File Offset: 0x00053745
		public virtual void PreRegister()
		{
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00055347 File Offset: 0x00053747
		public virtual void OnRegister()
		{
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00055349 File Offset: 0x00053749
		public virtual void OnRemove()
		{
		}
	}
}
