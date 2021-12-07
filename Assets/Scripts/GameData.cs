// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GameData : IGameData
{
	private FeatureToggles featureToggles;

	public StageDataStore StageData
	{
		get;
		private set;
	}

	public GameModeDataStore GameModeData
	{
		get;
		private set;
	}

	public ConfigData ConfigData
	{
		get;
		private set;
	}

	public GameData(ConfigData data, GameEnvironmentData environment, ILocalization localization = null)
	{
		this.StageData = new StageDataStore(localization);
		this.GameModeData = new GameModeDataStore(localization);
		this.ConfigData = data;
		this.StageData.Load(environment.stages);
		this.GameModeData.Load(environment.gameModes);
		this.featureToggles = environment.toggles;
	}

	public bool IsFeatureEnabled(FeatureID feature)
	{
		return this.featureToggles.GetToggle(feature);
	}
}
