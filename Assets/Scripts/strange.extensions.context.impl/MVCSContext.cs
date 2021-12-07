// Decompile from assembly: Assembly-CSharp.dll

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
using System;
using UnityEngine;

namespace strange.extensions.context.impl
{
	public class MVCSContext : CrossContext
	{
		protected static ISemiBinding viewCache = new SemiBinding();

		public ICommandBinder commandBinder
		{
			get;
			set;
		}

		public IEventDispatcher dispatcher
		{
			get;
			set;
		}

		public IMediationBinder mediationBinder
		{
			get;
			set;
		}

		public IImplicitBinder implicitBinder
		{
			get;
			set;
		}

		public ISequencer sequencer
		{
			get;
			set;
		}

		public MVCSContext()
		{
		}

		public MVCSContext(MonoBehaviour view) : base(view)
		{
		}

		public MVCSContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
		{
		}

		public MVCSContext(MonoBehaviour view, bool autoMapping) : base(view, autoMapping)
		{
		}

		public override IContext SetContextView(object view)
		{
			base.contextView = (view as MonoBehaviour).gameObject;
			if (base.contextView == null)
			{
				throw new ContextException("MVCSContext requires a ContextView of type MonoBehaviour", ContextExceptionType.NO_CONTEXT_VIEW);
			}
			return this;
		}

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

		protected override void postBindings()
		{
			this.mediateViewCache();
			this.mediationBinder.Trigger(MediationEvent.AWAKE, (base.contextView as GameObject).GetComponent<ContextView>());
		}

		public override void Launch()
		{
			this.dispatcher.Dispatch(ContextEvent.START);
		}

		public override object GetComponent<T>()
		{
			return this.GetComponent<T>(null);
		}

		public override object GetComponent<T>(object name)
		{
			IInjectionBinding binding = base.injectionBinder.GetBinding<T>(name);
			if (binding != null)
			{
				return base.injectionBinder.GetInstance<T>(name);
			}
			return null;
		}

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

		public override void RemoveView(object view)
		{
			this.mediationBinder.Trigger(MediationEvent.DESTROYED, view as IView);
		}

		protected virtual void cacheView(MonoBehaviour view)
		{
			if (MVCSContext.viewCache.constraint.Equals(BindingConstraintType.ONE))
			{
				MVCSContext.viewCache.constraint = BindingConstraintType.MANY;
			}
			MVCSContext.viewCache.Add(view);
		}

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

		public override void OnRemove()
		{
			base.OnRemove();
			this.commandBinder.OnRemove();
		}
	}
}
