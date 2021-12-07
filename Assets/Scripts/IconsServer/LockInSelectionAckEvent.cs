// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class LockInSelectionAckEvent : ServerEvent
	{
		public Guid matchId;

		public bool success;

		public LockInSelectionAckEvent(Guid matchId, bool success)
		{
			this.matchId = matchId;
			this.success = success;
		}
	}
}
