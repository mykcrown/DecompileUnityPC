using System;

// Token: 0x0200029E RID: 670
public interface ISceneController : IStartupLoader, ITickable
{
	// Token: 0x06000E4E RID: 3662
	void Init();

	// Token: 0x06000E4F RID: 3663
	void PreloadScene(string sceneName, bool async, Action callback = null);

	// Token: 0x06000E50 RID: 3664
	void PreloadUIScene(ScreenType type, Action callback);

	// Token: 0x06000E51 RID: 3665
	void ActivateUIScene(ScreenType type);

	// Token: 0x06000E52 RID: 3666
	void LoadBattleScene(string sceneName, Action callback);

	// Token: 0x06000E53 RID: 3667
	void LoadVictoryPoseScene(Action callback);

	// Token: 0x06000E54 RID: 3668
	void ExitBattle(Action callback);

	// Token: 0x06000E55 RID: 3669
	T GetUIScene<T>() where T : UIScene;
}
