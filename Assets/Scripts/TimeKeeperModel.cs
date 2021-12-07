// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class TimeKeeperModel : RollbackStateTyped<TimeKeeperModel>
{
	public int ticksElapsed;

	public bool hasStarted;

	public override void CopyTo(TimeKeeperModel target)
	{
		target.ticksElapsed = this.ticksElapsed;
		target.hasStarted = this.hasStarted;
	}
}
