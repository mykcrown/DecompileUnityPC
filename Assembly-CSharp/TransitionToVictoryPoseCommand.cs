using System;

// Token: 0x02000ACB RID: 2763
public class TransitionToVictoryPoseCommand : GameEvent
{
	// Token: 0x060050AC RID: 20652 RVA: 0x001505AD File Offset: 0x0014E9AD
	public TransitionToVictoryPoseCommand(VictoryScreenPayload payload)
	{
		this.Payload = payload;
	}

	// Token: 0x040033F0 RID: 13296
	public VictoryScreenPayload Payload;
}
