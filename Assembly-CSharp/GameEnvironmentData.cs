using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000408 RID: 1032
[Serializable]
public class GameEnvironmentData : ScriptableObject
{
	// Token: 0x060015B1 RID: 5553 RVA: 0x000772F0 File Offset: 0x000756F0
	public static GameEnvironmentData Load()
	{
		return Resources.Load<GameEnvironmentData>("Config/GameEnvironmentData");
	}

	// Token: 0x0400105F RID: 4191
	public static readonly string GAME_ENVIRONMENT_DATA_PATH = "Assets/Wavedash/Resources/Config/GameEnvironmentData.asset";

	// Token: 0x04001060 RID: 4192
	public List<CharacterDefinition> characters = new List<CharacterDefinition>();

	// Token: 0x04001061 RID: 4193
	public List<GameModeData> gameModes = new List<GameModeData>();

	// Token: 0x04001062 RID: 4194
	public List<StageData> stages = new List<StageData>();

	// Token: 0x04001063 RID: 4195
	public FeatureToggles toggles = new FeatureToggles();

	// Token: 0x04001064 RID: 4196
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

	// Token: 0x02000409 RID: 1033
	[Serializable]
	public class FeatureToggleDictionary : SerializableDictionary<FeatureID, int>
	{
		// Token: 0x060015B3 RID: 5555 RVA: 0x00077377 File Offset: 0x00075777
		public FeatureToggleDictionary(int capacity, IEqualityComparer<FeatureID> comparer) : base(capacity, comparer)
		{
		}
	}
}
