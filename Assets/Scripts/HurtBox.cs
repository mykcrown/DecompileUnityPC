// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class HurtBox
{
	public BodyPart startBone;

	public BodyPart endBone;

	public float radius;

	public Vector3F offset;

	public bool isGrabbable = true;

	public HurtBoxTumbleAnimation tumbleAnim;

	public HurtBoxPriority priority = HurtBoxPriority.MID;

	public bool isCircle
	{
		get
		{
			return this.endBone == BodyPart.none || this.endBone == this.startBone;
		}
	}

	public HurtBox(BodyPart startBone, BodyPart endBone, float radius, Vector3 offset, bool isGrabbable)
	{
		this.startBone = startBone;
		this.endBone = endBone;
		this.radius = radius;
		this.offset = Vector3F.zero;
		this.isGrabbable = isGrabbable;
	}

	public void Rescale(Fixed scale)
	{
		this.radius *= (float)scale;
		this.offset *= scale;
	}

	public Vector3F GetPoint1(IBodyOwner body)
	{
		Vector3F b = Vector3F.zero;
		if (this.offset != Vector3F.zero)
		{
			b = body.GetRotation(this.startBone, false) * this.offset;
		}
		return body.GetBonePosition(this.startBone, false) + b;
	}

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
}
