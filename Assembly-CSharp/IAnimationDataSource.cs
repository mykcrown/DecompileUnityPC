using System;
using FixedPoint;

// Token: 0x02000343 RID: 835
public interface IAnimationDataSource
{
	// Token: 0x060011B0 RID: 4528
	BoneFrameData GetBoneFrameData(string animationName, BodyPart bodyPart, int gameFrame, HorizontalDirection facing);

	// Token: 0x060011B1 RID: 4529
	bool HasRootDeltaData(string animationName);

	// Token: 0x060011B2 RID: 4530
	Vector3F GetRootDelta(string animationName, int gameFrame);

	// Token: 0x060011B3 RID: 4531
	FixedRect GetMaxBounds(string animationName);

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x060011B4 RID: 4532
	bool IsBoneDataAbsolute { get; }
}
