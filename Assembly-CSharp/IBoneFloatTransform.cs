using System;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public interface IBoneFloatTransform
{
	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06001403 RID: 5123
	Vector3 position { get; }

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06001404 RID: 5124
	Quaternion rotation { get; }

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06001405 RID: 5125
	BodyPart Bone { get; }
}
