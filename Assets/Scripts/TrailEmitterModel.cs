// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[RollbackStatePoolMultiplier(8)]
[Serializable]
public class TrailEmitterModel : RollbackStateTyped<TrailEmitterModel>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	public TrailEmitterData defaultData;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public TrailEmitterData overrideData;

	public int frameCount;

	public Vector2F lastEmitPosition;

	public bool isDead;

	public override void CopyTo(TrailEmitterModel target)
	{
		target.defaultData = this.defaultData;
		target.overrideData = this.overrideData;
		target.frameCount = this.frameCount;
		target.isDead = this.isDead;
		target.lastEmitPosition = this.lastEmitPosition;
	}
}
