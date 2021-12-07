using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
[Serializable]
public class CameraConfig
{
	// Token: 0x06001598 RID: 5528 RVA: 0x00076EB4 File Offset: 0x000752B4
	public void Rescale(Fixed rescale)
	{
	}

	// Token: 0x04000F97 RID: 3991
	public CameraPreset usePreset;

	// Token: 0x04000F98 RID: 3992
	public float fovSpeed = 3.75f;

	// Token: 0x04000F99 RID: 3993
	public CameraSpeedScaling xySpeedScale = new CameraSpeedScaling();

	// Token: 0x04000F9A RID: 3994
	public float xMotionOut = 3f;

	// Token: 0x04000F9B RID: 3995
	public float yMotionOut = 3.75f;

	// Token: 0x04000F9C RID: 3996
	public bool useSplitDolly;

	// Token: 0x04000F9D RID: 3997
	public float dollySpeedOut = 8f;

	// Token: 0x04000F9E RID: 3998
	public float dollySpeedIn = 5f;

	// Token: 0x04000F9F RID: 3999
	public int dollyInDelay;

	// Token: 0x04000FA0 RID: 4000
	public CameraSpeedScaling dollySpeedScale = new CameraSpeedScaling();

	// Token: 0x04000FA1 RID: 4001
	public bool stabilizeEdges;

	// Token: 0x04000FA2 RID: 4002
	public CameraMotionParams dollyMotion = new CameraMotionParams(5f);

	// Token: 0x04000FA3 RID: 4003
	public CameraMotionParams panMotion = new CameraMotionParams(3.75f);

	// Token: 0x04000FA4 RID: 4004
	public CameraMotionParams rotateMotion = new CameraMotionParams(5f);

	// Token: 0x04000FA5 RID: 4005
	public CameraConfig.StageData defaultStageData = new CameraConfig.StageData();

	// Token: 0x04000FA6 RID: 4006
	public bool debugPosition;

	// Token: 0x04000FA7 RID: 4007
	public bool useBoundsMemory;

	// Token: 0x04000FA8 RID: 4008
	public int memoryFrames;

	// Token: 0x04000FA9 RID: 4009
	public bool dynamicPitch;

	// Token: 0x04000FAA RID: 4010
	public CameraEquation pitchEquation;

	// Token: 0x04000FAB RID: 4011
	public float pitchCurve = 1f;

	// Token: 0x04000FAC RID: 4012
	public float pitchMidpoint = 1.5f;

	// Token: 0x04000FAD RID: 4013
	public bool pitchNegative;

	// Token: 0x04000FAE RID: 4014
	public float pitchMin;

	// Token: 0x04000FAF RID: 4015
	public bool pitchBelowBounds;

	// Token: 0x04000FB0 RID: 4016
	public float pitchDeadZone;

	// Token: 0x04000FB1 RID: 4017
	public float pitchBottomBound = 4f;

	// Token: 0x04000FB2 RID: 4018
	public bool debugPitch;

	// Token: 0x04000FB3 RID: 4019
	public bool dynamicYaw;

	// Token: 0x04000FB4 RID: 4020
	public CameraYawMethod yawMethod;

	// Token: 0x04000FB5 RID: 4021
	public CameraEquation yawEquation;

	// Token: 0x04000FB6 RID: 4022
	public float yawCurve = 0.85f;

	// Token: 0x04000FB7 RID: 4023
	public bool useVerticalityYawReduce;

	// Token: 0x04000FB8 RID: 4024
	public float verticalityYawRatioSplit = 0.56f;

	// Token: 0x04000FB9 RID: 4025
	public float verticalityMinWidth = 1f;

	// Token: 0x04000FBA RID: 4026
	public float verticalitySoften = 1f;

	// Token: 0x04000FBB RID: 4027
	public float yawDeadZone = 2f;

	// Token: 0x04000FBC RID: 4028
	public float yawTravelSplitRatio = 0.5f;

	// Token: 0x04000FBD RID: 4029
	public float yawTravelSplitMaxHyp = 5f;

	// Token: 0x04000FBE RID: 4030
	public bool debugYaw;

	// Token: 0x04000FBF RID: 4031
	public bool debugYawVerticalityReduce;

	// Token: 0x04000FC0 RID: 4032
	public bool useLedgeGrabYaw;

	// Token: 0x04000FC1 RID: 4033
	public float ledgeGrabYaw;

	// Token: 0x04000FC2 RID: 4034
	public float ledgeGrabPitch = 4f;

	// Token: 0x04000FC3 RID: 4035
	public float ledgeGrabSlowYaw = -12f;

	// Token: 0x04000FC4 RID: 4036
	public float ledgeGrabSlowPitch = -4f;

	// Token: 0x04000FC5 RID: 4037
	public float ledgeGrabRotationSpeedMulti = 0.5f;

	// Token: 0x04000FC6 RID: 4038
	public float ledgeGrabRotationSlowMulti = 0.1f;

	// Token: 0x04000FC7 RID: 4039
	public int ledgeGrabRotateReleaseFrames = 60;

	// Token: 0x04000FC8 RID: 4040
	public bool useImpactHighlight;

	// Token: 0x04000FC9 RID: 4041
	public float impactFramesMulti = 1f;

	// Token: 0x04000FCA RID: 4042
	public float impactYawMulti;

	// Token: 0x04000FCB RID: 4043
	public float impactTransitionSpeed = 0.6f;

	// Token: 0x04000FCC RID: 4044
	public float impactModMin;

	// Token: 0x04000FCD RID: 4045
	public float impactModMax;

	// Token: 0x04000FCE RID: 4046
	public float impactReturnSpeed = 0.2f;

	// Token: 0x04000FCF RID: 4047
	public bool debugImpactHighlight;

	// Token: 0x04000FD0 RID: 4048
	public float guiOverlapAlpha = 0.3f;

	// Token: 0x04000FD1 RID: 4049
	public int characterTurnaroundFrames = 15;

	// Token: 0x04000FD2 RID: 4050
	public int characterTurnaroundDelay = 15;

	// Token: 0x04000FD3 RID: 4051
	public bool increaseCameraBoxAccuracy;

	// Token: 0x04000FD4 RID: 4052
	public bool adjustForLargeCharacters;

	// Token: 0x04000FD5 RID: 4053
	public float defaultCameraHeightBox = 3f;

	// Token: 0x04000FD6 RID: 4054
	public bool horizontalDriftBack;

	// Token: 0x04000FD7 RID: 4055
	public float driftbackStrength = 0.5f;

	// Token: 0x04000FD8 RID: 4056
	public bool useAdjustPaddingByZoom;

	// Token: 0x04000FD9 RID: 4057
	public bool debugCloseupPadding;

	// Token: 0x04000FDA RID: 4058
	public float closeUpPaddingMin;

	// Token: 0x04000FDB RID: 4059
	public float closeUpPaddingMax;

	// Token: 0x04000FDC RID: 4060
	public float closeUpPaddingAdjust;

	// Token: 0x04000FDD RID: 4061
	public float characterBoxYOffset;

	// Token: 0x04000FDE RID: 4062
	public CameraConfig.Padding trainingModePadding = new CameraConfig.Padding();

	// Token: 0x04000FDF RID: 4063
	public Fixed movementVelocityCompensation = (Fixed)3.0;

	// Token: 0x04000FE0 RID: 4064
	public bool debugMotion;

	// Token: 0x04000FE1 RID: 4065
	public bool adjustPositionWithRotation = true;

	// Token: 0x04000FE2 RID: 4066
	public bool deadPlayerFocusRespawn;

	// Token: 0x04000FE3 RID: 4067
	public int deadPlayerCameraDelayFrames = 60;

	// Token: 0x04000FE4 RID: 4068
	public int gameEndCameraDelayFrames = 60;

	// Token: 0x04000FE5 RID: 4069
	public int deadPlayerDollyDelayFrames;

	// Token: 0x04000FE6 RID: 4070
	public Vector2 approximateRespawnPoint = new Vector2(0f, 5.1f);

	// Token: 0x04000FE7 RID: 4071
	public GlobalCameraShakeData shakeData = new GlobalCameraShakeData();

	// Token: 0x020003F2 RID: 1010
	[Serializable]
	public class Padding
	{
		// Token: 0x04000FE8 RID: 4072
		public float top = 0.4f;

		// Token: 0x04000FE9 RID: 4073
		public float bottom = 0.4f;

		// Token: 0x04000FEA RID: 4074
		public float sides = 2.2f;
	}

	// Token: 0x020003F3 RID: 1011
	[Serializable]
	public class StageData
	{
		// Token: 0x04000FEB RID: 4075
		public float pitch = 12f;

		// Token: 0x04000FEC RID: 4076
		public float pitchFactor = 12f;

		// Token: 0x04000FED RID: 4077
		public float pitchMax = 15f;

		// Token: 0x04000FEE RID: 4078
		public float yawFactor = 28f;

		// Token: 0x04000FEF RID: 4079
		public float yawMax = 12f;

		// Token: 0x04000FF0 RID: 4080
		public float minDolly = 6.8f;

		// Token: 0x04000FF1 RID: 4081
		public float characterForwardCameraExtend = 2.4f;

		// Token: 0x04000FF2 RID: 4082
		public CameraConfig.Padding padding = new CameraConfig.Padding();
	}
}
