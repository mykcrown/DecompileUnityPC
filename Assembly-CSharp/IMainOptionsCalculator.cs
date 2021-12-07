using System;

// Token: 0x020008E6 RID: 2278
public interface IMainOptionsCalculator
{
	// Token: 0x06003A5C RID: 14940
	MainOptionsList GetLeftRightOptions(GameMode mode, GameRules rules);

	// Token: 0x06003A5D RID: 14941
	MoreOptionsList GetAllOptions(GameMode mode, GameRules rules);
}
