using System;

// Token: 0x020007C8 RID: 1992
public class ConnectToRoomCommand : ConnectionEvent
{
	// Token: 0x06003166 RID: 12646 RVA: 0x000F109E File Offset: 0x000EF49E
	public ConnectToRoomCommand(string room, Action<ConnectToRoomCommand.ConnectResponse> onConnection)
	{
		this.room = room;
		this.onConnection = onConnection;
	}

	// Token: 0x040022A1 RID: 8865
	public Action<ConnectToRoomCommand.ConnectResponse> onConnection;

	// Token: 0x040022A2 RID: 8866
	public string room;

	// Token: 0x020007C9 RID: 1993
	public class ConnectResponse
	{
		// Token: 0x06003167 RID: 12647 RVA: 0x000F10B4 File Offset: 0x000EF4B4
		public ConnectResponse(bool success, IRoomData data)
		{
			this.success = success;
			this.data = data;
		}

		// Token: 0x040022A3 RID: 8867
		public bool success;

		// Token: 0x040022A4 RID: 8868
		public IRoomData data;
	}
}
