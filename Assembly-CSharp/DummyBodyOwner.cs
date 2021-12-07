using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200039F RID: 927
public class DummyBodyOwner : IBodyOwner
{
	// Token: 0x060013EE RID: 5102 RVA: 0x000714C8 File Offset: 0x0006F8C8
	public DummyBodyOwner(HurtBoxState hurtBox, Vector3F position, QuaternionF rotation = default(QuaternionF))
	{
		this.hurtBoxes = new List<HurtBoxState>
		{
			hurtBox
		};
		this.boneFrameData = new BoneFrameData(position, rotation);
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x00071507 File Offset: 0x0006F907
	public HurtBoxState GetHurtBox(BodyPart bodyPart)
	{
		return this.hurtBoxes[0];
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x00071515 File Offset: 0x0006F915
	public QuaternionF GetRotation(BodyPart bodyPart, bool forceInvert = false)
	{
		if (forceInvert)
		{
			Debug.LogError("Dummy doesn't support inverting");
		}
		return this.boneFrameData.rotation;
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x00071532 File Offset: 0x0006F932
	public Vector3F GetBonePosition(BodyPart bodyPart, bool forceInvert = false)
	{
		if (forceInvert)
		{
			Debug.LogError("Dummy doesn't support inverting");
		}
		return this.boneFrameData.position;
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x060013F2 RID: 5106 RVA: 0x0007154F File Offset: 0x0006F94F
	public List<HurtBoxState> HurtBoxes
	{
		get
		{
			return this.hurtBoxes;
		}
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x00071557 File Offset: 0x0006F957
	public void AttachVFX(BodyPart bodyPart, Transform vfxTransform, Vector3 vfxOffset, ParticleOffsetSpace particleOffsetSpace, bool invertHorizontal)
	{
		throw new NotImplementedException();
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x060013F4 RID: 5108 RVA: 0x0007155E File Offset: 0x0006F95E
	public Vector3F DeltaPosition
	{
		get
		{
			return Vector3F.zero;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x060013F5 RID: 5109 RVA: 0x00071565 File Offset: 0x0006F965
	public bool HasRootMotion
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00071568 File Offset: 0x0006F968
	public FixedRect CurrentMaxBounds
	{
		get
		{
			return default(FixedRect);
		}
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x0007157E File Offset: 0x0006F97E
	public void DrawGizmos()
	{
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x00071580 File Offset: 0x0006F980
	public IBoneTransform GetBoneTransform(BodyPart bodyPart)
	{
		return new BoneTransform(bodyPart, this);
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x0007158E File Offset: 0x0006F98E
	public IBoneTransform GetBoneOffsetTransform(BodyPart bodyPart, BodyPart secondaryBodyPart, Fixed offset)
	{
		return new BoneTransform(bodyPart, this);
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x060013FA RID: 5114 RVA: 0x0007159C File Offset: 0x0006F99C
	public string AnimationName
	{
		get
		{
			return "Debug";
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x060013FB RID: 5115 RVA: 0x000715A3 File Offset: 0x0006F9A3
	public int AnimationFrame
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x04000D54 RID: 3412
	private List<HurtBoxState> hurtBoxes = new List<HurtBoxState>();

	// Token: 0x04000D55 RID: 3413
	private BoneFrameData boneFrameData;
}
