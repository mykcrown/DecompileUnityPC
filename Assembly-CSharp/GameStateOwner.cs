using System;

// Token: 0x02000866 RID: 2150
public abstract class GameStateOwner : IGameStateOwner
{
	// Token: 0x060035B2 RID: 13746 RVA: 0x000FE250 File Offset: 0x000FC650
	~GameStateOwner()
	{
		this.Dispose();
	}

	// Token: 0x060035B3 RID: 13747 RVA: 0x000FE280 File Offset: 0x000FC680
	protected virtual void Dispose()
	{
		if (!this.isStateReleased)
		{
			this.ReleaseOwnedStates();
			this.isStateReleased = true;
		}
	}

	// Token: 0x060035B4 RID: 13748
	public abstract void ExportState(GameStateContainer container);

	// Token: 0x060035B5 RID: 13749
	public abstract void LoadState(GameStateContainer container);

	// Token: 0x060035B6 RID: 13750
	public abstract void ReleaseOwnedStates();

	// Token: 0x040024D1 RID: 9425
	private bool isStateReleased;
}
