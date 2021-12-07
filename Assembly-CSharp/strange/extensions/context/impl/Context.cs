using System;
using strange.extensions.context.api;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.context.impl
{
	// Token: 0x02000225 RID: 549
	public class Context : Binder, IContext, IBinder
	{
		// Token: 0x06000A9D RID: 2717 RVA: 0x00052B0A File Offset: 0x00050F0A
		public Context()
		{
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00052B14 File Offset: 0x00050F14
		public Context(object view, ContextStartupFlags flags)
		{
			if (Context.firstContext == null || Context.firstContext.GetContextView() == null)
			{
				Context.firstContext = this;
			}
			else
			{
				Context.firstContext.AddContext(this);
			}
			this.SetContextView(view);
			this.addCoreComponents();
			this.autoStartup = ((flags & ContextStartupFlags.MANUAL_LAUNCH) != ContextStartupFlags.MANUAL_LAUNCH);
			if ((flags & ContextStartupFlags.MANUAL_MAPPING) != ContextStartupFlags.MANUAL_MAPPING)
			{
				this.Start();
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00052B84 File Offset: 0x00050F84
		public Context(object view) : this(view, ContextStartupFlags.AUTOMATIC)
		{
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00052B8E File Offset: 0x00050F8E
		public Context(object view, bool autoMapping) : this(view, (!autoMapping) ? (ContextStartupFlags.MANUAL_MAPPING | ContextStartupFlags.MANUAL_LAUNCH) : ContextStartupFlags.MANUAL_MAPPING)
		{
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00052BA4 File Offset: 0x00050FA4
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x00052BAC File Offset: 0x00050FAC
		public object contextView { get; set; }

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00052BB5 File Offset: 0x00050FB5
		protected virtual void addCoreComponents()
		{
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00052BB7 File Offset: 0x00050FB7
		protected virtual void instantiateCoreComponents()
		{
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00052BB9 File Offset: 0x00050FB9
		public virtual IContext SetContextView(object view)
		{
			this.contextView = view;
			return this;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00052BC3 File Offset: 0x00050FC3
		public virtual object GetContextView()
		{
			return this.contextView;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00052BCB File Offset: 0x00050FCB
		public virtual IContext Start()
		{
			this.instantiateCoreComponents();
			this.mapBindings();
			this.postBindings();
			if (this.autoStartup)
			{
				this.Launch();
			}
			return this;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00052BF1 File Offset: 0x00050FF1
		public virtual void Launch()
		{
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00052BF3 File Offset: 0x00050FF3
		protected virtual void mapBindings()
		{
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00052BF5 File Offset: 0x00050FF5
		protected virtual void postBindings()
		{
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00052BF7 File Offset: 0x00050FF7
		public virtual IContext AddContext(IContext context)
		{
			return this;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00052BFA File Offset: 0x00050FFA
		public virtual IContext RemoveContext(IContext context)
		{
			if (context == Context.firstContext)
			{
				Context.firstContext = null;
			}
			else
			{
				context.OnRemove();
			}
			return this;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00052C19 File Offset: 0x00051019
		public virtual object GetComponent<T>()
		{
			return null;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00052C1C File Offset: 0x0005101C
		public virtual object GetComponent<T>(object name)
		{
			return null;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x00052C1F File Offset: 0x0005101F
		public virtual void AddView(object view)
		{
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00052C21 File Offset: 0x00051021
		public virtual void RemoveView(object view)
		{
		}

		// Token: 0x04000721 RID: 1825
		public static IContext firstContext;

		// Token: 0x04000722 RID: 1826
		public bool autoStartup;
	}
}
