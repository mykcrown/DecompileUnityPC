using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020005CB RID: 1483
public class ResumableChargeComponent : CharacterComponent, IFlinchListener, IDeathListener, IGrabListener, ITickCharacterComponent, IChargeLevelComponent, IRollbackStateOwner
{
	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x0600211D RID: 8477 RVA: 0x000A5AC1 File Offset: 0x000A3EC1
	// (set) Token: 0x0600211E RID: 8478 RVA: 0x000A5AC9 File Offset: 0x000A3EC9
	[Inject]
	public IRollbackStatePooling pooling { get; set; }

	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x0600211F RID: 8479 RVA: 0x000A5AD2 File Offset: 0x000A3ED2
	public int MaxChargeFrames
	{
		get
		{
			return this.configData.maxChargeFrames;
		}
	}

	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x06002120 RID: 8480 RVA: 0x000A5ADF File Offset: 0x000A3EDF
	public bool IsFullyCharged
	{
		get
		{
			return this.model.chargeFrames >= this.configData.maxChargeFrames;
		}
	}

	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x06002121 RID: 8481 RVA: 0x000A5AFC File Offset: 0x000A3EFC
	public int ChargeFrames
	{
		get
		{
			return this.model.chargeFrames;
		}
	}

	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x06002122 RID: 8482 RVA: 0x000A5B09 File Offset: 0x000A3F09
	public Fixed ChargeLevel
	{
		get
		{
			return this.ChargeFrames / this.configData.maxChargeFrames;
		}
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000A5B28 File Offset: 0x000A3F28
	public override void RegisterPreload(PreloadContext context)
	{
		base.RegisterPreload(context);
		if (this.configData.fullyChargedLoopParticles != null)
		{
			foreach (ParticleData particleData in this.configData.fullyChargedLoopParticles)
			{
				particleData.RegisterPreload(context);
			}
		}
		if (this.configData.chargeLevel2LoopParticles != null)
		{
			foreach (ParticleData particleData2 in this.configData.chargeLevel2LoopParticles)
			{
				particleData2.RegisterPreload(context);
			}
		}
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000A5C00 File Offset: 0x000A4000
	public void TickFrame(InputButtonsData input)
	{
		if (this.model.isCharging)
		{
			this.model.chargeFrames++;
			this.updateThresholdEffects(true);
			if (this.model.chargeFrames >= this.configData.maxChargeFrames)
			{
				this.model.chargeFrames = this.configData.maxChargeFrames;
				this.EndCharging(false);
				this.onFullyCharged(input);
			}
			else if (this.isPastSecondThreshold())
			{
				this.onLevel2Charge();
			}
		}
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x000A5C8C File Offset: 0x000A408C
	private bool isPastSecondThreshold()
	{
		return this.thresholds.Count >= 2 && this.model.chargeFrames >= this.thresholds[1].chargeFramesNeeded;
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x000A5CC4 File Offset: 0x000A40C4
	private void updateThresholdEffects(bool playEffects)
	{
		if (this.thresholds != null)
		{
			for (int i = 0; i < this.thresholds.Count; i++)
			{
				ChargeThresholdMoveData chargeThresholdMoveData = this.thresholds[i];
				if (chargeThresholdMoveData.chargeFramesNeeded > this.model.chargeFrames)
				{
					break;
				}
				if (this.model.lastThresholdFrame < chargeThresholdMoveData.chargeFramesNeeded)
				{
					this.model.lastThresholdFrame = chargeThresholdMoveData.chargeFramesNeeded;
					if (playEffects)
					{
						if (chargeThresholdMoveData.audio.sound != null)
						{
							this.playerDelegate.Audio.PlayGameSound(new AudioRequest(chargeThresholdMoveData.audio, this.playerDelegate.AudioOwner, null));
						}
						this.playerDelegate.GameVFX.PlayParticleList(chargeThresholdMoveData.particles, null, null);
					}
				}
			}
		}
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000A5DA4 File Offset: 0x000A41A4
	private void onFullyCharged(InputButtonsData input)
	{
		MoveData moveData = (!this.playerDelegate.State.IsGrounded) ? this.configData.onChargeCompleteMoveAerial : this.configData.onChargeCompleteMoveGrounded;
		if (moveData != null)
		{
			this.playerDelegate.SetMove(moveData, input, HorizontalDirection.None, this.playerDelegate.ActiveMove.Model.uid, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
		}
		else
		{
			this.playerDelegate.EndActiveMove(MoveEndType.Cancelled, true, false);
		}
		this.killAndClearPersistentParticles();
		IGameVFX gameVFX = this.playerDelegate.GameVFX;
		List<ParticleData> fullyChargedLoopParticles = this.configData.fullyChargedLoopParticles;
		List<Effect> persistentParticles = this.model.persistentParticles;
		gameVFX.PlayParticleList(fullyChargedLoopParticles, null, persistentParticles);
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x000A5E70 File Offset: 0x000A4270
	private void onLevel2Charge()
	{
		this.killAndClearPersistentParticles();
		IGameVFX gameVFX = this.playerDelegate.GameVFX;
		List<ParticleData> chargeLevel2LoopParticles = this.configData.chargeLevel2LoopParticles;
		List<Effect> persistentParticles = this.model.persistentParticles;
		gameVFX.PlayParticleList(chargeLevel2LoopParticles, null, persistentParticles);
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000A5EB0 File Offset: 0x000A42B0
	private void killAndClearPersistentParticles()
	{
		foreach (Effect effect in this.model.persistentParticles)
		{
			effect.EnterSoftKill();
		}
		this.model.persistentParticles.Clear();
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x000A5F24 File Offset: 0x000A4324
	private void clearChargeIfCharging()
	{
		if (this.model.isCharging)
		{
			this.EndCharging(true);
		}
	}

	// Token: 0x0600212B RID: 8491 RVA: 0x000A5F3D File Offset: 0x000A433D
	public void OnFlinch()
	{
		this.clearChargeIfCharging();
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x000A5F45 File Offset: 0x000A4345
	public void OnGrabbed()
	{
		this.clearChargeIfCharging();
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x000A5F4D File Offset: 0x000A434D
	public void OnDeath()
	{
		this.EndCharging(true);
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000A5F56 File Offset: 0x000A4356
	public void StartCharging()
	{
		this.model.isCharging = true;
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x000A5F64 File Offset: 0x000A4364
	public void EndCharging(bool clearCharge)
	{
		this.model.isCharging = false;
		if (clearCharge)
		{
			this.model.lastThresholdFrame = -1;
			this.model.chargeFrames = 0;
			this.killAndClearPersistentParticles();
		}
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x000A5F98 File Offset: 0x000A4398
	public void AddChargeFrames(int chargeFrames)
	{
		int val = this.model.chargeFrames + chargeFrames;
		int chargeFrames2 = Math.Max(0, Math.Min(this.configData.maxChargeFrames, val));
		this.model.chargeFrames = chargeFrames2;
		this.updateThresholdEffects(false);
		if (this.model.chargeFrames == this.configData.maxChargeFrames)
		{
			this.killAndClearPersistentParticles();
			IGameVFX gameVFX = this.playerDelegate.GameVFX;
			List<ParticleData> fullyChargedLoopParticles = this.configData.fullyChargedLoopParticles;
			List<Effect> persistentParticles = this.model.persistentParticles;
			gameVFX.PlayParticleList(fullyChargedLoopParticles, null, persistentParticles);
		}
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x000A602A File Offset: 0x000A442A
	public void OnChargeMoveUsed()
	{
		this.EndCharging(true);
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000A6033 File Offset: 0x000A4433
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.pooling.Clone<ResumableChargeComponent.ResumableChargeComponentModel>(this.model));
		return true;
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000A604F File Offset: 0x000A444F
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ResumableChargeComponent.ResumableChargeComponentModel>(ref this.model);
		return true;
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000A605F File Offset: 0x000A445F
	public void SetThresholds(List<ChargeThresholdMoveData> thresholds)
	{
		this.thresholds = thresholds;
	}

	// Token: 0x04001A31 RID: 6705
	public ResumableChargeComponentData configData;

	// Token: 0x04001A32 RID: 6706
	private ResumableChargeComponent.ResumableChargeComponentModel model = new ResumableChargeComponent.ResumableChargeComponentModel();

	// Token: 0x04001A33 RID: 6707
	private List<ChargeThresholdMoveData> thresholds;

	// Token: 0x020005CC RID: 1484
	[Serializable]
	public class ResumableChargeComponentModel : RollbackStateTyped<ResumableChargeComponent.ResumableChargeComponentModel>
	{
		// Token: 0x06002136 RID: 8502 RVA: 0x000A6084 File Offset: 0x000A4484
		public override void CopyTo(ResumableChargeComponent.ResumableChargeComponentModel target)
		{
			target.isCharging = this.isCharging;
			target.chargeFrames = this.chargeFrames;
			target.lastThresholdFrame = this.lastThresholdFrame;
			base.copyList<Effect>(this.persistentParticles, target.persistentParticles);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000A60BC File Offset: 0x000A44BC
		public override object Clone()
		{
			ResumableChargeComponent.ResumableChargeComponentModel resumableChargeComponentModel = new ResumableChargeComponent.ResumableChargeComponentModel();
			this.CopyTo(resumableChargeComponentModel);
			return resumableChargeComponentModel;
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000A60D7 File Offset: 0x000A44D7
		public override void Clear()
		{
			base.Clear();
			this.persistentParticles.Clear();
		}

		// Token: 0x04001A34 RID: 6708
		public bool isCharging;

		// Token: 0x04001A35 RID: 6709
		public int chargeFrames;

		// Token: 0x04001A36 RID: 6710
		public int lastThresholdFrame = -1;

		// Token: 0x04001A37 RID: 6711
		[IsClonedManually]
		[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
		[IgnoreCopyValidation]
		[NonSerialized]
		public List<Effect> persistentParticles = new List<Effect>(16);
	}
}
