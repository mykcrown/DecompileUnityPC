// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class LedgeConfig
{
	public int invincibilityFrames = 35;

	public int ledgeCooldownFrames = 20;

	public Fixed invincibilityDecay = (Fixed)0.2;

	public int minInvincibilityCliffFrames = 5;

	public int maxEdgeHoldFrames = 300;

	public Fixed multiGrabOffset = 1;

	public int multigrabTranslateFrames = 5;

	public bool secondPlayerNoIntangible = true;

	public int secondPlayerLedgeLag = 30;
}
