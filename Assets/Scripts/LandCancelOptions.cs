// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public struct LandCancelOptions
{
	public MoveData landMove;

	public bool startLandAtCurrentFrame;

	public bool transferHitDisabledTargets;

	public bool transferChargeData;

	public bool haltMovementOnLand;

	public Fixed maxHorizontalLandSpeed;

	public Fixed bounceSpeed;

	public ParticleData bounceParticle;

	public MoveData bounceCancelMove;

	public int noAutoCancelFramesBegin;

	public int noAutoCancelFramesEnd;

	public AnimationClip landCancelClip;

	public AnimationClip leftLandCancelClip;

	public int landLagFrames;

	public int landLagVisualFrames;

	public bool useLandCameraShake;

	public MoveCameraShakeData landingCameraShake;

	public AudioData sound;
}
