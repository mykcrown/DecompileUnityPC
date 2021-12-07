using System;
using System.Collections.Generic;

// Token: 0x0200087F RID: 2175
public interface IRollbackQuitGame
{
	// Token: 0x060036A1 RID: 13985
	void Init(IBattleServerAPI battleServerAPI, RollbackSettings settings);

	// Token: 0x060036A2 RID: 13986
	void Destroy();

	// Token: 0x060036A3 RID: 13987
	bool IsQuitting(int playerID);

	// Token: 0x060036A4 RID: 13988
	void Setup(int playerID, int frame, List<int> players);

	// Token: 0x060036A5 RID: 13989
	void Tick();
}
