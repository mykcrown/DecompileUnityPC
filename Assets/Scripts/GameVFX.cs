// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameVFX : IGameVFX
{
	private DynamicObjectContainer dynamicObjectContainer;

	private List<ParticlePlayData> delayedParticles = new List<ParticlePlayData>();

	private AudioManager audio;

	private ParticlePlayContext particleContext;

	private ParticleData bufferData = new ParticleData();

	[Inject]
	public IEffectHelper effectHelper
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public ICombatCalculator combatCalculator
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public IUserVideoSettingsModel videoSettings
	{
		get;
		set;
	}

	[Inject]
	public UserAudioSettings audioSettings
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	public bool AllowDelayedParticles
	{
		get;
		private set;
	}

	public IGameVFX Initialize(DynamicObjectContainer dynamicObjectContainer, IBoneController boneController, IFacing facing, IPhysicsStateOwner physics, ConfigData config, AudioManager audio, bool allowDelayedParticles, Action<ParticleData, GameObject> onParticleCreated = null)
	{
		this.particleContext = new ParticlePlayContext
		{
			effectInstantiator = new ParticlePlayContext.EffectInstantiator(this.generatePooledEffectPrefab),
			physics = physics,
			facing = facing,
			boneController = boneController,
			onParticleCreated = onParticleCreated
		};
		this.audio = audio;
		this.dynamicObjectContainer = dynamicObjectContainer;
		this.AllowDelayedParticles = allowDelayedParticles;
		return this;
	}

	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, Vector3F overridePosition, bool shouldOverrideQualityFilter)
	{
		bool shouldOverridePosition = true;
		Vector3 overridePosition2 = (Vector3)overridePosition;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		BodyPart overrideBodyPart = BodyPart.none;
		return this.playParticle(particle, shouldOverridePosition, overridePosition2, shouldOverrideRotation, identity, overrideBodyPart, null, shouldOverrideQualityFilter, TeamNum.None);
	}

	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, Vector3 overridePosition, bool shouldOverrideQualityFilter)
	{
		bool shouldOverridePosition = true;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		BodyPart overrideBodyPart = BodyPart.none;
		return this.playParticle(particle, shouldOverridePosition, overridePosition, shouldOverrideRotation, identity, overrideBodyPart, null, shouldOverrideQualityFilter, TeamNum.None);
	}

	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, BodyPart overrideBodyPart, TeamNum teamNum)
	{
		bool shouldOverridePosition = false;
		Vector3 zero = Vector3.zero;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		return this.playParticle(particle, shouldOverridePosition, zero, shouldOverrideRotation, identity, overrideBodyPart, null, false, teamNum);
	}

	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, bool shouldOverrideQualityFilter, TeamNum teamNum)
	{
		bool shouldOverridePosition = false;
		Vector3 zero = Vector3.zero;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		BodyPart overrideBodyPart = BodyPart.none;
		return this.playParticle(particle, shouldOverridePosition, zero, shouldOverrideRotation, identity, overrideBodyPart, null, shouldOverrideQualityFilter, teamNum);
	}

	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, SkinData skinData, bool shouldOverrideQualityFilter)
	{
		return this.playParticle(particle, false, Vector3.zero, false, Quaternion.identity, BodyPart.none, skinData, shouldOverrideQualityFilter, TeamNum.None);
	}

	GeneratedEffect IGameVFX.PlayParticle(GameObject prefab, int frames, Vector3 position, bool shouldOverrideQualityFilter)
	{
		this.bufferData.attach = false;
		this.bufferData.bodyPart = BodyPart.none;
		this.bufferData.prefab = prefab;
		this.bufferData.frames = frames;
		ParticleData particle = this.bufferData;
		bool shouldOverridePosition = true;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		BodyPart overrideBodyPart = BodyPart.none;
		return this.playParticle(particle, shouldOverridePosition, position, shouldOverrideRotation, identity, overrideBodyPart, null, shouldOverrideQualityFilter, TeamNum.None);
	}

	void IGameVFX.PlayDelayedParticle(ParticleData particle, bool shouldOverrideQualityFilter)
	{
		if (!this.AllowDelayedParticles)
		{
			throw new Exception("This GameVFX does not support delayed particles.  Update the GameVFX initialization to allow it and make sure to call PlayDelayedParticles every frame.");
		}
		this.delayedParticles.Add(new ParticlePlayData
		{
			particle = particle,
			shouldOverrideQualityFilter = shouldOverrideQualityFilter
		});
	}

	void IGameVFX.PlayDelayedParticle(ParticlePlayData particle)
	{
		if (!this.AllowDelayedParticles)
		{
			throw new Exception("This GameVFX does not support delayed particles.  Update the GameVFX initialization to allow it and make sure to call PlayDelayedParticles every frame.");
		}
		this.delayedParticles.Add(particle);
	}

	void IGameVFX.PlayDelayedParticles()
	{
		foreach (ParticlePlayData current in this.delayedParticles)
		{
			this.playParticle(current);
		}
		this.delayedParticles.Clear();
	}

	void IGameVFX.PlayParticleList(List<ParticleData> particles, List<GeneratedEffect> generatedEffectsOut, List<Effect> effectsOut)
	{
		foreach (ParticleData current in particles)
		{
			if (current.prefab != null && current.IsAppropriateQualityLevel(this.videoSettings.ParticleQuality))
			{
				GeneratedEffect generatedEffect = ((IGameVFX)this).PlayParticle(current, false, TeamNum.None);
				if (generatedEffect != null)
				{
					if (generatedEffectsOut != null)
					{
						generatedEffectsOut.Add(generatedEffect);
					}
					if (effectsOut != null)
					{
						effectsOut.Add(generatedEffect.EffectController);
					}
				}
			}
		}
	}

	private GeneratedEffect playParticle(ParticleData particle, bool shouldOverridePosition, Vector3 overridePosition, bool shouldOverrideRotation, Quaternion overrideRotation, BodyPart overrideBodyPart, SkinData skinData = null, bool shouldOverrideQualityFilter = false, TeamNum teamNum = TeamNum.None)
	{
		return this.playParticle(new ParticlePlayData
		{
			particle = particle,
			shouldOverridePosition = shouldOverridePosition,
			overridePosition = overridePosition,
			shouldOverrideRotation = shouldOverrideRotation,
			overrideRotation = overrideRotation,
			overrideBodyPart = overrideBodyPart,
			skinData = skinData,
			shouldOverrideQualityFilter = shouldOverrideQualityFilter,
			teamNum = teamNum
		});
	}

	private GeneratedEffect playParticle(ParticlePlayData particlePlayData)
	{
		return this.effectHelper.PlayParticle(this.particleContext, particlePlayData);
	}

	private bool generatePooledEffectPrefab(GameObject prefab, out Effect effect)
	{
		if (prefab == null)
		{
			effect = null;
			return false;
		}
		effect = this.dynamicObjectContainer.InstantiateDynamicObject<Effect>(prefab, 4, true);
		return true;
	}

	void IGameVFX.CreateHitEffect(IHitOwner hitInstigator, HitData hitData, Vector3F hitPosition, int hitLagFrames, ImpactType impactType, IHitOwner hitVictim)
	{
		this.createHitEffect(hitInstigator, hitData, hitPosition, hitLagFrames, impactType, hitVictim);
	}

	private void createHitEffect(IHitOwner hitInstigator, HitData hitData, Vector3F hitPosition, int hitLagFrames, ImpactType impactType, IHitOwner hitVictim)
	{
		HitParticleData[] hitParticles = hitData.hitParticles;
		HitEffectsData hitEffectsData = null;
		if (impactType == ImpactType.Shield)
		{
			if (hitData.hitEffectType == HitEffectType.Auto || hitData.hitEffectType == HitEffectType.Grab)
			{
				hitEffectsData = this.config.hitConfig.GetShieldEffect((Fixed)((double)hitData.damage));
			}
			else
			{
				hitEffectsData = this.config.hitConfig.GetShieldEffect(hitData.hitEffectType);
			}
		}
		else if (!hitData.overrideHitParticle)
		{
			if (hitData.hitEffectType == HitEffectType.Auto)
			{
				hitEffectsData = this.config.hitConfig.GetEffect((Fixed)((double)hitData.damage));
			}
			else
			{
				if (impactType == ImpactType.Grab && hitVictim is IPlayerDelegate && (hitInstigator as IPlayerDelegate).GrabData.victimUnderChainGrabPrevention)
				{
					hitEffectsData = this.config.hitConfig.GetEffect(HitEffectType.ChainGrab);
				}
				if (hitEffectsData == null)
				{
					hitEffectsData = this.config.hitConfig.GetEffect(hitData.hitEffectType);
				}
			}
		}
		if (hitVictim.ArmorResistsHit(hitData, hitInstigator, hitPosition))
		{
			HitEffectType hitEffectType = hitData.hitEffectType;
			if (hitEffectType == HitEffectType.Auto)
			{
				hitEffectType = this.config.hitConfig.GetEffectTypeFromDamage((Fixed)((double)hitData.damage));
			}
			switch (hitEffectType)
			{
			case HitEffectType.Light:
				hitEffectsData = hitVictim.ActiveMove.Data.armorOptions.LightHitEffects;
				break;
			case HitEffectType.Medium:
				hitEffectsData = hitVictim.ActiveMove.Data.armorOptions.MediumHitEffects;
				break;
			case HitEffectType.MediumHeavy:
				hitEffectsData = hitVictim.ActiveMove.Data.armorOptions.MediumHeavyHitEffects;
				break;
			case HitEffectType.Heavy:
				hitEffectsData = hitVictim.ActiveMove.Data.armorOptions.HeavyHitEffects;
				break;
			default:
				hitEffectsData = null;
				break;
			}
		}
		if (hitEffectsData != null)
		{
			hitParticles = hitEffectsData.hitParticles;
		}
		Vector3 hitParticleOffset = hitData.hitParticleOffset;
		hitParticleOffset.x *= (float)InputUtils.GetDirectionMultiplier(hitVictim.Facing);
		int num;
		this.combatCalculator.CheckReverseHit(hitData, hitInstigator, hitVictim, out num);
		HitParticleData[] array = hitParticles;
		for (int i = 0; i < array.Length; i++)
		{
			HitParticleData hitParticleData = array[i];
			if (hitParticleData != null)
			{
				if (hitParticleData.hitParticle != null && hitParticleData.IsAppropriateQualityLevel(this.videoSettings.ParticleQuality))
				{
					Effect effect;
					this.generatePooledEffectPrefab(hitParticleData.hitParticle, out effect);
					if (effect != null)
					{
						BodyPart bodyPart = BodyPart.none;
						if (hitVictim is PlayerController)
						{
							if (hitParticleData.overrideAttachPoint)
							{
								bodyPart = hitParticleData.AttachPoint;
							}
							else
							{
								bodyPart = hitData.AttachToBodyPart;
							}
						}
						if (bodyPart != BodyPart.none)
						{
							PlayerController playerController = hitVictim as PlayerController;
							playerController.Body.AttachVFX(bodyPart, effect.transform, hitParticleOffset, hitData.hitParticleOffsetSpace, false);
							if (bodyPart == BodyPart.shield)
							{
								if (hitParticleData.scaleWithShield)
								{
									effect.transform.localScale = Vector3.one * (float)playerController.Shield.ShieldPercentage;
								}
								else
								{
									effect.transform.localScale = Vector3.one;
								}
							}
						}
						else
						{
							effect.transform.position = (Vector3)hitPosition;
							Vector3 localScale = effect.transform.localScale;
							localScale.x = Mathf.Abs(localScale.x) * (float)num;
							effect.transform.localScale = localScale;
							float z;
							if (hitParticleData.directionMode == HitParticleData.DirectionMode.KNOCKBACK)
							{
								z = hitData.knockbackAngle * (float)num;
							}
							else
							{
								Vector2 from = (Vector2)hitVictim.Position - (Vector2)hitInstigator.Position;
								float num2 = Vector2.Angle(from, Vector2.right);
								float num3 = hitData.knockbackAngle;
								if (num == -1)
								{
									num3 = 180f - num3;
								}
								float num4 = Mathf.DeltaAngle(num3, num2);
								if (num3 >= num2)
								{
									if (num3 - num4 == num2)
									{
										num2 += num4 / 2f;
									}
									else
									{
										num2 -= num4 / 2f;
									}
								}
								else if (num2 - num4 == num3)
								{
									num2 -= num4 / 2f;
								}
								else
								{
									num2 += num4 / 2f;
								}
								if (num == -1)
								{
									z = num2 - 180f;
								}
								else
								{
									z = num2;
								}
							}
							effect.transform.rotation = Quaternion.Euler(0f, 0f, z);
						}
						effect.Init(hitParticleData.particleFrames, null, 1f);
					}
				}
				AudioData data;
				if (this.audioSettings.UseAltSounds() && hitParticleData.altSound.sound != null)
				{
					data = hitParticleData.altSound;
					if (hitParticleData.isSoundOverridable && hitData.overrideAltImpactSound)
					{
						data = hitData.altImpactSound;
					}
				}
				else
				{
					data = hitParticleData.hitSound;
					if (hitParticleData.isSoundOverridable && hitData.overrideImpactSound)
					{
						data = hitData.impactSound;
					}
				}
				if (data.sound != null)
				{
					this.audio.PlayGameSound(new AudioRequest(data, (Vector3)hitPosition, null));
				}
			}
		}
	}
}
