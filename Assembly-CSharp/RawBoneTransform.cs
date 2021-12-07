using System;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public struct RawBoneTransform : IBoneFloatTransform
{
	// Token: 0x0600140D RID: 5133 RVA: 0x00071621 File Offset: 0x0006FA21
	public RawBoneTransform(Transform transform, BodyPart bone = BodyPart.none)
	{
		this.transform = transform;
		this.Bone = bone;
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x0600140E RID: 5134 RVA: 0x00071631 File Offset: 0x0006FA31
	// (set) Token: 0x0600140F RID: 5135 RVA: 0x00071639 File Offset: 0x0006FA39
	public BodyPart Bone { get; private set; }

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06001410 RID: 5136 RVA: 0x00071642 File Offset: 0x0006FA42
	Vector3 IBoneFloatTransform.position
	{
		get
		{
			return this.transform.position;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06001411 RID: 5137 RVA: 0x0007164F File Offset: 0x0006FA4F
	Quaternion IBoneFloatTransform.rotation
	{
		get
		{
			return this.transform.rotation;
		}
	}

	// Token: 0x04000D58 RID: 3416
	private Transform transform;
}
