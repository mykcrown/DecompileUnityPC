using System;
using IconsServer;
using UnityEngine;

// Token: 0x0200079E RID: 1950
public class NetworkHealthReporter : ClientBehavior
{
	// Token: 0x17000B84 RID: 2948
	// (get) Token: 0x06003003 RID: 12291 RVA: 0x000EF21F File Offset: 0x000ED61F
	// (set) Token: 0x06003004 RID: 12292 RVA: 0x000EF227 File Offset: 0x000ED627
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x06003005 RID: 12293 RVA: 0x000EF230 File Offset: 0x000ED630
	// (set) Token: 0x06003006 RID: 12294 RVA: 0x000EF238 File Offset: 0x000ED638
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000B86 RID: 2950
	// (get) Token: 0x06003007 RID: 12295 RVA: 0x000EF241 File Offset: 0x000ED641
	// (set) Token: 0x06003008 RID: 12296 RVA: 0x000EF249 File Offset: 0x000ED649
	[Inject]
	public IGameClient gameClient { get; set; }

	// Token: 0x17000B87 RID: 2951
	// (get) Token: 0x06003009 RID: 12297 RVA: 0x000EF252 File Offset: 0x000ED652
	// (set) Token: 0x0600300A RID: 12298 RVA: 0x000EF25A File Offset: 0x000ED65A
	[Inject]
	public IRollbackStatus rollbackStatus { get; set; }

	// Token: 0x0600300B RID: 12299 RVA: 0x000EF264 File Offset: 0x000ED664
	private void Update()
	{
		double totalSeconds = DateTime.UtcNow.TimeOfDay.TotalSeconds;
		if (totalSeconds - this.lastHealthTimeSeconds >= 1.0)
		{
			this.lastHealthTimeSeconds = totalSeconds;
		}
	}

	// Token: 0x0600300C RID: 12300 RVA: 0x000EF2A4 File Offset: 0x000ED6A4
	public void ReportHealth(NetworkHealthReport health)
	{
		this.calculatedLatencyMs = health.calculatedLatencyMs;
		this.calculatedLatencyStats.RecordValue((float)this.calculatedLatencyMs);
		this.frameDelay = health.messageDelay;
		this.frameDelayStats.RecordValue((float)this.frameDelay);
	}

	// Token: 0x0600300D RID: 12301 RVA: 0x000EF2E2 File Offset: 0x000ED6E2
	private void OnGUI()
	{
		if (base.battleServerAPI.IsOnlineMatchReady && BuildConfig.environmentType == BuildEnvironment.Local)
		{
			GUI.contentColor = Color.white;
		}
	}

	// Token: 0x0600300E RID: 12302 RVA: 0x000EF308 File Offset: 0x000ED708
	private Color getPingColor(int ping)
	{
		if (ping < 40)
		{
			return Color.green;
		}
		if (ping < 60)
		{
			return Color.yellow;
		}
		if (ping < 100)
		{
			return new Color(1f, 0.5f, 0f);
		}
		if (ping < 200)
		{
			return Color.red;
		}
		return new Color(0.3f, 0f, 0f);
	}

	// Token: 0x04002197 RID: 8599
	private double lastHealthTimeSeconds;

	// Token: 0x04002198 RID: 8600
	private int calculatedLatencyMs;

	// Token: 0x04002199 RID: 8601
	private int frameDelay;

	// Token: 0x0400219E RID: 8606
	private TimeStatTracker calculatedLatencyStats = new TimeStatTracker(1000, true, false, string.Empty);

	// Token: 0x0400219F RID: 8607
	private TimeStatTracker frameDelayStats = new TimeStatTracker(1000, true, false, string.Empty);
}
