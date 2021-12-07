// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PlayAnnouncementCommand : GameEvent
{
	public string AnnouncementType
	{
		get;
		private set;
	}

	public PlayAnnouncementCommand(string type)
	{
		this.AnnouncementType = type;
	}
}
