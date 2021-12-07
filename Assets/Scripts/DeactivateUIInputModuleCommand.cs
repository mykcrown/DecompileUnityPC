// Decompile from assembly: Assembly-CSharp.dll

using System;

public class DeactivateUIInputModuleCommand : GameEvent
{
	public UIInputModuleType type;

	public DeactivateUIInputModuleCommand(UIInputModuleType type)
	{
		this.type = type;
	}
}
