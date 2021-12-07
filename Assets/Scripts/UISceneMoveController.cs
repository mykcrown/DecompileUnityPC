// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Xft;

public class UISceneMoveController : IMoveOwner
{
	public class UICharacterFacing : IFacing
	{
		public HorizontalDirection Facing
		{
			get
			{
				return HorizontalDirection.Right;
			}
		}

		public HorizontalDirection OppositeFacing
		{
			get
			{
				return HorizontalDirection.None;
			}
		}
	}

	public class UICharacterPhysicsStateOwner : IPhysicsStateOwner
	{
		private Func<MoveData> currentMoveGetter;

		public MoveData CurrentMove
		{
			get
			{
				return this.currentMoveGetter();
			}
		}

		public PhysicsOverride PhysicsOverride
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Vector3F Velocity
		{
			get
			{
				return Vector3F.zero;
			}
		}

		public UICharacterPhysicsStateOwner(Func<MoveData> currentMoveGetter)
		{
			this.currentMoveGetter = currentMoveGetter;
		}
	}

	private SkinData skinData;

	private Transform effectContainer;

	private BoneController boneController;

	private MaterialAnimationsController matAnimsController;

	private MoveData currentMove;

	private int currentFrame;

	private ParticlePlayContext particleContext;

	private List<Effect> spawnedEffects = new List<Effect>();

	private List<AudioReference> spawnedSoundHandles = new List<AudioReference>();

	private List<Effect> willTick = new List<Effect>(30);

	private List<Effect> willDestroy = new List<Effect>(30);

	private Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap = new Dictionary<WeaponTrailData, XWeaponTrail>();

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public IEffectHelper effectHelper
	{
		get;
		set;
	}

	[Inject]
	public IWeaponTrailHelper weaponTrailHelper
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
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public MoveData MoveData
	{
		get
		{
			return this.currentMove;
		}
	}

	public bool MoveIsValid
	{
		get
		{
			return this.currentMove != null;
		}
	}

	public int InternalFrame
	{
		get
		{
			return this.currentFrame;
		}
	}

	public PlayerController Player
	{
		get
		{
			return null;
		}
	}

	public void Init(CharacterMenusData characterMenusData, SkinData skinData, GameObject characterModel, IAnimationPlayer animationPlayer, Animator animator, Transform effectContainer)
	{
		this.skinData = skinData;
		this.effectContainer = effectContainer;
		BoneData component = characterModel.GetComponent<BoneData>();
		IAnimationDataSource animationDataSource = new RawAnimationDataSource(component, animator);
		this.boneController = new BoneController();
		this.injector.Inject(this.boneController);
		this.boneController.Init(null, component, animationDataSource, new DummyOrientationController(Vector3F.zero), animationPlayer, new DummyPositionOwner(Vector3F.zero), characterMenusData.bounds.rotationCenterOffset, characterModel.transform, null);
		IFacing facing = new UISceneMoveController.UICharacterFacing();
		IPhysicsStateOwner physics = new UISceneMoveController.UICharacterPhysicsStateOwner(new Func<MoveData>(this._Init_m__0));
		this.particleContext = new ParticlePlayContext
		{
			effectInstantiator = new ParticlePlayContext.EffectInstantiator(this.createEffect),
			boneController = this.boneController,
			facing = facing,
			physics = physics,
			onParticleCreated = null
		};
		MaterialTargetsData orAddComponent = characterModel.GetOrAddComponent<MaterialTargetsData>();
		this.matAnimsController = this.injector.GetInstance<MaterialAnimationsController>();
		this.matAnimsController.Init(this, orAddComponent);
		this.signalBus.GetSignal<EffectReleaseSignal>().AddListener(new Action<Effect>(this.onEffectReleased));
	}

	public void Destroy()
	{
		this.clearEffects();
		if (this.boneController != null)
		{
			this.boneController.Destroy();
		}
		this.signalBus.GetSignal<EffectReleaseSignal>().RemoveListener(new Action<Effect>(this.onEffectReleased));
	}

	private void onEffectReleased(Effect theEffect)
	{
		this.spawnedEffects.Remove(theEffect);
	}

	public void SetMove(MoveData moveData)
	{
		if (this.currentMove != null)
		{
			this.OnMoveEnd();
		}
		this.currentMove = moveData;
		this.currentFrame = 0;
	}

	public void OnMoveEnd()
	{
		this.signalBus.GetSignal<PlayerController.InteractionSignal>().Dispatch(new PlayerController.InteractionSignalData(null, PlayerController.InteractionSignalData.Type.MoveEnd));
		this.currentMove = null;
		this.clearEffects();
	}

	public void Update(int moveFrame)
	{
		while (this.currentFrame <= moveFrame)
		{
			this.updateVFX();
			this.updateSFX();
			this.updateWeaponTrails();
			this.updateMaterialAnimations();
			this.boneController.TickFrame();
			this.currentFrame++;
		}
	}

	private void updateVFX()
	{
		if (this.currentMove != null)
		{
			MoveParticleEffect[] particleEffects = this.currentMove.particleEffects;
			for (int i = 0; i < particleEffects.Length; i++)
			{
				MoveParticleEffect moveParticleEffect = particleEffects[i];
				if (moveParticleEffect.particleEffect.HasPrefab() && this.currentFrame == moveParticleEffect.frame)
				{
					ParticleData particleEffect = moveParticleEffect.particleEffect;
					bool shouldOverridePosition = false;
					Vector3 overridePosition = Vector3.zero;
					if (!particleEffect.attach || particleEffect.offSetSpace == ParticleOffsetSpace.ParticleOffsetWorldRotation)
					{
						shouldOverridePosition = true;
						Vector3 offset = Quaternion.Euler(0f, -90f, 0f) * particleEffect.offSet;
						overridePosition = this.boneController.GetOffsetParticlePosition(particleEffect.bodyPart, ParticleOffsetSpace.ParticleOffsetRootRotation, offset, false);
					}
					Quaternion overrideRotation = Quaternion.Euler(0f, -90f, 0f) * this.effectContainer.transform.rotation;
					ParticlePlayData particleData = new ParticlePlayData
					{
						particle = particleEffect,
						shouldOverridePosition = shouldOverridePosition,
						overridePosition = overridePosition,
						shouldOverrideRotation = true,
						overrideRotation = overrideRotation,
						overrideBodyPart = BodyPart.none,
						skinData = this.skinData,
						shouldOverrideQualityFilter = false
					};
					GeneratedEffect generatedEffect = this.effectHelper.PlayParticle(this.particleContext, particleData);
					this.spawnedEffects.Add(generatedEffect.EffectController);
				}
			}
		}
		this.willTick.Clear();
		foreach (Effect current in this.spawnedEffects)
		{
			this.willTick.Add(current);
		}
		foreach (Effect current2 in this.willTick)
		{
			current2.TickFrame();
		}
	}

	private void updateSFX()
	{
		if (this.currentMove != null)
		{
			SoundEffect[] soundEffects = this.currentMove.soundEffects;
			for (int i = 0; i < soundEffects.Length; i++)
			{
				SoundEffect soundEffect = soundEffects[i];
				if (this.currentFrame == soundEffect.frame)
				{
					AudioReference item = this.audioManager.PlaySound(soundEffect.sounds, new Action<AudioReference, bool>(this.onSoundFinished));
					this.spawnedSoundHandles.Add(item);
				}
			}
		}
	}

	private void updateWeaponTrails()
	{
		if (this.gameDataManager.ConfigData.defaultCharacterEffects.enableWeaponTrails && this.currentMove != null)
		{
			this.weaponTrailHelper.UpdateWeaponTrailMap(this.weaponTrailMap, this.currentFrame, this.currentMove, this.boneController, new Func<XWeaponTrail>(this.createWeaponTrail));
		}
		foreach (XWeaponTrail current in this.weaponTrailMap.Values)
		{
			current.TickFrame();
		}
	}

	private void updateMaterialAnimations()
	{
		if (this.currentMove != null)
		{
			MaterialAnimationTrigger[] materialAnimationTriggers = this.currentMove.materialAnimationTriggers;
			for (int i = 0; i < materialAnimationTriggers.Length; i++)
			{
				MaterialAnimationTrigger materialAnimationTrigger = materialAnimationTriggers[i];
				if (materialAnimationTrigger.MatchesTarget(MaterialAnimationTrigger.TargetType.Attacker) && this.currentFrame == materialAnimationTrigger.startFrame)
				{
					this.matAnimsController.AddAnimation(materialAnimationTrigger);
				}
			}
		}
		this.matAnimsController.TickFrame();
	}

	private bool createEffect(GameObject prefab, out Effect effect)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, this.effectContainer);
		effect = (gameObject.GetComponent<Effect>() ?? gameObject.AddComponent<Effect>());
		this.injector.Inject(effect);
		return true;
	}

	private XWeaponTrail createWeaponTrail()
	{
		return UnityEngine.Object.Instantiate<GameObject>(this.gameDataManager.ConfigData.moveData.WeaponTrailPrefab, this.effectContainer).GetComponent<XWeaponTrail>();
	}

	private void clearEffects()
	{
		this.weaponTrailHelper.ClearWeaponTrails(this.weaponTrailMap);
		this.willDestroy.Clear();
		foreach (Effect current in this.spawnedEffects)
		{
			this.willDestroy.Add(current);
		}
		for (int i = this.willDestroy.Count - 1; i >= 0; i--)
		{
			this.willDestroy[i].Destroy();
		}
		this.spawnedEffects.Clear();
		for (int j = this.spawnedSoundHandles.Count - 1; j >= 0; j--)
		{
			this.audioManager.StopSound(this.spawnedSoundHandles[j], 0f);
		}
		this.spawnedSoundHandles.Clear();
		this.matAnimsController.OnDestroy();
	}

	private void onSoundFinished(AudioReference handle, bool didComplete)
	{
		this.spawnedSoundHandles.Remove(handle);
	}

	private MoveData _Init_m__0()
	{
		return this.currentMove;
	}
}
