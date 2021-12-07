// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

internal class ActionBinding
{
	public PlayerAction Action;

	public KeyBindingSource Key;

	public DeviceBindingSource Button;

	public ActionBinding(PlayerAction action, Key key)
	{
		this.Action = action;
		this.Key = new KeyBindingSource(new Key[]
		{
			key
		});
	}

	public ActionBinding(PlayerAction action, InputControlType button)
	{
		this.Action = action;
		this.Button = new DeviceBindingSource(button);
	}
}
