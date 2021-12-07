// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LoopTrigger : StageTrigger
{
	public int DelayStartFrames;

	public int LoopFrames = 1;

	public override void TickFrame()
	{
		int frame = this.triggerDependency.Frame;
		bool flag = frame >= this.DelayStartFrames && (frame - this.DelayStartFrames) % this.LoopFrames == 0;
		if (flag && this.Triggered != null)
		{
			base.CallTriggered(null);
		}
	}
}
