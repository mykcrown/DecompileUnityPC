using System;
using FixedPoint;

// Token: 0x02000576 RID: 1398
[Serializable]
public class AnimationControllerState : RollbackStateTyped<AnimationControllerState>
{
	// Token: 0x06001F26 RID: 7974 RVA: 0x0009F767 File Offset: 0x0009DB67
	public override void CopyTo(AnimationControllerState targetIn)
	{
		targetIn.blendOutDuration = this.blendOutDuration;
		targetIn.actionState = this.actionState;
		targetIn.paused = this.paused;
		targetIn.overrideSpeed = this.overrideSpeed;
		targetIn.currentGameFrame = this.currentGameFrame;
	}

	// Token: 0x040018D4 RID: 6356
	[IgnoreFloatValidation]
	public float blendOutDuration = -1f;

	// Token: 0x040018D5 RID: 6357
	public ActionState actionState = ActionState.None;

	// Token: 0x040018D6 RID: 6358
	public bool paused;

	// Token: 0x040018D7 RID: 6359
	public Fixed overrideSpeed = 1;

	// Token: 0x040018D8 RID: 6360
	public int currentGameFrame;
}
