// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ConnectToRoomCommand : ConnectionEvent
{
	public class ConnectResponse
	{
		public bool success;

		public IRoomData data;

		public ConnectResponse(bool success, IRoomData data)
		{
			this.success = success;
			this.data = data;
		}
	}

	public Action<ConnectToRoomCommand.ConnectResponse> onConnection;

	public string room;

	public ConnectToRoomCommand(string room, Action<ConnectToRoomCommand.ConnectResponse> onConnection)
	{
		this.room = room;
		this.onConnection = onConnection;
	}
}
