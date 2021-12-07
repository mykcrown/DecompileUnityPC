using System;
using UnityEngine;

// Token: 0x0200040C RID: 1036
[Serializable]
public class FeatureToggles
{
	// Token: 0x060015B7 RID: 5559 RVA: 0x000773BA File Offset: 0x000757BA
	public void SetToggle(FeatureID feature, bool value)
	{
		this.toggles[feature] = ((!value) ? 0 : 1);
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x000773D5 File Offset: 0x000757D5
	public void SetToggle(FeatureID feature, int value)
	{
		this.SetToggle(feature, value > 0);
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x000773EC File Offset: 0x000757EC
	public bool GetToggle(FeatureID feature)
	{
		if (GameEnvironmentData.DeprecatedFeatureToggles.Contains(feature))
		{
			Debug.LogWarningFormat("Use of deprecated feature toggle '{0}'.", new object[]
			{
				feature
			});
		}
		return this.toggles != null && this.toggles.ContainsKey(feature) && this.toggles[feature] > 0;
	}

	// Token: 0x04001082 RID: 4226
	[SerializeField]
	private GameEnvironmentData.FeatureToggleDictionary toggles = new GameEnvironmentData.FeatureToggleDictionary(30, default(FeatureIDComparer));
}
