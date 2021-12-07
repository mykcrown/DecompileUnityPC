using System;
using UnityEngine;

// Token: 0x020008B1 RID: 2225
public interface ICrewsGUI
{
	// Token: 0x060037EA RID: 14314
	void Initialize(BattleSettings config, PlayerSelectionList players);

	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x060037EB RID: 14315
	Transform Transform { get; }

	// Token: 0x060037EC RID: 14316
	void TickFrame();
}
