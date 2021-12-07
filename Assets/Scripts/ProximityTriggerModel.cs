// Decompile from assembly: Assembly-CSharp.dll

using System;

[RollbackStatePoolMultiplier(2)]
[Serializable]
public class ProximityTriggerModel : StageObjectModel<ProximityTriggerModel>, ISituationalValidation
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	public bool[] playerInsideLookup;

	public override void CopyTo(ProximityTriggerModel target)
	{
		base.CopyTo(target);
		target.shouldValidate = this.shouldValidate;
		if (this.playerInsideLookup == null)
		{
			target.playerInsideLookup = null;
		}
		else
		{
			if (target.playerInsideLookup == null || target.playerInsideLookup.Length != this.playerInsideLookup.Length)
			{
				target.playerInsideLookup = new bool[this.playerInsideLookup.Length];
			}
			for (int i = 0; i < this.playerInsideLookup.Length; i++)
			{
				target.playerInsideLookup[i] = this.playerInsideLookup[i];
			}
		}
	}

	public override object Clone()
	{
		ProximityTriggerModel proximityTriggerModel = new ProximityTriggerModel();
		this.CopyTo(proximityTriggerModel);
		return proximityTriggerModel;
	}
}
