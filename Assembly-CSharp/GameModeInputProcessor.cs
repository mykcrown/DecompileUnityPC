using System;

// Token: 0x02000692 RID: 1682
public abstract class GameModeInputProcessor : IInputProcessor
{
	// Token: 0x06002987 RID: 10631 RVA: 0x000860FE File Offset: 0x000844FE
	public virtual void Start(GameModeData modeData, ConfigData config, IEvents events)
	{
		this.config = config;
		this.modeData = modeData;
		this.events = events;
	}

	// Token: 0x06002988 RID: 10632
	public abstract void ProcessInput(int currentFrame, InputController inputController, PlayerReference player, bool retainBuffer);

	// Token: 0x04001DEA RID: 7658
	protected ConfigData config;

	// Token: 0x04001DEB RID: 7659
	protected GameModeData modeData;

	// Token: 0x04001DEC RID: 7660
	protected IEvents events;
}
