using System;

// Token: 0x02000655 RID: 1621
[Serializable]
public class SurfaceTriggerModel : RollbackStateTyped<SurfaceTriggerModel>
{
	// Token: 0x060027B7 RID: 10167 RVA: 0x000C1980 File Offset: 0x000BFD80
	public override void CopyTo(SurfaceTriggerModel target)
	{
		if (this.playerOnSurface == null)
		{
			target.playerOnSurface = null;
		}
		else
		{
			if (target.playerOnSurface == null || target.playerOnSurface.Length != this.playerOnSurface.Length)
			{
				target.playerOnSurface = new bool[this.playerOnSurface.Length];
			}
			for (int i = 0; i < this.playerOnSurface.Length; i++)
			{
				target.playerOnSurface[i] = this.playerOnSurface[i];
			}
		}
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x000C1A00 File Offset: 0x000BFE00
	public override object Clone()
	{
		SurfaceTriggerModel surfaceTriggerModel = new SurfaceTriggerModel();
		this.CopyTo(surfaceTriggerModel);
		return surfaceTriggerModel;
	}

	// Token: 0x04001D04 RID: 7428
	[IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	public bool[] playerOnSurface;
}
