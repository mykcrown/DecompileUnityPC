// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CreateRoomCommand : ConnectionEvent
{
	public class CreateRoomResponse
	{
		public bool success;

		public IRoomData room;

		public CreateRoomResponse(bool success, IRoomData room)
		{
			this.room = room;
			this.success = success;
		}
	}

	public Action<CreateRoomCommand.CreateRoomResponse> onCreateRoom;

	public IRoomSettings settings;

	public string name;

	public CreateRoomCommand(IRoomSettings settings, string name, Action<CreateRoomCommand.CreateRoomResponse> onCreateRoom)
	{
		this.settings = settings;
		this.onCreateRoom = onCreateRoom;
		this.name = name;
	}
}
