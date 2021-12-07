using System;
using System.Collections.Generic;

// Token: 0x02000AC0 RID: 2752
[Serializable]
public class GameLoadPayload
{
	// Token: 0x06005095 RID: 20629 RVA: 0x001503F8 File Offset: 0x0014E7F8
	public PlayerSelectionInfo FindPlayerInfo(PlayerNum playerNum)
	{
		PlayerSelectionInfo result = null;
		for (int i = 0; i < this.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.players[i];
			if (playerSelectionInfo.playerNum == playerNum)
			{
				result = playerSelectionInfo;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005096 RID: 20630 RVA: 0x00150444 File Offset: 0x0014E844
	public GameLoadPayload Clone()
	{
		return Serialization.DeepClone<GameLoadPayload>(this);
	}

	// Token: 0x040033CD RID: 13261
	public bool isReplay;

	// Token: 0x040033CE RID: 13262
	public bool isOnlineGame;

	// Token: 0x040033CF RID: 13263
	public bool customLobbyMatch;

	// Token: 0x040033D0 RID: 13264
	public BattleSettings battleConfig;

	// Token: 0x040033D1 RID: 13265
	public StageID stage;

	// Token: 0x040033D2 RID: 13266
	public bool isConfirmed;

	// Token: 0x040033D3 RID: 13267
	public StagePayloadData stagePayloadData;

	// Token: 0x040033D4 RID: 13268
	public PlayerSelectionList players;

	// Token: 0x040033D5 RID: 13269
	public Dictionary<GameMode, Dictionary<TeamNum, List<PlayerNum>>> teamPlayerMap;
}
