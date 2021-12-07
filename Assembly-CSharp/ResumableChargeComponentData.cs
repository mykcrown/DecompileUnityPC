using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

// Token: 0x020005CA RID: 1482
[Serializable]
public class ResumableChargeComponentData
{
	// Token: 0x04001A2B RID: 6699
	public int maxChargeFrames;

	// Token: 0x04001A2C RID: 6700
	[FormerlySerializedAs("onChargeCompleteMove")]
	public MoveData onChargeCompleteMoveGrounded;

	// Token: 0x04001A2D RID: 6701
	public MoveData onChargeCompleteMoveAerial;

	// Token: 0x04001A2E RID: 6702
	public List<ParticleData> fullyChargedLoopParticles;

	// Token: 0x04001A2F RID: 6703
	public List<ParticleData> chargeLevel2LoopParticles;
}
