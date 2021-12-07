// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameEnvironmentData : ScriptableObject
{
	[Serializable]
	public class FeatureToggleDictionary : SerializableDictionary<FeatureID, int>
	{
		public FeatureToggleDictionary(int capacity, IEqualityComparer<FeatureID> comparer) : base(capacity, comparer)
		{
		}
	}

	public static readonly string GAME_ENVIRONMENT_DATA_PATH = "Assets/Wavedash/Resources/Config/GameEnvironmentData.asset";

	public List<CharacterDefinition> characters = new List<CharacterDefinition>();

	public List<GameModeData> gameModes = new List<GameModeData>();

	public List<StageData> stages = new List<StageData>();

	public FeatureToggles toggles = new FeatureToggles();

	public static readonly HashSet<FeatureID> DeprecatedFeatureToggles = new HashSet<FeatureID>
	{
		FeatureID.PlayerProgression,
		FeatureID.HitFXSync,
		FeatureID.DetailQualitySettings,
		FeatureID.Taunts,
		FeatureID.HiddenProps,
		FeatureID.StoreCollectiblesTab,
		FeatureID.PlayerCardLevel,
		FeatureID.RightStickGallerySpin,
		FeatureID.Netsuke,
		FeatureID.DevManualStore
	};

	public static GameEnvironmentData Load()
	{
		return Resources.Load<GameEnvironmentData>("Config/GameEnvironmentData");
	}
}
