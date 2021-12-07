// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PauseSoundCommand : GameEvent
{
	public SoundType type;

	public bool paused;

	public PauseSoundCommand(SoundType type, bool paused)
	{
		this.type = type;
		this.paused = paused;
	}
}
