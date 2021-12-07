// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace BattleServer
{
	public interface IQueueDuplicateHandler
	{
		bool HandledAsDuplicate(INetMsg messageInQueue);
	}
}
