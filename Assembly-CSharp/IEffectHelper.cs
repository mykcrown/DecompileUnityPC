using System;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public interface IEffectHelper
{
	// Token: 0x0600164F RID: 5711
	GeneratedEffect PlayParticle(ParticlePlayContext particleContext, ParticleData particle, bool shouldOverridePosition, Vector3 overridePosition, bool shouldOverrideRotation, Quaternion overrideRotation, BodyPart overrideBodyPart, SkinData skinData = null, bool shouldOverrideQualityFilter = false);

	// Token: 0x06001650 RID: 5712
	GeneratedEffect PlayParticle(ParticlePlayContext particleContext, ParticlePlayData particleData);
}
