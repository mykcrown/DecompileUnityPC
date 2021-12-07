// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[GameModeDataType(GameMode.Gauntlet)]
public class GauntletCustomData : CustomGameModeData
{
	public Fixed knockbackIncrease = 50;

	public TeamAbilityConfig teamAbilities = new TeamAbilityConfig();
}
