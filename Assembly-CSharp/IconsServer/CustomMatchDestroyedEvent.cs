using System;

namespace IconsServer
{
	// Token: 0x020007E0 RID: 2016
	public class CustomMatchDestroyedEvent : ServerEvent
	{
		// Token: 0x060031FB RID: 12795 RVA: 0x000F2559 File Offset: 0x000F0959
		public CustomMatchDestroyedEvent(CustomMatchDestroyedEvent.EReason reason, ulong id, string lobbyName)
		{
			this.reason = reason;
			this.id = id;
			this.lobbyName = lobbyName;
		}

		// Token: 0x0400231A RID: 8986
		public CustomMatchDestroyedEvent.EReason reason;

		// Token: 0x0400231B RID: 8987
		public ulong id;

		// Token: 0x0400231C RID: 8988
		public string lobbyName;

		// Token: 0x020007E1 RID: 2017
		public enum EReason
		{
			// Token: 0x0400231E RID: 8990
			Reason_OwnerDestroyed,
			// Token: 0x0400231F RID: 8991
			ReasonCount
		}
	}
}
