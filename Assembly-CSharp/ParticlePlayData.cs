using System;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public struct ParticlePlayData
{
	// Token: 0x0400112E RID: 4398
	public ParticleData particle;

	// Token: 0x0400112F RID: 4399
	public bool shouldOverridePosition;

	// Token: 0x04001130 RID: 4400
	public Vector3 overridePosition;

	// Token: 0x04001131 RID: 4401
	public bool shouldOverrideRotation;

	// Token: 0x04001132 RID: 4402
	public Quaternion overrideRotation;

	// Token: 0x04001133 RID: 4403
	public BodyPart overrideBodyPart;

	// Token: 0x04001134 RID: 4404
	public SkinData skinData;

	// Token: 0x04001135 RID: 4405
	public bool shouldOverrideQualityFilter;

	// Token: 0x04001136 RID: 4406
	public TeamNum teamNum;
}
