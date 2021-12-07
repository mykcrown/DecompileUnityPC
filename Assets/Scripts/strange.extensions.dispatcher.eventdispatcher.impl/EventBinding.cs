// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.impl;
using strange.framework.api;
using strange.framework.impl;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	public class EventBinding : Binding, IEventBinding, IBinding
	{
		private Dictionary<Delegate, EventCallbackType> callbackTypes;

		public EventBinding() : this(null)
		{
		}

		public EventBinding(strange.framework.impl.Binder.BindingResolver resolver) : base(resolver)
		{
			base.keyConstraint = BindingConstraintType.ONE;
			base.valueConstraint = BindingConstraintType.MANY;
			this.callbackTypes = new Dictionary<Delegate, EventCallbackType>();
		}

		public EventCallbackType TypeForCallback(EmptyCallback callback)
		{
			if (this.callbackTypes.ContainsKey(callback))
			{
				return this.callbackTypes[callback];
			}
			return EventCallbackType.NOT_FOUND;
		}

		public EventCallbackType TypeForCallback(EventCallback callback)
		{
			if (this.callbackTypes.ContainsKey(callback))
			{
				return this.callbackTypes[callback];
			}
			return EventCallbackType.NOT_FOUND;
		}

		public new IEventBinding Bind(object key)
		{
			return base.Bind(key) as IEventBinding;
		}

		public IEventBinding To(EventCallback value)
		{
			base.To(value);
			this.storeMethodType(value);
			return this;
		}

		public IEventBinding To(EmptyCallback value)
		{
			base.To(value);
			this.storeMethodType(value);
			return this;
		}

		public new IEventBinding To(object value)
		{
			base.To(value);
			this.storeMethodType(value as Delegate);
			return this;
		}

		public override void RemoveValue(object value)
		{
			base.RemoveValue(value);
			this.callbackTypes.Remove(value as Delegate);
		}

		private void storeMethodType(Delegate value)
		{
			if (value == null)
			{
				throw new DispatcherException("EventDispatcher can't map something that isn't a delegate'", DispatcherExceptionType.ILLEGAL_CALLBACK_HANDLER);
			}
			MethodInfo method = value.Method;
			int num = method.GetParameters().Length;
			if (num != 0)
			{
				if (num != 1)
				{
					throw new DispatcherException("Event callbacks must have either one or no arguments", DispatcherExceptionType.ILLEGAL_CALLBACK_HANDLER);
				}
				this.callbackTypes[value] = EventCallbackType.ONE_ARGUMENT;
			}
			else
			{
				this.callbackTypes[value] = EventCallbackType.NO_ARGUMENTS;
			}
		}
	}
}
