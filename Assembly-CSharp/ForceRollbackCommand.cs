using System;

// Token: 0x02000AF4 RID: 2804
public class ForceRollbackCommand : GameEvent
{
	// Token: 0x060050DD RID: 20701 RVA: 0x00150948 File Offset: 0x0014ED48
	public ForceRollbackCommand(int delta, int toFrame)
	{
		this.toFrame = toFrame;
		this.delta = delta;
	}

	// Token: 0x0400343A RID: 13370
	public int toFrame;

	// Token: 0x0400343B RID: 13371
	public int delta;
}
