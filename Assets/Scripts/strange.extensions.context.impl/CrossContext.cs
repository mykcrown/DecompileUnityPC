// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using strange.framework.api;
using System;

namespace strange.extensions.context.impl
{
	public class CrossContext : Context, ICrossContextCapable
	{
		private ICrossContextInjectionBinder _injectionBinder;

		private IBinder _crossContextBridge;

		protected IEventDispatcher _crossContextDispatcher;

		public ICrossContextInjectionBinder injectionBinder
		{
			get
			{
				ICrossContextInjectionBinder arg_1B_0;
				if ((arg_1B_0 = this._injectionBinder) == null)
				{
					arg_1B_0 = (this._injectionBinder = new CrossContextInjectionBinder());
				}
				return arg_1B_0;
			}
			set
			{
				this._injectionBinder = value;
			}
		}

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

		public CrossContext()
		{
		}

		public CrossContext(object view) : base(view)
		{
		}

		public CrossContext(object view, ContextStartupFlags flags) : base(view, flags)
		{
		}

		public CrossContext(object view, bool autoMapping) : base(view, autoMapping)
		{
		}

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

		public override IContext AddContext(IContext context)
		{
			base.AddContext(context);
			if (context is ICrossContextCapable)
			{
				this.AssignCrossContext((ICrossContextCapable)context);
			}
			return this;
		}

		public virtual void AssignCrossContext(ICrossContextCapable childContext)
		{
			childContext.crossContextDispatcher = this.crossContextDispatcher;
			childContext.injectionBinder.CrossContextBinder = this.injectionBinder.CrossContextBinder;
		}

		public virtual void RemoveCrossContext(ICrossContextCapable childContext)
		{
			if (childContext.crossContextDispatcher != null)
			{
				(childContext.crossContextDispatcher as ITriggerProvider).RemoveTriggerable(childContext.GetComponent<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER) as ITriggerable);
				childContext.crossContextDispatcher = null;
			}
		}

		public override IContext RemoveContext(IContext context)
		{
			if (context is ICrossContextCapable)
			{
				this.RemoveCrossContext((ICrossContextCapable)context);
			}
			return base.RemoveContext(context);
		}
	}
}
