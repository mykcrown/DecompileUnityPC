using System;

// Token: 0x02000AD7 RID: 2775
public class PauseSoundCommand : GameEvent
{
	// Token: 0x060050BA RID: 20666 RVA: 0x0015069D File Offset: 0x0014EA9D
	public PauseSoundCommand(SoundType type, bool paused)
	{
		this.type = type;
		this.paused = paused;
	}

	// Token: 0x04003402 RID: 13314
	public SoundType type;

	// Token: 0x04003403 RID: 13315
	public bool paused;
}
