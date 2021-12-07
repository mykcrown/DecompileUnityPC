// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

public class InputBindingEvent : GameEvent
{
	public PlayerAction action;

	public BindingSource binding;

	public InputBindingActionType type;

	public InputBindingEvent(PlayerAction action, BindingSource binding, InputBindingActionType type)
	{
		this.action = action;
		this.binding = binding;
		this.type = type;
	}
}
