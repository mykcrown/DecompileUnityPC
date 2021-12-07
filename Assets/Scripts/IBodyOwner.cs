// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBodyOwner
{
	FixedRect CurrentMaxBounds
	{
		get;
	}

	Vector3F DeltaPosition
	{
		get;
	}

	bool HasRootMotion
	{
		get;
	}

	List<HurtBoxState> HurtBoxes
	{
		get;
	}

	string AnimationName
	{
		get;
	}

	int AnimationFrame
	{
		get;
	}

	HurtBoxState GetHurtBox(BodyPart bodyPart);

	QuaternionF GetRotation(BodyPart bodyPart, bool forceInvert = false);

	Vector3F GetBonePosition(BodyPart bodyPart, bool forceInvert = false);

	void AttachVFX(BodyPart bodyPart, Transform vfxTransform, Vector3 vfxOffset, ParticleOffsetSpace particleOffsetSpace, bool invertHorizontal);

	void DrawGizmos();

	IBoneTransform GetBoneTransform(BodyPart bodyPart);

	IBoneTransform GetBoneOffsetTransform(BodyPart bodyPart, BodyPart secondaryBodyPart, Fixed offset);
}
