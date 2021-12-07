using System;
using FixedPoint;

// Token: 0x020003A2 RID: 930
public interface IBoneTransform
{
	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06001400 RID: 5120
	Vector3F Position { get; }

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06001401 RID: 5121
	QuaternionF Rotation { get; }

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06001402 RID: 5122
	BodyPart Bone { get; }
}
