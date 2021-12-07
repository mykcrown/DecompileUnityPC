// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class ComboEscapeConfig
{
	public int maxRotationAngle = 20;

	public Fixed escapeDistance = (Fixed)0.0989999994635582;

	public Fixed autoEscapeDistance = (Fixed)0.0099000008776783943;

	public int maxEscapeInputs = 1;

	public int minAngleDifference = 45;

	public bool allowLandingDuringHitlag = true;

	public Fixed shieldEscapeMultiplier = (Fixed)0.5;

	public bool scaling;

	public Fixed scalingFloor = 20;

	public Fixed scalingCeiling = 80;

	public Fixed scalingMin = 8;

	public Fixed scalingMax = 24;

	public bool debugScaling;

	public void Rescale(Fixed rescale)
	{
		this.escapeDistance *= rescale;
		this.autoEscapeDistance *= rescale;
	}
}
