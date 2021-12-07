// Decompile from assembly: Assembly-CSharp.dll

using P2P;
using System;

namespace IconsServer
{
	public class MatchCharacterStagingEvent : ServerEvent
	{
		public Guid matchId;

		public ulong userID;

		public int playerIndex;

		public uint characterSelectSeconds;

		public SP2PMatchBasicPlayerDesc[] players;

		public MatchCharacterStagingEvent(Guid matchId, ulong userID, int playerIndex, SP2PMatchBasicPlayerDesc[] players, uint characterSelectSeconds)
		{
			this.matchId = matchId;
			this.userID = userID;
			this.playerIndex = playerIndex;
			this.players = players;
			this.characterSelectSeconds = characterSelectSeconds;
		}
	}
}
