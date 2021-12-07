using System;

namespace IconsServer
{
	// Token: 0x020007E6 RID: 2022
	public class MatchFailureEvent : ServerEvent
	{
		// Token: 0x06003200 RID: 12800 RVA: 0x000F2613 File Offset: 0x000F0A13
		public MatchFailureEvent(Guid matchId, MatchFailureEvent.EReason reason)
		{
			this.matchId = matchId;
			this.reason = reason;
		}

		// Token: 0x04002331 RID: 9009
		public Guid matchId;

		// Token: 0x04002332 RID: 9010
		public MatchFailureEvent.EReason reason;

		// Token: 0x020007E7 RID: 2023
		public enum EReason
		{
			// Token: 0x04002334 RID: 9012
			InternalFailure,
			// Token: 0x04002335 RID: 9013
			PlayerLeft,
			// Token: 0x04002336 RID: 9014
			Count
		}
	}
}
