using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020003CE RID: 974
[Serializable]
public class SoundSettings
{
	// Token: 0x04000E65 RID: 3685
	public bool dynamicHitSfxPitch;

	// Token: 0x04000E66 RID: 3686
	public float purchaseCharacterItemDelay;

	// Token: 0x04000E67 RID: 3687
	public float doubleLevelUpDampen = 0.5f;

	// Token: 0x04000E68 RID: 3688
	public float experienceAfterLevelUpDampen = 0.5f;

	// Token: 0x04000E69 RID: 3689
	public Fixed flyoutSoundKnockback = 1;

	// Token: 0x04000E6A RID: 3690
	public Fixed flyoutSoundKnockbackSmall = 10;

	// Token: 0x04000E6B RID: 3691
	public int experiencePerTick = 1;

	// Token: 0x04000E6C RID: 3692
	public AnimationCurve tickPitchVariation = AnimationCurve.Linear(0f, 0.75f, 1f, 1.25f);

	// Token: 0x04000E6D RID: 3693
	public float soundEffectSliderSetRepeatDelay = 0.5f;

	// Token: 0x04000E6E RID: 3694
	public float spatialBlend = 0.3f;

	// Token: 0x04000E6F RID: 3695
	public float spatialBlendMinDist = 50f;

	// Token: 0x04000E70 RID: 3696
	public float spatialBlendMaxDist = 500f;

	// Token: 0x04000E71 RID: 3697
	public float randomPitchVariation = 0.05f;
}
