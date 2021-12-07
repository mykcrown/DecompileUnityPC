// Decompile from assembly: Assembly-CSharp.dll

using System;

public class SetupGameCommand : GameEvent
{
	public float fadeTime
	{
		get;
		private set;
	}

	public GameLoadPayload payload
	{
		get;
		set;
	}

	public SetupGameCommand(GameLoadPayload payload, float fadeTime = -1f)
	{
		this.fadeTime = fadeTime;
		this.payload = payload;
	}
}
