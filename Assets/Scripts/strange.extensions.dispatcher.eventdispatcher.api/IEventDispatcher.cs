// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.dispatcher.api;
using System;

namespace strange.extensions.dispatcher.eventdispatcher.api
{
	public interface IEventDispatcher : IDispatcher
	{
		IEventBinding Bind(object key);

		void AddListener(object evt, EventCallback callback);

		void AddListener(object evt, EmptyCallback callback);

		void RemoveListener(object evt, EventCallback callback);

		void RemoveListener(object evt, EmptyCallback callback);

		bool HasListener(object evt, EventCallback callback);

		bool HasListener(object evt, EmptyCallback callback);

		void UpdateListener(bool toAdd, object evt, EventCallback callback);

		void UpdateListener(bool toAdd, object evt, EmptyCallback callback);

		void ReleaseEvent(IEvent evt);
	}
}
