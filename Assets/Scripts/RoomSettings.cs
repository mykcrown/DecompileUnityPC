// Decompile from assembly: Assembly-CSharp.dll

using System;

public class RoomSettings : IRoomSettings
{
	public bool IsVisible
	{
		get;
		set;
	}

	public byte MaxPlayers
	{
		get;
		set;
	}

	public RoomSettings(bool isVisible, byte maxPlayers)
	{
		this.IsVisible = isVisible;
		this.MaxPlayers = maxPlayers;
	}
}
