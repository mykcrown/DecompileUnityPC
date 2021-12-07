using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;
using Xft;

// Token: 0x02000A72 RID: 2674
public class UISceneMoveController : IMoveOwner
{
	// Token: 0x17001268 RID: 4712
	// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x001484A2 File Offset: 0x001468A2
	// (set) Token: 0x06004DE5 RID: 19941 RVA: 0x001484AA File Offset: 0x001468AA
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17001269 RID: 4713
	// (get) Token: 0x06004DE6 RID: 19942 RVA: 0x001484B3 File Offset: 0x001468B3
	// (set) Token: 0x06004DE7 RID: 19943 RVA: 0x001484BB File Offset: 0x001468BB
	[Inject]
	public IEffectHelper effectHelper { get; set; }

	// Token: 0x1700126A RID: 4714
	// (get) Token: 0x06004DE8 RID: 19944 RVA: 0x001484C4 File Offset: 0x001468C4
	// (set) Token: 0x06004DE9 RID: 19945 RVA: 0x001484CC File Offset: 0x001468CC
	[Inject]
	public IWeaponTrailHelper weaponTrailHelper { get; set; }

	// Token: 0x1700126B RID: 4715
	// (get) Token: 0x06004DEA RID: 19946 RVA: 0x001484D5 File Offset: 0x001468D5
	// (set) Token: 0x06004DEB RID: 19947 RVA: 0x001484DD File Offset: 0x001468DD
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x1700126C RID: 4716
	// (get) Token: 0x06004DEC RID: 19948 RVA: 0x001484E6 File Offset: 0x001468E6
	// (set) Token: 0x06004DED RID: 19949 RVA: 0x001484EE File Offset: 0x001468EE
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x1700126D RID: 4717
	// (get) Token: 0x06004DEE RID: 19950 RVA: 0x001484F7 File Offset: 0x001468F7
	// (set) Token: 0x06004DEF RID: 19951 RVA: 0x001484FF File Offset: 0x001468FF
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x1700126E RID: 4718
	// (get) Token: 0x06004DF0 RID: 19952 RVA: 0x00148508 File Offset: 0x00146908
	public MoveData MoveData
	{
		get
		{
			return this.currentMove;
		}
	}

	// Token: 0x1700126F RID: 4719
	// (get) Token: 0x06004DF1 RID: 19953 RVA: 0x00148510 File Offset: 0x00146910
	public bool MoveIsValid
	{
		get
		{
			return this.currentMove != null;
		}
	}

	// Token: 0x17001270 RID: 4720
	// (get) Token: 0x06004DF2 RID: 19954 RVA: 0x0014851E File Offset: 0x0014691E
	public int InternalFrame
	{
		get
		{
			return this.currentFrame;
		}
	}

	// Token: 0x17001271 RID: 4721
	// (get) Token: 0x06004DF3 RID: 19955 RVA: 0x00148526 File Offset: 0x00146926
	public PlayerController Player
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06004DF4 RID: 19956 RVA: 0x0014852C File Offset: 0x0014692C
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
		IPhysicsStateOwner physics = new UISceneMoveController.UICharacterPhysicsStateOwner(() => this.currentMove);
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

	// Token: 0x06004DF5 RID: 19957 RVA: 0x0014864D File Offset: 0x00146A4D
	public void Destroy()
	{
		this.clearEffects();
		if (this.boneController != null)
		{
			this.boneController.Destroy();
		}
		this.signalBus.GetSignal<EffectReleaseSignal>().RemoveListener(new Action<Effect>(this.onEffectReleased));
	}

	// Token: 0x06004DF6 RID: 19958 RVA: 0x00148687 File Offset: 0x00146A87
	private void onEffectReleased(Effect theEffect)
	{
		this.spawnedEffects.Remove(theEffect);
	}

	// Token: 0x06004DF7 RID: 19959 RVA: 0x00148696 File Offset: 0x00146A96
	public void SetMove(MoveData moveData)
	{
		if (this.currentMove != null)
		{
			this.OnMoveEnd();
		}
		this.currentMove = moveData;
		this.currentFrame = 0;
	}

	// Token: 0x06004DF8 RID: 19960 RVA: 0x001486BD File Offset: 0x00146ABD
	public void OnMoveEnd()
	{
		this.signalBus.GetSignal<PlayerController.InteractionSignal>().Dispatch(new PlayerController.InteractionSignalData(null, PlayerController.InteractionSignalData.Type.MoveEnd));
		this.currentMove = null;
		this.clearEffects();
	}

	// Token: 0x06004DF9 RID: 19961 RVA: 0x001486E4 File Offset: 0x00146AE4
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

	// Token: 0x06004DFA RID: 19962 RVA: 0x00148734 File Offset: 0x00146B34
	private void updateVFX()
	{
		if (this.currentMove != null)
		{
			foreach (MoveParticleEffect moveParticleEffect in this.currentMove.particleEffects)
			{
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
		foreach (Effect item in this.spawnedEffects)
		{
			this.willTick.Add(item);
		}
		foreach (Effect effect in this.willTick)
		{
			effect.TickFrame();
		}
	}

	// Token: 0x06004DFB RID: 19963 RVA: 0x00148950 File Offset: 0x00146D50
	private void updateSFX()
	{
		if (this.currentMove != null)
		{
			foreach (SoundEffect soundEffect in this.currentMove.soundEffects)
			{
				if (this.currentFrame == soundEffect.frame)
				{
					AudioReference item = this.audioManager.PlaySound(soundEffect.sounds, new Action<AudioReference, bool>(this.onSoundFinished));
					this.spawnedSoundHandles.Add(item);
				}
			}
		}
	}

	// Token: 0x06004DFC RID: 19964 RVA: 0x001489D0 File Offset: 0x00146DD0
	private void updateWeaponTrails()
	{
		if (this.gameDataManager.ConfigData.defaultCharacterEffects.enableWeaponTrails && this.currentMove != null)
		{
			this.weaponTrailHelper.UpdateWeaponTrailMap(this.weaponTrailMap, this.currentFrame, this.currentMove, this.boneController, new Func<XWeaponTrail>(this.createWeaponTrail));
		}
		foreach (XWeaponTrail xweaponTrail in this.weaponTrailMap.Values)
		{
			xweaponTrail.TickFrame();
		}
	}

	// Token: 0x06004DFD RID: 19965 RVA: 0x00148A8C File Offset: 0x00146E8C
	private void updateMaterialAnimations()
	{
		if (this.currentMove != null)
		{
			foreach (MaterialAnimationTrigger materialAnimationTrigger in this.currentMove.materialAnimationTriggers)
			{
				if (materialAnimationTrigger.MatchesTarget(MaterialAnimationTrigger.TargetType.Attacker) && this.currentFrame == materialAnimationTrigger.startFrame)
				{
					this.matAnimsController.AddAnimation(materialAnimationTrigger);
				}
			}
		}
		this.matAnimsController.TickFrame();
	}

	// Token: 0x06004DFE RID: 19966 RVA: 0x00148B04 File Offset: 0x00146F04
	private bool createEffect(GameObject prefab, out Effect effect)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, this.effectContainer);
		effect = (gameObject.GetComponent<Effect>() ?? gameObject.AddComponent<Effect>());
		this.injector.Inject(effect);
		return true;
	}

	// Token: 0x06004DFF RID: 19967 RVA: 0x00148B41 File Offset: 0x00146F41
	private XWeaponTrail createWeaponTrail()
	{
		return UnityEngine.Object.Instantiate<GameObject>(this.gameDataManager.ConfigData.moveData.WeaponTrailPrefab, this.effectContainer).GetComponent<XWeaponTrail>();
	}

	// Token: 0x06004E00 RID: 19968 RVA: 0x00148B68 File Offset: 0x00146F68
	private void clearEffects()
	{
		this.weaponTrailHelper.ClearWeaponTrails(this.weaponTrailMap);
		this.willDestroy.Clear();
		foreach (Effect item in this.spawnedEffects)
		{
			this.willDestroy.Add(item);
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

	// Token: 0x06004E01 RID: 19969 RVA: 0x00148C70 File Offset: 0x00147070
	private void onSoundFinished(AudioReference handle, bool didComplete)
	{
		this.spawnedSoundHandles.Remove(handle);
	}

	// Token: 0x040032FE RID: 13054
	private SkinData skinData;

	// Token: 0x040032FF RID: 13055
	private Transform effectContainer;

	// Token: 0x04003300 RID: 13056
	private BoneController boneController;

	// Token: 0x04003301 RID: 13057
	private MaterialAnimationsController matAnimsController;

	// Token: 0x04003302 RID: 13058
	private MoveData currentMove;

	// Token: 0x04003303 RID: 13059
	private int currentFrame;

	// Token: 0x04003304 RID: 13060
	private ParticlePlayContext particleContext;

	// Token: 0x04003305 RID: 13061
	private List<Effect> spawnedEffects = new List<Effect>();

	// Token: 0x04003306 RID: 13062
	private List<AudioReference> spawnedSoundHandles = new List<AudioReference>();

	// Token: 0x04003307 RID: 13063
	private List<Effect> willTick = new List<Effect>(30);

	// Token: 0x04003308 RID: 13064
	private List<Effect> willDestroy = new List<Effect>(30);

	// Token: 0x04003309 RID: 13065
	private Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap = new Dictionary<WeaponTrailData, XWeaponTrail>();

	// Token: 0x02000A73 RID: 2675
	public class UICharacterFacing : IFacing
	{
		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06004E04 RID: 19972 RVA: 0x00148C8F File Offset: 0x0014708F
		public HorizontalDirection Facing
		{
			get
			{
				return HorizontalDirection.Right;
			}
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06004E05 RID: 19973 RVA: 0x00148C92 File Offset: 0x00147092
		public HorizontalDirection OppositeFacing
		{
			get
			{
				return HorizontalDirection.None;
			}
		}
	}

	// Token: 0x02000A74 RID: 2676
	public class UICharacterPhysicsStateOwner : IPhysicsStateOwner
	{
		// Token: 0x06004E06 RID: 19974 RVA: 0x00148C95 File Offset: 0x00147095
		public UICharacterPhysicsStateOwner(Func<MoveData> currentMoveGetter)
		{
			this.currentMoveGetter = currentMoveGetter;
		}

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06004E07 RID: 19975 RVA: 0x00148CA4 File Offset: 0x001470A4
		public MoveData CurrentMove
		{
			get
			{
				return this.currentMoveGetter();
			}
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06004E08 RID: 19976 RVA: 0x00148CB1 File Offset: 0x001470B1
		// (set) Token: 0x06004E09 RID: 19977 RVA: 0x00148CB4 File Offset: 0x001470B4
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

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06004E0A RID: 19978 RVA: 0x00148CB6 File Offset: 0x001470B6
		public Vector3F Velocity
		{
			get
			{
				return Vector3F.zero;
			}
		}

		// Token: 0x0400330A RID: 13066
		private Func<MoveData> currentMoveGetter;
	}
}
