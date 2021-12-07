using System;
using FixedPoint;

// Token: 0x0200050C RID: 1292
[Serializable]
public class BlockMovementData
{
	// Token: 0x0400167E RID: 5758
	public int startFrame;

	// Token: 0x0400167F RID: 5759
	public int endFrame;

	// Token: 0x04001680 RID: 5760
	public bool blockAllMovement = true;

	// Token: 0x04001681 RID: 5761
	public bool blockFastFall;

	// Token: 0x04001682 RID: 5762
	public Fixed airMobilityMulti = 1;

	// Token: 0x04001683 RID: 5763
	public Fixed maxHAirVelocityMulti = 1;
}
