using System;

// Token: 0x020004AC RID: 1196
[Serializable]
public class GameModeState : RollbackStateTyped<GameModeState>
{
	// Token: 0x06001A71 RID: 6769 RVA: 0x00089364 File Offset: 0x00087764
	public override void CopyTo(GameModeState target)
	{
		target.endedGameFrame = this.endedGameFrame;
		target.endedGameTime = this.endedGameTime;
	}

	// Token: 0x040013B7 RID: 5047
	public int endedGameFrame = -1;

	// Token: 0x040013B8 RID: 5048
	[IgnoreRollback(IgnoreRollbackType.External)]
	public long endedGameTime;
}
