// Decompile from assembly: Assembly-CSharp.dll

using System;

public class FrameControlModeChangedEvent : GameEvent
{
	public FrameControlMode mode;

	public FrameControlModeChangedEvent(FrameControlMode mode)
	{
		this.mode = mode;
	}
}
