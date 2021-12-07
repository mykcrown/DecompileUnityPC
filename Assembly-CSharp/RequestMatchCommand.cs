using System;
using MatchMaking;

// Token: 0x020007CC RID: 1996
public class RequestMatchCommand : ConnectionEvent
{
	// Token: 0x0600316A RID: 12650 RVA: 0x000F10DA File Offset: 0x000EF4DA
	public RequestMatchCommand(EQueueTypes queueType)
	{
		this.queueType = queueType;
	}

	// Token: 0x040022A5 RID: 8869
	public EQueueTypes queueType;
}
