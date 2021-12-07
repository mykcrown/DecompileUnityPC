// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class WallJumpConfig
{
	public Fixed wallFlushDistance = (Fixed)0.10000000149011612;

	public int wallPressFrameWindow = 5;

	public int ledgeReleaseLockoutFrames = 5;

	public int ledgeGrabCooldown = 50;
}
