// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IEnterNewGame
{
	GameLoadPayload GamePayload
	{
		get;
	}

	void StartPreviewGame(bool skipsToVictory);

	void StartTestGame();

	bool StartReplay(IReplaySystem replaySystem);

	void InitPayload(GameStartType startType, GameLoadPayload copyFromPayload = null);
}
