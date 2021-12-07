// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[RollbackStatePoolMultiplier(5)]
[Serializable]
public class StageSurfaceModel : StageObjectModel<StageSurfaceModel>
{
	public bool collidersEnabled = true;

	public Vector3F lastPosition;

	public Vector3F position;

	public override void CopyTo(StageSurfaceModel target)
	{
		base.CopyTo(target);
		target.collidersEnabled = this.collidersEnabled;
		target.lastPosition = this.lastPosition;
		target.position = this.position;
	}
}
