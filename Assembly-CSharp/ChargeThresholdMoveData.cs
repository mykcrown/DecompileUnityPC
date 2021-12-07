using System;
using System.Collections.Generic;

// Token: 0x020004E7 RID: 1255
[Serializable]
public class ChargeThresholdMoveData
{
	// Token: 0x040014B8 RID: 5304
	public int chargeFramesNeeded;

	// Token: 0x040014B9 RID: 5305
	public MoveData moveData;

	// Token: 0x040014BA RID: 5306
	public List<ParticleData> particles = new List<ParticleData>();

	// Token: 0x040014BB RID: 5307
	public AudioData audio = default(AudioData);
}
