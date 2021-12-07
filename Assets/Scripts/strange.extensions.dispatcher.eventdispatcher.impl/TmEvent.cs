// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.pool.api;
using System;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	public class TmEvent : IEvent, IPoolable
	{
		protected int retainCount;

		public object type
		{
			get;
			set;
		}

		public IEventDispatcher target
		{
			get;
			set;
		}

		public object data
		{
			get;
			set;
		}

		public bool retain
		{
			get
			{
				return this.retainCount > 0;
			}
		}

		public TmEvent()
		{
		}

		public TmEvent(object type, IEventDispatcher target, object data)
		{
			this.type = type;
			this.target = target;
			this.data = data;
		}

		public void Restore()
		{
			this.type = null;
			this.target = null;
			this.data = null;
		}

		public void Retain()
		{
			this.retainCount++;
		}

		public void Release()
		{
			this.retainCount--;
			if (this.retainCount == 0)
			{
				this.target.ReleaseEvent(this);
			}
		}
	}
}
