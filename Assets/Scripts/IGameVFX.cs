// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameVFX
{
	GeneratedEffect PlayParticle(ParticleData particle, bool shouldOverrideQualityFilter = false, TeamNum teamNum = TeamNum.None);

	GeneratedEffect PlayParticle(ParticleData particle, SkinData skinData, bool shouldOverrideQualityFilter = false);

	GeneratedEffect PlayParticle(ParticleData particle, Vector3F overridePosition, bool shouldOverrideQualityFilter = false);

	GeneratedEffect PlayParticle(ParticleData particle, Vector3 overridePosition, bool shouldOverrideQualityFilter = false);

	GeneratedEffect PlayParticle(ParticleData particle, BodyPart overrideBodyPart, TeamNum teamNum);

	GeneratedEffect PlayParticle(GameObject prefab, int frames, Vector3 position, bool shouldOverrideQualityFilter = false);

	void PlayDelayedParticle(ParticleData particle, bool shouldOverrideQualityFilter = false);

	void PlayDelayedParticle(ParticlePlayData particle);

	void PlayDelayedParticles();

	void PlayParticleList(List<ParticleData> particles, List<GeneratedEffect> generatedEffectsOut = null, List<Effect> effectsOut = null);

	void CreateHitEffect(IHitOwner hitInstigator, HitData hitData, Vector3F hitPosition, int hitLagFrames, ImpactType impactType, IHitOwner hitVictim);
}
