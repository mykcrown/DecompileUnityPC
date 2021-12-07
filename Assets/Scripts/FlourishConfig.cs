// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class FlourishConfig
{
	public bool useKillFlourish;

	public bool lastStockOnly;

	public Fixed requiredOverkill;

	public Fixed minKnockback = 30;

	public Fixed minDamage = 10;

	public Fixed minDistanceX = -(Fixed)0.5;

	public Fixed maxDistanceX = (Fixed)1.0;

	public Fixed minDistanceY = -(Fixed)0.0;

	public Fixed maxDistanceY = (Fixed)1.0;

	public Fixed secondRaycastOffsetY = (Fixed)1.0;

	public bool predictDI = true;

	public bool printDebug;

	public Fixed cameraZoomSpeed = 1;

	public int hitLagFrames = 45;

	public bool disableVibrate;

	public bool disableCameraShake;

	public int pauseVfxFrame;

	public int advanceFrames;

	public bool highlightAttacker = true;

	public bool highlightReceiver;

	public bool gravityAssist;

	public int gravityAssistFrames = 20;

	public float cameraBoxWidth = 3f;

	public float cameraBoxHeight = 3f;

	public bool stopSDI = true;

	public Fixed increaseKnockback = 0;

	public Fixed miniZoomSpeed = 1;
}
