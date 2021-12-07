using System;
using System.Collections.Generic;

// Token: 0x020003F8 RID: 1016
[Serializable]
public class UIConfig
{
	// Token: 0x04001000 RID: 4096
	public float defaultFadeDuration = 2f;

	// Token: 0x04001001 RID: 4097
	public bool ignoreDataDependencies;

	// Token: 0x04001002 RID: 4098
	public CanvasScalerConfig canvasScaler;

	// Token: 0x04001003 RID: 4099
	public bool useCanvasScaler;

	// Token: 0x04001004 RID: 4100
	public GameScreenDictionary screens = new GameScreenDictionary();

	// Token: 0x04001005 RID: 4101
	public UISceneDictionary scenes = new UISceneDictionary();

	// Token: 0x04001006 RID: 4102
	public UISceneDictionary lowDetailScenes = new UISceneDictionary();

	// Token: 0x04001007 RID: 4103
	public List<ScreenType> preloadSceneScreens = new List<ScreenType>();

	// Token: 0x04001008 RID: 4104
	public List<string> preloadScenes = new List<string>();
}
