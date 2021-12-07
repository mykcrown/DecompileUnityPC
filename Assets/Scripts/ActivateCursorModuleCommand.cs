// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class ActivateCursorModuleCommand : GameEvent
{
	public List<IPlayerCursor> cursors;

	public ActivateCursorModuleCommand(List<IPlayerCursor> cursors)
	{
		this.cursors = cursors;
	}
}
