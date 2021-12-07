// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class SpikeConfig
{
	public int spikeAngleThreshold = 15;

	public bool firstSpikeIsUntechable = true;

	public int spikeEscapeFrames = 16;

	public Fixed groundedSpikeHitStunMulti = 1;

	public int spikeBounceHitlagBaseFrames = 3;

	public Fixed spikeBounceHitlagFromVelocity = (Fixed)0.15000000596046448;

	public bool resetUntechableSpikeWhenGrabbed;

	public bool useGroundbounceComboEscape;

	public int comboEscapeMaxRotationAngle = 20;

	public bool isSpike(int hitAngle)
	{
		hitAngle = (hitAngle + 360) % 360;
		return hitAngle >= 270 - this.spikeAngleThreshold && hitAngle <= 270 + this.spikeAngleThreshold;
	}

	public bool isSpike(HitData hitData)
	{
		return this.isSpike((int)hitData.knockbackAngle);
	}

	public int getEscapeFrames(HitData hitData)
	{
		return (!this.isSpike(hitData)) ? 0 : this.spikeEscapeFrames;
	}
}
