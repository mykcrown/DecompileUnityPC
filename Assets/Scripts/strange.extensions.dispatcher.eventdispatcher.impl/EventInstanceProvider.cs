// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	internal class EventInstanceProvider : IInstanceProvider
	{
		public T GetInstance<T>()
		{
			object obj = new TmEvent();
			return (T)((object)obj);
		}

		public object GetInstance(Type key)
		{
			return new TmEvent();
		}
	}
}
