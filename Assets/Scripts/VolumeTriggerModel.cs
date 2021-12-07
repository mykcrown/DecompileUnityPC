// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class VolumeTriggerModel : RollbackStateTyped<VolumeTriggerModel>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	public bool[] playersInVolume;

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

	public override object Clone()
	{
		VolumeTriggerModel volumeTriggerModel = new VolumeTriggerModel();
		this.CopyTo(volumeTriggerModel);
		return volumeTriggerModel;
	}
}
