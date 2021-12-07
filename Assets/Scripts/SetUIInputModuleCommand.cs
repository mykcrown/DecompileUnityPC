// Decompile from assembly: Assembly-CSharp.dll

using System;

public class SetUIInputModuleCommand : GameEvent
{
	public UIInputModuleType type;

	public SetUIInputModuleCommand(UIInputModuleType type)
	{
		this.type = type;
	}
}
