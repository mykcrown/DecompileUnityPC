using System;
using strange.extensions.context.api;
using strange.extensions.mediation.api;
using UnityEngine;

namespace strange.extensions.context.impl
{
	// Token: 0x02000227 RID: 551
	public class ContextView : MonoBehaviour, IContextView, IView
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00052C54 File Offset: 0x00051054
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x00052C5C File Offset: 0x0005105C
		public IContext context { get; set; }

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00052C65 File Offset: 0x00051065
		protected virtual void OnDestroy()
		{
			if (this.context != null && Context.firstContext != null)
			{
				Context.firstContext.RemoveContext(this.context);
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00052C8D File Offset: 0x0005108D
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00052C95 File Offset: 0x00051095
		public bool requiresContext { get; set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00052C9E File Offset: 0x0005109E
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x00052CA6 File Offset: 0x000510A6
		public bool registeredWithContext { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00052CAF File Offset: 0x000510AF
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x00052CB7 File Offset: 0x000510B7
		public bool autoRegisterWithContext { get; set; }
	}
}
