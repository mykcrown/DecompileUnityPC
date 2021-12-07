using System;
using FixedPoint;

namespace AI
{
	// Token: 0x020002FC RID: 764
	public interface IAICalculator
	{
		// Token: 0x060010BA RID: 4282
		bool TestOffstage(PlayerController player);

		// Token: 0x060010BB RID: 4283
		bool TestReachedEdgeOfStage(PlayerController player, int direction);

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060010BC RID: 4284
		int reactionFrames { get; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060010BD RID: 4285
		int buttonSpamFrames { get; }

		// Token: 0x060010BE RID: 4286
		int RandomizeReactionSpeed(int baseReactionSpeed);

		// Token: 0x060010BF RID: 4287
		int RandomizeButtonSpam(int baseButtonSpamFrames);

		// Token: 0x060010C0 RID: 4288
		Fixed GenerateRandomNumber();

		// Token: 0x060010C1 RID: 4289
		PlayerReference FindClosestEnemy(PlayerController player);
	}
}
