using System;
using FixedPoint;

// Token: 0x020004C9 RID: 1225
[Serializable]
public class AssistAttackSpawnAxisData
{
	// Token: 0x04001460 RID: 5216
	public AssistSpawnMethod spawnMethod = AssistSpawnMethod.Teammate;

	// Token: 0x04001461 RID: 5217
	public AssistSpawnOffscreenOption spawnOffscreenOption = AssistSpawnOffscreenOption.Teammmate;

	// Token: 0x04001462 RID: 5218
	public Fixed spawnOffset = 0;

	// Token: 0x04001463 RID: 5219
	public AssistSpawnFlipOffsetOption spawnFlipOffsetOption = AssistSpawnFlipOffsetOption.Facing;

	// Token: 0x04001464 RID: 5220
	public bool enforceMinValue;

	// Token: 0x04001465 RID: 5221
	public Fixed minValue = 0;

	// Token: 0x04001466 RID: 5222
	public bool enforceMaxValue;

	// Token: 0x04001467 RID: 5223
	public Fixed maxValue = 0;
}
