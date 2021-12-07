using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000645 RID: 1605
public class StageParticleSystem : StageProp
{
	// Token: 0x170009AA RID: 2474
	// (get) Token: 0x0600274C RID: 10060 RVA: 0x000BFA6B File Offset: 0x000BDE6B
	private bool hasParticleSystems
	{
		get
		{
			return this.particleSystems.Count > 0;
		}
	}

	// Token: 0x170009AB RID: 2475
	// (get) Token: 0x0600274D RID: 10061 RVA: 0x000BFA7B File Offset: 0x000BDE7B
	public override bool IsSimulation
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x000BFA80 File Offset: 0x000BDE80
	public override void Awake()
	{
		base.Awake();
		this.model = new StageParticleSystemModel();
		this.particleSystems.AddRange(base.GetComponentsInChildren<ParticleSystem>());
		if (!this.hasParticleSystems)
		{
			Debug.LogError("No Partcle System or PKFxFX Component attached to or childed to the Stage Partcle System script on Game Object: '" + base.name + "'");
			return;
		}
		this.model.isPlaying = false;
		if (this.particleSystems.Count > 0)
		{
			this.model.isPlaying |= this.particleSystems[0].isPlaying;
		}
		if (this.model.isPlaying)
		{
			this.Play(true);
		}
		else
		{
			this.Stop(true);
		}
		this.model.shouldValidate = this.IsSimulation;
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x000BFB49 File Offset: 0x000BDF49
	public void Play(bool resetAndClear)
	{
		if (this.hasParticleSystems)
		{
			if (resetAndClear)
			{
				this.ResetParticleSystems();
			}
			this.PlayParticleSystems();
			this.model.isPlaying = true;
			this.model.stopFrame = -1;
		}
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x000BFB80 File Offset: 0x000BDF80
	public void PlayForFrames(bool resetAndClear, int frameDuration)
	{
		if (this.hasParticleSystems)
		{
			if (resetAndClear)
			{
				this.ResetParticleSystems();
			}
			this.PlayParticleSystems();
			this.model.isPlaying = true;
			this.model.stopFrame = base.gameManager.Frame + frameDuration;
		}
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x000BFBCE File Offset: 0x000BDFCE
	public void Stop(bool resetAndClear)
	{
		if (this.hasParticleSystems)
		{
			if (resetAndClear)
			{
				this.ResetParticleSystems();
			}
			this.StopParticleSystems();
			this.model.isPlaying = false;
		}
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x000BFBFC File Offset: 0x000BDFFC
	public override void TickFrame()
	{
		if (this.model.stopFrame == base.gameManager.Frame)
		{
			this.Stop(false);
		}
		if (this.hasParticleSystems && base.gameManager.Frame > this.lastSimulatedFrame)
		{
			this.SimulateParticleSystems((float)(base.gameManager.Frame - this.lastSimulatedFrame) / WTime.fps);
			this.lastSimulatedFrame = base.gameManager.Frame;
		}
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x000BFC7C File Offset: 0x000BE07C
	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<StageParticleSystemModel>(this.model));
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x000BFC96 File Offset: 0x000BE096
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StageParticleSystemModel>(ref this.model);
		if (this.hasParticleSystems)
		{
			if (this.model.isPlaying)
			{
				this.PlayParticleSystems();
			}
			else
			{
				this.StopParticleSystems();
			}
		}
		return true;
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x000BFCD4 File Offset: 0x000BE0D4
	private void PlayParticleSystems()
	{
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.particleSystems[i].Play();
			this.particleSystems[i].Pause();
		}
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x000BFD20 File Offset: 0x000BE120
	private void StopParticleSystems()
	{
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.particleSystems[i].Stop();
		}
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x000BFD5C File Offset: 0x000BE15C
	private void ResetParticleSystems()
	{
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.particleSystems[i].Reset();
		}
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x000BFD98 File Offset: 0x000BE198
	private void SimulateParticleSystems(float simulationTime)
	{
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.particleSystems[i].Simulate(simulationTime, false, false);
		}
	}

	// Token: 0x04001CD2 RID: 7378
	private StageParticleSystemModel model;

	// Token: 0x04001CD3 RID: 7379
	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Token: 0x04001CD4 RID: 7380
	private int lastSimulatedFrame;
}
