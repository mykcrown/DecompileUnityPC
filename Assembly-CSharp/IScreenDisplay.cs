using System;

// Token: 0x02000668 RID: 1640
public interface IScreenDisplay
{
	// Token: 0x060028A4 RID: 10404
	GameScreen GetScreenByType(ScreenType type);

	// Token: 0x060028A5 RID: 10405
	void ShowScreen(GameScreen screen, Payload payload, LoadSequenceResults data, bool waitToClearPrevious = false);

	// Token: 0x060028A6 RID: 10406
	void ClearPreviousScreen();
}
