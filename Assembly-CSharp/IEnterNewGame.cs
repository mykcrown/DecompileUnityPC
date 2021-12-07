using System;

// Token: 0x02000535 RID: 1333
public interface IEnterNewGame
{
	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06001D07 RID: 7431
	GameLoadPayload GamePayload { get; }

	// Token: 0x06001D08 RID: 7432
	void StartPreviewGame(bool skipsToVictory);

	// Token: 0x06001D09 RID: 7433
	void StartTestGame();

	// Token: 0x06001D0A RID: 7434
	bool StartReplay(IReplaySystem replaySystem);

	// Token: 0x06001D0B RID: 7435
	void InitPayload(GameStartType startType, GameLoadPayload copyFromPayload = null);
}
