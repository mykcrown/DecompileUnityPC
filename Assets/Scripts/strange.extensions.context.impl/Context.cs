// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.framework.api;
using strange.framework.impl;
using System;

namespace strange.extensions.context.impl
{
	public class Context : Binder, IContext, IBinder
	{
		public static IContext firstContext;

		public bool autoStartup;

		public object contextView
		{
			get;
			set;
		}

		public Context()
		{
		}

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

		public Context(object view) : this(view, ContextStartupFlags.AUTOMATIC)
		{
		}

		public Context(object view, bool autoMapping) : this(view, (!autoMapping) ? (ContextStartupFlags.MANUAL_MAPPING | ContextStartupFlags.MANUAL_LAUNCH) : ContextStartupFlags.MANUAL_MAPPING)
		{
		}

		protected virtual void addCoreComponents()
		{
		}

		protected virtual void instantiateCoreComponents()
		{
		}

		public virtual IContext SetContextView(object view)
		{
			this.contextView = view;
			return this;
		}

		public virtual object GetContextView()
		{
			return this.contextView;
		}

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

		public virtual void Launch()
		{
		}

		protected virtual void mapBindings()
		{
		}

		protected virtual void postBindings()
		{
		}

		public virtual IContext AddContext(IContext context)
		{
			return this;
		}

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

		public virtual object GetComponent<T>()
		{
			return null;
		}

		public virtual object GetComponent<T>(object name)
		{
			return null;
		}

		public virtual void AddView(object view)
		{
		}

		public virtual void RemoveView(object view)
		{
		}
	}
}
