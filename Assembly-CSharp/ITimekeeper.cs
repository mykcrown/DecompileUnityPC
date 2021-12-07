using System;

// Token: 0x02000891 RID: 2193
public interface ITimekeeper
{
	// Token: 0x06003722 RID: 14114
	void Start(double startTimeMs);

	// Token: 0x06003723 RID: 14115
	void Init(RollbackSettings settings);

	// Token: 0x06003724 RID: 14116
	int CalculateTargetFrame();

	// Token: 0x06003725 RID: 14117
	void ResetMilestone(int currentFrame);

	// Token: 0x17000D6E RID: 3438
	// (get) Token: 0x06003726 RID: 14118
	double MsSinceStart { get; }

	// Token: 0x06003727 RID: 14119
	void IncreasePlaybackSpeed();

	// Token: 0x06003728 RID: 14120
	void DecreasePlaybackSpeed();

	// Token: 0x06003729 RID: 14121
	void SetPlaybackSpeed(float newSpeed);

	// Token: 0x0600372A RID: 14122
	void OnAllInputsFrameUpdated(IRollbackClient client, int frame);

	// Token: 0x0600372B RID: 14123
	void Destroy();

	// Token: 0x0600372C RID: 14124
	double GetMSFrameOffset();
}
