using System;

// Token: 0x0200065E RID: 1630
[Serializable]
public class VolumeTriggerModel : RollbackStateTyped<VolumeTriggerModel>
{
	// Token: 0x060027EA RID: 10218 RVA: 0x000C2464 File Offset: 0x000C0864
	public override void CopyTo(VolumeTriggerModel target)
	{
		if (this.playersInVolume == null)
		{
			target.playersInVolume = null;
		}
		else
		{
			if (target.playersInVolume == null || target.playersInVolume.Length != this.playersInVolume.Length)
			{
				target.playersInVolume = new bool[this.playersInVolume.Length];
			}
			for (int i = 0; i < this.playersInVolume.Length; i++)
			{
				target.playersInVolume[i] = this.playersInVolume[i];
			}
		}
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x000C24E4 File Offset: 0x000C08E4
	public override object Clone()
	{
		VolumeTriggerModel volumeTriggerModel = new VolumeTriggerModel();
		this.CopyTo(volumeTriggerModel);
		return volumeTriggerModel;
	}

	// Token: 0x04001D23 RID: 7459
	[IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	public bool[] playersInVolume;
}
