using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200043D RID: 1085
public class GameVFX : IGameVFX
{
	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x0600165E RID: 5726 RVA: 0x000796F6 File Offset: 0x00077AF6
	// (set) Token: 0x0600165F RID: 5727 RVA: 0x000796FE File Offset: 0x00077AFE
	[Inject]
	public IEffectHelper effectHelper { get; set; }

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06001660 RID: 5728 RVA: 0x00079707 File Offset: 0x00077B07
	// (set) Token: 0x06001661 RID: 5729 RVA: 0x0007970F File Offset: 0x00077B0F
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06001662 RID: 5730 RVA: 0x00079718 File Offset: 0x00077B18
	// (set) Token: 0x06001663 RID: 5731 RVA: 0x00079720 File Offset: 0x00077B20
	[Inject]
	public ICombatCalculator combatCalculator { get; set; }

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06001664 RID: 5732 RVA: 0x00079729 File Offset: 0x00077B29
	// (set) Token: 0x06001665 RID: 5733 RVA: 0x00079731 File Offset: 0x00077B31
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06001666 RID: 5734 RVA: 0x0007973A File Offset: 0x00077B3A
	// (set) Token: 0x06001667 RID: 5735 RVA: 0x00079742 File Offset: 0x00077B42
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06001668 RID: 5736 RVA: 0x0007974B File Offset: 0x00077B4B
	// (set) Token: 0x06001669 RID: 5737 RVA: 0x00079753 File Offset: 0x00077B53
	[Inject]
	public IUserVideoSettingsModel videoSettings { get; set; }

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x0600166A RID: 5738 RVA: 0x0007975C File Offset: 0x00077B5C
	// (set) Token: 0x0600166B RID: 5739 RVA: 0x00079764 File Offset: 0x00077B64
	[Inject]
	public UserAudioSettings audioSettings { get; set; }

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x0600166C RID: 5740 RVA: 0x0007976D File Offset: 0x00077B6D
	// (set) Token: 0x0600166D RID: 5741 RVA: 0x00079775 File Offset: 0x00077B75
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x0600166E RID: 5742 RVA: 0x0007977E File Offset: 0x00077B7E
	// (set) Token: 0x0600166F RID: 5743 RVA: 0x00079786 File Offset: 0x00077B86
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06001670 RID: 5744 RVA: 0x0007978F File Offset: 0x00077B8F
	// (set) Token: 0x06001671 RID: 5745 RVA: 0x00079797 File Offset: 0x00077B97
	public bool AllowDelayedParticles { get; private set; }

	// Token: 0x06001672 RID: 5746 RVA: 0x000797A0 File Offset: 0x00077BA0
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

	// Token: 0x06001673 RID: 5747 RVA: 0x0007980C File Offset: 0x00077C0C
	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, Vector3F overridePosition, bool shouldOverrideQualityFilter)
	{
		bool shouldOverridePosition = true;
		Vector3 overridePosition2 = (Vector3)overridePosition;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		BodyPart overrideBodyPart = BodyPart.none;
		return this.playParticle(particle, shouldOverridePosition, overridePosition2, shouldOverrideRotation, identity, overrideBodyPart, null, shouldOverrideQualityFilter, TeamNum.None);
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x00079848 File Offset: 0x00077C48
	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, Vector3 overridePosition, bool shouldOverrideQualityFilter)
	{
		bool shouldOverridePosition = true;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		BodyPart overrideBodyPart = BodyPart.none;
		return this.playParticle(particle, shouldOverridePosition, overridePosition, shouldOverrideRotation, identity, overrideBodyPart, null, shouldOverrideQualityFilter, TeamNum.None);
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x00079880 File Offset: 0x00077C80
	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, BodyPart overrideBodyPart, TeamNum teamNum)
	{
		bool shouldOverridePosition = false;
		Vector3 zero = Vector3.zero;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		return this.playParticle(particle, shouldOverridePosition, zero, shouldOverrideRotation, identity, overrideBodyPart, null, false, teamNum);
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x000798B8 File Offset: 0x00077CB8
	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, bool shouldOverrideQualityFilter, TeamNum teamNum)
	{
		bool shouldOverridePosition = false;
		Vector3 zero = Vector3.zero;
		bool shouldOverrideRotation = false;
		Quaternion identity = Quaternion.identity;
		BodyPart overrideBodyPart = BodyPart.none;
		return this.playParticle(particle, shouldOverridePosition, zero, shouldOverrideRotation, identity, overrideBodyPart, null, shouldOverrideQualityFilter, teamNum);
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x000798F0 File Offset: 0x00077CF0
	GeneratedEffect IGameVFX.PlayParticle(ParticleData particle, SkinData skinData, bool shouldOverrideQualityFilter)
	{
		return this.playParticle(particle, false, Vector3.zero, false, Quaternion.identity, BodyPart.none, skinData, shouldOverrideQualityFilter, TeamNum.None);
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x00079918 File Offset: 0x00077D18
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

	// Token: 0x06001679 RID: 5753 RVA: 0x00079984 File Offset: 0x00077D84
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

	// Token: 0x0600167A RID: 5754 RVA: 0x000799CB File Offset: 0x00077DCB
	void IGameVFX.PlayDelayedParticle(ParticlePlayData particle)
	{
		if (!this.AllowDelayedParticles)
		{
			throw new Exception("This GameVFX does not support delayed particles.  Update the GameVFX initialization to allow it and make sure to call PlayDelayedParticles every frame.");
		}
		this.delayedParticles.Add(particle);
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x000799F0 File Offset: 0x00077DF0
	void IGameVFX.PlayDelayedParticles()
	{
		foreach (ParticlePlayData particlePlayData in this.delayedParticles)
		{
			this.playParticle(particlePlayData);
		}
		this.delayedParticles.Clear();
	}

	// Token: 0x0600167C RID: 5756 RVA: 0x00079A58 File Offset: 0x00077E58
	void IGameVFX.PlayParticleList(List<ParticleData> particles, List<GeneratedEffect> generatedEffectsOut, List<Effect> effectsOut)
	{
		foreach (ParticleData particleData in particles)
		{
			if (particleData.prefab != null && particleData.IsAppropriateQualityLevel(this.videoSettings.ParticleQuality))
			{
				GeneratedEffect generatedEffect = ((IGameVFX)this).PlayParticle(particleData, false, TeamNum.None);
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

	// Token: 0x0600167D RID: 5757 RVA: 0x00079B00 File Offset: 0x00077F00
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

	// Token: 0x0600167E RID: 5758 RVA: 0x00079B6A File Offset: 0x00077F6A
	private GeneratedEffect playParticle(ParticlePlayData particlePlayData)
	{
		return this.effectHelper.PlayParticle(this.particleContext, particlePlayData);
	}

	// Token: 0x0600167F RID: 5759 RVA: 0x00079B7E File Offset: 0x00077F7E
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

	// Token: 0x06001680 RID: 5760 RVA: 0x00079BA2 File Offset: 0x00077FA2
	void IGameVFX.CreateHitEffect(IHitOwner hitInstigator, HitData hitData, Vector3F hitPosition, int hitLagFrames, ImpactType impactType, IHitOwner hitVictim)
	{
		this.createHitEffect(hitInstigator, hitData, hitPosition, hitLagFrames, impactType, hitVictim);
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x00079BB4 File Offset: 0x00077FB4
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
		foreach (HitParticleData hitParticleData in hitParticles)
		{
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

	// Token: 0x04001140 RID: 4416
	private DynamicObjectContainer dynamicObjectContainer;

	// Token: 0x04001141 RID: 4417
	private List<ParticlePlayData> delayedParticles = new List<ParticlePlayData>();

	// Token: 0x04001142 RID: 4418
	private AudioManager audio;

	// Token: 0x04001143 RID: 4419
	private ParticlePlayContext particleContext;

	// Token: 0x04001144 RID: 4420
	private ParticleData bufferData = new ParticleData();
}
