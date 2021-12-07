// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.framework.api;
using strange.framework.impl;
using System;
using System.Collections.Generic;

namespace strange.extensions.context.impl
{
	public class CrossContextBridge : Binder, ITriggerable
	{
		protected HashSet<object> eventsInProgress = new HashSet<object>();

		[Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
		public IEventDispatcher crossContextDispatcher
		{
			get;
			set;
		}

		public override IBinding Bind(object key)
		{
			IBinding rawBinding = this.GetRawBinding();
			rawBinding.Bind(key);
			this.resolver(rawBinding);
			return rawBinding;
		}

		public bool Trigger<T>(object data)
		{
			return this.Trigger(typeof(T), data);
		}

		public bool Trigger(object key, object data)
		{
			IBinding binding = this.GetBinding(key, null);
			if (binding != null && !this.eventsInProgress.Contains(key))
			{
				this.eventsInProgress.Add(key);
				this.crossContextDispatcher.Dispatch(key, data);
				this.eventsInProgress.Remove(key);
			}
			return true;
		}
	}
}
