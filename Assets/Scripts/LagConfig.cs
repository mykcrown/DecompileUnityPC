// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class LagConfig
{
	public int lightLandLagFrames = 1;

	public int heavyLandLagFrames = 4;

	public Fixed heavyLandSpeedThreshold = (Fixed)20.0;

	public int platformDropLagFrames = 3;

	public int fallThroughPlatformDurationFrames = 3;

	public int platformFallPreventFastfall = 3;

	public int dashPivotFrames = 2;

	public int collisionDiamondAirDelayFrames = 8;

	public int runShieldDelayFrames = 4;

	public void Rescale(Fixed rescale)
	{
		this.heavyLandSpeedThreshold *= rescale;
	}
}
