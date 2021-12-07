// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Xft;

public class RollbackStateCodeValidator : IRollbackStateCodeValidator
{
	private List<Type> didValidate = new List<Type>();

	public void Validate()
	{
		this.validateTypesManual();
		this.validateAllTypesWereChecked();
		this.validateAllTypesHaveAPool();
	}

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

	private void validateAllTypesHaveAPool()
	{
		RollbackStatePooling rollbackStatePooling = new RollbackStatePooling();
		rollbackStatePooling.Init();
		List<Type> list = this.reflectionFindAllRequiredTypes();
		foreach (Type current in list)
		{
			if (!(current == typeof(MoveModel.MoveVisualData)) && !(current == typeof(PlayerStats)) && !(current == typeof(Hit)) && !(current == typeof(HostedHit)) && !(current == typeof(TimedHostedHit)) && !(current == typeof(EnvironmentBounds)) && !(current == typeof(BufferedPlayerInput)) && !(current == typeof(InputButtonsData)) && !(current == typeof(GrabData)) && !(current == typeof(HitBoxState)))
			{
				if (!rollbackStatePooling.HasPool(current))
				{
					throw new UnityException("!!! Rollback state error !!!! Type " + current + " does not have a pool! Please add one to RollbackStatePooling.cs -> Init()");
				}
			}
		}
	}

	private void validateAllTypesWereChecked()
	{
		List<Type> list = this.reflectionFindAllRequiredTypes();
		foreach (Type current in list)
		{
			if (!this.didValidate.Contains(current))
			{
				throw new UnityException("!!! Rollback state error !!!! Type " + current + " is missing from rollback state code validator!");
			}
		}
	}

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

	private bool validateUnequal<T>(T obj1, T obj2) where T : ICopyable<T>, new()
	{
		FieldInfo[] fields = typeof(T).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo fieldInfo = fields[i];
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

	private bool validateEqual<T>(T obj1, T obj2) where T : ICopyable<T>, new()
	{
		FieldInfo[] fields = typeof(T).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo fieldInfo = fields[i];
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

	private T createNonDefaultTestObject<T>() where T : ICopyable<T>, new()
	{
		ICopyable<T> copyable = Activator.CreateInstance<T>();
		this.fillObject<T>(copyable);
		return (T)((object)copyable);
	}

	private void fillObject<T>(ICopyable<T> obj)
	{
		FieldInfo[] fields = typeof(T).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo fieldInfo = fields[i];
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

	private List<Type> reflectionFindAllRequiredTypes()
	{
		List<Type> list = new List<Type>();
		Type[] types = Assembly.GetExecutingAssembly().GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			Type type = types[i];
			if (typeof(ICopyable).IsAssignableFrom(type) && type != typeof(ICopyable) && type != typeof(ICopyable<>) && type != typeof(RollbackStateTyped<>) && type != typeof(StageObjectModel<>))
			{
				list.Add(type);
			}
		}
		return list;
	}
}
