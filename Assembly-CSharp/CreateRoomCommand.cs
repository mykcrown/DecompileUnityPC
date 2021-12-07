using System;

// Token: 0x020007CE RID: 1998
public class CreateRoomCommand : ConnectionEvent
{
	// Token: 0x0600316C RID: 12652 RVA: 0x000F10F1 File Offset: 0x000EF4F1
	public CreateRoomCommand(IRoomSettings settings, string name, Action<CreateRoomCommand.CreateRoomResponse> onCreateRoom)
	{
		this.settings = settings;
		this.onCreateRoom = onCreateRoom;
		this.name = name;
	}

	// Token: 0x040022A6 RID: 8870
	public Action<CreateRoomCommand.CreateRoomResponse> onCreateRoom;

	// Token: 0x040022A7 RID: 8871
	public IRoomSettings settings;

	// Token: 0x040022A8 RID: 8872
	public string name;

	// Token: 0x020007CF RID: 1999
	public class CreateRoomResponse
	{
		// Token: 0x0600316D RID: 12653 RVA: 0x000F110E File Offset: 0x000EF50E
		public CreateRoomResponse(bool success, IRoomData room)
		{
			this.room = room;
			this.success = success;
		}

		// Token: 0x040022A9 RID: 8873
		public bool success;

		// Token: 0x040022AA RID: 8874
		public IRoomData room;
	}
}
