using System;

// Token: 0x02000800 RID: 2048
public interface INetworkStatus
{
	// Token: 0x060032AB RID: 12971
	bool WaitingFor(int clientID);

	// Token: 0x060032AC RID: 12972
	int LatencyTo(int clientID, bool average);

	// Token: 0x060032AD RID: 12973
	int CalculatedLatencyTo(int clientID, bool average);

	// Token: 0x060032AE RID: 12974
	int FrameDelayTo(int clientID);

	// Token: 0x060032AF RID: 12975
	int CompensationFor(int clientID);

	// Token: 0x060032B0 RID: 12976
	double TotalWaitTimeFor(int clientID);
}
