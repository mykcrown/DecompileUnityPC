using System;

// Token: 0x020005F1 RID: 1521
[Serializable]
public class PlayerShakeModel : CloneableObject
{
	// Token: 0x060023F0 RID: 9200 RVA: 0x000B5808 File Offset: 0x000B3C08
	public void CopyTo(PlayerShakeModel targetIn)
	{
		targetIn.framesUntilReduction = this.framesUntilReduction;
		targetIn.linearReduction = this.linearReduction;
		targetIn.frameCount = this.frameCount;
		targetIn.amplitude = this.amplitude;
		targetIn.startingAmplitude = this.startingAmplitude;
		targetIn.wavelength = this.wavelength;
		targetIn.xFactor = this.xFactor;
		targetIn.yFactor = this.yFactor;
	}

	// Token: 0x04001B56 RID: 6998
	public int framesUntilReduction;

	// Token: 0x04001B57 RID: 6999
	public float linearReduction;

	// Token: 0x04001B58 RID: 7000
	public int frameCount;

	// Token: 0x04001B59 RID: 7001
	public float amplitude;

	// Token: 0x04001B5A RID: 7002
	public float startingAmplitude;

	// Token: 0x04001B5B RID: 7003
	public float wavelength = 1f;

	// Token: 0x04001B5C RID: 7004
	public float xFactor;

	// Token: 0x04001B5D RID: 7005
	public float yFactor;
}
