using System;
using System.Collections.Generic;
using System.Reflection;
using FixedPoint;
using UnityEngine;
using Xft;

// Token: 0x0200085E RID: 2142
public class RollbackStateCodeValidator : IRollbackStateCodeValidator
{
	// Token: 0x0600357E RID: 13694 RVA: 0x000FD54D File Offset: 0x000FB94D
	public void Validate()
	{
		this.validateTypesManual();
		this.validateAllTypesWereChecked();
		this.validateAllTypesHaveAPool();
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x000FD564 File Offset: 0x000FB964
	private void validateTypesManual()
	{
		this.validateType<FrameControllerState>();
		this.validateType<InputStateSnapshot>();
		this.validateType<MoveModel>();
		this.validateType<MoveModel.MoveVisualData>();
		this.validateType<ShieldModel>();
		this.validateType<ArticleModel>();
		this.validateType<PlayerModel>();
		this.validateType<PhysicsModel>();
		this.validateType<PlayerPhysicsModel>();
		this.validateType<PlayerSpawnerState>();
		this.validateType<AnimationControllerState>();
		this.validateType<MecanimControlState>();
		this.validateType<AnimationData>();
		this.validateType<StatTrackerModel>();
		this.validateType<PlayerStats>();
		this.validateType<BoneControllerState>();
		this.validateType<Hit>();
		this.validateType<HostedHit>();
		this.validateType<TimedHostedHit>();
		this.validateType<EnvironmentBounds>();
		this.validateType<BufferedPlayerInput>();
		this.validateType<InputButtonsData>();
		this.validateType<GrabData>();
		this.validateType<HitBoxState>();
		this.validateType<RotationState>();
		this.validateType<StaleMoveQueueState>();
		this.validateType<ComboEscapeState>();
		this.validateType<CharacterRendererState>();
		this.validateType<RespawnControllerModel>();
		this.validateType<InvincibilityControllerState>();
		this.validateType<ChargeLevelComponentState>();
		this.validateType<AshaniChargeLevelComponentState>();
		this.validateType<StageSurfaceModel>();
		this.validateType<StageParticleSystemModel>();
		this.validateType<LedgeModel>();
		this.validateType<StageAnimationModel>();
		this.validateType<StageCameraInfluencerModel>();
		this.validateType<ProximityTriggerModel>();
		this.validateType<SurfaceTriggerModel>();
		this.validateType<StageBehaviourGroupModel>();
		this.validateType<StageModel>();
		this.validateType<HitsManagerModel>();
		this.validateType<ComboStateModel>();
		this.validateType<AnnouncementStatModel>();
		this.validateType<PlayerReferenceState>();
		this.validateType<GameModeState>();
		this.validateType<EndGameConditionModel>();
		this.validateType<TimeEndGameConditionModel>();
		this.validateType<TimeKeeperModel>();
		this.validateType<EffectModel>();
		this.validateType<GameManagerState>();
		this.validateType<TrailEmitterModel>();
		this.validateType<XWeaponTrailModel>();
		this.validateType<AimReticleComponentModel>();
		this.validateType<ChargeMoveComponentState>();
		this.validateType<CrewBattlePlayerSpawnerState>();
		this.validateType<DynamicObjectContainerState>();
		this.validateType<ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel>();
		this.validateType<FloatCharacterComponent.FloatCharacterComponentModel>();
		this.validateType<ResumableChargeComponent.ResumableChargeComponentModel>();
		this.validateType<CameraShakeModel>();
		this.validateType<ArticleWaveMotionComponent.ArticleWaveMotionComponentModel>();
		this.validateType<VolumeTriggerModel>();
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x000FD6EC File Offset: 0x000FBAEC
	private void validateAllTypesHaveAPool()
	{
		RollbackStatePooling rollbackStatePooling = new RollbackStatePooling();
		rollbackStatePooling.Init();
		List<Type> list = this.reflectionFindAllRequiredTypes();
		foreach (Type type in list)
		{
			if (!(type == typeof(MoveModel.MoveVisualData)) && !(type == typeof(PlayerStats)) && !(type == typeof(Hit)) && !(type == typeof(HostedHit)) && !(type == typeof(TimedHostedHit)) && !(type == typeof(EnvironmentBounds)) && !(type == typeof(BufferedPlayerInput)) && !(type == typeof(InputButtonsData)) && !(type == typeof(GrabData)) && !(type == typeof(HitBoxState)))
			{
				if (!rollbackStatePooling.HasPool(type))
				{
					throw new UnityException("!!! Rollback state error !!!! Type " + type + " does not have a pool! Please add one to RollbackStatePooling.cs -> Init()");
				}
			}
		}
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x000FD854 File Offset: 0x000FBC54
	private void validateAllTypesWereChecked()
	{
		List<Type> list = this.reflectionFindAllRequiredTypes();
		foreach (Type type in list)
		{
			if (!this.didValidate.Contains(type))
			{
				throw new UnityException("!!! Rollback state error !!!! Type " + type + " is missing from rollback state code validator!");
			}
		}
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x000FD8D4 File Offset: 0x000FBCD4
	private void validateType<T>() where T : ICopyable<T>, new()
	{
		this.didValidate.Add(typeof(T));
		T obj = Activator.CreateInstance<T>();
		T t = this.createNonDefaultTestObject<T>();
		this.validateUnequal<T>(obj, t);
		T t2 = Activator.CreateInstance<T>();
		t.CopyTo(t2);
		this.validateEqual<T>(t, t2);
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x000FD92C File Offset: 0x000FBD2C
	private bool validateUnequal<T>(T obj1, T obj2) where T : ICopyable<T>, new()
	{
		foreach (FieldInfo fieldInfo in typeof(T).GetFields())
		{
			if (!fieldInfo.IsStatic)
			{
				if (fieldInfo.GetCustomAttributes(typeof(IgnoreCopyValidation), true).Length <= 0)
				{
					object value = fieldInfo.GetValue(obj1);
					object value2 = fieldInfo.GetValue(obj2);
					if ((value == null && value2 == null) || (value != null && value.Equals(value2)) || (value2 != null && value2.Equals(value)))
					{
						throw new UnityException(string.Concat(new string[]
						{
							"!!! Rollback state error !!!! Field ",
							fieldInfo.Name,
							" on type ",
							typeof(T).Name,
							" is not setup correctly in the VALIDATOR. Please add a special case to the fillObject function."
						}));
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x000FDA24 File Offset: 0x000FBE24
	private bool validateEqual<T>(T obj1, T obj2) where T : ICopyable<T>, new()
	{
		foreach (FieldInfo fieldInfo in typeof(T).GetFields())
		{
			if (!fieldInfo.IsStatic)
			{
				if (fieldInfo.GetCustomAttributes(typeof(IgnoreCopyValidation), true).Length <= 0)
				{
					if (!fieldInfo.GetValue(obj1).Equals(fieldInfo.GetValue(obj2)))
					{
						throw new UnityException(string.Concat(new string[]
						{
							"!!! Rollback state error !!!! Field ",
							fieldInfo.Name,
							" on type ",
							typeof(T).Name,
							" is not in the COPY  function. Please add me to the CopyTo function of my class."
						}));
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x000FDAF0 File Offset: 0x000FBEF0
	private T createNonDefaultTestObject<T>() where T : ICopyable<T>, new()
	{
		ICopyable<T> copyable = Activator.CreateInstance<T>();
		this.fillObject<T>(copyable);
		return (T)((object)copyable);
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x000FDB18 File Offset: 0x000FBF18
	private void fillObject<T>(ICopyable<T> obj)
	{
		foreach (FieldInfo fieldInfo in typeof(T).GetFields())
		{
			if (!fieldInfo.IsStatic)
			{
				Type fieldType = fieldInfo.FieldType;
				if (fieldType == typeof(int) || fieldType == typeof(long))
				{
					fieldInfo.SetValue(obj, -99);
				}
				else if (fieldType == typeof(float))
				{
					fieldInfo.SetValue(obj, -99);
				}
				else if (fieldType == typeof(string))
				{
					fieldInfo.SetValue(obj, "test");
				}
				else if (fieldType == typeof(bool))
				{
					fieldInfo.SetValue(obj, true);
				}
				else if (fieldType == typeof(Fixed))
				{
					fieldInfo.SetValue(obj, -99);
				}
				else if (fieldType == typeof(Vector2F))
				{
					fieldInfo.SetValue(obj, new Vector2F(-1, -1));
				}
				else if (fieldType == typeof(Vector3F))
				{
					fieldInfo.SetValue(obj, new Vector3F(-1, -1, -1));
				}
				else if (fieldType == typeof(QuaternionF))
				{
					fieldInfo.SetValue(obj, QuaternionF.Euler(75, 75, 75));
				}
				else if (fieldType.IsEnum)
				{
					Array values = Enum.GetValues(fieldType);
					object value;
					if (values.Length > 2)
					{
						value = values.GetValue(1);
					}
					else
					{
						value = values.GetValue(values.Length - 1);
					}
					fieldInfo.SetValue(obj, value);
				}
				else if (fieldType == typeof(object))
				{
					fieldInfo.SetValue(obj, new object());
				}
			}
		}
		if (obj.GetType() == typeof(MoveModel))
		{
			MoveModel moveModel = obj as MoveModel;
			moveModel.data = ScriptableObject.CreateInstance<MoveData>();
			moveModel.hits = new List<Hit>(32);
			moveModel.seedData = default(MoveSeedData);
			moveModel.seedData.damage = 5;
			moveModel.seedData.isActive = true;
			moveModel.ChargeData = new ChargeConfig();
			moveModel.visualData = new MoveModel.MoveVisualData();
		}
		else if (obj.GetType() == typeof(PlayerModel))
		{
			PlayerModel playerModel = obj as PlayerModel;
			playerModel.bufferMoveData = ScriptableObject.CreateInstance<MoveData>();
			playerModel.bufferInterruptData = new InterruptData();
			playerModel.isActive = false;
		}
		else if (obj.GetType() == typeof(StageSurfaceModel))
		{
			StageSurfaceModel stageSurfaceModel = obj as StageSurfaceModel;
			stageSurfaceModel.collidersEnabled = false;
		}
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x000FDE68 File Offset: 0x000FC268
	private List<Type> reflectionFindAllRequiredTypes()
	{
		List<Type> list = new List<Type>();
		foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
		{
			if (typeof(ICopyable).IsAssignableFrom(type) && type != typeof(ICopyable) && type != typeof(ICopyable<>) && type != typeof(RollbackStateTyped<>) && type != typeof(StageObjectModel<>))
			{
				list.Add(type);
			}
		}
		return list;
	}

	// Token: 0x040024C5 RID: 9413
	private List<Type> didValidate = new List<Type>();
}
