// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class MatchBeginEvent : ServerEvent
	{
		public Guid matchId;

		public long matchStartTime;

		public uint countdownSeconds;

		public MatchBeginEvent(Guid matchId, long matchStartTime, uint countdownSeconds)
		{
			this.matchId = matchId;
			this.matchStartTime = matchStartTime;
			this.countdownSeconds = countdownSeconds;
		}
	}
}
