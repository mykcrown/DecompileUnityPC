using System;

// Token: 0x02000A77 RID: 2679
public interface IUIAdapter
{
	// Token: 0x06004E4A RID: 20042
	void Initialize(IEvents events, IScreenDisplay display);

	// Token: 0x06004E4B RID: 20043
	void OnScreenCreated(GameScreen screen);

	// Token: 0x1700128B RID: 4747
	// (get) Token: 0x06004E4C RID: 20044
	ScreenType CurrentScreen { get; }

	// Token: 0x1700128C RID: 4748
	// (get) Token: 0x06004E4D RID: 20045
	ScreenType PreviousScreen { get; }

	// Token: 0x1700128D RID: 4749
	// (get) Token: 0x06004E4E RID: 20046
	ScreenType LoadingScreen { get; }

	// Token: 0x06004E4F RID: 20047
	bool AtOrGoingTo(ScreenType screen);

	// Token: 0x06004E50 RID: 20048
	void PreloadScreen(ScreenType screen);

	// Token: 0x06004E51 RID: 20049
	void ShowPreloaded();

	// Token: 0x06004E52 RID: 20050
	void OnGameSynced();

	// Token: 0x06004E53 RID: 20051
	void SendUpdate(GameEvent message);

	// Token: 0x06004E54 RID: 20052
	void Subscribe(Type type);

	// Token: 0x06004E55 RID: 20053
	T GetUIScene<T>() where T : UIScene;
}
