using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200059F RID: 1439
public class AshaniChargeLevelComponent : ChargeLevelComponent, IRemovedfromGameListener, IRollbackStateOwner
{
	// Token: 0x0600207F RID: 8319 RVA: 0x000A45DC File Offset: 0x000A29DC
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

	// Token: 0x06002080 RID: 8320 RVA: 0x000A4630 File Offset: 0x000A2A30
	public void OnRemovedFromGame()
	{
		this.cleanUp();
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x000A4638 File Offset: 0x000A2A38
	protected override void onChargeLevelChanged(Fixed newLevel)
	{
		base.onChargeLevelChanged(newLevel);
		if (newLevel >= this.effectsByChargeLevel.Count)
		{
			Debug.LogWarning("Changed charge level to " + newLevel + ", but there is not particle data");
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

	// Token: 0x06002082 RID: 8322 RVA: 0x000A47C4 File Offset: 0x000A2BC4
	private void cleanUp()
	{
		if (this.loopingSoundID.sourceId > -1)
		{
			this.playerDelegate.Audio.StopSound(this.loopingSoundID, 0f);
		}
		foreach (Effect effect in this.ashaniState.persistentParticles.Values)
		{
			effect.Destroy();
		}
		this.ashaniState.persistentParticles.Clear();
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x000A4868 File Offset: 0x000A2C68
	public override void Destroy()
	{
		this.cleanUp();
		base.Destroy();
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x000A4876 File Offset: 0x000A2C76
	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		container.ReadState<AshaniChargeLevelComponentState>(ref this.ashaniState);
		return true;
	}

	// Token: 0x06002085 RID: 8325 RVA: 0x000A488E File Offset: 0x000A2C8E
	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		container.WriteState(base.rollbackStatePooling.Clone<AshaniChargeLevelComponentState>(this.ashaniState));
		return true;
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x000A48B4 File Offset: 0x000A2CB4
	public override void RegisterPreload(PreloadContext context)
	{
		foreach (ChargeLevelEffectData chargeLevelEffectData in this.effectsByChargeLevel)
		{
			chargeLevelEffectData.particleData.RegisterPreload(context);
		}
	}

	// Token: 0x040019E4 RID: 6628
	public List<ChargeLevelEffectData> effectsByChargeLevel;

	// Token: 0x040019E5 RID: 6629
	public BodyPart bodyPartFlags;

	// Token: 0x040019E6 RID: 6630
	private List<BodyPart> particleParts = new List<BodyPart>();

	// Token: 0x040019E7 RID: 6631
	private AudioReference loopingSoundID = new AudioReference(null, -1);

	// Token: 0x040019E8 RID: 6632
	private AshaniChargeLevelComponentState ashaniState = new AshaniChargeLevelComponentState();
}
