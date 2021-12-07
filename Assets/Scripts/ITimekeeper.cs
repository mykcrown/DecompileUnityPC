// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ITimekeeper
{
	double MsSinceStart
	{
		get;
	}

	void Start(double startTimeMs);

	void Init(RollbackSettings settings);

	int CalculateTargetFrame();

	void ResetMilestone(int currentFrame);

	void IncreasePlaybackSpeed();

	void DecreasePlaybackSpeed();

	void SetPlaybackSpeed(float newSpeed);

	void OnAllInputsFrameUpdated(IRollbackClient client, int frame);

	void Destroy();

	double GetMSFrameOffset();
}
