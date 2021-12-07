using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020005C2 RID: 1474
public class ChargeLevelComponent : CharacterComponent, ITickCharacterComponent, IDamageDealtListener, IDamageTakenListener, IDebugStringComponent, IChargeLevelComponent, IDeathListener, IRollbackStateOwner
{
	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x060020C6 RID: 8390 RVA: 0x000A3E0C File Offset: 0x000A220C
	// (set) Token: 0x060020C7 RID: 8391 RVA: 0x000A3E14 File Offset: 0x000A2214
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x060020C8 RID: 8392 RVA: 0x000A3E1D File Offset: 0x000A221D
	public Fixed ChargeLevel
	{
		get
		{
			return this.state.currentChargeLevel;
		}
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x060020C9 RID: 8393 RVA: 0x000A3E2A File Offset: 0x000A222A
	public int ChargeFrames
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000A3E2D File Offset: 0x000A222D
	public virtual bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ChargeLevelComponentState>(ref this.state);
		return true;
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x000A3E3D File Offset: 0x000A223D
	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<ChargeLevelComponentState>(this.state));
		return true;
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000A3E59 File Offset: 0x000A2259
	public void TickFrame(InputButtonsData input)
	{
		if (base.gameManager.Frame % 10 == 0)
		{
			this.evaluateChargeGain();
		}
		this.evaluateChargeLoss();
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x000A3E7A File Offset: 0x000A227A
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

	// Token: 0x060020CE RID: 8398 RVA: 0x000A3EB8 File Offset: 0x000A22B8
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

	// Token: 0x060020CF RID: 8399 RVA: 0x000A3F30 File Offset: 0x000A2330
	public void OnDeath()
	{
		this.state.Clear();
		base.events.Broadcast(new ChargeLevelChangedEvent(this.playerDelegate.PlayerNum, this.state.currentChargeLevel));
		this.onChargeLevelChanged(0);
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x000A3F70 File Offset: 0x000A2370
	public void OnChargeMoveUsed()
	{
		if (this.DischargeOnUse && this.state.currentChargeLevel >= this.MinDischargeThreshold)
		{
			this.state.Clear();
			base.events.Broadcast(new ChargeLevelChangedEvent(this.playerDelegate.PlayerNum, this.state.currentChargeLevel));
			this.onChargeLevelChanged(0);
		}
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x000A3FE0 File Offset: 0x000A23E0
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

	// Token: 0x060020D2 RID: 8402 RVA: 0x000A4064 File Offset: 0x000A2464
	private Fixed calculateDamageSinceLastChargeGain()
	{
		Fixed @fixed = 0;
		for (int i = this.state.frameDamage.Count - 1; i >= 0; i--)
		{
			@fixed += this.state.frameDamage[i].damage;
		}
		return @fixed;
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x000A40BC File Offset: 0x000A24BC
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

	// Token: 0x060020D4 RID: 8404 RVA: 0x000A42A4 File Offset: 0x000A26A4
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

	// Token: 0x060020D5 RID: 8405 RVA: 0x000A4370 File Offset: 0x000A2770
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

	// Token: 0x060020D6 RID: 8406 RVA: 0x000A4458 File Offset: 0x000A2858
	protected virtual void onChargeLevelChanged(Fixed newLevel)
	{
		this.state.hasWarnedChargeLoss = false;
		if (newLevel == this.MaxCharge)
		{
			base.events.Broadcast(new LogStatEvent(StatType.MaxCharge, 1, PointsValueType.Addition, this.playerDelegate.PlayerNum, this.playerDelegate.Team));
		}
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x060020D7 RID: 8407 RVA: 0x000A44AC File Offset: 0x000A28AC
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

	// Token: 0x040019F8 RID: 6648
	public Fixed ChargeGainPeriodSeconds = 10;

	// Token: 0x040019F9 RID: 6649
	public Fixed ChargeLossPeriodSeconds = 15;

	// Token: 0x040019FA RID: 6650
	public Fixed ChargeLossDamageTaken = 70;

	// Token: 0x040019FB RID: 6651
	public Fixed ChargeGainDamage = 30;

	// Token: 0x040019FC RID: 6652
	public int MaxCharge = 3;

	// Token: 0x040019FD RID: 6653
	public bool DischargeOnUse;

	// Token: 0x040019FE RID: 6654
	public Fixed MinDischargeThreshold = 0;

	// Token: 0x040019FF RID: 6655
	public Fixed ChargeLossWarningSeconds = 1;

	// Token: 0x04001A00 RID: 6656
	public AudioData ChargeGainSound;

	// Token: 0x04001A01 RID: 6657
	public AudioData ChargeLossSound;

	// Token: 0x04001A02 RID: 6658
	private List<FrameDamage> expiredFramesBuffer = new List<FrameDamage>();

	// Token: 0x04001A03 RID: 6659
	private ChargeLevelComponentState state = new ChargeLevelComponentState();
}
