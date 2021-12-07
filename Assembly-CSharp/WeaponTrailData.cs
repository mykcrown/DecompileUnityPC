using System;
using FixedPoint;
using UnityEngine;

// Token: 0x0200050D RID: 1293
[Serializable]
public class WeaponTrailData
{
	// Token: 0x04001684 RID: 5764
	public int startFrame;

	// Token: 0x04001685 RID: 5765
	public int endFrame = 1;

	// Token: 0x04001686 RID: 5766
	public BodyPart bodyPart = BodyPart.leftHand;

	// Token: 0x04001687 RID: 5767
	public BodyPart bodyPart2;

	// Token: 0x04001688 RID: 5768
	public int frameLength = 10;

	// Token: 0x04001689 RID: 5769
	public int granularity = 60;

	// Token: 0x0400168A RID: 5770
	public int fadeFrames = 6;

	// Token: 0x0400168B RID: 5771
	public Material overrideMaterial;

	// Token: 0x0400168C RID: 5772
	public Color color = Color.white;

	// Token: 0x0400168D RID: 5773
	public bool useOffsets;

	// Token: 0x0400168E RID: 5774
	public Fixed bodyPartOffset1 = 0;

	// Token: 0x0400168F RID: 5775
	public Fixed bodyPartOffset2 = 0;
}
