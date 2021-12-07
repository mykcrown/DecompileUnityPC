using System;
using System.Collections.Generic;

// Token: 0x02000893 RID: 2195
public interface ISceneLists
{
	// Token: 0x06003739 RID: 14137
	string GetSceneForUIScreen(ScreenType type);

	// Token: 0x0600373A RID: 14138
	List<string> GetUIPreloadSceneList();
}
