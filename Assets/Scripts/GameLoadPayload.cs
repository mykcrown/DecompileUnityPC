// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class GameLoadPayload
{
	public bool isReplay;

	public bool isOnlineGame;

	public bool customLobbyMatch;

	public BattleSettings battleConfig;

	public StageID stage;

	public bool isConfirmed;

	public StagePayloadData stagePayloadData;

	public PlayerSelectionList players;

	public Dictionary<GameMode, Dictionary<TeamNum, List<PlayerNum>>> teamPlayerMap;

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

	public GameLoadPayload Clone()
	{
		return Serialization.DeepClone<GameLoadPayload>(this);
	}
}
