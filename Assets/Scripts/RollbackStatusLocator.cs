// Decompile from assembly: Assembly-CSharp.dll

using System;

public class RollbackStatusLocator : IRollbackStatus
{
	bool IRollbackStatus.RollbackEnabled
	{
		get
		{
			return this.source != null && this.source.RollbackEnabled;
		}
	}

	bool IRollbackStatus.IsRollingBack
	{
		get
		{
			return this.source != null && this.source.IsRollingBack;
		}
	}

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

	[Inject]
	public GameController locator
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

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
}
