using System;
using FixedPoint;

// Token: 0x020003EC RID: 1004
[Serializable]
public class HitParticleConfig : IPreloadedGameAsset
{
	// Token: 0x0600158D RID: 5517 RVA: 0x00076AAE File Offset: 0x00074EAE
	public HitEffectsData GetEffect(HitEffectType type)
	{
		if (this.defaultHitEffects.ContainsKey(type))
		{
			return this.defaultHitEffects[type];
		}
		return null;
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x00076ACF File Offset: 0x00074ECF
	public HitEffectType GetEffectTypeFromDamage(Fixed damage)
	{
		if (damage > this.heavyHitDamage)
		{
			return HitEffectType.Heavy;
		}
		if (damage > this.mediumHitDamage)
		{
			return HitEffectType.Medium;
		}
		if (damage > this.weakHitDamage)
		{
			return HitEffectType.Light;
		}
		return HitEffectType.None;
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x00076B0C File Offset: 0x00074F0C
	public HitEffectsData GetEffect(Fixed damage)
	{
		HitEffectType effectTypeFromDamage = this.GetEffectTypeFromDamage(damage);
		return this.GetEffect(effectTypeFromDamage);
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x00076B28 File Offset: 0x00074F28
	public HitEffectsData GetShieldEffect(HitEffectType type)
	{
		switch (type)
		{
		case HitEffectType.Light:
			return this.GetEffect(HitEffectType.ShieldLight);
		case HitEffectType.Medium:
			return this.GetEffect(HitEffectType.ShieldMedium);
		case HitEffectType.MediumHeavy:
			return this.GetEffect(HitEffectType.ShieldMediumHeavy);
		case HitEffectType.Heavy:
			return this.GetEffect(HitEffectType.ShieldHeavy);
		default:
			return null;
		}
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x00076B78 File Offset: 0x00074F78
	public HitEffectsData GetShieldEffect(Fixed damage)
	{
		HitEffectType effectTypeFromDamage = this.GetEffectTypeFromDamage(damage);
		return this.GetShieldEffect(effectTypeFromDamage);
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x00076B94 File Offset: 0x00074F94
	public void RegisterPreload(PreloadContext context)
	{
		foreach (HitEffectsData hitEffectsData in this.defaultHitEffects.Values)
		{
			foreach (HitParticleData hitParticleData in hitEffectsData.hitParticles)
			{
				if (hitParticleData != null)
				{
					hitParticleData.RegisterPreload(context);
				}
			}
		}
	}

	// Token: 0x04000F79 RID: 3961
	public HitEffectsDictionary defaultHitEffects = new HitEffectsDictionary();

	// Token: 0x04000F7A RID: 3962
	public Fixed weakHitDamage = (Fixed)0.0;

	// Token: 0x04000F7B RID: 3963
	public Fixed mediumHitDamage = (Fixed)7.0;

	// Token: 0x04000F7C RID: 3964
	public Fixed heavyHitDamage = (Fixed)15.0;

	// Token: 0x04000F7D RID: 3965
	public int lethalHitStunFrameBuffer;
}
