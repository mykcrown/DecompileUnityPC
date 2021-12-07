using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public interface IGameVFX
{
	// Token: 0x06001682 RID: 5762
	GeneratedEffect PlayParticle(ParticleData particle, bool shouldOverrideQualityFilter = false, TeamNum teamNum = TeamNum.None);

	// Token: 0x06001683 RID: 5763
	GeneratedEffect PlayParticle(ParticleData particle, SkinData skinData, bool shouldOverrideQualityFilter = false);

	// Token: 0x06001684 RID: 5764
	GeneratedEffect PlayParticle(ParticleData particle, Vector3F overridePosition, bool shouldOverrideQualityFilter = false);

	// Token: 0x06001685 RID: 5765
	GeneratedEffect PlayParticle(ParticleData particle, Vector3 overridePosition, bool shouldOverrideQualityFilter = false);

	// Token: 0x06001686 RID: 5766
	GeneratedEffect PlayParticle(ParticleData particle, BodyPart overrideBodyPart, TeamNum teamNum);

	// Token: 0x06001687 RID: 5767
	GeneratedEffect PlayParticle(GameObject prefab, int frames, Vector3 position, bool shouldOverrideQualityFilter = false);

	// Token: 0x06001688 RID: 5768
	void PlayDelayedParticle(ParticleData particle, bool shouldOverrideQualityFilter = false);

	// Token: 0x06001689 RID: 5769
	void PlayDelayedParticle(ParticlePlayData particle);

	// Token: 0x0600168A RID: 5770
	void PlayDelayedParticles();

	// Token: 0x0600168B RID: 5771
	void PlayParticleList(List<ParticleData> particles, List<GeneratedEffect> generatedEffectsOut = null, List<Effect> effectsOut = null);

	// Token: 0x0600168C RID: 5772
	void CreateHitEffect(IHitOwner hitInstigator, HitData hitData, Vector3F hitPosition, int hitLagFrames, ImpactType impactType, IHitOwner hitVictim);
}
