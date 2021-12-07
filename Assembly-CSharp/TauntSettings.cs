using System;
using FixedPoint;

// Token: 0x020003FF RID: 1023
[Serializable]
public class TauntSettings
{
	// Token: 0x0400102D RID: 4141
	public int emoteCooldownFrames = 300;

	// Token: 0x0400102E RID: 4142
	public AudioData emoteCooldownSound;

	// Token: 0x0400102F RID: 4143
	public bool useEmotesPerTime = true;

	// Token: 0x04001030 RID: 4144
	public int emotesPerTimeFrames = 300;

	// Token: 0x04001031 RID: 4145
	public int emotesPerTimeMax = 5;

	// Token: 0x04001032 RID: 4146
	public Fixed holoOffsetX = 1;

	// Token: 0x04001033 RID: 4147
	public Fixed holoOffsetY = (Fixed)2.5;
}
