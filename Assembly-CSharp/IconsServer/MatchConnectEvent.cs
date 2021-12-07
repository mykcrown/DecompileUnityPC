using System;
using MatchMaking;

namespace IconsServer
{
	// Token: 0x020007E3 RID: 2019
	public class MatchConnectEvent : ServerEvent
	{
		// Token: 0x060031FD RID: 12797 RVA: 0x000F258C File Offset: 0x000F098C
		public MatchConnectEvent(EIconStages[] stages, uint matchLengthSeconds, uint numberOfLives, uint assistCount, SBasicMatchPlayerDesc[] players, LobbyGameMode gameMode, ETeamAttack teamAttack)
		{
			this.stages = stages;
			this.matchLengthSeconds = matchLengthSeconds;
			this.numberOfLives = numberOfLives;
			this.assistCount = assistCount;
			this.gameMode = gameMode;
			this.teamAttack = teamAttack;
			this.players = players;
		}

		// Token: 0x04002322 RID: 8994
		public EIconStages[] stages;

		// Token: 0x04002323 RID: 8995
		public uint matchLengthSeconds;

		// Token: 0x04002324 RID: 8996
		public uint numberOfLives;

		// Token: 0x04002325 RID: 8997
		public uint assistCount;

		// Token: 0x04002326 RID: 8998
		public LobbyGameMode gameMode;

		// Token: 0x04002327 RID: 8999
		public ETeamAttack teamAttack;

		// Token: 0x04002328 RID: 9000
		public SBasicMatchPlayerDesc[] players;
	}
}
