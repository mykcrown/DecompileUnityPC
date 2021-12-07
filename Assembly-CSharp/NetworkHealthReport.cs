using System;

// Token: 0x0200079C RID: 1948
public class NetworkHealthReport : INetworkHealthReport
{
	// Token: 0x17000B80 RID: 2944
	// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x000EF106 File Offset: 0x000ED506
	// (set) Token: 0x06002FF9 RID: 12281 RVA: 0x000EF10E File Offset: 0x000ED50E
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000B81 RID: 2945
	// (get) Token: 0x06002FFA RID: 12282 RVA: 0x000EF117 File Offset: 0x000ED517
	// (set) Token: 0x06002FFB RID: 12283 RVA: 0x000EF11F File Offset: 0x000ED51F
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000B82 RID: 2946
	// (get) Token: 0x06002FFC RID: 12284 RVA: 0x000EF128 File Offset: 0x000ED528
	// (set) Token: 0x06002FFD RID: 12285 RVA: 0x000EF130 File Offset: 0x000ED530
	[Inject]
	public IRollbackStatus rollbackStatus { get; set; }

	// Token: 0x17000B83 RID: 2947
	// (get) Token: 0x06002FFE RID: 12286 RVA: 0x000EF139 File Offset: 0x000ED539
	protected IRollbackClient client
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x000EF146 File Offset: 0x000ED546
	private void Set(int calculatedLatencyMs, int messageDelay, int skippedFrames, int networkBytesSent, int networkBytesReceived)
	{
		this.messageDelay = messageDelay;
		this.calculatedLatencyMs = calculatedLatencyMs;
		this.networkBytesSent = networkBytesSent;
		this.networkBytesReceived = networkBytesReceived;
		this.skippedFrames = skippedFrames;
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x000EF170 File Offset: 0x000ED570
	public void Update(GameManager gameManager, int frameDelta)
	{
		if (frameDelta > 0)
		{
			int num = frameDelta;
			if (gameManager.StartedGame)
			{
				num = 0;
			}
			this.Set(this.rollbackStatus.CalculatedLatencyMs, this.client.Frame - this.rollbackStatus.FullyConfirmedFrame, num, this.battleServerAPI.NetworkBytesSent, this.battleServerAPI.NetworkBytesReceived);
			this.client.ReportHealth(this);
			this.battleServerAPI.ClearNetUsage();
		}
	}

	// Token: 0x04002192 RID: 8594
	public int messageDelay;

	// Token: 0x04002193 RID: 8595
	public int calculatedLatencyMs;

	// Token: 0x04002194 RID: 8596
	public int skippedFrames;

	// Token: 0x04002195 RID: 8597
	public int networkBytesSent;

	// Token: 0x04002196 RID: 8598
	public int networkBytesReceived;
}
