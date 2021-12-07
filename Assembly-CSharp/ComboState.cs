using System;
using FixedPoint;

// Token: 0x020003C7 RID: 967
public class ComboState : IRollbackStateOwner
{
	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06001522 RID: 5410 RVA: 0x00075024 File Offset: 0x00073424
	// (set) Token: 0x06001523 RID: 5411 RVA: 0x0007502C File Offset: 0x0007342C
	[Inject]
	public ICombatCalculator combatCalculator { get; set; }

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06001524 RID: 5412 RVA: 0x00075035 File Offset: 0x00073435
	// (set) Token: 0x06001525 RID: 5413 RVA: 0x0007503D File Offset: 0x0007343D
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06001526 RID: 5414 RVA: 0x00075046 File Offset: 0x00073446
	public Fixed Damage
	{
		get
		{
			return this.model.Damage;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06001527 RID: 5415 RVA: 0x00075053 File Offset: 0x00073453
	public int Count
	{
		get
		{
			return this.model.Count;
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06001528 RID: 5416 RVA: 0x00075060 File Offset: 0x00073460
	public bool IsRecovered
	{
		get
		{
			return this.model.IsRecovered;
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06001529 RID: 5417 RVA: 0x0007506D File Offset: 0x0007346D
	public int FramesRecovered
	{
		get
		{
			return this.model.FramesRecovered;
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x0600152A RID: 5418 RVA: 0x0007507A File Offset: 0x0007347A
	public bool IsActive
	{
		get
		{
			return this.model.IsActive;
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x0600152B RID: 5419 RVA: 0x00075087 File Offset: 0x00073487
	public int WindowFrames
	{
		get
		{
			return this.model.WindowFrames;
		}
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x00075094 File Offset: 0x00073494
	public void Setup(int framesTilInactive)
	{
		this.model.Damage = 0;
		this.model.Count = 0;
		this.model.IsRecovered = false;
		this.model.FramesRecovered = 0;
		this.model.IsActive = false;
		this.model.WindowFrames = framesTilInactive;
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x000750EE File Offset: 0x000734EE
	public void UpdateFrame(HitData hit, IHitOwner hitOwner, bool isRecovered = false, bool hasLostControl = false)
	{
		if (hit != null)
		{
			this.RecordHit(hit, hitOwner);
		}
		if (!this.IsActive)
		{
			return;
		}
		if (isRecovered)
		{
			this.RecordRecovered();
		}
		if (hasLostControl)
		{
			this.RecordLostControl();
		}
		this.AdvanceFrame();
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x0007512C File Offset: 0x0007352C
	public void Clear()
	{
		this.model.IsActive = false;
		this.model.Damage = 0;
		this.model.Count = 0;
		this.model.FramesRecovered = 0;
		this.model.IsRecovered = false;
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x0007517A File Offset: 0x0007357A
	public void ForceRecordHit(HitData hit, IHitOwner hitOwner)
	{
		this.RecordHit(hit, hitOwner);
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x00075184 File Offset: 0x00073584
	private void RecordHit(HitData hit, IHitOwner hitOwner)
	{
		if (!this.IsActive)
		{
			this.model.Count = 1;
			this.model.Damage = 0;
		}
		else
		{
			this.model.Count++;
		}
		this.model.IsActive = true;
		this.model.IsRecovered = false;
		this.model.FramesRecovered = 0;
		this.model.Damage += this.combatCalculator.CalculateModifiedDamage(hit, hitOwner);
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x00075218 File Offset: 0x00073618
	private void RecordRecovered()
	{
		if (!this.IsRecovered)
		{
			this.model.FramesRecovered = 0;
		}
		this.model.IsRecovered = true;
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x0007523D File Offset: 0x0007363D
	private void RecordLostControl()
	{
		if (this.IsRecovered)
		{
			this.model.IsRecovered = false;
			this.model.FramesRecovered = 0;
		}
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x00075262 File Offset: 0x00073662
	private void AdvanceFrame()
	{
		if (this.IsRecovered)
		{
			this.model.FramesRecovered++;
		}
		if (this.FramesRecovered >= this.WindowFrames)
		{
			this.model.IsActive = false;
		}
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x0007529F File Offset: 0x0007369F
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ComboStateModel>(ref this.model);
		return true;
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x000752AF File Offset: 0x000736AF
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<ComboStateModel>(this.model));
	}

	// Token: 0x04000DEA RID: 3562
	private ComboStateModel model = new ComboStateModel();
}
