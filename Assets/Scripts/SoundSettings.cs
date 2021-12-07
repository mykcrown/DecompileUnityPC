// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class SoundSettings
{
	public bool dynamicHitSfxPitch;

	public float purchaseCharacterItemDelay;

	public float doubleLevelUpDampen = 0.5f;

	public float experienceAfterLevelUpDampen = 0.5f;

	public Fixed flyoutSoundKnockback = 1;

	public Fixed flyoutSoundKnockbackSmall = 10;

	public int experiencePerTick = 1;

	public AnimationCurve tickPitchVariation = AnimationCurve.Linear(0f, 0.75f, 1f, 1.25f);

	public float soundEffectSliderSetRepeatDelay = 0.5f;

	public float spatialBlend = 0.3f;

	public float spatialBlendMinDist = 50f;

	public float spatialBlendMaxDist = 500f;

	public float randomPitchVariation = 0.05f;
}
