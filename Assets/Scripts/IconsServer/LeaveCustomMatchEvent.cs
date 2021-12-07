// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class LeaveCustomMatchEvent : ServerEvent
	{
		public enum EResult
		{
			Result_Ok,
			Result_InQueue,
			Result_NotInMatch,
			Result_SystemError,
			Result_TooLate,
			ResultCount
		}

		public LeaveCustomMatchEvent.EResult result;

		public LeaveCustomMatchEvent(LeaveCustomMatchEvent.EResult result)
		{
			this.result = result;
		}
	}
}
