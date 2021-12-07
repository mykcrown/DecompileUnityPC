using System;

namespace IconsServer
{
	// Token: 0x020007DE RID: 2014
	public class LeaveCustomMatchEvent : ServerEvent
	{
		// Token: 0x060031FA RID: 12794 RVA: 0x000F254A File Offset: 0x000F094A
		public LeaveCustomMatchEvent(LeaveCustomMatchEvent.EResult result)
		{
			this.result = result;
		}

		// Token: 0x04002312 RID: 8978
		public LeaveCustomMatchEvent.EResult result;

		// Token: 0x020007DF RID: 2015
		public enum EResult
		{
			// Token: 0x04002314 RID: 8980
			Result_Ok,
			// Token: 0x04002315 RID: 8981
			Result_InQueue,
			// Token: 0x04002316 RID: 8982
			Result_NotInMatch,
			// Token: 0x04002317 RID: 8983
			Result_SystemError,
			// Token: 0x04002318 RID: 8984
			Result_TooLate,
			// Token: 0x04002319 RID: 8985
			ResultCount
		}
	}
}
