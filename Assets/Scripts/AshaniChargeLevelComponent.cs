// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AshaniChargeLevelComponent : ChargeLevelComponent, IRemovedfromGameListener, IRollbackStateOwner
{
	public List<ChargeLevelEffectData> effectsByChargeLevel;

	public BodyPart bodyPartFlags;

	private List<BodyPart> particleParts = new List<BodyPart>();

	private AudioReference loopingSoundID = new AudioReference(null, -1);

	private AshaniChargeLevelComponentState ashaniState = new AshaniChargeLevelComponentState();

	public override void Init(IPlayerDelegate playerDelegate)
	{
		base.Init(playerDelegate);
		BodyPart[] values = EnumUtil.GetValues<BodyPart>();
		for (int i = 0; i < values.Length; i++)
		{
			if (((long)this.bodyPartFlags & 1L << (int)values[i]) != 0L)
			{
				this.particleParts.Add(values[i]);
			}
		}
	}

	public void OnRemovedFromGame()
	{
		this.cleanUp();
	}

	protected override void onChargeLevelChanged(Fixed newLevel)
	{
		base.onChargeLevelChanged(newLevel);
		if (newLevel >= this.effectsByChargeLevel.Count)
		{
			UnityEngine.Debug.LogWarning("Changed charge level to " + newLevel + ", but there is not particle data");
			return;
		}
		ChargeLevelEffectData chargeLevelEffectData = this.effectsByChargeLevel[(int)newLevel];
		if (chargeLevelEffectData == null)
		{
			return;
		}
		if (this.loopingSoundID.sourceId > -1)
		{
			this.playerDelegate.Audio.StopSound(this.loopingSoundID, 0f);
		}
		ParticleData particleData = chargeLevelEffectData.particleData;
		AudioData soundLoop = chargeLevelEffectData.soundLoop;
		if (soundLoop.sound != null)
		{
			this.loopingSoundID = this.playerDelegate.Audio.PlayLoopingSound(new AudioRequest(soundLoop, this.playerDelegate.AudioOwner, null));
		}
		for (int i = 0; i < this.particleParts.Count; i++)
		{
			BodyPart bodyPart = this.particleParts[i];
			if (this.ashaniState.persistentParticles.ContainsKey(bodyPart))
			{
				this.ashaniState.persistentParticles[bodyPart].EnterSoftKill();
				this.ashaniState.persistentParticles.Remove(bodyPart);
			}
			if (particleData != null && particleData.prefab != null)
			{
				GeneratedEffect generatedEffect = this.playerDelegate.GameVFX.PlayParticle(particleData, bodyPart, TeamNum.None);
				if (generatedEffect != null)
				{
					this.ashaniState.persistentParticles[bodyPart] = generatedEffect.EffectController;
				}
			}
		}
	}

	private void cleanUp()
	{
		if (this.loopingSoundID.sourceId > -1)
		{
			this.playerDelegate.Audio.StopSound(this.loopingSoundID, 0f);
		}
		foreach (Effect current in this.ashaniState.persistentParticles.Values)
		{
			current.Destroy();
		}
		this.ashaniState.persistentParticles.Clear();
	}

	public override void Destroy()
	{
		this.cleanUp();
		base.Destroy();
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		container.ReadState<AshaniChargeLevelComponentState>(ref this.ashaniState);
		return true;
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		container.WriteState(base.rollbackStatePooling.Clone<AshaniChargeLevelComponentState>(this.ashaniState));
		return true;
	}

	public override void RegisterPreload(PreloadContext context)
	{
		foreach (ChargeLevelEffectData current in this.effectsByChargeLevel)
		{
			current.particleData.RegisterPreload(context);
		}
	}
}
