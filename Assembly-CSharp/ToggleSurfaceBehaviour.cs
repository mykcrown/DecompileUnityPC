using System;

// Token: 0x02000658 RID: 1624
public class ToggleSurfaceBehaviour : StageBehaviour
{
	// Token: 0x060027C1 RID: 10177 RVA: 0x000C1C2C File Offset: 0x000C002C
	public override void Play(object context)
	{
		if (this.TargetSurface != null)
		{
			ToggleSurfaceBehaviour.ToggleSurfaceType type = this.Type;
			if (type != ToggleSurfaceBehaviour.ToggleSurfaceType.Deactivate)
			{
				if (type != ToggleSurfaceBehaviour.ToggleSurfaceType.Activate)
				{
					if (type == ToggleSurfaceBehaviour.ToggleSurfaceType.Toggle)
					{
						this.TargetSurface.CollidersEnabled = !this.TargetSurface.CollidersEnabled;
					}
				}
				else
				{
					this.TargetSurface.CollidersEnabled = true;
				}
			}
			else
			{
				this.TargetSurface.CollidersEnabled = false;
			}
		}
	}

	// Token: 0x04001D0D RID: 7437
	public StageSurface TargetSurface;

	// Token: 0x04001D0E RID: 7438
	public ToggleSurfaceBehaviour.ToggleSurfaceType Type;

	// Token: 0x02000659 RID: 1625
	public enum ToggleSurfaceType
	{
		// Token: 0x04001D10 RID: 7440
		Deactivate,
		// Token: 0x04001D11 RID: 7441
		Activate,
		// Token: 0x04001D12 RID: 7442
		Toggle
	}
}
