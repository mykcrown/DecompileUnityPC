// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using System;

namespace IconsServer
{
	public class MatchConnectEvent : ServerEvent
	{
		public EIconStages[] stages;

		public uint matchLengthSeconds;

		public uint numberOfLives;

		public uint assistCount;

		public LobbyGameMode gameMode;

		public ETeamAttack teamAttack;

		public SBasicMatchPlayerDesc[] players;

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
	}
}
