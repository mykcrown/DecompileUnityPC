using System;

// Token: 0x02000864 RID: 2148
public interface IGameStateOwner
{
	// Token: 0x060035A8 RID: 13736
	void ReleaseOwnedStates();

	// Token: 0x060035A9 RID: 13737
	void LoadState(GameStateContainer container);

	// Token: 0x060035AA RID: 13738
	void ExportState(GameStateContainer container);
}
