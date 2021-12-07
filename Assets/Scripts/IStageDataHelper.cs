// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;

public interface IStageDataHelper
{
	EIconStages GetIconStageFromStageID(StageID stage);

	StageID GetStageIDFromIconStage(EIconStages stage);

	List<StageID> GetPossibleOnlineStageChoices();

	StageID GetRandomPossibleStage();
}
