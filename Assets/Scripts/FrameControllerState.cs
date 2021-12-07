// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class FrameControllerState : RollbackStateTyped<FrameControllerState>
{
	public int currentFrame;

	public override void CopyTo(FrameControllerState target)
	{
		target.currentFrame = this.currentFrame;
	}
}
