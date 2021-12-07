using System;
using System.Collections.Generic;

// Token: 0x02000AC2 RID: 2754
[Serializable]
public class VictoryScreenPayload : Payload
{
	// Token: 0x040033D9 RID: 13273
	public List<PlayerStats> stats = new List<PlayerStats>(8);

	// Token: 0x040033DA RID: 13274
	public List<int> endGameCharacterIndicies = new List<int>();

	// Token: 0x040033DB RID: 13275
	public bool wasForfeited;

	// Token: 0x040033DC RID: 13276
	public bool wasExited;

	// Token: 0x040033DD RID: 13277
	public List<PlayerNum> victors = new List<PlayerNum>();

	// Token: 0x040033DE RID: 13278
	public List<TeamNum> winningTeams = new List<TeamNum>();

	// Token: 0x040033DF RID: 13279
	public GameLoadPayload gamePayload;

	// Token: 0x040033E0 RID: 13280
	public ScreenType nextScreen;
}
