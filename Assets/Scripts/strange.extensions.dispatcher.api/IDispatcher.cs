// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.extensions.dispatcher.api
{
	public interface IDispatcher
	{
		void Dispatch(object eventType);

		void Dispatch(object eventType, object data);
	}
}
