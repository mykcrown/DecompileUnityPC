// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class SurfaceTriggerModel : RollbackStateTyped<SurfaceTriggerModel>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	public bool[] playerOnSurface;

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

	public override object Clone()
	{
		SurfaceTriggerModel surfaceTriggerModel = new SurfaceTriggerModel();
		this.CopyTo(surfaceTriggerModel);
		return surfaceTriggerModel;
	}
}
