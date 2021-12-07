// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StageParticleSystem : StageProp
{
	private StageParticleSystemModel model;

	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	private int lastSimulatedFrame;

	private bool hasParticleSystems
	{
		get
		{
			return this.particleSystems.Count > 0;
		}
	}

	public override bool IsSimulation
	{
		get
		{
			return false;
		}
	}

	public override void Awake()
	{
		base.Awake();
		this.model = new StageParticleSystemModel();
		this.particleSystems.AddRange(base.GetComponentsInChildren<ParticleSystem>());
		if (!this.hasParticleSystems)
		{
			UnityEngine.Debug.LogError("No Partcle System or PKFxFX Component attached to or childed to the Stage Partcle System script on Game Object: '" + base.name + "'");
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

	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<StageParticleSystemModel>(this.model));
	}

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

	private void PlayParticleSystems()
	{
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.particleSystems[i].Play();
			this.particleSystems[i].Pause();
		}
	}

	private void StopParticleSystems()
	{
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.particleSystems[i].Stop();
		}
	}

	private void ResetParticleSystems()
	{
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.particleSystems[i].Reset();
		}
	}

	private void SimulateParticleSystems(float simulationTime)
	{
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.particleSystems[i].Simulate(simulationTime, false, false);
		}
	}
}
