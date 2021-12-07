// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class ResumableChargeComponent : CharacterComponent, IFlinchListener, IDeathListener, IGrabListener, ITickCharacterComponent, IChargeLevelComponent, IRollbackStateOwner
{
	[Serializable]
	public class ResumableChargeComponentModel : RollbackStateTyped<ResumableChargeComponent.ResumableChargeComponentModel>
	{
		public bool isCharging;

		public int chargeFrames;

		public int lastThresholdFrame = -1;

		[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually]
		[NonSerialized]
		public List<Effect> persistentParticles = new List<Effect>(16);

		public override void CopyTo(ResumableChargeComponent.ResumableChargeComponentModel target)
		{
			target.isCharging = this.isCharging;
			target.chargeFrames = this.chargeFrames;
			target.lastThresholdFrame = this.lastThresholdFrame;
			base.copyList<Effect>(this.persistentParticles, target.persistentParticles);
		}

		public override object Clone()
		{
			ResumableChargeComponent.ResumableChargeComponentModel resumableChargeComponentModel = new ResumableChargeComponent.ResumableChargeComponentModel();
			this.CopyTo(resumableChargeComponentModel);
			return resumableChargeComponentModel;
		}

		public override void Clear()
		{
			base.Clear();
			this.persistentParticles.Clear();
		}
	}

	public ResumableChargeComponentData configData;

	private ResumableChargeComponent.ResumableChargeComponentModel model = new ResumableChargeComponent.ResumableChargeComponentModel();

	private List<ChargeThresholdMoveData> thresholds;

	[Inject]
	public IRollbackStatePooling pooling
	{
		get;
		set;
	}

	public int MaxChargeFrames
	{
		get
		{
			return this.configData.maxChargeFrames;
		}
	}

	public bool IsFullyCharged
	{
		get
		{
			return this.model.chargeFrames >= this.configData.maxChargeFrames;
		}
	}

	public int ChargeFrames
	{
		get
		{
			return this.model.chargeFrames;
		}
	}

	public Fixed ChargeLevel
	{
		get
		{
			return this.ChargeFrames / this.configData.maxChargeFrames;
		}
	}

	public override void RegisterPreload(PreloadContext context)
	{
		base.RegisterPreload(context);
		if (this.configData.fullyChargedLoopParticles != null)
		{
			foreach (ParticleData current in this.configData.fullyChargedLoopParticles)
			{
				current.RegisterPreload(context);
			}
		}
		if (this.configData.chargeLevel2LoopParticles != null)
		{
			foreach (ParticleData current2 in this.configData.chargeLevel2LoopParticles)
			{
				current2.RegisterPreload(context);
			}
		}
	}

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

	private bool isPastSecondThreshold()
	{
		return this.thresholds.Count >= 2 && this.model.chargeFrames >= this.thresholds[1].chargeFramesNeeded;
	}

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
		IGameVFX arg_B8_0 = this.playerDelegate.GameVFX;
		List<ParticleData> fullyChargedLoopParticles = this.configData.fullyChargedLoopParticles;
		List<Effect> persistentParticles = this.model.persistentParticles;
		arg_B8_0.PlayParticleList(fullyChargedLoopParticles, null, persistentParticles);
	}

	private void onLevel2Charge()
	{
		this.killAndClearPersistentParticles();
		IGameVFX arg_2C_0 = this.playerDelegate.GameVFX;
		List<ParticleData> chargeLevel2LoopParticles = this.configData.chargeLevel2LoopParticles;
		List<Effect> persistentParticles = this.model.persistentParticles;
		arg_2C_0.PlayParticleList(chargeLevel2LoopParticles, null, persistentParticles);
	}

	private void killAndClearPersistentParticles()
	{
		foreach (Effect current in this.model.persistentParticles)
		{
			current.EnterSoftKill();
		}
		this.model.persistentParticles.Clear();
	}

	private void clearChargeIfCharging()
	{
		if (this.model.isCharging)
		{
			this.EndCharging(true);
		}
	}

	public void OnFlinch()
	{
		this.clearChargeIfCharging();
	}

	public void OnGrabbed()
	{
		this.clearChargeIfCharging();
	}

	public void OnDeath()
	{
		this.EndCharging(true);
	}

	public void StartCharging()
	{
		this.model.isCharging = true;
	}

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

	public void AddChargeFrames(int chargeFrames)
	{
		int val = this.model.chargeFrames + chargeFrames;
		int chargeFrames2 = Math.Max(0, Math.Min(this.configData.maxChargeFrames, val));
		this.model.chargeFrames = chargeFrames2;
		this.updateThresholdEffects(false);
		if (this.model.chargeFrames == this.configData.maxChargeFrames)
		{
			this.killAndClearPersistentParticles();
			IGameVFX arg_80_0 = this.playerDelegate.GameVFX;
			List<ParticleData> fullyChargedLoopParticles = this.configData.fullyChargedLoopParticles;
			List<Effect> persistentParticles = this.model.persistentParticles;
			arg_80_0.PlayParticleList(fullyChargedLoopParticles, null, persistentParticles);
		}
	}

	public void OnChargeMoveUsed()
	{
		this.EndCharging(true);
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.pooling.Clone<ResumableChargeComponent.ResumableChargeComponentModel>(this.model));
		return true;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ResumableChargeComponent.ResumableChargeComponentModel>(ref this.model);
		return true;
	}

	public void SetThresholds(List<ChargeThresholdMoveData> thresholds)
	{
		this.thresholds = thresholds;
	}
}
