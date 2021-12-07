using System;

// Token: 0x02000ACC RID: 2764
[Serializable]
public class SetPlayerTypeRequest : GameEvent, IUIRequest
{
	// Token: 0x060050AD RID: 20653 RVA: 0x001505BC File Offset: 0x0014E9BC
	public SetPlayerTypeRequest(PlayerNum playerNum, PlayerType playerType, bool playSound = true)
	{
		this.playerNum = playerNum;
		this.playerType = playerType;
		this.playSound = playSound;
	}

	// Token: 0x040033F1 RID: 13297
	public PlayerNum playerNum;

	// Token: 0x040033F2 RID: 13298
	public PlayerType playerType;

	// Token: 0x040033F3 RID: 13299
	public bool playSound;
}
