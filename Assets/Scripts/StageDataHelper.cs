// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageDataHelper : IStageDataHelper
{
	private List<StageID> possibleStageIds;

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public StageID GetStageIDFromIconStage(EIconStages stage)
	{
		if (stage == EIconStages.Random)
		{
			return StageID.Random;
		}
		return (StageID)(stage + 1);
	}

	public EIconStages GetIconStageFromStageID(StageID stage)
	{
		if (stage == StageID.Random)
		{
			return EIconStages.Random;
		}
		return (EIconStages)(stage - 1);
	}

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

	public StageID GetRandomPossibleStage()
	{
		List<StageID> possibleOnlineStageChoices = this.GetPossibleOnlineStageChoices();
		int index = UnityEngine.Random.Range(0, possibleOnlineStageChoices.Count);
		return possibleOnlineStageChoices[index];
	}
}
