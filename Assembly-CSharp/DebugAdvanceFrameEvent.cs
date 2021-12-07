using System;

// Token: 0x02000AF7 RID: 2807
public class DebugAdvanceFrameEvent : GameEvent
{
	// Token: 0x060050E0 RID: 20704 RVA: 0x0015096E File Offset: 0x0014ED6E
	public DebugAdvanceFrameEvent(int frameCount = 1)
	{
		this.frameCount = frameCount;
	}

	// Token: 0x0400343C RID: 13372
	public int frameCount;
}
