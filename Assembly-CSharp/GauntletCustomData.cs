using System;
using FixedPoint;

// Token: 0x020004A8 RID: 1192
[GameModeDataType(GameMode.Gauntlet)]
public class GauntletCustomData : CustomGameModeData
{
	// Token: 0x040013B3 RID: 5043
	public Fixed knockbackIncrease = 50;

	// Token: 0x040013B4 RID: 5044
	public TeamAbilityConfig teamAbilities = new TeamAbilityConfig();
}
