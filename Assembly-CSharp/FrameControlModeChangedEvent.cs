using System;

// Token: 0x02000AF1 RID: 2801
public class FrameControlModeChangedEvent : GameEvent
{
	// Token: 0x060050DB RID: 20699 RVA: 0x00150923 File Offset: 0x0014ED23
	public FrameControlModeChangedEvent(FrameControlMode mode)
	{
		this.mode = mode;
	}

	// Token: 0x04003433 RID: 13363
	public FrameControlMode mode;
}
