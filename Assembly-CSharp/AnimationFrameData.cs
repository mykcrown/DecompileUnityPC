using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x02000341 RID: 833
public class AnimationFrameData
{
	// Token: 0x17000311 RID: 785
	// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00066252 File Offset: 0x00064652
	public bool HasRootDeltaData
	{
		get
		{
			return this.rootDeltaData != null;
		}
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x00066260 File Offset: 0x00064660
	public Vector3F GetRootDeltaData(int gameFrame)
	{
		if (!this.rootDeltaData.ContainsKey(gameFrame))
		{
			return Vector3F.zero;
		}
		return this.rootDeltaData[gameFrame];
	}

	// Token: 0x04000B45 RID: 2885
	public Dictionary<int, Dictionary<BodyPart, BoneFrameData>> boneData;

	// Token: 0x04000B46 RID: 2886
	public Dictionary<int, Vector3F> rootDeltaData;

	// Token: 0x04000B47 RID: 2887
	public FixedRect maxBounds;

	// Token: 0x04000B48 RID: 2888
	public bool reversesFacing;
}
