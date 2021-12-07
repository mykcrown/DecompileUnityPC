// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class MoveCancelData
{
	public bool haltMovementOnLand;

	public Fixed maxHorizontalLandSpeed = 0;

	public float bounceSpeed;

	public MoveData bounceCancelMove;

	public ParticleData bounceParticle;

	public bool cancelOnLand;

	public int noAutoCancelFramesBegin;

	public int noAutoCancelFramesEnd;

	public AnimationClipFile landCancelClipFile = new AnimationClipFile();

	public AnimationClipFile leftLandCancelClipFile = new AnimationClipFile();

	public int landLagFrames = 10;

	public int landLagVisualFrames = 20;

	public bool useLandCameraShake;

	public MoveCameraShakeData landingCameraShake;

	public AudioData sound;

	public MoveData landMove;

	public bool startLandMoveAtCurrentFrame;

	public bool landTransferHitDisabledTargets = true;

	public bool landTransferChargeData;

	public bool cancelOnFall;

	public MoveData fallMove;

	public bool startFallMoveAtCurrentFrame;

	public bool fallTransferHitDisabledTargets = true;

	public AnimationClip landCancelClip
	{
		get
		{
			return this.landCancelClipFile.obj;
		}
	}

	public AnimationClip leftLandCancelClip
	{
		get
		{
			return this.leftLandCancelClipFile.obj;
		}
	}

	public static string GetClipName(MoveData move, bool isLeft)
	{
		return string.Format("{0}_land{1}", move.name, (!isLeft) ? string.Empty : "_left");
	}

	public LandCancelOptions GetLandCancelOptions()
	{
		return new LandCancelOptions
		{
			landMove = this.landMove,
			startLandAtCurrentFrame = this.startLandMoveAtCurrentFrame,
			transferHitDisabledTargets = this.landTransferHitDisabledTargets,
			transferChargeData = this.landTransferChargeData,
			haltMovementOnLand = this.haltMovementOnLand,
			maxHorizontalLandSpeed = this.maxHorizontalLandSpeed,
			bounceSpeed = (Fixed)((double)this.bounceSpeed),
			bounceParticle = this.bounceParticle,
			bounceCancelMove = this.bounceCancelMove,
			noAutoCancelFramesBegin = this.noAutoCancelFramesBegin,
			noAutoCancelFramesEnd = this.noAutoCancelFramesEnd,
			landCancelClip = this.landCancelClipFile.obj,
			leftLandCancelClip = this.leftLandCancelClipFile.obj,
			landLagFrames = this.landLagFrames,
			landLagVisualFrames = this.landLagVisualFrames,
			useLandCameraShake = this.useLandCameraShake,
			landingCameraShake = this.landingCameraShake,
			sound = this.sound
		};
	}

	public FallCancelOptions GetFallCancelOptions()
	{
		return new FallCancelOptions
		{
			fallMove = this.fallMove,
			startFallAtCurrentFrame = this.startFallMoveAtCurrentFrame,
			transferHitDisableTargets = this.fallTransferHitDisabledTargets
		};
	}
}
