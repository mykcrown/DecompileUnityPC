// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class AshaniChargeLevelComponentState : RollbackStateTyped<AshaniChargeLevelComponentState>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually]
	[NonSerialized]
	public Dictionary<BodyPart, Effect> persistentParticles = new Dictionary<BodyPart, Effect>(8, default(BodyPartComparer));

	public override void CopyTo(AshaniChargeLevelComponentState target)
	{
		base.copyDictionary<BodyPart, Effect>(this.persistentParticles, target.persistentParticles);
	}

	public override object Clone()
	{
		AshaniChargeLevelComponentState ashaniChargeLevelComponentState = new AshaniChargeLevelComponentState();
		this.CopyTo(ashaniChargeLevelComponentState);
		return ashaniChargeLevelComponentState;
	}

	public override void Clear()
	{
		base.Clear();
		this.persistentParticles.Clear();
	}
}
