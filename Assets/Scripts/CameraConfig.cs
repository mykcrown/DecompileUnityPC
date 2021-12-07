// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class CameraConfig
{
	[Serializable]
	public class Padding
	{
		public float top = 0.4f;

		public float bottom = 0.4f;

		public float sides = 2.2f;
	}

	[Serializable]
	public class StageData
	{
		public float pitch = 12f;

		public float pitchFactor = 12f;

		public float pitchMax = 15f;

		public float yawFactor = 28f;

		public float yawMax = 12f;

		public float minDolly = 6.8f;

		public float characterForwardCameraExtend = 2.4f;

		public CameraConfig.Padding padding = new CameraConfig.Padding();
	}

	public CameraPreset usePreset;

	public float fovSpeed = 3.75f;

	public CameraSpeedScaling xySpeedScale = new CameraSpeedScaling();

	public float xMotionOut = 3f;

	public float yMotionOut = 3.75f;

	public bool useSplitDolly;

	public float dollySpeedOut = 8f;

	public float dollySpeedIn = 5f;

	public int dollyInDelay;

	public CameraSpeedScaling dollySpeedScale = new CameraSpeedScaling();

	public bool stabilizeEdges;

	public CameraMotionParams dollyMotion = new CameraMotionParams(5f);

	public CameraMotionParams panMotion = new CameraMotionParams(3.75f);

	public CameraMotionParams rotateMotion = new CameraMotionParams(5f);

	public CameraConfig.StageData defaultStageData = new CameraConfig.StageData();

	public bool debugPosition;

	public bool useBoundsMemory;

	public int memoryFrames;

	public bool dynamicPitch;

	public CameraEquation pitchEquation;

	public float pitchCurve = 1f;

	public float pitchMidpoint = 1.5f;

	public bool pitchNegative;

	public float pitchMin;

	public bool pitchBelowBounds;

	public float pitchDeadZone;

	public float pitchBottomBound = 4f;

	public bool debugPitch;

	public bool dynamicYaw;

	public CameraYawMethod yawMethod;

	public CameraEquation yawEquation;

	public float yawCurve = 0.85f;

	public bool useVerticalityYawReduce;

	public float verticalityYawRatioSplit = 0.56f;

	public float verticalityMinWidth = 1f;

	public float verticalitySoften = 1f;

	public float yawDeadZone = 2f;

	public float yawTravelSplitRatio = 0.5f;

	public float yawTravelSplitMaxHyp = 5f;

	public bool debugYaw;

	public bool debugYawVerticalityReduce;

	public bool useLedgeGrabYaw;

	public float ledgeGrabYaw;

	public float ledgeGrabPitch = 4f;

	public float ledgeGrabSlowYaw = -12f;

	public float ledgeGrabSlowPitch = -4f;

	public float ledgeGrabRotationSpeedMulti = 0.5f;

	public float ledgeGrabRotationSlowMulti = 0.1f;

	public int ledgeGrabRotateReleaseFrames = 60;

	public bool useImpactHighlight;

	public float impactFramesMulti = 1f;

	public float impactYawMulti;

	public float impactTransitionSpeed = 0.6f;

	public float impactModMin;

	public float impactModMax;

	public float impactReturnSpeed = 0.2f;

	public bool debugImpactHighlight;

	public float guiOverlapAlpha = 0.3f;

	public int characterTurnaroundFrames = 15;

	public int characterTurnaroundDelay = 15;

	public bool increaseCameraBoxAccuracy;

	public bool adjustForLargeCharacters;

	public float defaultCameraHeightBox = 3f;

	public bool horizontalDriftBack;

	public float driftbackStrength = 0.5f;

	public bool useAdjustPaddingByZoom;

	public bool debugCloseupPadding;

	public float closeUpPaddingMin;

	public float closeUpPaddingMax;

	public float closeUpPaddingAdjust;

	public float characterBoxYOffset;

	public CameraConfig.Padding trainingModePadding = new CameraConfig.Padding();

	public Fixed movementVelocityCompensation = (Fixed)3.0;

	public bool debugMotion;

	public bool adjustPositionWithRotation = true;

	public bool deadPlayerFocusRespawn;

	public int deadPlayerCameraDelayFrames = 60;

	public int gameEndCameraDelayFrames = 60;

	public int deadPlayerDollyDelayFrames;

	public Vector2 approximateRespawnPoint = new Vector2(0f, 5.1f);

	public GlobalCameraShakeData shakeData = new GlobalCameraShakeData();

	public void Rescale(Fixed rescale)
	{
	}
}
