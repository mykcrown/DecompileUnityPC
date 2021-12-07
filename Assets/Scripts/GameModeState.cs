// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class GameModeState : RollbackStateTyped<GameModeState>
{
	public int endedGameFrame = -1;

	[IgnoreRollback(IgnoreRollbackType.External)]
	public long endedGameTime;

	public override void CopyTo(GameModeState target)
	{
		target.endedGameFrame = this.endedGameFrame;
		target.endedGameTime = this.endedGameTime;
	}
}
