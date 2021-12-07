using System;

// Token: 0x0200065B RID: 1627
public interface IVictoryPoseController
{
	// Token: 0x060027DD RID: 10205
	void InitVictoryPose(VictoryScreenPayload victoryPayload, GameDataManager gameData);

	// Token: 0x060027DE RID: 10206
	void Update();

	// Token: 0x060027DF RID: 10207
	void Clear();
}
