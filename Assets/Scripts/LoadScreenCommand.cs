// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LoadScreenCommand : GameEvent
{
	public ScreenType type
	{
		get;
		private set;
	}

	public Payload payload
	{
		get;
		private set;
	}

	public ScreenUpdateType updateType
	{
		get;
		private set;
	}

	public LoadScreenCommand(ScreenType type, Payload payload = null, ScreenUpdateType updateType = ScreenUpdateType.Next)
	{
		this.type = type;
		this.payload = payload;
		this.updateType = updateType;
	}
}
