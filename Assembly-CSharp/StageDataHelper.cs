using System;
using System.Collections.Generic;
using IconsServer;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class StageDataHelper : IStageDataHelper
{
	// Token: 0x17000968 RID: 2408
	// (get) Token: 0x0600261C RID: 9756 RVA: 0x000BC1FC File Offset: 0x000BA5FC
	// (set) Token: 0x0600261D RID: 9757 RVA: 0x000BC204 File Offset: 0x000BA604
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x0600261E RID: 9758 RVA: 0x000BC20D File Offset: 0x000BA60D
	public StageID GetStageIDFromIconStage(EIconStages stage)
	{
		if (stage == EIconStages.Random)
		{
			return StageID.Random;
		}
		return (StageID)(stage + 1);
	}

	// Token: 0x0600261F RID: 9759 RVA: 0x000BC21B File Offset: 0x000BA61B
	public EIconStages GetIconStageFromStageID(StageID stage)
	{
		if (stage == StageID.Random)
		{
			return EIconStages.Random;
		}
		return (EIconStages)(stage - 1);
	}

	// Token: 0x06002620 RID: 9760 RVA: 0x000BC22C File Offset: 0x000BA62C
	public List<StageID> GetPossibleOnlineStageChoices()
	{
		if (this.possibleStageIds != null)
		{
			return this.possibleStageIds;
		}
		this.possibleStageIds = new List<StageID>();
		Dictionary<string, string> sceneNamePathMap = SceneUtil.GetSceneNamePathMap();
		for (int i = 0; i < 9; i++)
		{
			EIconStages stage = (EIconStages)i;
			StageID stageIDFromIconStage = this.GetStageIDFromIconStage(stage);
			StageData dataByID = this.gameDataManager.StageData.GetDataByID(stageIDFromIconStage);
			if (dataByID != null && dataByID.enabled && !dataByID.isDev && sceneNamePathMap.ContainsKey(dataByID.sceneName))
			{
				this.possibleStageIds.Add(stageIDFromIconStage);
			}
		}
		return this.possibleStageIds;
	}

	// Token: 0x06002621 RID: 9761 RVA: 0x000BC2D8 File Offset: 0x000BA6D8
	public StageID GetRandomPossibleStage()
	{
		List<StageID> possibleOnlineStageChoices = this.GetPossibleOnlineStageChoices();
		int index = UnityEngine.Random.Range(0, possibleOnlineStageChoices.Count);
		return possibleOnlineStageChoices[index];
	}

	// Token: 0x04001BF4 RID: 7156
	private List<StageID> possibleStageIds;
}
