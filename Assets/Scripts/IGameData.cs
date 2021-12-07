// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IGameData
{
	StageDataStore StageData
	{
		get;
	}

	GameModeDataStore GameModeData
	{
		get;
	}

	ConfigData ConfigData
	{
		get;
	}

	bool IsFeatureEnabled(FeatureID feature);
}
