// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class FeatureToggles
{
	[SerializeField]
	private GameEnvironmentData.FeatureToggleDictionary toggles = new GameEnvironmentData.FeatureToggleDictionary(30, default(FeatureIDComparer));

	public void SetToggle(FeatureID feature, bool value)
	{
		this.toggles[feature] = ((!value) ? 0 : 1);
	}

	public void SetToggle(FeatureID feature, int value)
	{
		this.SetToggle(feature, value > 0);
	}

	public bool GetToggle(FeatureID feature)
	{
		if (GameEnvironmentData.DeprecatedFeatureToggles.Contains(feature))
		{
			UnityEngine.Debug.LogWarningFormat("Use of deprecated feature toggle '{0}'.", new object[]
			{
				feature
			});
		}
		return this.toggles != null && this.toggles.ContainsKey(feature) && this.toggles[feature] > 0;
	}
}
