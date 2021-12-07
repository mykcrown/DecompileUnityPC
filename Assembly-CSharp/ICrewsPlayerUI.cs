using System;
using UnityEngine;

// Token: 0x020008BB RID: 2235
public interface ICrewsPlayerUI
{
	// Token: 0x06003845 RID: 14405
	void Initialize(BattleSettings config, PlayerSelectionInfo playerInfo, TeamNum team, CrewsGUISide side);

	// Token: 0x17000DA2 RID: 3490
	// (get) Token: 0x06003846 RID: 14406
	Transform Transform { get; }

	// Token: 0x06003847 RID: 14407
	void TickFrame();
}
