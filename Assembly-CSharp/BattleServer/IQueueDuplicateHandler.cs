using System;
using network;

namespace BattleServer
{
	// Token: 0x02000796 RID: 1942
	public interface IQueueDuplicateHandler
	{
		// Token: 0x06002FEA RID: 12266
		bool HandledAsDuplicate(INetMsg messageInQueue);
	}
}
