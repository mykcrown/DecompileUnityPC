using System;

// Token: 0x0200050E RID: 1294
[Serializable]
public class AnimationSpeedMultiplier
{
	// Token: 0x06001BF8 RID: 7160 RVA: 0x0008D5DB File Offset: 0x0008B9DB
	public AnimationSpeedMultiplier()
	{
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x0008D5FC File Offset: 0x0008B9FC
	public AnimationSpeedMultiplier(int startFrame, int endFrame, float speedMultiplier)
	{
		this.startFrame = startFrame;
		this.endFrame = endFrame;
		this.speedMultiplier = speedMultiplier;
	}

	// Token: 0x04001690 RID: 5776
	public int startFrame = 1;

	// Token: 0x04001691 RID: 5777
	public int endFrame = 2;

	// Token: 0x04001692 RID: 5778
	public float speedMultiplier = 1f;
}
