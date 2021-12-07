using System;

// Token: 0x02000631 RID: 1585
public class LoopTrigger : StageTrigger
{
	// Token: 0x060026F6 RID: 9974 RVA: 0x000BE940 File Offset: 0x000BCD40
	public override void TickFrame()
	{
		int frame = this.triggerDependency.Frame;
		bool flag = frame >= this.DelayStartFrames && (frame - this.DelayStartFrames) % this.LoopFrames == 0;
		if (flag && this.Triggered != null)
		{
			base.CallTriggered(null);
		}
	}

	// Token: 0x04001C7B RID: 7291
	public int DelayStartFrames;

	// Token: 0x04001C7C RID: 7292
	public int LoopFrames = 1;
}
