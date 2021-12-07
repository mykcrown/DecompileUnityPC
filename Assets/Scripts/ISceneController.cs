// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ISceneController : IStartupLoader, ITickable
{
	void Init();

	void PreloadScene(string sceneName, bool async, Action callback = null);

	void PreloadUIScene(ScreenType type, Action callback);

	void ActivateUIScene(ScreenType type);

	void LoadBattleScene(string sceneName, Action callback);

	void LoadVictoryPoseScene(Action callback);

	void ExitBattle(Action callback);

	T GetUIScene<T>() where T : UIScene;
}
