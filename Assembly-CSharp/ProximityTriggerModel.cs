using System;

// Token: 0x02000636 RID: 1590
[RollbackStatePoolMultiplier(2)]
[Serializable]
public class ProximityTriggerModel : StageObjectModel<ProximityTriggerModel>, ISituationalValidation
{
	// Token: 0x060026FE RID: 9982 RVA: 0x000BEBB8 File Offset: 0x000BCFB8
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

	// Token: 0x060026FF RID: 9983 RVA: 0x000BEC4C File Offset: 0x000BD04C
	public override object Clone()
	{
		ProximityTriggerModel proximityTriggerModel = new ProximityTriggerModel();
		this.CopyTo(proximityTriggerModel);
		return proximityTriggerModel;
	}

	// Token: 0x04001C8D RID: 7309
	[IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	public bool[] playerInsideLookup;
}
