// Decompile from assembly: Assembly-CSharp.dll

using System;

public abstract class GameStateOwner : IGameStateOwner
{
	private bool isStateReleased;

	~GameStateOwner()
	{
		this.Dispose();
	}

	protected virtual void Dispose()
	{
		if (!this.isStateReleased)
		{
			this.ReleaseOwnedStates();
			this.isStateReleased = true;
		}
	}

	public abstract void ExportState(GameStateContainer container);

	public abstract void LoadState(GameStateContainer container);

	public abstract void ReleaseOwnedStates();
}
