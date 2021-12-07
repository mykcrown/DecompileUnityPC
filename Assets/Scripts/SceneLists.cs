// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class SceneLists : ISceneLists
{
	[Inject]
	public ConfigData config
	{
		private get;
		set;
	}

	[Inject]
	public IUserVideoSettingsModel userVideoSettings
	{
		private get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		private get;
		set;
	}

	[Inject]
	public IMainMenuAPI mainMenuAPI
	{
		private get;
		set;
	}

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

	public List<string> GetUIPreloadSceneList()
	{
		List<string> list = new List<string>();
		foreach (ScreenType current in this.gameDataManager.ConfigData.uiConfig.preloadSceneScreens)
		{
			string sceneForUIScreen = this.GetSceneForUIScreen(current);
			list.Add(sceneForUIScreen);
		}
		list.AddRange(this.gameDataManager.ConfigData.uiConfig.preloadScenes);
		return list;
	}

	private string getSceneForTypeAndQuality(ScreenType type, ThreeTierQualityLevel quality)
	{
		UISceneDictionary uISceneDictionary;
		if (quality == ThreeTierQualityLevel.Low)
		{
			uISceneDictionary = this.config.uiConfig.lowDetailScenes;
		}
		else
		{
			uISceneDictionary = this.config.uiConfig.scenes;
		}
		if (!uISceneDictionary.ContainsKey(type))
		{
			return null;
		}
		if (string.IsNullOrEmpty(uISceneDictionary[type]))
		{
			return null;
		}
		return uISceneDictionary[type];
	}
}
