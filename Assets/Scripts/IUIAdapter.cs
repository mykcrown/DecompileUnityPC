// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUIAdapter
{
	ScreenType CurrentScreen
	{
		get;
	}

	ScreenType PreviousScreen
	{
		get;
	}

	ScreenType LoadingScreen
	{
		get;
	}

	void Initialize(IEvents events, IScreenDisplay display);

	void OnScreenCreated(GameScreen screen);

	bool AtOrGoingTo(ScreenType screen);

	void PreloadScreen(ScreenType screen);

	void ShowPreloaded();

	void OnGameSynced();

	void SendUpdate(GameEvent message);

	void Subscribe(Type type);

	T GetUIScene<T>() where T : UIScene;
}
