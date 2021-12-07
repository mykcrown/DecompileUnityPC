// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class ShieldConfig
{
	public Fixed maxShieldRadius = (Fixed)0.824999988079071;

	public Fixed minShieldPercent = (Fixed)0.30000001192092896;

	public Fixed maxShieldHealth = (Fixed)60.0;

	public int shieldExpandFrames = 5;

	public Fixed shieldRestoreHealth = (Fixed)20.0;

	public Fixed holdDepletionPerSecond = (Fixed)10.0;

	public Fixed regenerationPerSecond = (Fixed)8.0;

	public int maxShieldBreakFrames = 300;

	public Fixed shieldBreakFrameDamageMultiplier = -(Fixed)0.5;

	public Fixed shieldDamageMultiplier = (Fixed)1.0;

	public Fixed shieldTiltMaxDistanceX = (Fixed)0.34999999403953552;

	public Fixed shieldTiltMaxDistanceY = (Fixed)0.550000011920929;

	public Fixed shieldTiltAmountPerFrame = (Fixed)0.05000000074505806;

	public bool allowTiltDuringGust;

	public Fixed shieldRotationSpeed = (Fixed)0.0;

	public Color normalShieldColor;

	public Color lowShieldColor;

	public AnimationCurve shieldColorCurve;

	public GameObject shieldPrefab;

	public string shieldMaterialColorName = "_Color";

	public void Rescale(Fixed rescale)
	{
		this.maxShieldRadius *= rescale;
	}
}
