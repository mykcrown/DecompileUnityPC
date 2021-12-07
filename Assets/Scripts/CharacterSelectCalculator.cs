// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CharacterSelectCalculator
{
	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	public int GetMaxPlayers()
	{
		if (this.enterNewGame.GamePayload == null)
		{
			return 999999;
		}
		GameModeData dataByType = this.gameDataManager.GameModeData.GetDataByType(this.enterNewGame.GamePayload.battleConfig.mode);
		return dataByType.settings.maxPlayerCount;
	}
}
