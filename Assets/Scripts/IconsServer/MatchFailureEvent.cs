// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class MatchFailureEvent : ServerEvent
	{
		public enum EReason
		{
			InternalFailure,
			PlayerLeft,
			Count
		}

		public Guid matchId;

		public MatchFailureEvent.EReason reason;

		public MatchFailureEvent(Guid matchId, MatchFailureEvent.EReason reason)
		{
			this.matchId = matchId;
			this.reason = reason;
		}
	}
}
