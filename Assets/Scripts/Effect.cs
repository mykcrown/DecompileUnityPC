// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour, ITickable, IExpirable, IRollbackStateOwner, IPooledGameObjectListener
{
	private struct FramePosition
	{
		public int frame;

		public Vector3 position;

		public Vector3 localScale;

		public Quaternion rotation;
	}

	public static int DefaultSoftKillDuration = 120;

	protected EffectModel model = new EffectModel();

	private List<ParticleSystem> particleSystems = new List<ParticleSystem>(4);

	private List<VFXBehavior> vfxBehaviors = new List<VFXBehavior>(4);

	public List<ParticleSystem> HardKillParticleSystems = new List<ParticleSystem>(4);

	private Vector3 baseScale;

	private Vector3 resizeScale = Vector3.one;

	private Quaternion baseRotation;

	private bool instantiatedAtRunTime;

	[HideInInspector]
	public VFXAutoRotate autoRotateComp;

	public ParticleSystemRenderer particleSystemRenderer;

	public VFXMirrorOffset[] vfxMirrorOffsets = new VFXMirrorOffset[0];

	private int furthestSimulatedFrame;

	[Inject]
	public GameController gameController
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
	public DeveloperConfig devConfig
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

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	private bool shouldSimulateParticles
	{
		get
		{
			return GameClient.IsCurrentFrame || (this.config.networkSettings.simulateRollback && this.config.networkSettings.autoRollbackSimulationAmount > 0);
		}
	}

	public virtual bool IsExpired
	{
		get
		{
			return this.model.isDead;
		}
	}

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

	[PostConstruct]
	public void Init()
	{
		bool flag = true;
		if (this.instantiatedAtRunTime && flag && this.gameController.MatchIsRunning)
		{
			UnityEngine.Debug.LogError("Instantiated effect outside of preload time. Please add me to GameAssetPreloader! " + base.name);
		}
	}

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
		this.model.prewarmFrames = ((particleInfo == null || !particleInfo.prewarm) ? (-1) : particleInfo.prewarmFrames);
		this.model.softKillFrameDuration = ((particleInfo == null) ? Effect.DefaultSoftKillDuration : particleInfo.softKillFrameDuration);
		if (particleInfo != null)
		{
			this.model.billboard = particleInfo.billboard;
		}
		else
		{
			this.model.billboard = false;
		}
		foreach (ParticleSystem current in this.particleSystems)
		{
			current.transform.localScale = new Vector3(current.transform.localScale.x * this.resizeScale.x, current.transform.localScale.y * this.resizeScale.y, current.transform.localScale.z * this.resizeScale.z);
			current.Reset();
			if (current.useAutoRandomSeed)
			{
				current.randomSeed = 1u;
			}
			if (particleInfo != null && particleInfo.prewarm)
			{
				current.Simulate((float)this.model.prewarmFrames / WTime.fps, false, false);
			}
			current.gameObject.layer = LayerMask.NameToLayer(Layers.Foreground_Lighting);
		}
		if (this.gameController.MatchIsRunning && !this.gameController.currentGame.IsRollingBack && this.model.prewarmFrames > 0)
		{
			this.furthestSimulatedFrame += this.model.prewarmFrames;
		}
	}

	public virtual void TickFrame()
	{
		if (this.model.isDead)
		{
			return;
		}
		if (this.gameController.MatchIsRunning && this.gameController == null)
		{
			UnityEngine.Debug.LogError("No injection, what wrong? " + base.name);
			return;
		}
		if (this.gameController.MatchIsRunning && this.gameController.currentGame == null)
		{
			UnityEngine.Debug.LogError("Game not exist, what wrong? " + base.name);
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

	public void StopEmissions()
	{
		this.simulateParticles();
		foreach (ParticleSystem current in this.particleSystems)
		{
			current.emission.enabled = false;
		}
	}

	private bool onEnterSoftKill()
	{
		this.simulateParticles();
		bool result = false;
		foreach (ParticleSystem current in this.particleSystems)
		{
			if (current != null)
			{
				if (!this.HardKillParticleSystems.Contains(current))
				{
					result = true;
					current.Stop(false, ParticleSystemStopBehavior.StopEmitting);
				}
				else
				{
					current.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
			}
		}
		return result;
	}

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

	public virtual void Destroy()
	{
		if (base.gameObject != null)
		{
			base.gameObject.DestroySafe();
			this.signalBus.GetSignal<EffectReleaseSignal>().Dispatch(this);
		}
	}

	private void simulateParticles()
	{
		int num = Math.Max(0, this.model.prewarmFrames);
		int num2 = this.model.ticksAlive + num;
		int num3 = num2 - this.furthestSimulatedFrame;
		if (num3 > 0)
		{
			foreach (ParticleSystem current in this.particleSystems)
			{
				current.Simulate(this.model.frameTime * (float)num3, false, false);
			}
			this.furthestSimulatedFrame = num2;
		}
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<EffectModel>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<EffectModel>(ref this.model);
		return true;
	}

	void IPooledGameObjectListener.OnAcquired()
	{
		foreach (VFXBehavior current in this.vfxBehaviors)
		{
			current.OnVFXStart();
		}
	}

	void IPooledGameObjectListener.OnReleased()
	{
		this.resetEffect();
	}

	void IPooledGameObjectListener.OnCooledOff()
	{
		this.furthestSimulatedFrame = 0;
	}

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
		foreach (ParticleSystem current in this.particleSystems)
		{
			current.transform.localScale = new Vector3(current.transform.localScale.x * (1f / this.resizeScale.x), current.transform.localScale.y * (1f / this.resizeScale.y), current.transform.localScale.z * (1f / this.resizeScale.z));
			current.Reset();
		}
		this.model.frameTime = WTime.frameTime;
		base.transform.localRotation = this.baseRotation;
		base.transform.localScale = this.baseScale;
		if (this.autoRotateComp != null)
		{
			this.autoRotateComp.Reset();
		}
	}
}
