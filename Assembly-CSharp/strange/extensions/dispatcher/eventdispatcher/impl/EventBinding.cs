using System;
using System.Collections.Generic;
using System.Reflection;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.impl;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	// Token: 0x02000236 RID: 566
	public class EventBinding : Binding, IEventBinding, IBinding
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x00053392 File Offset: 0x00051792
		public EventBinding() : this(null)
		{
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0005339B File Offset: 0x0005179B
		public EventBinding(strange.framework.impl.Binder.BindingResolver resolver) : base(resolver)
		{
			base.keyConstraint = BindingConstraintType.ONE;
			base.valueConstraint = BindingConstraintType.MANY;
			this.callbackTypes = new Dictionary<Delegate, EventCallbackType>();
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x000533C7 File Offset: 0x000517C7
		public EventCallbackType TypeForCallback(EmptyCallback callback)
		{
			if (this.callbackTypes.ContainsKey(callback))
			{
				return this.callbackTypes[callback];
			}
			return EventCallbackType.NOT_FOUND;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x000533E8 File Offset: 0x000517E8
		public EventCallbackType TypeForCallback(EventCallback callback)
		{
			if (this.callbackTypes.ContainsKey(callback))
			{
				return this.callbackTypes[callback];
			}
			return EventCallbackType.NOT_FOUND;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00053409 File Offset: 0x00051809
		public new IEventBinding Bind(object key)
		{
			return base.Bind(key) as IEventBinding;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00053417 File Offset: 0x00051817
		public IEventBinding To(EventCallback value)
		{
			base.To(value);
			this.storeMethodType(value);
			return this;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00053429 File Offset: 0x00051829
		public IEventBinding To(EmptyCallback value)
		{
			base.To(value);
			this.storeMethodType(value);
			return this;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0005343B File Offset: 0x0005183B
		public new IEventBinding To(object value)
		{
			base.To(value);
			this.storeMethodType(value as Delegate);
			return this;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00053452 File Offset: 0x00051852
		public override void RemoveValue(object value)
		{
			base.RemoveValue(value);
			this.callbackTypes.Remove(value as Delegate);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00053470 File Offset: 0x00051870
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

		// Token: 0x0400073E RID: 1854
		private Dictionary<Delegate, EventCallbackType> callbackTypes;
	}
}
