// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class UIConfig
{
	public float defaultFadeDuration = 2f;

	public bool ignoreDataDependencies;

	public CanvasScalerConfig canvasScaler;

	public bool useCanvasScaler;

	public GameScreenDictionary screens = new GameScreenDictionary();

	public UISceneDictionary scenes = new UISceneDictionary();

	public UISceneDictionary lowDetailScenes = new UISceneDictionary();

	public List<ScreenType> preloadSceneScreens = new List<ScreenType>();

	public List<string> preloadScenes = new List<string>();
}
