// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

[Serializable]
public class LedgeModel : StageObjectModel<LedgeModel>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually]
	public Dictionary<int, PlayerNum> playerSlots = new Dictionary<int, PlayerNum>();

	public Vector3F position;

	public override void CopyTo(LedgeModel target)
	{
		base.CopyTo(target);
		target.playerSlots.Clear();
		foreach (KeyValuePair<int, PlayerNum> current in this.playerSlots)
		{
			target.playerSlots[current.Key] = current.Value;
		}
		target.position = this.position;
	}
}
