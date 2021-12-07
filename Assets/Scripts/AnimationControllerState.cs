// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class AnimationControllerState : RollbackStateTyped<AnimationControllerState>
{
	[IgnoreFloatValidation]
	public float blendOutDuration = -1f;

	public ActionState actionState = ActionState.None;

	public bool paused;

	public Fixed overrideSpeed = 1;

	public int currentGameFrame;

	public override void CopyTo(AnimationControllerState targetIn)
	{
		targetIn.blendOutDuration = this.blendOutDuration;
		targetIn.actionState = this.actionState;
		targetIn.paused = this.paused;
		targetIn.overrideSpeed = this.overrideSpeed;
		targetIn.currentGameFrame = this.currentGameFrame;
	}
}
