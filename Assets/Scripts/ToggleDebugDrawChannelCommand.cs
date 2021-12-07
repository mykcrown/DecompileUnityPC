// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ToggleDebugDrawChannelCommand : GameEvent
{
	public DebugDrawChannel channel;

	public bool enabled;

	public ToggleDebugDrawChannelCommand(DebugDrawChannel channel, bool enabled)
	{
		this.channel = channel;
		this.enabled = enabled;
	}
}
