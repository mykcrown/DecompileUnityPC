// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class RoomListRequest : ConnectionEvent
{
	public Action<List<IRoomData>> onRoomList;

	public RoomListRequest(Action<List<IRoomData>> listHandler)
	{
		this.onRoomList = listHandler;
	}
}
