// Decompile from assembly: Assembly-CSharp.dll

using System;

public class NetworkStatusLocator : INetworkStatus
{
	[Inject]
	public GameController locator
	{
		get;
		set;
	}

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

	bool INetworkStatus.WaitingFor(int clientID)
	{
		return this.source != null && this.source.WaitingFor(clientID);
	}

	int INetworkStatus.CalculatedLatencyTo(int clientID, bool average)
	{
		if (this.source == null)
		{
			return -1;
		}
		return this.source.CalculatedLatencyTo(clientID, average);
	}

	int INetworkStatus.LatencyTo(int clientID, bool average)
	{
		if (this.source == null)
		{
			return -1;
		}
		return this.source.LatencyTo(clientID, average);
	}

	int INetworkStatus.FrameDelayTo(int clientID)
	{
		if (this.source == null)
		{
			return -1;
		}
		return this.source.FrameDelayTo(clientID);
	}

	int INetworkStatus.CompensationFor(int clientID)
	{
		if (this.source == null)
		{
			return -1;
		}
		return this.source.CompensationFor(clientID);
	}

	double INetworkStatus.TotalWaitTimeFor(int clientID)
	{
		if (this.source == null)
		{
			return -1.0;
		}
		return this.source.TotalWaitTimeFor(clientID);
	}
}
