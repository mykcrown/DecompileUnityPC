using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000396 RID: 918
[Serializable]
public class HurtBox
{
	// Token: 0x060013B0 RID: 5040 RVA: 0x000702D4 File Offset: 0x0006E6D4
	public HurtBox(BodyPart startBone, BodyPart endBone, float radius, Vector3 offset, bool isGrabbable)
	{
		this.startBone = startBone;
		this.endBone = endBone;
		this.radius = radius;
		this.offset = Vector3F.zero;
		this.isGrabbable = isGrabbable;
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00070321 File Offset: 0x0006E721
	public bool isCircle
	{
		get
		{
			return this.endBone == BodyPart.none || this.endBone == this.startBone;
		}
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x0007033F File Offset: 0x0006E73F
	public void Rescale(Fixed scale)
	{
		this.radius *= (float)scale;
		this.offset *= scale;
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x00070368 File Offset: 0x0006E768
	public Vector3F GetPoint1(IBodyOwner body)
	{
		Vector3F b = Vector3F.zero;
		if (this.offset != Vector3F.zero)
		{
			b = body.GetRotation(this.startBone, false) * this.offset;
		}
		return body.GetBonePosition(this.startBone, false) + b;
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x000703BC File Offset: 0x0006E7BC
	public Vector3F GetPoint2(IBodyOwner body)
	{
		Vector3F vector3F = Vector3F.zero;
		if (this.offset != Vector3F.zero)
		{
			vector3F = body.GetRotation(this.startBone, false) * this.offset;
		}
		if (this.endBone == BodyPart.none)
		{
			return vector3F;
		}
		return body.GetBonePosition(this.endBone, false) + vector3F;
	}

	// Token: 0x04000D27 RID: 3367
	public BodyPart startBone;

	// Token: 0x04000D28 RID: 3368
	public BodyPart endBone;

	// Token: 0x04000D29 RID: 3369
	public float radius;

	// Token: 0x04000D2A RID: 3370
	public Vector3F offset;

	// Token: 0x04000D2B RID: 3371
	public bool isGrabbable = true;

	// Token: 0x04000D2C RID: 3372
	public HurtBoxTumbleAnimation tumbleAnim;

	// Token: 0x04000D2D RID: 3373
	public HurtBoxPriority priority = HurtBoxPriority.MID;
}
