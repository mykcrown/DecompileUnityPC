// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using UnityEngine;

public class InputBindingCommand : GameEvent
{
	public PlayerAction action;

	public BindingSource binding;

	public InputBindingActionType type;

	public GameObject source;

	public InputBindingCommand(PlayerAction action, BindingSource binding, InputBindingActionType type, GameObject source)
	{
		this.action = action;
		this.binding = binding;
		this.type = type;
		this.source = source;
	}
}
