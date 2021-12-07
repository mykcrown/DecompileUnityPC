// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.dispatcher.eventdispatcher.api
{
	public interface IEventBinding : IBinding
	{
		EventCallbackType TypeForCallback(EventCallback callback);

		EventCallbackType TypeForCallback(EmptyCallback callback);

		IEventBinding Bind(object key);

		IEventBinding To(EventCallback callback);

		IEventBinding To(EmptyCallback callback);
	}
}
