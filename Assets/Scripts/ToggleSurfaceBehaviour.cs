// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ToggleSurfaceBehaviour : StageBehaviour
{
	public enum ToggleSurfaceType
	{
		Deactivate,
		Activate,
		Toggle
	}

	public StageSurface TargetSurface;

	public ToggleSurfaceBehaviour.ToggleSurfaceType Type;

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
}
