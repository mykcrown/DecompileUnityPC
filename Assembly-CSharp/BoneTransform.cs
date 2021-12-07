using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public struct BoneTransform : IBoneTransform, IBoneFloatTransform
{
	// Token: 0x06001406 RID: 5126 RVA: 0x000715A6 File Offset: 0x0006F9A6
	public BoneTransform(BodyPart bone, IBodyOwner data)
	{
		this.Bone = bone;
		this.data = data;
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06001407 RID: 5127 RVA: 0x000715B6 File Offset: 0x0006F9B6
	// (set) Token: 0x06001408 RID: 5128 RVA: 0x000715BE File Offset: 0x0006F9BE
	public BodyPart Bone { get; private set; }

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06001409 RID: 5129 RVA: 0x000715C7 File Offset: 0x0006F9C7
	public Vector3F Position
	{
		get
		{
			return this.data.GetBonePosition(this.Bone, false);
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x0600140A RID: 5130 RVA: 0x000715DB File Offset: 0x0006F9DB
	public QuaternionF Rotation
	{
		get
		{
			return this.data.GetRotation(this.Bone, false);
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x0600140B RID: 5131 RVA: 0x000715EF File Offset: 0x0006F9EF
	Vector3 IBoneFloatTransform.position
	{
		get
		{
			return (Vector3)this.data.GetBonePosition(this.Bone, false);
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x0600140C RID: 5132 RVA: 0x00071608 File Offset: 0x0006FA08
	Quaternion IBoneFloatTransform.rotation
	{
		get
		{
			return (Quaternion)this.data.GetRotation(this.Bone, false);
		}
	}

	// Token: 0x04000D56 RID: 3414
	private IBodyOwner data;
}
