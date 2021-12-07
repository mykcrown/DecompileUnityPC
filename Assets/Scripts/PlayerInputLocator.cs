// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PlayerInputLocator
{
	public PlayerInputManager Input
	{
		get;
		private set;
	}

	public void Set(PlayerInputManager input)
	{
		this.Input = input;
	}
}
