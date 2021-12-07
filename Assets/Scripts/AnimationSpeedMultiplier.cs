// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class AnimationSpeedMultiplier
{
	public int startFrame = 1;

	public int endFrame = 2;

	public float speedMultiplier = 1f;

	public AnimationSpeedMultiplier()
	{
	}

	public AnimationSpeedMultiplier(int startFrame, int endFrame, float speedMultiplier)
	{
		this.startFrame = startFrame;
		this.endFrame = endFrame;
		this.speedMultiplier = speedMultiplier;
	}
}
