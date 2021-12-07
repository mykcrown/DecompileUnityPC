using System;
using P2P;

namespace IconsServer
{
	// Token: 0x020007F3 RID: 2035
	public class MatchDetailsEvent : ServerEvent
	{
		// Token: 0x0600321B RID: 12827 RVA: 0x000F2A29 File Offset: 0x000F0E29
		public MatchDetailsEvent(Guid matchId, P2PMatchDetailsMsg.SPlayerDesc[] players)
		{
			this.matchId = matchId;
			this.players = players;
		}

		// Token: 0x04002353 RID: 9043
		public Guid matchId;

		// Token: 0x04002354 RID: 9044
		public P2PMatchDetailsMsg.SPlayerDesc[] players;
	}
}
