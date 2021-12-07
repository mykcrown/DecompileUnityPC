using System;
using System.Collections.Generic;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	// Token: 0x02000237 RID: 567
	public class EventDispatcher : Binder, IEventDispatcher, ITriggerProvider, ITriggerable, IDispatcher
	{
		// Token: 0x06000B1E RID: 2846 RVA: 0x000534E1 File Offset: 0x000518E1
		public EventDispatcher()
		{
			if (EventDispatcher.eventPool == null)
			{
				EventDispatcher.eventPool = new Pool<TmEvent>();
				EventDispatcher.eventPool.instanceProvider = new EventInstanceProvider();
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0005350C File Offset: 0x0005190C
		public override IBinding GetRawBinding()
		{
			return new EventBinding(new Binder.BindingResolver(this.resolver));
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00053520 File Offset: 0x00051920
		public new IEventBinding Bind(object key)
		{
			return base.Bind(key) as IEventBinding;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0005352E File Offset: 0x0005192E
		public void Dispatch(object eventType)
		{
			this.Dispatch(eventType, null);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00053538 File Offset: 0x00051938
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
				foreach (ITriggerable triggerable in this.triggerClients)
				{
					try
					{
						if (!triggerable.Trigger(@event.type, @event))
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

		// Token: 0x06000B23 RID: 2851 RVA: 0x000536F8 File Offset: 0x00051AF8
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

		// Token: 0x06000B24 RID: 2852 RVA: 0x0005376C File Offset: 0x00051B6C
		protected virtual IEvent createEvent(object eventType, object data)
		{
			IEvent instance = EventDispatcher.eventPool.GetInstance();
			instance.type = eventType;
			instance.target = this;
			instance.data = data;
			return instance;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0005379C File Offset: 0x00051B9C
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

		// Token: 0x06000B26 RID: 2854 RVA: 0x0005380C File Offset: 0x00051C0C
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

		// Token: 0x06000B27 RID: 2855 RVA: 0x00053844 File Offset: 0x00051C44
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

		// Token: 0x06000B28 RID: 2856 RVA: 0x0005387C File Offset: 0x00051C7C
		public void RemoveListener(object evt, EventCallback callback)
		{
			IBinding binding = this.GetBinding(evt);
			this.RemoveValue(binding, callback);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0005389C File Offset: 0x00051C9C
		public void RemoveListener(object evt, EmptyCallback callback)
		{
			IBinding binding = this.GetBinding(evt);
			this.RemoveValue(binding, callback);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000538BC File Offset: 0x00051CBC
		public bool HasListener(object evt, EventCallback callback)
		{
			IEventBinding eventBinding = this.GetBinding(evt) as IEventBinding;
			return eventBinding != null && eventBinding.TypeForCallback(callback) != EventCallbackType.NOT_FOUND;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x000538EC File Offset: 0x00051CEC
		public bool HasListener(object evt, EmptyCallback callback)
		{
			IEventBinding eventBinding = this.GetBinding(evt) as IEventBinding;
			return eventBinding != null && eventBinding.TypeForCallback(callback) != EventCallbackType.NOT_FOUND;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0005391B File Offset: 0x00051D1B
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

		// Token: 0x06000B2D RID: 2861 RVA: 0x00053938 File Offset: 0x00051D38
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

		// Token: 0x06000B2E RID: 2862 RVA: 0x00053955 File Offset: 0x00051D55
		public void AddTriggerable(ITriggerable target)
		{
			if (this.triggerClients == null)
			{
				this.triggerClients = new HashSet<ITriggerable>();
			}
			this.triggerClients.Add(target);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0005397C File Offset: 0x00051D7C
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

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x000539CE File Offset: 0x00051DCE
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

		// Token: 0x06000B31 RID: 2865 RVA: 0x000539E8 File Offset: 0x00051DE8
		protected void flushRemovals()
		{
			if (this.triggerClientRemovals == null)
			{
				return;
			}
			foreach (ITriggerable item in this.triggerClientRemovals)
			{
				if (this.triggerClients.Contains(item))
				{
					this.triggerClients.Remove(item);
				}
			}
			this.triggerClientRemovals = null;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00053A70 File Offset: 0x00051E70
		public bool Trigger<T>(object data)
		{
			return this.Trigger(typeof(T), data);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00053A84 File Offset: 0x00051E84
		public bool Trigger(object key, object data)
		{
			bool flag = (data is IEvent && !object.ReferenceEquals((data as IEvent).target, this)) || (key is IEvent && !object.ReferenceEquals((data as IEvent).target, this));
			if (flag)
			{
				this.Dispatch(key, data);
			}
			return true;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00053AE7 File Offset: 0x00051EE7
		protected void internalReleaseEvent(IEvent evt)
		{
			if (evt is IPoolable)
			{
				(evt as IPoolable).Release();
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00053AFF File Offset: 0x00051EFF
		public void ReleaseEvent(IEvent evt)
		{
			if (!(evt as IPoolable).retain)
			{
				this.cleanEvent(evt);
				EventDispatcher.eventPool.ReturnInstance(evt);
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00053B23 File Offset: 0x00051F23
		protected void cleanEvent(IEvent evt)
		{
			evt.target = null;
			evt.data = null;
			evt.type = null;
		}

		// Token: 0x0400073F RID: 1855
		protected HashSet<ITriggerable> triggerClients;

		// Token: 0x04000740 RID: 1856
		protected HashSet<ITriggerable> triggerClientRemovals;

		// Token: 0x04000741 RID: 1857
		protected bool isTriggeringClients;

		// Token: 0x04000742 RID: 1858
		public static IPool<TmEvent> eventPool;
	}
}
