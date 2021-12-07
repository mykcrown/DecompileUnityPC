using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000433 RID: 1075
public class Effect : MonoBehaviour, ITickable, IExpirable, IRollbackStateOwner, IPooledGameObjectListener
{
	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x0600162B RID: 5675 RVA: 0x000785F5 File Offset: 0x000769F5
	// (set) Token: 0x0600162C RID: 5676 RVA: 0x000785FD File Offset: 0x000769FD
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x0600162D RID: 5677 RVA: 0x00078606 File Offset: 0x00076A06
	// (set) Token: 0x0600162E RID: 5678 RVA: 0x0007860E File Offset: 0x00076A0E
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x0600162F RID: 5679 RVA: 0x00078617 File Offset: 0x00076A17
	// (set) Token: 0x06001630 RID: 5680 RVA: 0x0007861F File Offset: 0x00076A1F
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06001631 RID: 5681 RVA: 0x00078628 File Offset: 0x00076A28
	// (set) Token: 0x06001632 RID: 5682 RVA: 0x00078630 File Offset: 0x00076A30
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06001633 RID: 5683 RVA: 0x00078639 File Offset: 0x00076A39
	// (set) Token: 0x06001634 RID: 5684 RVA: 0x00078641 File Offset: 0x00076A41
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06001635 RID: 5685 RVA: 0x0007864A File Offset: 0x00076A4A
	private bool shouldSimulateParticles
	{
		get
		{
			return GameClient.IsCurrentFrame || (this.config.networkSettings.simulateRollback && this.config.networkSettings.autoRollbackSimulationAmount > 0);
		}
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x00078684 File Offset: 0x00076A84
	public virtual void Awake()
	{
		if (!GameAssetPreloader.InProgress)
		{
			this.instantiatedAtRunTime = true;
		}
		this.baseScale = base.transform.localScale;
		this.baseRotation = base.transform.localRotation;
		this.autoRotateComp = base.GetComponent<VFXAutoRotate>();
		this.vfxMirrorOffsets = base.GetComponentsInChildren<VFXMirrorOffset>();
		this.particleSystems.AddRange(base.GetComponentsInChildren<ParticleSystem>());
		this.vfxBehaviors.AddRange(base.GetComponentsInChildren<VFXBehavior>());
		this.particleSystemRenderer = base.GetComponentInChildren<ParticleSystemRenderer>();
		this.resetEffect();
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x00078710 File Offset: 0x00076B10
	[PostConstruct]
	public void Init()
	{
		bool flag = true;
		if (this.instantiatedAtRunTime && flag && this.gameController.MatchIsRunning)
		{
			Debug.LogError("Instantiated effect outside of preload time. Please add me to GameAssetPreloader! " + base.name);
		}
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x00078758 File Offset: 0x00076B58
	public void Init(int lifespanFrames, ParticleData particleInfo = null, float speedMulti = 1f)
	{
		this.resizeScale = ((particleInfo == null) ? Vector3.one : particleInfo.resizeScale);
		this.model.frameTime = ((particleInfo == null) ? WTime.frameTime : (WTime.frameTime / particleInfo.lifetimeScale));
		this.model.frameTime *= speedMulti;
		this.model.lifespanFrames = lifespanFrames;
		this.model.ticksAlive = 2;
		this.model.softKillFrame = -1;
		this.model.isDead = false;
		this.model.effectName = base.name;
		this.model.prewarmFrames = ((particleInfo == null || !particleInfo.prewarm) ? -1 : particleInfo.prewarmFrames);
		this.model.softKillFrameDuration = ((particleInfo == null) ? Effect.DefaultSoftKillDuration : particleInfo.softKillFrameDuration);
		if (particleInfo != null)
		{
			this.model.billboard = particleInfo.billboard;
		}
		else
		{
			this.model.billboard = false;
		}
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			particleSystem.transform.localScale = new Vector3(particleSystem.transform.localScale.x * this.resizeScale.x, particleSystem.transform.localScale.y * this.resizeScale.y, particleSystem.transform.localScale.z * this.resizeScale.z);
			particleSystem.Reset();
			if (particleSystem.useAutoRandomSeed)
			{
				particleSystem.randomSeed = 1U;
			}
			if (particleInfo != null && particleInfo.prewarm)
			{
				particleSystem.Simulate((float)this.model.prewarmFrames / WTime.fps, false, false);
			}
			particleSystem.gameObject.layer = LayerMask.NameToLayer(Layers.Foreground_Lighting);
		}
		if (this.gameController.MatchIsRunning && !this.gameController.currentGame.IsRollingBack && this.model.prewarmFrames > 0)
		{
			this.furthestSimulatedFrame += this.model.prewarmFrames;
		}
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x000789CC File Offset: 0x00076DCC
	public virtual void TickFrame()
	{
		if (this.model.isDead)
		{
			return;
		}
		if (this.gameController.MatchIsRunning && this.gameController == null)
		{
			Debug.LogError("No injection, what wrong? " + base.name);
			return;
		}
		if (this.gameController.MatchIsRunning && this.gameController.currentGame == null)
		{
			Debug.LogError("Game not exist, what wrong? " + base.name);
			return;
		}
		if (this.model.billboard)
		{
			base.transform.LookAt(this.gameController.currentGame.Camera.current.transform);
		}
		if (this.gameController.currentGame.Camera.isFlourishMode)
		{
			if (this.model.pauseTicks < this.config.flourishConfig.pauseVfxFrame)
			{
				this.model.pauseTicks++;
				return;
			}
			this.model.pauseTicks = 0;
		}
		this.model.ticksAlive++;
		if (this.shouldSimulateParticles)
		{
			this.simulateParticles();
		}
		if (this.model.softKillFrame == -1 && this.model.lifespanFrames > 0 && this.model.ticksAlive >= this.model.lifespanFrames)
		{
			this.EnterSoftKill();
			if (this.model.isDead)
			{
				return;
			}
		}
		if (this.model.softKillFrame != -1 && this.model.ticksAlive >= this.model.softKillFrame + this.model.softKillFrameDuration)
		{
			this.Destroy();
		}
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x00078BA0 File Offset: 0x00076FA0
	public void StopEmissions()
	{
		this.simulateParticles();
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			particleSystem.emission.enabled = false;
		}
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x00078C0C File Offset: 0x0007700C
	private bool onEnterSoftKill()
	{
		this.simulateParticles();
		bool result = false;
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			if (particleSystem != null)
			{
				if (!this.HardKillParticleSystems.Contains(particleSystem))
				{
					result = true;
					particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
				}
				else
				{
					particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
			}
		}
		return result;
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x00078CA0 File Offset: 0x000770A0
	public bool EnterSoftKill()
	{
		if (this.model.softKillFrame == -1)
		{
			if (!this.onEnterSoftKill())
			{
				this.Destroy();
				return false;
			}
			this.model.softKillFrame = this.model.ticksAlive;
		}
		return true;
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x00078CED File Offset: 0x000770ED
	public virtual void Destroy()
	{
		if (base.gameObject != null)
		{
			base.gameObject.DestroySafe();
			this.signalBus.GetSignal<EffectReleaseSignal>().Dispatch(this);
		}
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x00078D1C File Offset: 0x0007711C
	private void simulateParticles()
	{
		int num = Math.Max(0, this.model.prewarmFrames);
		int num2 = this.model.ticksAlive + num;
		int num3 = num2 - this.furthestSimulatedFrame;
		if (num3 > 0)
		{
			foreach (ParticleSystem particleSystem in this.particleSystems)
			{
				particleSystem.Simulate(this.model.frameTime * (float)num3, false, false);
			}
			this.furthestSimulatedFrame = num2;
		}
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x00078DC4 File Offset: 0x000771C4
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<EffectModel>(this.model));
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x00078DDE File Offset: 0x000771DE
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<EffectModel>(ref this.model);
		return true;
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x00078DF0 File Offset: 0x000771F0
	void IPooledGameObjectListener.OnAcquired()
	{
		foreach (VFXBehavior vfxbehavior in this.vfxBehaviors)
		{
			vfxbehavior.OnVFXStart();
		}
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x00078E4C File Offset: 0x0007724C
	void IPooledGameObjectListener.OnReleased()
	{
		this.resetEffect();
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x00078E54 File Offset: 0x00077254
	void IPooledGameObjectListener.OnCooledOff()
	{
		this.furthestSimulatedFrame = 0;
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x00078E60 File Offset: 0x00077260
	private void resetEffect()
	{
		this.model.isDead = true;
		this.model.lifespanFrames = 0;
		this.model.prewarmFrames = -1;
		this.model.ticksAlive = 0;
		this.model.pauseTicks = 0;
		this.model.billboard = false;
		this.model.softKillFrame = -1;
		this.model.effectName = null;
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			particleSystem.transform.localScale = new Vector3(particleSystem.transform.localScale.x * (1f / this.resizeScale.x), particleSystem.transform.localScale.y * (1f / this.resizeScale.y), particleSystem.transform.localScale.z * (1f / this.resizeScale.z));
			particleSystem.Reset();
		}
		this.model.frameTime = WTime.frameTime;
		base.transform.localRotation = this.baseRotation;
		base.transform.localScale = this.baseScale;
		if (this.autoRotateComp != null)
		{
			this.autoRotateComp.Reset();
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06001645 RID: 5701 RVA: 0x00078FEC File Offset: 0x000773EC
	public virtual bool IsExpired
	{
		get
		{
			return this.model.isDead;
		}
	}

	// Token: 0x04001111 RID: 4369
	public static int DefaultSoftKillDuration = 120;

	// Token: 0x04001112 RID: 4370
	protected EffectModel model = new EffectModel();

	// Token: 0x04001113 RID: 4371
	private List<ParticleSystem> particleSystems = new List<ParticleSystem>(4);

	// Token: 0x04001114 RID: 4372
	private List<VFXBehavior> vfxBehaviors = new List<VFXBehavior>(4);

	// Token: 0x04001115 RID: 4373
	public List<ParticleSystem> HardKillParticleSystems = new List<ParticleSystem>(4);

	// Token: 0x04001116 RID: 4374
	private Vector3 baseScale;

	// Token: 0x04001117 RID: 4375
	private Vector3 resizeScale = Vector3.one;

	// Token: 0x04001118 RID: 4376
	private Quaternion baseRotation;

	// Token: 0x04001119 RID: 4377
	private bool instantiatedAtRunTime;

	// Token: 0x0400111A RID: 4378
	[HideInInspector]
	public VFXAutoRotate autoRotateComp;

	// Token: 0x0400111B RID: 4379
	public ParticleSystemRenderer particleSystemRenderer;

	// Token: 0x0400111C RID: 4380
	public VFXMirrorOffset[] vfxMirrorOffsets = new VFXMirrorOffset[0];

	// Token: 0x0400111D RID: 4381
	private int furthestSimulatedFrame;

	// Token: 0x02000434 RID: 1076
	private struct FramePosition
	{
		// Token: 0x0400111E RID: 4382
		public int frame;

		// Token: 0x0400111F RID: 4383
		public Vector3 position;

		// Token: 0x04001120 RID: 4384
		public Vector3 localScale;

		// Token: 0x04001121 RID: 4385
		public Quaternion rotation;
	}
}
