// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class GameManagerState : RollbackStateTyped<GameManagerState>
{
	[IgnoreRollback(IgnoreRollbackType.Debug)]
	public string gameVersion = string.Empty;

	public bool gameStarted;

	public bool gameWasForfeitted;

	public bool gameDebugDesync;

	public override void CopyTo(GameManagerState target)
	{
		target.gameVersion = this.gameVersion;
		target.gameStarted = this.gameStarted;
		target.gameWasForfeitted = this.gameWasForfeitted;
		target.gameDebugDesync = this.gameDebugDesync;
	}
}
