using System;

// Token: 0x0200037F RID: 895
public struct CameraImpactModeRequest
{
	// Token: 0x06001315 RID: 4885 RVA: 0x0006EFA5 File Offset: 0x0006D3A5
	public CameraImpactModeRequest(float strength, int direction, int delayFrames, int frameCount)
	{
		this.strength = strength;
		this.direction = direction;
		this.frameCount = frameCount;
		this.delayFrames = delayFrames;
	}

	// Token: 0x04000C99 RID: 3225
	public float strength;

	// Token: 0x04000C9A RID: 3226
	public int frameCount;

	// Token: 0x04000C9B RID: 3227
	public int direction;

	// Token: 0x04000C9C RID: 3228
	public int delayFrames;
}
