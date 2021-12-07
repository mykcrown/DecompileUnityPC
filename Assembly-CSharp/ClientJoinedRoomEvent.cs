using System;
using System.Collections.Generic;

// Token: 0x02000804 RID: 2052
[Serializable]
public class ClientJoinedRoomEvent : GameEvent
{
	// Token: 0x0400238C RID: 9100
	public List<PlayerNum> localPlayers = new List<PlayerNum>();
}
