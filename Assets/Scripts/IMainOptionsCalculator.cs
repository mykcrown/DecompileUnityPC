// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMainOptionsCalculator
{
	MainOptionsList GetLeftRightOptions(GameMode mode, GameRules rules);

	MoreOptionsList GetAllOptions(GameMode mode, GameRules rules);
}
