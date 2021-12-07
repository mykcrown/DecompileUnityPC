// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class AimLinkComponentData : MoveLinkComponentData
{
	public Fixed FireAngle;

	public override void Apply(ref MoveModel model)
	{
		model.articleFireAngle = this.FireAngle;
		model.overrideFireAngle = true;
	}
}
