// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IRollbackQuitGame
{
	void Init(IBattleServerAPI battleServerAPI, RollbackSettings settings);

	void Destroy();

	bool IsQuitting(int playerID);

	void Setup(int playerID, int frame, List<int> players);

	void Tick();
}
