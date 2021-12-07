// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ChangePlaybackSpeedCommand : GameEvent
{
	public ChangePlaybackSpeedType type;

	public float newSpeed;

	public ChangePlaybackSpeedCommand(ChangePlaybackSpeedType type, float newSpeed = 1f)
	{
		this.type = type;
		this.newSpeed = newSpeed;
	}
}
