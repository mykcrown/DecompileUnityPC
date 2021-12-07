using System;
using System.Collections.Generic;

// Token: 0x02000892 RID: 2194
public class SceneLists : ISceneLists
{
	// Token: 0x17000D6F RID: 3439
	// (get) Token: 0x0600372E RID: 14126 RVA: 0x00100C3F File Offset: 0x000FF03F
	// (set) Token: 0x0600372F RID: 14127 RVA: 0x00100C47 File Offset: 0x000FF047
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x17000D70 RID: 3440
	// (get) Token: 0x06003730 RID: 14128 RVA: 0x00100C50 File Offset: 0x000FF050
	// (set) Token: 0x06003731 RID: 14129 RVA: 0x00100C58 File Offset: 0x000FF058
	[Inject]
	public IUserVideoSettingsModel userVideoSettings { private get; set; }

	// Token: 0x17000D71 RID: 3441
	// (get) Token: 0x06003732 RID: 14130 RVA: 0x00100C61 File Offset: 0x000FF061
	// (set) Token: 0x06003733 RID: 14131 RVA: 0x00100C69 File Offset: 0x000FF069
	[Inject]
	public GameDataManager gameDataManager { private get; set; }

	// Token: 0x17000D72 RID: 3442
	// (get) Token: 0x06003734 RID: 14132 RVA: 0x00100C72 File Offset: 0x000FF072
	// (set) Token: 0x06003735 RID: 14133 RVA: 0x00100C7A File Offset: 0x000FF07A
	[Inject]
	public IMainMenuAPI mainMenuAPI { private get; set; }

	// Token: 0x06003736 RID: 14134 RVA: 0x00100C84 File Offset: 0x000FF084
	public string GetSceneForUIScreen(ScreenType type)
	{
		ThreeTierQualityLevel threeTierQualityLevel = this.userVideoSettings.StageQuality;
		string text = null;
		while (text == null && threeTierQualityLevel <= ThreeTierQualityLevel.High)
		{
			text = this.getSceneForTypeAndQuality(type, threeTierQualityLevel);
			if (text == null)
			{
				threeTierQualityLevel++;
			}
		}
		if (text == "__mainmenu")
		{
			text = this.mainMenuAPI.GetCurrentStage();
		}
		return text;
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x00100CE4 File Offset: 0x000FF0E4
	public List<string> GetUIPreloadSceneList()
	{
		List<string> list = new List<string>();
		foreach (ScreenType type in this.gameDataManager.ConfigData.uiConfig.preloadSceneScreens)
		{
			string sceneForUIScreen = this.GetSceneForUIScreen(type);
			list.Add(sceneForUIScreen);
		}
		list.AddRange(this.gameDataManager.ConfigData.uiConfig.preloadScenes);
		return list;
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x00100D7C File Offset: 0x000FF17C
	private string getSceneForTypeAndQuality(ScreenType type, ThreeTierQualityLevel quality)
	{
		UISceneDictionary uisceneDictionary;
		if (quality == ThreeTierQualityLevel.Low)
		{
			uisceneDictionary = this.config.uiConfig.lowDetailScenes;
		}
		else
		{
			uisceneDictionary = this.config.uiConfig.scenes;
		}
		if (!uisceneDictionary.ContainsKey(type))
		{
			return null;
		}
		if (string.IsNullOrEmpty(uisceneDictionary[type]))
		{
			return null;
		}
		return uisceneDictionary[type];
	}
}
