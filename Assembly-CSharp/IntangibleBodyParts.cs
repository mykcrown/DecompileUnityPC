using System;
using UnityEngine.Serialization;

// Token: 0x020004F9 RID: 1273
[Serializable]
public class IntangibleBodyParts : ICloneable
{
	// Token: 0x06001BB0 RID: 7088 RVA: 0x0008C063 File Offset: 0x0008A463
	public object Clone()
	{
		return CloneUtil.SlowDeepClone<IntangibleBodyParts>(this);
	}

	// Token: 0x04001590 RID: 5520
	public BodyPart[] bodyParts;

	// Token: 0x04001591 RID: 5521
	[FormerlySerializedAs("completelyInvincible")]
	public bool completelyIntangible = true;

	// Token: 0x04001592 RID: 5522
	[FormerlySerializedAs("activeFramesBegin")]
	public int startFrame;

	// Token: 0x04001593 RID: 5523
	[FormerlySerializedAs("activeFramesEnds")]
	public int endFrame;
}
