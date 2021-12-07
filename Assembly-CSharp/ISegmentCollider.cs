using System;
using FixedPoint;

// Token: 0x020003AD RID: 941
public interface ISegmentCollider
{
	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06001439 RID: 5177
	SegmentColliderType Type { get; }

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x0600143A RID: 5178
	Fixed Radius { get; }

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x0600143B RID: 5179
	Vector3F Point1 { get; }

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x0600143C RID: 5180
	Vector3F Point2 { get; }

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x0600143D RID: 5181
	bool IsCircle { get; }

	// Token: 0x0600143E RID: 5182
	bool MatchesVisiblityState(HurtBoxVisibilityState visState);

	// Token: 0x0600143F RID: 5183
	bool InteractsWithType(HitType hitType);
}
