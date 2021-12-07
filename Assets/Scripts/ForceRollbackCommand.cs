// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ForceRollbackCommand : GameEvent
{
	public int toFrame;

	public int delta;

	public ForceRollbackCommand(int delta, int toFrame)
	{
		this.toFrame = toFrame;
		this.delta = delta;
	}
}
