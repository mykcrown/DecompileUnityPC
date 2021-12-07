// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface INetworkStatus
{
	bool WaitingFor(int clientID);

	int LatencyTo(int clientID, bool average);

	int CalculatedLatencyTo(int clientID, bool average);

	int FrameDelayTo(int clientID);

	int CompensationFor(int clientID);

	double TotalWaitTimeFor(int clientID);
}
