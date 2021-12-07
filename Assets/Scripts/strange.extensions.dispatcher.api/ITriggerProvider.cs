// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.extensions.dispatcher.api
{
	public interface ITriggerProvider
	{
		int Triggerables
		{
			get;
		}

		void AddTriggerable(ITriggerable target);

		void RemoveTriggerable(ITriggerable target);
	}
}
