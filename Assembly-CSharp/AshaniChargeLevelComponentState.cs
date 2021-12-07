using System;
using System.Collections.Generic;

// Token: 0x0200059E RID: 1438
[Serializable]
public class AshaniChargeLevelComponentState : RollbackStateTyped<AshaniChargeLevelComponentState>
{
	// Token: 0x0600207B RID: 8315 RVA: 0x000A3D49 File Offset: 0x000A2149
	public override void CopyTo(AshaniChargeLevelComponentState target)
	{
		base.copyDictionary<BodyPart, Effect>(this.persistentParticles, target.persistentParticles);
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x000A3D60 File Offset: 0x000A2160
	public override object Clone()
	{
		AshaniChargeLevelComponentState ashaniChargeLevelComponentState = new AshaniChargeLevelComponentState();
		this.CopyTo(ashaniChargeLevelComponentState);
		return ashaniChargeLevelComponentState;
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x000A3D7B File Offset: 0x000A217B
	public override void Clear()
	{
		base.Clear();
		this.persistentParticles.Clear();
	}

	// Token: 0x040019E3 RID: 6627
	[IsClonedManually]
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public Dictionary<BodyPart, Effect> persistentParticles = new Dictionary<BodyPart, Effect>(8, default(BodyPartComparer));
}
