using System;

// Token: 0x02000AD5 RID: 2773
public class PortUnassignedPlayerEvent : GameEvent
{
	// Token: 0x060050B6 RID: 20662 RVA: 0x00150667 File Offset: 0x0014EA67
	public PortUnassignedPlayerEvent(int portId, PlayerNum playerNum)
	{
		this.portId = portId;
		this.playerNum = playerNum;
	}

	// Token: 0x040033FF RID: 13311
	public int portId;

	// Token: 0x04003400 RID: 13312
	public PlayerNum playerNum;
}
