using System;
using strange.extensions.context.api;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using strange.framework.api;

namespace strange.extensions.context.impl
{
	// Token: 0x02000228 RID: 552
	public class CrossContext : Context, ICrossContextCapable
	{
		// Token: 0x06000ABF RID: 2751 RVA: 0x00052CC0 File Offset: 0x000510C0
		public CrossContext()
		{
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00052CC8 File Offset: 0x000510C8
		public CrossContext(object view) : base(view)
		{
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00052CD1 File Offset: 0x000510D1
		public CrossContext(object view, ContextStartupFlags flags) : base(view, flags)
		{
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00052CDB File Offset: 0x000510DB
		public CrossContext(object view, bool autoMapping) : base(view, autoMapping)
		{
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00052CE8 File Offset: 0x000510E8
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x00052D10 File Offset: 0x00051110
		public ICrossContextInjectionBinder injectionBinder
		{
			get
			{
				ICrossContextInjectionBinder result;
				if ((result = this._injectionBinder) == null)
				{
					result = (this._injectionBinder = new CrossContextInjectionBinder());
				}
				return result;
			}
			set
			{
				this._injectionBinder = value;
			}
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00052D1C File Offset: 0x0005111C
		protected override void addCoreComponents()
		{
			base.addCoreComponents();
			if (this.injectionBinder.CrossContextBinder == null)
			{
				this.injectionBinder.CrossContextBinder = new CrossContextInjectionBinder();
			}
			if (Context.firstContext == this)
			{
				this.injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>().ToSingleton().ToName(ContextKeys.CROSS_CONTEXT_DISPATCHER).CrossContext();
				this.injectionBinder.Bind<CrossContextBridge>().ToSingleton().CrossContext();
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00052D98 File Offset: 0x00051198
		protected override void instantiateCoreComponents()
		{
			base.instantiateCoreComponents();
			IInjectionBinding binding = this.injectionBinder.GetBinding<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER);
			if (binding != null)
			{
				IEventDispatcher instance = this.injectionBinder.GetInstance<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER);
				if (instance != null)
				{
					this.crossContextDispatcher = this.injectionBinder.GetInstance<IEventDispatcher>(ContextKeys.CROSS_CONTEXT_DISPATCHER);
					(this.crossContextDispatcher as ITriggerProvider).AddTriggerable(instance as ITriggerable);
					(instance as ITriggerProvider).AddTriggerable(this.crossContextBridge as ITriggerable);
				}
			}
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00052E1E File Offset: 0x0005121E
		public override IContext AddContext(IContext context)
		{
			base.AddContext(context);
			if (context is ICrossContextCapable)
			{
				this.AssignCrossContext((ICrossContextCapable)context);
			}
			return this;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00052E40 File Offset: 0x00051240
		public virtual void AssignCrossContext(ICrossContextCapable childContext)
		{
			childContext.crossContextDispatcher = this.crossContextDispatcher;
			childContext.injectionBinder.CrossContextBinder = this.injectionBinder.CrossContextBinder;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00052E64 File Offset: 0x00051264
		public virtual void RemoveCrossContext(ICrossContextCapable childContext)
		{
			if (childContext.crossContextDispatcher != null)
			{
				(childContext.crossContextDispatcher as ITriggerProvider).RemoveTriggerable(childContext.GetComponent<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER) as ITriggerable);
				childContext.crossContextDispatcher = null;
			}
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00052E99 File Offset: 0x00051299
		public override IContext RemoveContext(IContext context)
		{
			if (context is ICrossContextCapable)
			{
				this.RemoveCrossContext((ICrossContextCapable)context);
			}
			return base.RemoveContext(context);
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00052EB9 File Offset: 0x000512B9
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x00052EC1 File Offset: 0x000512C1
		public virtual IDispatcher crossContextDispatcher
		{
			get
			{
				return this._crossContextDispatcher;
			}
			set
			{
				this._crossContextDispatcher = (value as IEventDispatcher);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00052ECF File Offset: 0x000512CF
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x00052EF3 File Offset: 0x000512F3
		public virtual IBinder crossContextBridge
		{
			get
			{
				if (this._crossContextBridge == null)
				{
					this._crossContextBridge = this.injectionBinder.GetInstance<CrossContextBridge>();
				}
				return this._crossContextBridge;
			}
			set
			{
				this._crossContextDispatcher = (value as IEventDispatcher);
			}
		}

		// Token: 0x04000728 RID: 1832
		private ICrossContextInjectionBinder _injectionBinder;

		// Token: 0x04000729 RID: 1833
		private IBinder _crossContextBridge;

		// Token: 0x0400072A RID: 1834
		protected IEventDispatcher _crossContextDispatcher;
	}
}
