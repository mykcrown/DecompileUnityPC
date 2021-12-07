using System;

// Token: 0x020007FF RID: 2047
public class NetworkStatusLocator : INetworkStatus
{
	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x060032A2 RID: 12962 RVA: 0x000F31BD File Offset: 0x000F15BD
	// (set) Token: 0x060032A3 RID: 12963 RVA: 0x000F31C5 File Offset: 0x000F15C5
	[Inject]
	public GameController locator { get; set; }

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x060032A4 RID: 12964 RVA: 0x000F31D0 File Offset: 0x000F15D0
	private INetworkStatus source
	{
		get
		{
			if (this.locator == null || this.locator.currentGame == null || this.locator.currentGame.FrameController == null)
			{
				return null;
			}
			return this.locator.currentGame.FrameController.rollbackLayer as INetworkStatus;
		}
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x000F3235 File Offset: 0x000F1635
	bool INetworkStatus.WaitingFor(int clientID)
	{
		return this.source != null && this.source.WaitingFor(clientID);
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x000F3250 File Offset: 0x000F1650
	int INetworkStatus.CalculatedLatencyTo(int clientID, bool average)
	{
		if (this.source == null)
		{
			return -1;
		}
		return this.source.CalculatedLatencyTo(clientID, average);
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x000F326C File Offset: 0x000F166C
	int INetworkStatus.LatencyTo(int clientID, bool average)
	{
		if (this.source == null)
		{
			return -1;
		}
		return this.source.LatencyTo(clientID, average);
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x000F3288 File Offset: 0x000F1688
	int INetworkStatus.FrameDelayTo(int clientID)
	{
		if (this.source == null)
		{
			return -1;
		}
		return this.source.FrameDelayTo(clientID);
	}

	// Token: 0x060032A9 RID: 12969 RVA: 0x000F32A3 File Offset: 0x000F16A3
	int INetworkStatus.CompensationFor(int clientID)
	{
		if (this.source == null)
		{
			return -1;
		}
		return this.source.CompensationFor(clientID);
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x000F32BE File Offset: 0x000F16BE
	double INetworkStatus.TotalWaitTimeFor(int clientID)
	{
		if (this.source == null)
		{
			return -1.0;
		}
		return this.source.TotalWaitTimeFor(clientID);
	}
}
