using System;
using System.Collections.Generic;

// Token: 0x020007D0 RID: 2000
public class RoomListRequest : ConnectionEvent
{
	// Token: 0x0600316E RID: 12654 RVA: 0x000F1124 File Offset: 0x000EF524
	public RoomListRequest(Action<List<IRoomData>> listHandler)
	{
		this.onRoomList = listHandler;
	}

	// Token: 0x040022AB RID: 8875
	public Action<List<IRoomData>> onRoomList;
}
