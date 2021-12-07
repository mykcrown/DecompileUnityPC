// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ToggleUIVisibilityCommand : GameEvent
{
	public bool visibility;

	public ToggleUIVisibilityCommand(bool visibility)
	{
		this.visibility = visibility;
	}
}
