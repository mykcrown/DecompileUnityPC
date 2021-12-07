// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface ISceneLists
{
	string GetSceneForUIScreen(ScreenType type);

	List<string> GetUIPreloadSceneList();
}
