// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[RollbackStatePoolMultiplier(2)]
[Serializable]
public class ComboStateModel : RollbackStateTyped<ComboStateModel>
{
	public Fixed Damage;

	public int Count;

	public bool IsRecovered;

	public int FramesRecovered;

	public bool IsActive;

	public int WindowFrames;

	public override void CopyTo(ComboStateModel target)
	{
		target.Damage = this.Damage;
		target.Count = this.Count;
		target.IsRecovered = this.IsRecovered;
		target.FramesRecovered = this.FramesRecovered;
		target.IsActive = this.IsActive;
		target.WindowFrames = this.WindowFrames;
	}
}
