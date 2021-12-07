using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200039E RID: 926
public interface IBodyOwner
{
	// Token: 0x060013E1 RID: 5089
	HurtBoxState GetHurtBox(BodyPart bodyPart);

	// Token: 0x060013E2 RID: 5090
	QuaternionF GetRotation(BodyPart bodyPart, bool forceInvert = false);

	// Token: 0x060013E3 RID: 5091
	Vector3F GetBonePosition(BodyPart bodyPart, bool forceInvert = false);

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x060013E4 RID: 5092
	FixedRect CurrentMaxBounds { get; }

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x060013E5 RID: 5093
	Vector3F DeltaPosition { get; }

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x060013E6 RID: 5094
	bool HasRootMotion { get; }

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x060013E7 RID: 5095
	List<HurtBoxState> HurtBoxes { get; }

	// Token: 0x060013E8 RID: 5096
	void AttachVFX(BodyPart bodyPart, Transform vfxTransform, Vector3 vfxOffset, ParticleOffsetSpace particleOffsetSpace, bool invertHorizontal);

	// Token: 0x060013E9 RID: 5097
	void DrawGizmos();

	// Token: 0x060013EA RID: 5098
	IBoneTransform GetBoneTransform(BodyPart bodyPart);

	// Token: 0x060013EB RID: 5099
	IBoneTransform GetBoneOffsetTransform(BodyPart bodyPart, BodyPart secondaryBodyPart, Fixed offset);

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x060013EC RID: 5100
	string AnimationName { get; }

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x060013ED RID: 5101
	int AnimationFrame { get; }
}
