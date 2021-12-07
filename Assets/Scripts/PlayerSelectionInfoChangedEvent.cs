// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PlayerSelectionInfoChangedEvent : UIEvent
{
	public PlayerSelectionInfo info;

	public PlayerSelectionInfoChangedEvent(PlayerSelectionInfo info)
	{
		this.info = info;
	}
}
