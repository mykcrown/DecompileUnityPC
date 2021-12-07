// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class EffectHelper : IEffectHelper
{
	[Inject]
	public IUserVideoSettingsModel videoSettings
	{
		get;
		set;
	}

	public GeneratedEffect PlayParticle(ParticlePlayContext particleContext, ParticleData particle, bool shouldOverridePosition, Vector3 overridePosition, bool shouldOverrideRotation, Quaternion overrideRotation, BodyPart overrideBodyPart, SkinData skinData = null, bool shouldOverrideQualityFilter = false)
	{
		return this.PlayParticle(particleContext, new ParticlePlayData
		{
			particle = particle,
			shouldOverridePosition = shouldOverridePosition,
			overridePosition = overridePosition,
			shouldOverrideRotation = shouldOverrideRotation,
			overrideRotation = overrideRotation,
			overrideBodyPart = overrideBodyPart,
			skinData = skinData,
			shouldOverrideQualityFilter = shouldOverrideQualityFilter
		});
	}

	public GeneratedEffect PlayParticle(ParticlePlayContext particleContext, ParticlePlayData particleData)
	{
		ParticleData particle = particleData.particle;
		bool shouldOverridePosition = particleData.shouldOverridePosition;
		Vector3 overridePosition = particleData.overridePosition;
		bool shouldOverrideRotation = particleData.shouldOverrideRotation;
		Quaternion overrideRotation = particleData.overrideRotation;
		BodyPart overrideBodyPart = particleData.overrideBodyPart;
		SkinData skinData = particleData.skinData;
		bool shouldOverrideQualityFilter = particleData.shouldOverrideQualityFilter;
		if (particle == null)
		{
			return null;
		}
		if (particle.teamParticles)
		{
			if (particle.redPrefab == null || particle.bluePrefab == null)
			{
				return null;
			}
		}
		else if (particle.prefab == null)
		{
			return null;
		}
		if (particleContext.boneController == null)
		{
			UnityEngine.Debug.LogWarning("Attempted to play a particle when BodyOwner was null");
			return null;
		}
		if (!shouldOverrideQualityFilter && !particle.IsAppropriateQualityLevel(this.videoSettings.ParticleQuality))
		{
			return null;
		}
		GameObject prefab = particle.prefab;
		if (particle.switchPrefabForSkin && particle.prefabsForSkins.ContainsKey(skinData.uniqueKey) && particle.prefabsForSkins[skinData.uniqueKey] != null)
		{
			prefab = particle.prefabsForSkins[skinData.uniqueKey];
		}
		if (particle.teamParticles)
		{
			UIColor uIColorFromTeam = PlayerUtil.GetUIColorFromTeam(particleData.teamNum);
			if (uIColorFromTeam == UIColor.Blue)
			{
				prefab = particle.bluePrefab;
			}
			else if (uIColorFromTeam == UIColor.Red)
			{
				prefab = particle.redPrefab;
			}
		}
		Effect effect;
		particleContext.effectInstantiator(prefab, out effect);
		GameObject gameObject = effect.gameObject;
		gameObject.transform.position = Vector3.zero;
		ParticleFacing particleFacing = particle.particleFacing;
		if (particleFacing != ParticleFacing.MatchPlayerFacing)
		{
			if (particleFacing != ParticleFacing.MatchPlayerVelocity)
			{
				goto IL_226;
			}
			if (!(particleContext.physics.Velocity.x == 0))
			{
				this.setParticleFacing(effect, (!(particleContext.physics.Velocity.x > 0)) ? (-1) : 1);
				goto IL_226;
			}
		}
		this.setParticleFacing(effect, InputUtils.GetDirectionMultiplier(particleContext.facing.Facing));
		IL_226:
		BodyPart bodyPart = particle.bodyPart;
		if (overrideBodyPart != BodyPart.none)
		{
			bodyPart = overrideBodyPart;
		}
		if (!shouldOverridePosition)
		{
			Vector3 offSet = particle.offSet;
			bool flag = false;
			if (particleContext.facing.Facing == HorizontalDirection.Left && particleContext.physics.CurrentMove != null && particleContext.physics.CurrentMove.animationClipLeft != null)
			{
				flag = true;
			}
			if (particle.attach && bodyPart != BodyPart.none)
			{
				particleContext.boneController.AttachVFX(bodyPart, gameObject.transform, offSet, particle.offSetSpace, flag);
			}
			else
			{
				offSet.x *= (float)InputUtils.GetDirectionMultiplier(particleContext.facing.Facing);
				gameObject.transform.position += (Vector3)particleContext.boneController.GetBonePosition(bodyPart, flag);
				gameObject.transform.position += offSet;
			}
		}
		else
		{
			gameObject.transform.position = overridePosition;
		}
		if (shouldOverrideRotation)
		{
			gameObject.transform.rotation = overrideRotation;
		}
		if (particleContext.onParticleCreated != null)
		{
			particleContext.onParticleCreated(particle, gameObject);
		}
		effect.Init(particle.frames, particle, 1f);
		return new GeneratedEffect(effect, gameObject, particle);
	}

	private void setParticleFacing(Effect effect, int direction)
	{
		GameObject gameObject = effect.gameObject;
		bool flag = direction == -1;
		VFXAutoRotate autoRotateComp = effect.autoRotateComp;
		if (autoRotateComp != null)
		{
			if (flag)
			{
				if (autoRotateComp.useScaleInvertForMirror)
				{
					gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * (float)direction, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
				}
				else if (autoRotateComp.rotationMirror)
				{
					autoRotateComp.ApplyRotation = new Vector3(0f, 180f);
				}
			}
		}
		else if (flag)
		{
			Vector3 eulerAngles = gameObject.transform.localRotation.eulerAngles;
			eulerAngles.y += 180f;
			gameObject.transform.localRotation = Quaternion.Euler(eulerAngles);
		}
		VFXMirrorOffset[] vfxMirrorOffsets = effect.vfxMirrorOffsets;
		for (int i = 0; i < vfxMirrorOffsets.Length; i++)
		{
			VFXMirrorOffset vFXMirrorOffset = vfxMirrorOffsets[i];
			Vector3 localPosition = vFXMirrorOffset.transform.localPosition;
			localPosition = ((!flag) ? vFXMirrorOffset.rightOffset : vFXMirrorOffset.leftOffset);
			vFXMirrorOffset.transform.localPosition = localPosition;
		}
	}
}
