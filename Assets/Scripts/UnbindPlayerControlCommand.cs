// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

public class UnbindPlayerControlCommand : GameEvent
{
	public BindingSource binding;

	public UnbindPlayerControlCommand(BindingSource binding)
	{
		this.binding = binding;
	}
}
