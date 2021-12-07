using System;
using FixedPoint;

// Token: 0x020003E8 RID: 1000
[Serializable]
public class SpikeConfig
{
	// Token: 0x06001586 RID: 5510 RVA: 0x000769BD File Offset: 0x00074DBD
	public bool isSpike(int hitAngle)
	{
		hitAngle = (hitAngle + 360) % 360;
		return hitAngle >= 270 - this.spikeAngleThreshold && hitAngle <= 270 + this.spikeAngleThreshold;
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x000769F5 File Offset: 0x00074DF5
	public bool isSpike(HitData hitData)
	{
		return this.isSpike((int)hitData.knockbackAngle);
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x00076A04 File Offset: 0x00074E04
	public int getEscapeFrames(HitData hitData)
	{
		return (!this.isSpike(hitData)) ? 0 : this.spikeEscapeFrames;
	}

	// Token: 0x04000F61 RID: 3937
	public int spikeAngleThreshold = 15;

	// Token: 0x04000F62 RID: 3938
	public bool firstSpikeIsUntechable = true;

	// Token: 0x04000F63 RID: 3939
	public int spikeEscapeFrames = 16;

	// Token: 0x04000F64 RID: 3940
	public Fixed groundedSpikeHitStunMulti = 1;

	// Token: 0x04000F65 RID: 3941
	public int spikeBounceHitlagBaseFrames = 3;

	// Token: 0x04000F66 RID: 3942
	public Fixed spikeBounceHitlagFromVelocity = (Fixed)0.15000000596046448;

	// Token: 0x04000F67 RID: 3943
	public bool resetUntechableSpikeWhenGrabbed;

	// Token: 0x04000F68 RID: 3944
	public bool useGroundbounceComboEscape;

	// Token: 0x04000F69 RID: 3945
	public int comboEscapeMaxRotationAngle = 20;
}
