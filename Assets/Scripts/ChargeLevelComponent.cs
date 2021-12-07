// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class ChargeLevelComponent : CharacterComponent, ITickCharacterComponent, IDamageDealtListener, IDamageTakenListener, IDebugStringComponent, IChargeLevelComponent, IDeathListener, IRollbackStateOwner
{
	public Fixed ChargeGainPeriodSeconds = 10;

	public Fixed ChargeLossPeriodSeconds = 15;

	public Fixed ChargeLossDamageTaken = 70;

	public Fixed ChargeGainDamage = 30;

	public int MaxCharge = 3;

	public bool DischargeOnUse;

	public Fixed MinDischargeThreshold = 0;

	public Fixed ChargeLossWarningSeconds = 1;

	public AudioData ChargeGainSound;

	public AudioData ChargeLossSound;

	private List<FrameDamage> expiredFramesBuffer = new List<FrameDamage>();

	private ChargeLevelComponentState state = new ChargeLevelComponentState();

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	public Fixed ChargeLevel
	{
		get
		{
			return this.state.currentChargeLevel;
		}
	}

	public int ChargeFrames
	{
		get
		{
			return 0;
		}
	}

	public string DebugString
	{
		get
		{
			Fixed @fixed = this.calculateDamageSinceLastChargeGain();
			Fixed other = 0;
			int val = 0;
			if (this.state.frameDamage.Count > 0)
			{
				val = this.state.frameDamage[this.state.frameDamage.Count - 1].frame;
			}
			other = (base.gameManager.Frame - Math.Max(val, this.state.lastChargeLossFrame)) * (Fixed)((double)WTime.frameTime);
			return string.Concat(new object[]
			{
				"Charge Level: ",
				this.state.currentChargeLevel,
				"/",
				this.MaxCharge,
				" Dmg: ",
				@fixed.ToString("N1"),
				" Expire: ",
				(this.ChargeLossPeriodSeconds - other).ToString("N1")
			});
		}
	}

	public virtual bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ChargeLevelComponentState>(ref this.state);
		return true;
	}

	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<ChargeLevelComponentState>(this.state));
		return true;
	}

	public void TickFrame(InputButtonsData input)
	{
		if (base.gameManager.Frame % 10 == 0)
		{
			this.evaluateChargeGain();
		}
		this.evaluateChargeLoss();
	}

	public void OnDamageDealt(Fixed damage, ImpactType impactType, bool chargesMeter)
	{
		if (impactType != ImpactType.Hit)
		{
			return;
		}
		this.state.frameDamage.Add(new FrameDamage(base.gameManager.Frame, damage));
		this.state.hasWarnedChargeLoss = false;
		this.evaluateChargeGain();
	}

	public void OnDamageTaken(Fixed damage, ImpactType impactType)
	{
		if (impactType != ImpactType.Hit)
		{
			return;
		}
		if (this.ChargeLossDamageTaken > 0 && this.state.currentChargeLevel == this.MaxCharge)
		{
			this.state.damageTowardsChargeLoss += damage;
			if (this.state.damageTowardsChargeLoss >= this.ChargeLossDamageTaken)
			{
				this.loseChargeLevel();
			}
		}
	}

	public void OnDeath()
	{
		this.state.Clear();
		base.events.Broadcast(new ChargeLevelChangedEvent(this.playerDelegate.PlayerNum, this.state.currentChargeLevel));
		this.onChargeLevelChanged(0);
	}

	public void OnChargeMoveUsed()
	{
		if (this.DischargeOnUse && this.state.currentChargeLevel >= this.MinDischargeThreshold)
		{
			this.state.Clear();
			base.events.Broadcast(new ChargeLevelChangedEvent(this.playerDelegate.PlayerNum, this.state.currentChargeLevel));
			this.onChargeLevelChanged(0);
		}
	}

	private void evaluateChargeGain()
	{
		Fixed @fixed = this.calculateDamageSinceLastChargeGain();
		if (@fixed != this.state.damageTowardsNextCharge)
		{
			base.events.Broadcast(new ChargeLevelChangedEvent(this.playerDelegate.PlayerNum, this.state.currentChargeLevel + @fixed / this.ChargeGainDamage));
			this.state.damageTowardsNextCharge = @fixed;
		}
		if (@fixed >= this.ChargeGainDamage)
		{
			this.gainChargeLevel();
		}
	}

	private Fixed calculateDamageSinceLastChargeGain()
	{
		Fixed @fixed = 0;
		for (int i = this.state.frameDamage.Count - 1; i >= 0; i--)
		{
			@fixed += this.state.frameDamage[i].damage;
		}
		return @fixed;
	}

	private void evaluateChargeLoss()
	{
		this.expiredFramesBuffer.Clear();
		bool flag = false;
		int num = 0;
		for (int i = this.state.frameDamage.Count - 1; i >= 0; i--)
		{
			FrameDamage item = this.state.frameDamage[i];
			int val = base.gameManager.Frame - (int)(this.ChargeLossPeriodSeconds * this.config.fps);
			if (item.frame < Math.Max(this.state.lastChargeLossFrame, val))
			{
				int val2 = base.gameManager.Frame - (int)(this.ChargeGainDamage * this.config.fps);
				if (item.frame < Math.Max(this.state.lastChargeGainFrame, val2))
				{
					this.expiredFramesBuffer.Add(item);
				}
			}
			else if (!flag)
			{
				flag = true;
				num = item.frame;
			}
		}
		if (this.ChargeLevel >= 1)
		{
			if (!flag)
			{
				Fixed one = (base.gameManager.Frame - this.state.lastChargeLossFrame) / this.config.fps;
				if (one >= this.ChargeLossPeriodSeconds)
				{
					this.loseChargeLevel();
				}
			}
			else
			{
				Fixed one2 = (base.gameManager.Frame - num) / this.config.fps;
				if (!this.state.hasWarnedChargeLoss && one2 >= this.ChargeLossPeriodSeconds - this.ChargeLossWarningSeconds)
				{
					this.state.hasWarnedChargeLoss = true;
					base.events.Broadcast(new ChargeLossWarningEvent(this.playerDelegate.PlayerNum, this.ChargeLevel, this.ChargeLossWarningSeconds));
				}
			}
		}
	}

	private void gainChargeLevel()
	{
		if (this.ChargeGainSound.sound != null && this.state.currentChargeLevel < this.MaxCharge)
		{
			base.gameManager.Audio.PlayGameSound(new AudioRequest(this.ChargeGainSound, this.playerDelegate.AudioOwner, null));
		}
		this.state.lastChargeGainFrame = base.gameManager.Frame;
		this.state.currentChargeLevel = FixedMath.Min(this.MaxCharge, this.state.currentChargeLevel + 1);
		this.state.damageTowardsChargeLoss = 0;
		this.onChargeLevelChanged(this.state.currentChargeLevel);
	}

	private void loseChargeLevel()
	{
		if (this.ChargeLossSound.sound != null && this.state.currentChargeLevel > 0)
		{
			base.gameManager.Audio.PlayGameSound(new AudioRequest(this.ChargeLossSound, this.playerDelegate.AudioOwner, null));
		}
		this.state.lastChargeLossFrame = base.gameManager.Frame;
		this.state.currentChargeLevel = FixedMath.Max(0, this.state.currentChargeLevel - 1);
		this.state.damageTowardsChargeLoss = 0;
		this.onChargeLevelChanged(this.state.currentChargeLevel);
		base.events.Broadcast(new ChargeLevelChangedEvent(this.playerDelegate.PlayerNum, this.state.currentChargeLevel));
	}

	protected virtual void onChargeLevelChanged(Fixed newLevel)
	{
		this.state.hasWarnedChargeLoss = false;
		if (newLevel == this.MaxCharge)
		{
			base.events.Broadcast(new LogStatEvent(StatType.MaxCharge, 1, PointsValueType.Addition, this.playerDelegate.PlayerNum, this.playerDelegate.Team));
		}
	}
}
