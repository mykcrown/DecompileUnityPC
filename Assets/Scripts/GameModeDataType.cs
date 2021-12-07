// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GameModeDataType : Attribute
{
	public readonly GameMode type;

	public GameModeDataType(GameMode type)
	{
		this.type = type;
	}
}
