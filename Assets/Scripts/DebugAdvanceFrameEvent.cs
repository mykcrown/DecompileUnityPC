// Decompile from assembly: Assembly-CSharp.dll

using System;

public class DebugAdvanceFrameEvent : GameEvent
{
	public int frameCount;

	public DebugAdvanceFrameEvent(int frameCount = 1)
	{
		this.frameCount = frameCount;
	}
}
