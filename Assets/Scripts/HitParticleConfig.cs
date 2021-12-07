// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class HitParticleConfig : IPreloadedGameAsset
{
	public HitEffectsDictionary defaultHitEffects = new HitEffectsDictionary();

	public Fixed weakHitDamage = (Fixed)0.0;

	public Fixed mediumHitDamage = (Fixed)7.0;

	public Fixed heavyHitDamage = (Fixed)15.0;

	public int lethalHitStunFrameBuffer;

	public HitEffectsData GetEffect(HitEffectType type)
	{
		if (this.defaultHitEffects.ContainsKey(type))
		{
			return this.defaultHitEffects[type];
		}
		return null;
	}

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

	public HitEffectsData GetEffect(Fixed damage)
	{
		HitEffectType effectTypeFromDamage = this.GetEffectTypeFromDamage(damage);
		return this.GetEffect(effectTypeFromDamage);
	}

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

	public HitEffectsData GetShieldEffect(Fixed damage)
	{
		HitEffectType effectTypeFromDamage = this.GetEffectTypeFromDamage(damage);
		return this.GetShieldEffect(effectTypeFromDamage);
	}

	public void RegisterPreload(PreloadContext context)
	{
		foreach (HitEffectsData current in this.defaultHitEffects.Values)
		{
			HitParticleData[] hitParticles = current.hitParticles;
			for (int i = 0; i < hitParticles.Length; i++)
			{
				HitParticleData hitParticleData = hitParticles[i];
				if (hitParticleData != null)
				{
					hitParticleData.RegisterPreload(context);
				}
			}
		}
	}
}
