using System;
using P2P;

namespace IconsServer
{
	// Token: 0x020007E4 RID: 2020
	public class MatchCharacterStagingEvent : ServerEvent
	{
		// Token: 0x060031FE RID: 12798 RVA: 0x000F25C9 File Offset: 0x000F09C9
		public MatchCharacterStagingEvent(Guid matchId, ulong userID, int playerIndex, SP2PMatchBasicPlayerDesc[] players, uint characterSelectSeconds)
		{
			this.matchId = matchId;
			this.userID = userID;
			this.playerIndex = playerIndex;
			this.players = players;
			this.characterSelectSeconds = characterSelectSeconds;
		}

		// Token: 0x04002329 RID: 9001
		public Guid matchId;

		// Token: 0x0400232A RID: 9002
		public ulong userID;

		// Token: 0x0400232B RID: 9003
		public int playerIndex;

		// Token: 0x0400232C RID: 9004
		public uint characterSelectSeconds;

		// Token: 0x0400232D RID: 9005
		public SP2PMatchBasicPlayerDesc[] players;
	}
}
