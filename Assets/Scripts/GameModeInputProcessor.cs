// Decompile from assembly: Assembly-CSharp.dll

using System;

public abstract class GameModeInputProcessor : IInputProcessor
{
	protected ConfigData config;

	protected GameModeData modeData;

	protected IEvents events;

	public virtual void Start(GameModeData modeData, ConfigData config, IEvents events)
	{
		this.config = config;
		this.modeData = modeData;
		this.events = events;
	}

	public abstract void ProcessInput(int currentFrame, InputController inputController, PlayerReference player, bool retainBuffer);
}
