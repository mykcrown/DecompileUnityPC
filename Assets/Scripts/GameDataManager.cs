// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GameDataManager : IGameData
{
	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	public IGameData GameData
	{
		get;
		private set;
	}

	public StageDataStore StageData
	{
		get
		{
			return this.GameData.StageData;
		}
	}

	public GameModeDataStore GameModeData
	{
		get
		{
			return this.GameData.GameModeData;
		}
	}

	public ConfigData ConfigData
	{
		get
		{
			return this.GameData.ConfigData;
		}
	}

	public bool IsFeatureEnabled(FeatureID feature)
	{
		return this.GameData.IsFeatureEnabled(feature);
	}

	public void Initialize(ConfigData data, GameEnvironmentData environmentData)
	{
		this.GameData = new GameData(data, environmentData, this.localization);
	}
}
