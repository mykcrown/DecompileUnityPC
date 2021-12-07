// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IEffectHelper
{
	GeneratedEffect PlayParticle(ParticlePlayContext particleContext, ParticleData particle, bool shouldOverridePosition, Vector3 overridePosition, bool shouldOverrideRotation, Quaternion overrideRotation, BodyPart overrideBodyPart, SkinData skinData = null, bool shouldOverrideQualityFilter = false);

	GeneratedEffect PlayParticle(ParticlePlayContext particleContext, ParticlePlayData particleData);
}
