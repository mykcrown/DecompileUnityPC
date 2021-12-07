// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[RollbackStatePoolMultiplier(2)]
[Serializable]
public class AnnouncementStatModel : RollbackStateTyped<AnnouncementStatModel>
{
	public int currentQuantity;

	public Fixed lastRecordedSeconds;

	public override void CopyTo(AnnouncementStatModel target)
	{
		target.currentQuantity = this.currentQuantity;
		target.lastRecordedSeconds = this.lastRecordedSeconds;
	}
}
