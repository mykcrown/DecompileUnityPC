using System;

// Token: 0x020007C6 RID: 1990
public class JoinLobbyCommand : ConnectionEvent
{
	// Token: 0x06003164 RID: 12644 RVA: 0x000F1080 File Offset: 0x000EF480
	public JoinLobbyCommand(Action<JoinLobbyCommand.JoinResponse> handler)
	{
		this.handler = handler;
	}

	// Token: 0x0400229F RID: 8863
	public Action<JoinLobbyCommand.JoinResponse> handler;

	// Token: 0x020007C7 RID: 1991
	public class JoinResponse
	{
		// Token: 0x06003165 RID: 12645 RVA: 0x000F108F File Offset: 0x000EF48F
		public JoinResponse(bool success)
		{
			this.success = success;
		}

		// Token: 0x040022A0 RID: 8864
		public bool success;
	}
}
