using System;

namespace IconsServer
{
	// Token: 0x020007F2 RID: 2034
	public class LockInSelectionAckEvent : ServerEvent
	{
		// Token: 0x0600321A RID: 12826 RVA: 0x000F2A13 File Offset: 0x000F0E13
		public LockInSelectionAckEvent(Guid matchId, bool success)
		{
			this.matchId = matchId;
			this.success = success;
		}

		// Token: 0x04002351 RID: 9041
		public Guid matchId;

		// Token: 0x04002352 RID: 9042
		public bool success;
	}
}
