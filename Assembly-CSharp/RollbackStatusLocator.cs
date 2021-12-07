using System;

// Token: 0x0200088E RID: 2190
public class RollbackStatusLocator : IRollbackStatus
{
	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x060036F1 RID: 14065 RVA: 0x00100769 File Offset: 0x000FEB69
	// (set) Token: 0x060036F2 RID: 14066 RVA: 0x00100771 File Offset: 0x000FEB71
	[Inject]
	public GameController locator { get; set; }

	// Token: 0x17000D5B RID: 3419
	// (get) Token: 0x060036F3 RID: 14067 RVA: 0x0010077A File Offset: 0x000FEB7A
	// (set) Token: 0x060036F4 RID: 14068 RVA: 0x00100782 File Offset: 0x000FEB82
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000D5C RID: 3420
	// (get) Token: 0x060036F5 RID: 14069 RVA: 0x0010078C File Offset: 0x000FEB8C
	private IRollbackStatus source
	{
		get
		{
			if (this.locator == null || this.locator.currentGame == null || this.locator.currentGame.FrameController == null)
			{
				return null;
			}
			return this.locator.currentGame.FrameController.rollbackLayer as IRollbackStatus;
		}
	}

	// Token: 0x17000D51 RID: 3409
	// (get) Token: 0x060036F6 RID: 14070 RVA: 0x001007F1 File Offset: 0x000FEBF1
	bool IRollbackStatus.RollbackEnabled
	{
		get
		{
			return this.source != null && this.source.RollbackEnabled;
		}
	}

	// Token: 0x17000D52 RID: 3410
	// (get) Token: 0x060036F7 RID: 14071 RVA: 0x0010080C File Offset: 0x000FEC0C
	bool IRollbackStatus.IsRollingBack
	{
		get
		{
			return this.source != null && this.source.IsRollingBack;
		}
	}

	// Token: 0x17000D53 RID: 3411
	// (get) Token: 0x060036F8 RID: 14072 RVA: 0x00100826 File Offset: 0x000FEC26
	int IRollbackStatus.FullyConfirmedFrame
	{
		get
		{
			if (this.source == null)
			{
				return -1;
			}
			return this.source.FullyConfirmedFrame;
		}
	}

	// Token: 0x17000D54 RID: 3412
	// (get) Token: 0x060036F9 RID: 14073 RVA: 0x00100840 File Offset: 0x000FEC40
	int IRollbackStatus.LowestInputAckFrame
	{
		get
		{
			if (this.source == null)
			{
				return -1;
			}
			return this.source.LowestInputAckFrame;
		}
	}

	// Token: 0x17000D55 RID: 3413
	// (get) Token: 0x060036FA RID: 14074 RVA: 0x0010085A File Offset: 0x000FEC5A
	int IRollbackStatus.InputDelayFrames
	{
		get
		{
			if (this.source == null)
			{
				return this.config.networkSettings.inputDelayFrames;
			}
			return this.source.InputDelayFrames;
		}
	}

	// Token: 0x17000D56 RID: 3414
	// (get) Token: 0x060036FB RID: 14075 RVA: 0x00100883 File Offset: 0x000FEC83
	int IRollbackStatus.InputDelayPing
	{
		get
		{
			if (this.source == null)
			{
				return 0;
			}
			return this.source.InputDelayPing;
		}
	}

	// Token: 0x17000D57 RID: 3415
	// (get) Token: 0x060036FC RID: 14076 RVA: 0x0010089D File Offset: 0x000FEC9D
	int IRollbackStatus.CalculatedLatencyMs
	{
		get
		{
			if (this.source == null)
			{
				return 0;
			}
			return this.source.CalculatedLatencyMs;
		}
	}

	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x060036FD RID: 14077 RVA: 0x001008B7 File Offset: 0x000FECB7
	long IRollbackStatus.MsSinceStart
	{
		get
		{
			if (this.source == null)
			{
				return 0L;
			}
			return this.source.MsSinceStart;
		}
	}

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x060036FE RID: 14078 RVA: 0x001008D2 File Offset: 0x000FECD2
	int IRollbackStatus.InitialTimestepDelta
	{
		get
		{
			if (this.source == null)
			{
				return 0;
			}
			return this.source.InitialTimestepDelta;
		}
	}
}
