using System;

// Token: 0x02000479 RID: 1145
[Serializable]
public class GameManagerState : RollbackStateTyped<GameManagerState>
{
	// Token: 0x060018CF RID: 6351 RVA: 0x00082D8D File Offset: 0x0008118D
	public override void CopyTo(GameManagerState target)
	{
		target.gameVersion = this.gameVersion;
		target.gameStarted = this.gameStarted;
		target.gameWasForfeitted = this.gameWasForfeitted;
		target.gameDebugDesync = this.gameDebugDesync;
	}

	// Token: 0x040012B8 RID: 4792
	[IgnoreRollback(IgnoreRollbackType.Debug)]
	public string gameVersion = string.Empty;

	// Token: 0x040012B9 RID: 4793
	public bool gameStarted;

	// Token: 0x040012BA RID: 4794
	public bool gameWasForfeitted;

	// Token: 0x040012BB RID: 4795
	public bool gameDebugDesync;
}
