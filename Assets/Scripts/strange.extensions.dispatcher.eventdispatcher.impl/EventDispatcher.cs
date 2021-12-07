// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using strange.framework.api;
using strange.framework.impl;
using System;
using System.Collections.Generic;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	public class EventDispatcher : Binder, IEventDispatcher, ITriggerProvider, ITriggerable, IDispatcher
	{
		protected HashSet<ITriggerable> triggerClients;

		protected HashSet<ITriggerable> triggerClientRemovals;

		protected bool isTriggeringClients;

		public static IPool<TmEvent> eventPool;

		public int Triggerables
		{
			get
			{
				if (this.triggerClients == null)
				{
					return 0;
				}
				return this.triggerClients.Count;
			}
		}

		public EventDispatcher()
		{
			if (EventDispatcher.eventPool == null)
			{
				EventDispatcher.eventPool = new Pool<TmEvent>();
				EventDispatcher.eventPool.instanceProvider = new EventInstanceProvider();
			}
		}

		public override IBinding GetRawBinding()
		{
			return new EventBinding(new Binder.BindingResolver(this.resolver));
		}

		public new IEventBinding Bind(object key)
		{
			return base.Bind(key) as IEventBinding;
		}

		public void Dispatch(object eventType)
		{
			this.Dispatch(eventType, null);
		}

		public void Dispatch(object eventType, object data)
		{
			IEvent @event = this.conformDataToEvent(eventType, data);
			if (@event is IPoolable)
			{
				(@event as IPoolable).Retain();
			}
			bool flag = true;
			if (this.triggerClients != null)
			{
				this.isTriggeringClients = true;
				foreach (ITriggerable current in this.triggerClients)
				{
					try
					{
						if (!current.Trigger(@event.type, @event))
						{
							flag = false;
							break;
						}
					}
					catch (Exception)
					{
						this.internalReleaseEvent(@event);
						throw;
					}
				}
				if (this.triggerClientRemovals != null)
				{
					this.flushRemovals();
				}
				this.isTriggeringClients = false;
			}
			if (!flag)
			{
				this.internalReleaseEvent(@event);
				return;
			}
			IEventBinding eventBinding = this.GetBinding(@event.type) as IEventBinding;
			if (eventBinding == null)
			{
				this.internalReleaseEvent(@event);
				return;
			}
			object[] array = (eventBinding.value as object[]).Clone() as object[];
			if (array == null)
			{
				this.internalReleaseEvent(@event);
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				object obj = array[i];
				if (obj != null)
				{
					array[i] = null;
					object[] array2 = eventBinding.value as object[];
					if (Array.IndexOf<object>(array2, obj) != -1)
					{
						if (obj is EventCallback)
						{
							this.invokeEventCallback(@event, obj as EventCallback);
						}
						else if (obj is EmptyCallback)
						{
							(obj as EmptyCallback)();
						}
					}
				}
			}
			this.internalReleaseEvent(@event);
		}

		protected virtual IEvent conformDataToEvent(object eventType, object data)
		{
			if (eventType == null)
			{
				throw new EventDispatcherException("Attempt to Dispatch to null.\ndata: " + data, EventDispatcherExceptionType.EVENT_KEY_NULL);
			}
			IEvent result;
			if (eventType is IEvent)
			{
				result = (IEvent)eventType;
			}
			else if (data == null)
			{
				result = this.createEvent(eventType, null);
			}
			else if (data is IEvent)
			{
				result = (IEvent)data;
			}
			else
			{
				result = this.createEvent(eventType, data);
			}
			return result;
		}

		protected virtual IEvent createEvent(object eventType, object data)
		{
			IEvent instance = EventDispatcher.eventPool.GetInstance();
			instance.type = eventType;
			instance.target = this;
			instance.data = data;
			return instance;
		}

		protected virtual void invokeEventCallback(object data, EventCallback callback)
		{
			try
			{
				callback(data as IEvent);
			}
			catch (InvalidCastException)
			{
				object target = callback.Target;
				string name = callback.Method.Name;
				string message = string.Concat(new object[]
				{
					"An EventCallback is attempting an illegal cast. One possible reason is not typing the payload to IEvent in your callback. Another is illegal casting of the data.\nTarget class: ",
					target,
					" method: ",
					name
				});
				throw new EventDispatcherException(message, EventDispatcherExceptionType.TARGET_INVOCATION);
			}
		}

		public void AddListener(object evt, EventCallback callback)
		{
			IBinding binding = this.GetBinding(evt);
			if (binding == null)
			{
				this.Bind(evt).To(callback);
			}
			else
			{
				binding.To(callback);
			}
		}

		public void AddListener(object evt, EmptyCallback callback)
		{
			IBinding binding = this.GetBinding(evt);
			if (binding == null)
			{
				this.Bind(evt).To(callback);
			}
			else
			{
				binding.To(callback);
			}
		}

		public void RemoveListener(object evt, EventCallback callback)
		{
			IBinding binding = this.GetBinding(evt);
			this.RemoveValue(binding, callback);
		}

		public void RemoveListener(object evt, EmptyCallback callback)
		{
			IBinding binding = this.GetBinding(evt);
			this.RemoveValue(binding, callback);
		}

		public bool HasListener(object evt, EventCallback callback)
		{
			IEventBinding eventBinding = this.GetBinding(evt) as IEventBinding;
			return eventBinding != null && eventBinding.TypeForCallback(callback) != EventCallbackType.NOT_FOUND;
		}

		public bool HasListener(object evt, EmptyCallback callback)
		{
			IEventBinding eventBinding = this.GetBinding(evt) as IEventBinding;
			return eventBinding != null && eventBinding.TypeForCallback(callback) != EventCallbackType.NOT_FOUND;
		}

		public void UpdateListener(bool toAdd, object evt, EventCallback callback)
		{
			if (toAdd)
			{
				this.AddListener(evt, callback);
			}
			else
			{
				this.RemoveListener(evt, callback);
			}
		}

		public void UpdateListener(bool toAdd, object evt, EmptyCallback callback)
		{
			if (toAdd)
			{
				this.AddListener(evt, callback);
			}
			else
			{
				this.RemoveListener(evt, callback);
			}
		}

		public void AddTriggerable(ITriggerable target)
		{
			if (this.triggerClients == null)
			{
				this.triggerClients = new HashSet<ITriggerable>();
			}
			this.triggerClients.Add(target);
		}

		public void RemoveTriggerable(ITriggerable target)
		{
			if (this.triggerClients.Contains(target))
			{
				if (this.triggerClientRemovals == null)
				{
					this.triggerClientRemovals = new HashSet<ITriggerable>();
				}
				this.triggerClientRemovals.Add(target);
				if (!this.isTriggeringClients)
				{
					this.flushRemovals();
				}
			}
		}

		protected void flushRemovals()
		{
			if (this.triggerClientRemovals == null)
			{
				return;
			}
			foreach (ITriggerable current in this.triggerClientRemovals)
			{
				if (this.triggerClients.Contains(current))
				{
					this.triggerClients.Remove(current);
				}
			}
			this.triggerClientRemovals = null;
		}

		public bool Trigger<T>(object data)
		{
			return this.Trigger(typeof(T), data);
		}

		public bool Trigger(object key, object data)
		{
			bool flag = (data is IEvent && !object.ReferenceEquals((data as IEvent).target, this)) || (key is IEvent && !object.ReferenceEquals((data as IEvent).target, this));
			if (flag)
			{
				this.Dispatch(key, data);
			}
			return true;
		}

		protected void internalReleaseEvent(IEvent evt)
		{
			if (evt is IPoolable)
			{
				(evt as IPoolable).Release();
			}
		}

		public void ReleaseEvent(IEvent evt)
		{
			if (!(evt as IPoolable).retain)
			{
				this.cleanEvent(evt);
				EventDispatcher.eventPool.ReturnInstance(evt);
			}
		}

		protected void cleanEvent(IEvent evt)
		{
			evt.target = null;
			evt.data = null;
			evt.type = null;
		}
	}
}
