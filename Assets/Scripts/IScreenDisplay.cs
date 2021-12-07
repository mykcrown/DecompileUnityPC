// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IScreenDisplay
{
	GameScreen GetScreenByType(ScreenType type);

	void ShowScreen(GameScreen screen, Payload payload, LoadSequenceResults data, bool waitToClearPrevious = false);

	void ClearPreviousScreen();
}
