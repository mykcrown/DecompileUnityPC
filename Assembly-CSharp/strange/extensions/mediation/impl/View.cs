using System;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.mediation.api;
using UnityEngine;

namespace strange.extensions.mediation.impl
{
	// Token: 0x02000262 RID: 610
	public class View : MonoBehaviour, IView
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x0005537A File Offset: 0x0005377A
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x00055382 File Offset: 0x00053782
		public bool requiresContext
		{
			get
			{
				return this._requiresContext;
			}
			set
			{
				this._requiresContext = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x0005538B File Offset: 0x0005378B
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x00055393 File Offset: 0x00053793
		public virtual bool autoRegisterWithContext
		{
			get
			{
				return this.registerWithContext;
			}
			set
			{
				this.registerWithContext = value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x0005539C File Offset: 0x0005379C
		// (set) Token: 0x06000C42 RID: 3138 RVA: 0x000553A4 File Offset: 0x000537A4
		public bool registeredWithContext { get; set; }

		// Token: 0x06000C43 RID: 3139 RVA: 0x000553AD File Offset: 0x000537AD
		protected virtual void Awake()
		{
			if (this.autoRegisterWithContext && !this.registeredWithContext)
			{
				this.bubbleToContext(this, true, false);
			}
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000553CE File Offset: 0x000537CE
		protected virtual void Start()
		{
			if (this.autoRegisterWithContext && !this.registeredWithContext)
			{
				this.bubbleToContext(this, true, true);
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x000553EF File Offset: 0x000537EF
		protected virtual void OnDestroy()
		{
			this.bubbleToContext(this, false, false);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x000553FC File Offset: 0x000537FC
		protected virtual void bubbleToContext(MonoBehaviour view, bool toAdd, bool finalTry)
		{
			int num = 0;
			Transform transform = view.gameObject.transform;
			while (transform.parent != null && num < 100)
			{
				num++;
				transform = transform.parent;
				if (transform.gameObject.GetComponent<ContextView>() != null)
				{
					ContextView component = transform.gameObject.GetComponent<ContextView>();
					if (component.context != null)
					{
						IContext context = component.context;
						if (toAdd)
						{
							context.AddView(view);
							this.registeredWithContext = true;
							return;
						}
						context.RemoveView(view);
						return;
					}
				}
			}
			if (!this.requiresContext || !finalTry)
			{
				return;
			}
			if (Context.firstContext != null)
			{
				Context.firstContext.AddView(view);
				this.registeredWithContext = true;
				return;
			}
			string text = (num != 100) ? "A view was added with no context. Views must be added into the hierarchy of their ContextView lest all hell break loose." : "A view couldn't find a context. Loop limit reached.";
			text = text + "\nView: " + view.ToString();
			throw new MediationException(text, MediationExceptionType.NO_CONTEXT);
		}

		// Token: 0x04000790 RID: 1936
		private bool _requiresContext = true;

		// Token: 0x04000791 RID: 1937
		protected bool registerWithContext = true;
	}
}
