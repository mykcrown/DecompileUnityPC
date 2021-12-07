using System;

// Token: 0x0200067D RID: 1661
[Serializable]
public class TimeKeeperModel : RollbackStateTyped<TimeKeeperModel>
{
	// Token: 0x06002915 RID: 10517 RVA: 0x000C6A6E File Offset: 0x000C4E6E
	public override void CopyTo(TimeKeeperModel target)
	{
		target.ticksElapsed = this.ticksElapsed;
		target.hasStarted = this.hasStarted;
	}

	// Token: 0x04001DBE RID: 7614
	public int ticksElapsed;

	// Token: 0x04001DBF RID: 7615
	public bool hasStarted;
}
