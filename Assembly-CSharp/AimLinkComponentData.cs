using System;
using FixedPoint;

// Token: 0x020004C5 RID: 1221
public class AimLinkComponentData : MoveLinkComponentData
{
	// Token: 0x06001B04 RID: 6916 RVA: 0x0008A03A File Offset: 0x0008843A
	public override void Apply(ref MoveModel model)
	{
		model.articleFireAngle = this.FireAngle;
		model.overrideFireAngle = true;
	}

	// Token: 0x04001452 RID: 5202
	public Fixed FireAngle;
}
