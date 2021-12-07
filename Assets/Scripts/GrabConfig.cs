// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class GrabConfig
{
	public int baseDurationFrames = 60;

	public Fixed dmgScaling = (Fixed)1.6000000238418579;

	public int attackBonusFrames = 10;

	public int buttonMashEscapeFrames = 7;

	public Fixed grabEscapeSpeed = (Fixed)10.0;

	public Vector2F airGrabEscapeVelocity = new Vector2F(5, 12);

	public int chainGrabPreventionFrames = 60;

	public bool useRegrabDelay;

	public int regrabDelayFrames;
}
