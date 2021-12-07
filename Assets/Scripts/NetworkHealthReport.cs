// Decompile from assembly: Assembly-CSharp.dll

using System;

public class NetworkHealthReport : INetworkHealthReport
{
	public int messageDelay;

	public int calculatedLatencyMs;

	public int skippedFrames;

	public int networkBytesSent;

	public int networkBytesReceived;

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatus rollbackStatus
	{
		get;
		set;
	}

	protected IRollbackClient client
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	private void Set(int calculatedLatencyMs, int messageDelay, int skippedFrames, int networkBytesSent, int networkBytesReceived)
	{
		this.messageDelay = messageDelay;
		this.calculatedLatencyMs = calculatedLatencyMs;
		this.networkBytesSent = networkBytesSent;
		this.networkBytesReceived = networkBytesReceived;
		this.skippedFrames = skippedFrames;
	}

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
}
