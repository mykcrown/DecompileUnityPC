// Decompile from assembly: Assembly-CSharp.dll

using System;

public struct CameraImpactModeRequest
{
	public float strength;

	public int frameCount;

	public int direction;

	public int delayFrames;

	public CameraImpactModeRequest(float strength, int direction, int delayFrames, int frameCount)
	{
		this.strength = strength;
		this.direction = direction;
		this.frameCount = frameCount;
		this.delayFrames = delayFrames;
	}
}
