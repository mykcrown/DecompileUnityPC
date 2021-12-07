// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class BlockMovementData
{
	public int startFrame;

	public int endFrame;

	public bool blockAllMovement = true;

	public bool blockFastFall;

	public Fixed airMobilityMulti = 1;

	public Fixed maxHAirVelocityMulti = 1;
}
