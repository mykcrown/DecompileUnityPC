using System;

// Token: 0x02000A51 RID: 2641
[Serializable]
public class SetPlayerTeamRequest : UIEvent, IUIRequest
{
	// Token: 0x06004CFC RID: 19708 RVA: 0x00145815 File Offset: 0x00143C15
	public SetPlayerTeamRequest(PlayerNum playerNum, TeamNum teamNum, bool playSound = true)
	{
		this.playerNum = playerNum;
		this.teamNum = teamNum;
		this.playSound = playSound;
	}

	// Token: 0x04003280 RID: 12928
	public PlayerNum playerNum;

	// Token: 0x04003281 RID: 12929
	public TeamNum teamNum;

	// Token: 0x04003282 RID: 12930
	public bool playSound;
}
