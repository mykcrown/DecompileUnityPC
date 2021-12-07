using System;

namespace IconsServer
{
	// Token: 0x020007E5 RID: 2021
	public class MatchBeginEvent : ServerEvent
	{
		// Token: 0x060031FF RID: 12799 RVA: 0x000F25F6 File Offset: 0x000F09F6
		public MatchBeginEvent(Guid matchId, long matchStartTime, uint countdownSeconds)
		{
			this.matchId = matchId;
			this.matchStartTime = matchStartTime;
			this.countdownSeconds = countdownSeconds;
		}

		// Token: 0x0400232E RID: 9006
		public Guid matchId;

		// Token: 0x0400232F RID: 9007
		public long matchStartTime;

		// Token: 0x04002330 RID: 9008
		public uint countdownSeconds;
	}
}
