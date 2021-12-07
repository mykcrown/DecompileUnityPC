using System;

// Token: 0x02000463 RID: 1123
[Serializable]
public class FrameControllerState : RollbackStateTyped<FrameControllerState>
{
	// Token: 0x06001729 RID: 5929 RVA: 0x0007D381 File Offset: 0x0007B781
	public override void CopyTo(FrameControllerState target)
	{
		target.currentFrame = this.currentFrame;
	}

	// Token: 0x040011FB RID: 4603
	public int currentFrame;
}
