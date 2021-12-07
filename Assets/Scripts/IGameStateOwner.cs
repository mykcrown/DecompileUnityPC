// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IGameStateOwner
{
	void ReleaseOwnedStates();

	void LoadState(GameStateContainer container);

	void ExportState(GameStateContainer container);
}
