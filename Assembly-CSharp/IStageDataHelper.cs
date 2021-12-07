using System;
using System.Collections.Generic;
using IconsServer;

// Token: 0x0200060C RID: 1548
public interface IStageDataHelper
{
	// Token: 0x06002622 RID: 9762
	EIconStages GetIconStageFromStageID(StageID stage);

	// Token: 0x06002623 RID: 9763
	StageID GetStageIDFromIconStage(EIconStages stage);

	// Token: 0x06002624 RID: 9764
	List<StageID> GetPossibleOnlineStageChoices();

	// Token: 0x06002625 RID: 9765
	StageID GetRandomPossibleStage();
}
