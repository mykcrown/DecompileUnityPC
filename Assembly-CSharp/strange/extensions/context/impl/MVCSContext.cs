using System;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.implicitBind.api;
using strange.extensions.implicitBind.impl;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using strange.extensions.sequencer.api;
using strange.extensions.sequencer.impl;
using strange.framework.api;
using strange.framework.impl;
using UnityEngine;

namespace strange.extensions.context.impl
{
	// Token: 0x0200022A RID: 554
	public class MVCSContext : CrossContext
	{
		// Token: 0x06000AD5 RID: 2773 RVA: 0x00052FB5 File Offset: 0x000513B5
		public MVCSContext()
		{
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00052FBD File Offset: 0x000513BD
		public MVCSContext(MonoBehaviour view) : base(view)
		{
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00052FC6 File Offset: 0x000513C6
		public MVCSContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
		{
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00052FD0 File Offset: 0x000513D0
		public MVCSContext(MonoBehaviour view, bool autoMapping) : base(view, autoMapping)
		{
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00052FDA File Offset: 0x000513DA
		// (set) Token: 0x06000ADA RID: 2778 RVA: 0x00052FE2 File Offset: 0x000513E2
		public ICommandBinder commandBinder { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00052FEB File Offset: 0x000513EB
		// (set) Token: 0x06000ADC RID: 2780 RVA: 0x00052FF3 File Offset: 0x000513F3
		public IEventDispatcher dispatcher { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00052FFC File Offset: 0x000513FC
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x00053004 File Offset: 0x00051404
		public IMediationBinder mediationBinder { get; set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0005300D File Offset: 0x0005140D
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x00053015 File Offset: 0x00051415
		public IImplicitBinder implicitBinder { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0005301E File Offset: 0x0005141E
		// (set) Token: 0x06000AE2 RID: 2786 RVA: 0x00053026 File Offset: 0x00051426
		public ISequencer sequencer { get; set; }

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0005302F File Offset: 0x0005142F
		public override IContext SetContextView(object view)
		{
			base.contextView = (view as MonoBehaviour).gameObject;
			if (base.contextView == null)
			{
				throw new ContextException("MVCSContext requires a ContextView of type MonoBehaviour", ContextExceptionType.NO_CONTEXT_VIEW);
			}
			return this;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0005305C File Offset: 0x0005145C
		protected override void addCoreComponents()
		{
			base.addCoreComponents();
			base.injectionBinder.Bind<IInstanceProvider>().Bind<IInjectionBinder>().ToValue(base.injectionBinder);
			base.injectionBinder.Bind<IContext>().ToValue(this).ToName(ContextKeys.CONTEXT);
			base.injectionBinder.Bind<ICommandBinder>().To<EventCommandBinder>().ToSingleton();
			base.injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>();
			base.injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>().ToSingleton().ToName(ContextKeys.CONTEXT_DISPATCHER);
			base.injectionBinder.Bind<IMediationBinder>().To<MediationBinder>().ToSingleton();
			base.injectionBinder.Bind<ISequencer>().To<EventSequencer>().ToSingleton();
			base.injectionBinder.Bind<IImplicitBinder>().To<ImplicitBinder>().ToSingleton();
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00053134 File Offset: 0x00051534
		protected override void instantiateCoreComponents()
		{
			base.instantiateCoreComponents();
			if (base.contextView == null)
			{
				throw new ContextException("MVCSContext requires a ContextView of type MonoBehaviour", ContextExceptionType.NO_CONTEXT_VIEW);
			}
			base.injectionBinder.Bind<GameObject>().ToValue(base.contextView).ToName(ContextKeys.CONTEXT_VIEW);
			this.commandBinder = base.injectionBinder.GetInstance<ICommandBinder>();
			this.dispatcher = base.injectionBinder.GetInstance<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER);
			this.mediationBinder = base.injectionBinder.GetInstance<IMediationBinder>();
			this.sequencer = base.injectionBinder.GetInstance<ISequencer>();
			this.implicitBinder = base.injectionBinder.GetInstance<IImplicitBinder>();
			(this.dispatcher as ITriggerProvider).AddTriggerable(this.commandBinder as ITriggerable);
			(this.dispatcher as ITriggerProvider).AddTriggerable(this.sequencer as ITriggerable);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00053211 File Offset: 0x00051611
		protected override void postBindings()
		{
			this.mediateViewCache();
			this.mediationBinder.Trigger(MediationEvent.AWAKE, (base.contextView as GameObject).GetComponent<ContextView>());
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00053235 File Offset: 0x00051635
		public override void Launch()
		{
			this.dispatcher.Dispatch(ContextEvent.START);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00053248 File Offset: 0x00051648
		public override object GetComponent<T>()
		{
			return this.GetComponent<T>(null);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00053254 File Offset: 0x00051654
		public override object GetComponent<T>(object name)
		{
			IInjectionBinding binding = base.injectionBinder.GetBinding<T>(name);
			if (binding != null)
			{
				return base.injectionBinder.GetInstance<T>(name);
			}
			return null;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00053287 File Offset: 0x00051687
		public override void AddView(object view)
		{
			if (this.mediationBinder != null)
			{
				this.mediationBinder.Trigger(MediationEvent.AWAKE, view as IView);
			}
			else
			{
				this.cacheView(view as MonoBehaviour);
			}
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x000532B7 File Offset: 0x000516B7
		public override void RemoveView(object view)
		{
			this.mediationBinder.Trigger(MediationEvent.DESTROYED, view as IView);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x000532CB File Offset: 0x000516CB
		protected virtual void cacheView(MonoBehaviour view)
		{
			if (MVCSContext.viewCache.constraint.Equals(BindingConstraintType.ONE))
			{
				MVCSContext.viewCache.constraint = BindingConstraintType.MANY;
			}
			MVCSContext.viewCache.Add(view);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00053304 File Offset: 0x00051704
		protected virtual void mediateViewCache()
		{
			if (this.mediationBinder == null)
			{
				throw new ContextException("MVCSContext cannot mediate views without a mediationBinder", ContextExceptionType.NO_MEDIATION_BINDER);
			}
			object[] array = MVCSContext.viewCache.value as object[];
			if (array == null)
			{
				return;
			}
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				this.mediationBinder.Trigger(MediationEvent.AWAKE, array[i] as IView);
			}
			MVCSContext.viewCache = new SemiBinding();
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00053373 File Offset: 0x00051773
		public override void OnRemove()
		{
			base.OnRemove();
			this.commandBinder.OnRemove();
		}

		// Token: 0x04000732 RID: 1842
		protected static ISemiBinding viewCache = new SemiBinding();
	}
}
